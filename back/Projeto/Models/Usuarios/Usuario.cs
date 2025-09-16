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
        [Newtonsoft.Json.JsonProperty("tipo_perfil")]
        public string? TipoPerfil { get; set; }
        #endregion Propriedades


        #region Construtores
        public Usuario(int? id, string? nomeCompleto, string? email, string? apelido, string? senha, string? tipoPerfil)
        {
            Id = id;
            NomeCompleto = nomeCompleto;
            Email = email;
            Apelido = apelido;
            Senha = senha;
            TipoPerfil = tipoPerfil;
        }

        public Usuario() { }
        #endregion Construtores


        #region Métodos
        public static Usuario? BuscarPorApelido(string apelido)
        {
            NpgsqlParameter paramApelido = new("@apelido", apelido);
            string comando = string.Concat("SELECT * FROM ", Tabela.NomeTabela, " WHERE apelido = @apelido");

            Usuario? usuario = Tabela.conexao.ExecutarUnico<Usuario>(comando, [paramApelido]);
            return usuario;
        }

        public static Usuario? VerificarLogin(string emailOuApelido, string senha)
        {
            NpgsqlParameter paramEmailOuApelido = new("@param_email_apelido", emailOuApelido);
            NpgsqlParameter paramSenha = new("@param_senha", senha);
            string nomeFunction = string.Concat("logar_usuario");

            Usuario? usuario = Tabela.ExecutarFunctionUnica<Usuario>(nomeFunction, [paramEmailOuApelido, paramSenha]);
            return usuario;
        }

        public static Usuario? BuscarPorId(string id)
        {
            NpgsqlParameter paramId = new NpgsqlParameter("@param_id", id);
            string function = string.Concat("buscar_usuario");

            Usuario? resultado = Tabela.ExecutarFunctionUnica<Usuario>(function, [paramId]);
            return resultado;
        }

        public static List<Usuario?>? BuscarTodos()
        {
            return Tabela.BuscarTodos();
        }
        #endregion Métodos
    }
}
