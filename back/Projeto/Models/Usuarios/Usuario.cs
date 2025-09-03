using System.Data;
using System.Text.Json.Serialization;
using Microsoft.Data.SqlClient;
using Projeto.Models.Bancos.Tabelas;
using Npgsql;

namespace Projeto.Models.Usuarios
{
    public class Usuario
    {
        #region Propriedades
        public static TabelaComTipo<Usuario> Tabela => new ("usuario");

        // Propriedades que não são enviadas ao front
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public string? Email { get; set; }
        [JsonIgnore]
        public string? Senha { get; set; }

        // Propriedades que são enviadas ao front
        [Newtonsoft.Json.JsonProperty("nome_completo")]
        public string? NomeCompleto { get; set; }
        public string? Apelido { get; set; }
        #endregion Propriedades


        #region Construtores
        public Usuario(int id, string? nomeCompleto, string? email, string? apelido, string? senha)
        {
            Id = id;
            NomeCompleto = nomeCompleto;
            Email = email;
            Apelido = apelido;
            Senha = senha;
        }

        public Usuario() { }
        #endregion Construtores


        #region Métodos
        public static Usuario? BuscarPorApelido(string apelido)
        {
            NpgsqlParameter paramApelido = new("@apelido", apelido);
            string comando = string.Concat("SELECT * FROM ", Tabela.NomeTabela, " WHERE apelido = @apelido");

            Usuario? usuario = Tabela.conexao.ExecutarUnico<Usuario>(comando, [paramApelido], true);
            return usuario;
        }

        public static Usuario? BuscarPorIdGitHub(string idGitHub)
        {
            NpgsqlParameter paramApelido = new("@id_github", idGitHub);
            string comando = string.Concat("SELECT * FROM ", Tabela.NomeTabela, " WHERE id_github = @id_github");

            Usuario? usuario = Tabela.conexao.ExecutarUnico<Usuario>(comando, [paramApelido], true);
            return usuario;
        }

        public static Usuario? BuscarPorId(string id)
        {
            return Tabela.BuscarPorId(id);
        }

        public static List<Usuario?>? BuscarTodos()
        {
            return Tabela.BuscarTodos();
        }
        #endregion Métodos
    }
}
