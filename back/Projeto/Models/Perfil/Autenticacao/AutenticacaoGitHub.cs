using System.Collections.Specialized;
using System.Web;

namespace Projeto.Models.Perfil.Autenticacao
{
    public class AutenticacaoGitHub : IAutenticacao
    {
        public static int IdPerfilSharp { get; set; }
        bool IAutenticacao.EstaAutenticado { get; set; }

        protected static HttpClient clienteHttp = new HttpClient();
        public static string? CLIENT_ID = System.Configuration.ConfigurationManager.AppSettings["GITHUB_CLIENT_ID"];
        private static string? CLIENT_SECRET = System.Configuration.ConfigurationManager.AppSettings["GITHUB_CLIENT_SECRET"];

        public AutenticacaoGitHub(int idPerfilSharp)
        {
            IdPerfilSharp = idPerfilSharp;
        }

        bool IAutenticacao.Autenticar()
        {
            throw new NotImplementedException();
        }

        async Task<string?> BuscarTokenAutenticacao(string codigo)
        {
            NameValueCollection? query = HttpUtility.ParseQueryString(string.Empty);
            query["client_id"] = CLIENT_ID;
            query["client_secret"] = CLIENT_SECRET;
            query["code"] = codigo;

            string? queryString = query.ToString();
            string urlToken = "https://github.com/login/oauth/access_token?" + queryString;
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
    }
}
