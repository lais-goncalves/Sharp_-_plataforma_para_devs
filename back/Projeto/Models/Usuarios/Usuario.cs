using System.Data;
using System.Text.Json.Serialization;
using Microsoft.Data.SqlClient;
using Npgsql;
using Projeto.Dados;
using Projeto.Models.Bancos;

namespace Projeto.Models.Usuarios
{
    public class Usuario
    {
        #region Propriedades
        public static TabelaBanco<Usuario> tabela = ConfigBanco.BuscarTabela<Usuario>("usuario");

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
            string comando = string.Concat("SELECT * FROM ", tabela.NomeTabela, " WHERE apelido = @apelido");

            Usuario? usuario = tabela.conexao.ExecutarUnico<Usuario>(comando, [paramApelido], true);
            return usuario;
        }

        public static Usuario? BuscarPorIdGitHub(string idGitHub)
        {
            NpgsqlParameter paramApelido = new("@id_github", idGitHub);
            string comando = string.Concat("SELECT * FROM ", tabela.NomeTabela, " WHERE id_github = @id_github");

            Usuario? usuario = tabela.conexao.ExecutarUnico<Usuario>(comando, [paramApelido], true);
            return usuario;
        }

        public static Usuario? BuscarPorId(string id)
        {
            return tabela.buscarPorId(id);
        }

        public static List<Usuario?>? BuscarTodos()
        {
            return tabela.buscarTodos();
        }
        #endregion Métodos
    }
}
