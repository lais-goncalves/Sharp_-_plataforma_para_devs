using System.Collections.Specialized;
using System.Net.Http.Headers;
using System.Web;
using Newtonsoft.Json;
using Sharp.Models.Paginas;
using Sharp.Models.Usuarios;

namespace Sharp.Models.ConexoesExternas.TiposDeConexoes
{
    public class ConexaoGitHub : ConexaoExterna
    {
        #region Propriedades
        public static string? urlSite = "https://api.github.com";
        public static string? CLIENT_ID = buscarClientId();
        public static string? CLIENT_SECRET = buscarClientSecret();
        public override string? NomeDaConexao { get; protected set; } = "github";
        #endregion Propriedades


        #region Construtores
        public ConexaoGitHub(UsuarioLogavel UsuarioLogavel) : base(UsuarioLogavel) { }
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
                string urlBusca = urlSite + "/user/" + IdLogin;
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
            try
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

            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return null;
            }
        }

        protected static Dictionary<string, string?> BuscaInfoDaFonte(string tokenDeAcesso)
        {
            Dictionary<string, string?> infoUsuario = new ();

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

                infoUsuario["id"] = objInfoGitHubUsuario?.id;
                infoUsuario["apelido"] = objInfoGitHubUsuario?.login;

                return infoUsuario;
            }

            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return infoUsuario;
            }
        }

        public static async Task<Dictionary<string, string?>> BuscarTokenEId(string codigoDoUsuario)
        {
            try
            {
                string? tokenDeAcesso = await BuscarTokenAutenticacaoDaFonte(codigoDoUsuario);
                if (tokenDeAcesso == null)
                {
                    throw new Exception("Login mal sucedido. Tente novamente.");
                }

                Dictionary<string, string?> infoUsuario = BuscaInfoDaFonte(tokenDeAcesso);
                return infoUsuario;
            }

            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return null;
            }
        }

        public void LogarUsuarioUsandoId(string idGitHub)
        {
            ConexaoGitHub conexaoGitHub = (ConexaoGitHub)UsuarioLogavel.Perfil.ConexoesDoUsuario["github"];

            Usuario? usuarioAutenticado = conexaoGitHub.BuscarUsuarioPorIdDeLogin(idGitHub);
            if (usuarioAutenticado == null)
            {
                throw new Exception("Para logar-se com o GitHub, é necessário criar uma conexaoExterna primeiro.");
            }

            // verificar se usuário encontrado pode ser logado
            UsuarioLogavel.Logar(usuarioAutenticado?.Email, usuarioAutenticado?.Senha);
            if (!UsuarioLogavel.EstaLogado())
            {
                // FIXME: dá problema ao tentar logar com o github, arruamr
                throw new Exception("Houve um problema ao tentar logar. Tente novamente.");
            }
        }

        public async Task<RetornoAPI<UsuarioLogavel?>> RetornoLoginGitHub(string codigo)
        {
            RetornoAPI<UsuarioLogavel?> resultado = new RetornoAPI<UsuarioLogavel?>();

            // TODO: separar método em partes menores
            try
            {
                Dictionary<string, string?>? infoUsuario = await BuscarTokenEId(codigo);
                string? id = infoUsuario["id"];
                string? apelido = infoUsuario["apelido"];

                if (id == null || apelido == null)
                {
                    throw new Exception("Um erro ocorreu ao tentar buscar as informações do GitHub. Tente logar novamente.");
                }

                if (!UsuarioLogavel.EstaLogado())
                {
                    LogarUsuarioUsandoId(id);
                }

                // se usuário já estiver logado
                else
                {
                    ConexaoGitHub conexaoGitUsuario = (ConexaoGitHub)UsuarioLogavel.Perfil.ConexoesDoUsuario["github"];

                    string? idGitHubUsuario = conexaoGitUsuario.IdLogin;
                    if (idGitHubUsuario == null)
                    {
                        // definir ID do github no banco caso não esteja definido
                        bool idFoiDefinido = conexaoGitUsuario.CadastrarConexao(id, apelido);
                        if (!idFoiDefinido)
                        {
                            throw new Exception("Um erro ocorreu ao tentar buscar as informações do GitHub. Tente logar novamente.");
                        }
                    }

                    // caso já esteja definido, mas o ID buscado é diferente do banco
                    else if (idGitHubUsuario != id)
                    {
                        throw new Exception("Já existe um usuário logado nesta conexaoExterna. Tente usar outra.");
                    }
                }

                resultado.DefinirDados(UsuarioLogavel);
            }

            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                resultado.DefinirErro(err);
            }

            return resultado;
        }
        #endregion Métodos
    }
}
