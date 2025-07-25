using System.Collections.Specialized;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using System.Web;
using Npgsql;
using NuGet.Common;
using Projeto.Dados;

namespace Projeto.Models.Perfil
{
    public class PerfilGitHub : IPerfil
    {
        [JsonIgnore]
        public int IdPerfilSharp { get; set; }
        [JsonIgnore]
        public string? Id { get; set; }
        public string? Apelido { get; }
        public static string? UrlSite => "https://api.github.com";

        public static string? CLIENT_ID = System.Configuration.ConfigurationManager.AppSettings["GITHUB_CLIENT_ID"];

        private static string? CLIENT_SECRET = System.Configuration.ConfigurationManager.AppSettings["GITHUB_CLIENT_SECRET"];

        public PerfilGitHub(int? idPerfilSharp)
        {
            if (idPerfilSharp != null)
            {
                IdPerfilSharp = (int) idPerfilSharp;
                BuscarInfoDoBanco();
                BuscarInfoDaFonte();
            }
        }

        // TODO: criar método para buscar informações já com o header adicionado

        public void BuscarInfoDaFonte()
        {
            try
            {
                string urlBusca = UrlSite + "/user/" + Id;
                HttpRequestMessage buscaPerfil = new(HttpMethod.Get, urlBusca);

                buscaPerfil.Headers.Add("Accept", "application/json");
                buscaPerfil.Headers.UserAgent.ParseAdd("Sharp");

                var retornoInfoUsuario = IPerfil.ClienteHttp.Send(buscaPerfil);

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
                Conexao conexao = Conexao.instancia;

                NpgsqlParameter paramId = new NpgsqlParameter("@id", IdPerfilSharp);
                paramId.DbType = System.Data.DbType.Int32;
                string comando = string.Concat("SELECT id_github FROM ", Usuario.nomeDaTabela, " WHERE id = @id");

                Id = IPerfil.Conexao.ExecutarUnico(comando, [paramId], true, Conexao.ExtrairString);
            }

            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
        }

        public bool DefinirInfoNoBanco(string idGitHub)
        {
            try
            {
                NpgsqlParameter paramId = new NpgsqlParameter("@id", IdPerfilSharp);
                NpgsqlParameter paramIdGitHub = new NpgsqlParameter("@id_github", idGitHub);
                string comando = string.Concat("UPDATE ", Usuario.nomeDaTabela, " SET id_github = @id_github WHERE id = @id");

                IPerfil.Conexao.ExecutarUnico<string>(comando, [paramId, paramIdGitHub], false, default);

                return true;
            }

            catch (Exception)
            {
                Console.WriteLine("Usuário não encontrado.");
                return false;
            }
        }
    }
}
