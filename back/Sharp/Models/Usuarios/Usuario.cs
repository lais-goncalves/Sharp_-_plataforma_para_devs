using System.Text.Json.Serialization;
using Sharp.Models.Bancos.Tabelas;
using Npgsql;

namespace Sharp.Models.Usuarios
{
    public class Usuario
    {
        #region Propriedades
        public static TabelaComTipo<Usuario> Tabela => new ("usuario");

        // Propriedades que não são enviadas ao front
        public int? Id { get; set; }
        [JsonIgnore]
        public string? Email { get; set; }
        [JsonIgnore]
        public string? Senha { get; set; }
        public int? Seguidores { get; set; }
        public int? Seguindo { get; set; }
        public string? Localizacao { get; set; }

        // Propriedades que são enviadas ao front
        [Newtonsoft.Json.JsonProperty("nome_completo")]
        public string? NomeCompleto { get; set; }
        public string? Apelido { get; set; }
        [Newtonsoft.Json.JsonProperty("tipo_perfil")]
        public string? TipoPerfil { get; set; }
        #endregion Propriedades


        #region Construtores
        public Usuario(int? id, string? nomeCompleto, string? email, string? apelido, string? senha, string? tipoPerfil, int? seguidores, int? seguindo, string? localizacao)
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
            // TODO: transformar em function
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

            Usuario? usuario = Tabela.conexao.ExecutarFunctionUnica<Usuario>(nomeFunction, [paramEmailOuApelido, paramSenha]);
            return usuario;
        }

        public static bool Cadastrar(string email, string nome_completo, string apelido, string senha, string tipoPerfil)
        {
            try
            {
                NpgsqlParameter paramNomeCompleto = new("@param_nome_completo", nome_completo);
                NpgsqlParameter paramEmail = new("@param_email", email);
                NpgsqlParameter paramApelido = new("@param_apelido", apelido);
                NpgsqlParameter paramSenha = new("@param_senha", senha);
                NpgsqlParameter paramTipoPerfil = new("@param_nome_tipo_perfil", tipoPerfil);
                string procedure = string.Concat("cadastrar_usuario");

                Tabela.conexao.ExecutarProcedure(procedure, [paramNomeCompleto,  paramEmail, paramApelido, paramSenha, paramTipoPerfil]);

                return true;
            }

            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return false;
            }
        }

        public static Usuario? BuscarPorId(string id)
        {
            bool podeConverter = int.TryParse(id, null, out int idInt);
            if (podeConverter)
            {
                NpgsqlParameter paramId = new NpgsqlParameter("@param_id", idInt);
                string function = string.Concat("buscar_usuario");
                Usuario? resultado = Tabela.conexao.ExecutarFunctionUnica<Usuario>(function, [paramId]);

                return resultado;
            }

            return null;
        }

        public static List<Usuario?>? BuscarTodos()
        {
            return Tabela.BuscarTodos();
        }
        #endregion Métodos
    }
}
