using System.Collections.Specialized;
using System.Net.Http.Headers;
using System.Web;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Npgsql;
using NuGet.Common;
using Projeto.Dados;

namespace Projeto.Models.Perfil
{
    public class PerfilGitHub : IPerfil
    {
        [JsonIgnore]
        public int IdPerfilSharp { get; }
        [JsonIgnore]
        public string? Id { get; protected set; }
        public string? Apelido { get; }

        protected static string? urlSite = "https://api.github.com";
        private static string? CLIENT_ID = System.Configuration.ConfigurationManager.AppSettings["GITHUB_CLIENT_ID"];
        private static string? CLIENT_SECRET = System.Configuration.ConfigurationManager.AppSettings["GITHUB_CLIENT_SECRET"];

        public PerfilGitHub(int? idPerfilSharp)
        {
            if (idPerfilSharp != null)
            {
                IdPerfilSharp = (int) idPerfilSharp;
                BuscarTodasAsInfomacoes();
            }
        }

        private void BuscarTodasAsInfomacoes()
        {
            BuscarInfoDoBanco();
            BuscarInfoDaFonte();
        }

        public void BuscarInfoDaFonte()
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

        public void BuscarInfoDoBanco()
        {
            try
            {
                // TODO: arrumar -> login é efetuado e isso aqui roda duas vezes
                NpgsqlParameter paramId = new NpgsqlParameter("@id", IdPerfilSharp);
                paramId.DbType = System.Data.DbType.Int32;
                string comando = string.Concat("SELECT id_github FROM ", Usuario.nomeDaTabela, " WHERE id = @id");

                Conexao conexao = Conexao.instancia;
                Id = conexao.ExecutarUnico(comando, [paramId], true, Conexao.ExtrairString);
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
                NpgsqlParameter paramId = new NpgsqlParameter("@id", IdPerfilSharp);
                NpgsqlParameter paramIdGitHub = new NpgsqlParameter("@id_github", id);
                string comando = string.Concat("UPDATE ", Usuario.nomeDaTabela, " SET id_github = @id_github WHERE id = @id");

                Conexao conexao = Conexao.instancia;
                conexao.ExecutarUnico<string>(comando, [paramId, paramIdGitHub], false, default);

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

            catch(Exception err)
            {
                Console.WriteLine(err.Message);
                return null;
            }
        }

        public static async Task<string?> BuscarIdELogar(string codigoDoUsuario)
        {
            try
            {
                string? tokenDeAcesso = await BuscarTokenAutenticacaoDaFonte(codigoDoUsuario);
                if (tokenDeAcesso == null)
                {
                    throw new Exception("Login mal sucedido. Tente novamente.");
                }

                string? idInfoGitHub = BuscarIdDaFonte(tokenDeAcesso);
                return idInfoGitHub;
            }

            catch (Exception err)
            {
                Console.WriteLine(err);
                return null;
            }
        }
    }
}
