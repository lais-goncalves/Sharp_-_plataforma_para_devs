using System.Text.Json.Serialization;
using Npgsql;
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

        public PerfilGitHub(string? idPerfilSharp)
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
                HttpResponseMessage buscaPerfil = await ClienteHttp.GetAsync(urlBusca);

                if (!buscaPerfil.IsSuccessStatusCode)
                {
                    return;
                }

                string? resultado = buscaPerfil.Content.ReadAsStringAsync().Result;

                Console.WriteLine(resultado);
            }

            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
        }

        public string? BuscarInfoDoBanco(string idUsuario)
        {
            try
            {
                Conexao conexao = Conexao.instancia;

                NpgsqlParameter paramId = new NpgsqlParameter("@id", idUsuario);
                string comando = string.Concat("SELECT id_github FROM ", Usuario.nomeDaTabela, " WHERE id = @id");

                string? codigo = conexao.ExecutarUnico(comando, [paramId], true, Conexao.ExtrairString);

                return codigo;
            }

            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return null;
            }
        }
    }
}
