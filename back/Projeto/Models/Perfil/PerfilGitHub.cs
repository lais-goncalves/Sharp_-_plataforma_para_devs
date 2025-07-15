using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using Npgsql;
using NuGet.Common;
using Projeto.Dados;

namespace Projeto.Models.Perfil
{
    public class PerfilGitHub : IPerfil
    {
        [JsonIgnore]
        public string? Id { get; set; }
        public string? Apelido { get; }
        public string? NomeCompleto { get; }
        public HttpClient ClienteHttp => new HttpClient();
        public static string? UrlSite => "https://api.github.com";

        public PerfilGitHub(int? idPerfilSharp)
        {
            if (idPerfilSharp != null)
            {
                BuscarInfoDoBanco(idPerfilSharp);
                BuscarInfoDaFonte();
            }
        }

        public async void BuscarInfoDaFonte()
        {
            try
            {
                if (Id == null)
                {
                    return;
                }

                string urlBusca = UrlSite + "/user/" + Id;
                HttpRequestMessage buscaPerfil = new(HttpMethod.Get, urlBusca);

                buscaPerfil.Headers.Add("Accept", "application/json");
                buscaPerfil.Headers.UserAgent.ParseAdd("Sharp");

                var retornoInfoUsuario = ClienteHttp.Send(buscaPerfil);

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

        public void BuscarInfoDoBanco(int? idPerfilSharp)
        {
            try
            {
                Conexao conexao = Conexao.instancia;

                NpgsqlParameter paramId = new NpgsqlParameter("@id", idPerfilSharp);
                paramId.DbType = System.Data.DbType.Int32;
                string comando = string.Concat("SELECT id_github FROM ", Usuario.nomeDaTabela, " WHERE id = @id");

                Id = conexao.ExecutarUnico(comando, [paramId], true, Conexao.ExtrairString);
            }

            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
        }
    }
}
