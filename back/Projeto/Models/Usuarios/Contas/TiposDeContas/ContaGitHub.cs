using System.Collections.Specialized;
using System.Net.Http.Headers;
using System.Web;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Npgsql;
using Projeto.Models.Bancos;
using Projeto.Models.Paginas;
using Projeto.Models.Usuarios.Perfis.TiposDePerfis;

namespace Projeto.Models.Usuarios.Contas.TiposDeContas
{
    public class ContaGitHub : Conta
    {
        #region Propriedades
        protected static string? urlSite = "https://api.github.com";
        private static string? CLIENT_ID = buscarClientId();
        private static string? CLIENT_SECRET = buscarClientSecret();

        protected override string ColunaIdBanco { get; set; } = "id_github";
        #endregion Propriedades


        #region Construtores
        public ContaGitHub(UsuarioLogavel UsuarioLogavel) : base(UsuarioLogavel) { }
        #endregion Construtores


        #region Métodos
        #region Utils
        private static string? buscarClientId()
        {
            return System.Configuration.ConfigurationManager.AppSettings["GITHUB_CLIENT_ID"];
        }

        private static string? buscarClientSecret()
        {
            return System.Configuration.ConfigurationManager.AppSettings["GITHUB_CLIENT_SECRET"];
        }
        #endregion Utils

        public override void BuscarInformacoesDaFonte()
        {
            try
            {
                string urlBusca = urlSite + "/user/" + Id;
                HttpRequestMessage buscaPerfil = new(HttpMethod.Get, urlBusca);

                buscaPerfil.Headers.Add("Accept", "application/json");
                buscaPerfil.Headers.UserAgent.ParseAdd("Sharp");

                HttpClient clienteHttp = new();

                var retornoInfoUsuario = clienteHttp.Send(buscaPerfil);
                if (!retornoInfoUsuario.IsSuccessStatusCode)
                {
                    return;
                }

                string? resultado = retornoInfoUsuario.Content.ReadAsStringAsync().Result;

                Console.WriteLine(resultado);
            }

            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
        }

        public override void BuscarInformacoesDoBanco()
        {
            try
            {
                // TODO: arrumar -> login é efetuado e isso aqui roda duas vezes
                NpgsqlParameter paramId = new NpgsqlParameter("@id", UsuarioLogavel?.Id);
                paramId.DbType = System.Data.DbType.Int32;
                string comando = string.Concat("SELECT id_github FROM ", Usuario.Tabela.NomeTabela, " WHERE id = @id");

                Conexao conexao = Conexao.instancia;
                Id = conexao.ExecutarUnico<string>(comando, [paramId]);
            }

            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
        }

        private bool DefinirInfoNoBanco(string id)
        {
            try
            {
                NpgsqlParameter paramId = new NpgsqlParameter("@id", UsuarioLogavel?.Id);
                NpgsqlParameter paramIdGitHub = new NpgsqlParameter("@id_github", id);
                string comando = string.Concat("UPDATE ", Usuario.Tabela.NomeTabela, " SET id_github = @id_github WHERE id = @id");

                Conexao conexao = Conexao.instancia;
                conexao.ExecutarUnico<string>(comando, [paramId, paramIdGitHub]);

                return true;
            }

            catch (Exception)
            {
                Console.WriteLine("Usuário não encontrado.");
                return false;
            }
        }

        public bool CadastrarId(string id)
        {
            try
            {
                bool idFoiDefinido = DefinirInfoNoBanco(id);
                if (!idFoiDefinido)
                {
                    throw new Exception("Um erro ocorreu ao tentar buscar as informações do GitHub. Tente logar novamente.");
                }

                BuscarTodasAsInfomacoes();

                return idFoiDefinido;
            }

            catch (Exception)
            {
                return false;
            }
        }

        public static string BuscarURLLogin()
        {
            NameValueCollection? query = HttpUtility.ParseQueryString(string.Empty);
            query["client_id"] = CLIENT_ID;
            string? queryString = query.ToString();

            string urlLogin = "https://github.com/login/oauth/authorize?" + queryString;
            return urlLogin;
        }

        protected static async Task<string?> BuscarTokenAutenticacaoDaFonte(string codigoDoUsuario)
        {
            NameValueCollection? query = HttpUtility.ParseQueryString(string.Empty);
            query["client_id"] = CLIENT_ID;
            query["client_secret"] = CLIENT_SECRET;
            query["code"] = codigoDoUsuario;

            string? queryString = query.ToString();
            string urlToken = "https://github.com/login/oauth/access_token?" + queryString;
            HttpClient clienteHttp = new HttpClient();

            HttpResponseMessage postToken = await clienteHttp.PostAsync(urlToken, null);
            if (!postToken.IsSuccessStatusCode)
            {
                throw new Exception("Login mal sucedido. Tente novamente.");
            }

            // PASSO 2 -> PEGAR TOKEN NA STRING DE RETORNO DO GITHUB
            string queryStringToken = postToken.Content.ReadAsStringAsync().Result;
            NameValueCollection? queryToken = HttpUtility.ParseQueryString(queryStringToken);

            string? token = queryToken["access_token"];
            return token;
        }

        protected static string? BuscarIdDaFonte(string tokenDeAcesso)
        {
            try
            {
                HttpClient clienteHttp = new();
                HttpRequestMessage getInfoUsuario = new(HttpMethod.Get, "https://api.github.com/user");

                getInfoUsuario.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenDeAcesso);
                getInfoUsuario.Headers.Add("Accept", "application/json");
                getInfoUsuario.Headers.UserAgent.ParseAdd("Sharp");

                var retornoInfoUsuario = clienteHttp.Send(getInfoUsuario);
                if (!retornoInfoUsuario.IsSuccessStatusCode)
                {
                    throw new Exception("Login mal sucedido. Tente novamente.");
                }

                // verificar se usuário já está conectado ao GitHub
                string? infoGitHubUsuario = retornoInfoUsuario.Content.ReadAsStringAsync().Result;
                if (infoGitHubUsuario == null)
                {
                    throw new Exception("Um erro ocorreu ao tentar buscar as informações do GitHub. Tente logar novamente.");
                }

                dynamic? objInfoGitHubUsuario = JsonConvert.DeserializeObject(infoGitHubUsuario);
                string? idInfoGitHub = objInfoGitHubUsuario?.id;

                return idInfoGitHub;
            }

            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return null;
            }
        }

        public static async Task<string?> BuscarTokenEId(string codigoDoUsuario)
        {
            try
            {
                string? tokenDeAcesso = await BuscarTokenAutenticacaoDaFonte(codigoDoUsuario);
                if (tokenDeAcesso == null)
                {
                    throw new Exception("Login mal sucedido. Tente novamente.");
                }

                string? idGitHub = BuscarIdDaFonte(tokenDeAcesso);
                return idGitHub;
            }

            catch (Exception err)
            {
                Console.WriteLine(err);
                return null;
            }
        }

        public void LogarUsuarioUsandoId(string idGitHub)
        {
            Usuario? usuarioAutenticado = PerfilDev.BuscarPorIdGitHub(idGitHub);
            if (usuarioAutenticado == null)
            {
                throw new Exception("Para logar-se com o GitHub, é necessário criar uma conta primeiro.");
            }

            UsuarioLogavel.Logar(usuarioAutenticado?.Email, usuarioAutenticado?.Senha);
            if (!UsuarioLogavel.EstaLogado())
            {
                throw new Exception("Houve um problema ao tentar logar. Tente novamente.");
            }
        }

        public async Task<RetornoAPI<UsuarioLogavel?>> RetornoLoginGitHub(string codigo)
        {
            RetornoAPI<UsuarioLogavel?> resultado = new RetornoAPI<UsuarioLogavel?>();

            // TODO: separar método em partes menores
            try
            {
                // TODO: criar verificação se usuário é dev

                string? idGitHub = await BuscarTokenEId(codigo);
                if (idGitHub == null)
                {
                    throw new Exception("Um erro ocorreu ao tentar buscar as informações do GitHub. Tente logar novamente.");
                }

                if (!UsuarioLogavel.EstaLogado())
                {
                    LogarUsuarioUsandoId(idGitHub);
                }

                // se usuário já estiver logado
                else
                {
                    // TODO: mandar essa autenticação pro PerfilDev
                    PerfilDev perfilUsuario = (PerfilDev) UsuarioLogavel.Perfil;
                    ContaGitHub contaGitUsuario = (ContaGitHub) perfilUsuario.ContasDoUsuario["github"];

                    string? idGitHubUsuario = contaGitUsuario.Id;
                    if (idGitHubUsuario == null)
                    {
                        // definir ID do github no banco caso não esteja definido
                        bool idFoiDefinido = contaGitUsuario.CadastrarId(idGitHub);
                        if (!idFoiDefinido)
                        {
                            throw new Exception("Um erro ocorreu ao tentar buscar as informações do GitHub. Tente logar novamente.");
                        }
                    }

                    // caso já esteja definido, mas o ID buscado é diferente do banco
                    else if (idGitHubUsuario != idGitHub)
                    {
                        throw new Exception("Já existe um usuário logado nesta conta. Tente usar outra.");
                    }
                }

                resultado.DefinirDados(UsuarioLogavel);
            }

            catch (Exception err)
            {
                resultado.DefinirErro(err);
            }

            return resultado;
        }
        #endregion Métodos
    }
}
