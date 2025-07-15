using System.Data;
using System.Text.Json.Serialization;
using Microsoft.Data.SqlClient;
using Npgsql;
using Projeto.Dados;
using Projeto.Models.Perfil;

namespace Projeto.Models
{
    public class Usuario : ITabela<Usuario>
    {
        #region Propriedades
        public static string nomeDaTabela { get; set; } = "Usuario";
        public static Conexao conexao { get; set; } = Conexao.instancia;

        [JsonIgnore]
        public int? Id { get; set; }
        public string? NomeCompleto { get; set; }
        [JsonIgnore]
        public string? Email { get; set; }
        public string? Apelido { get; set; }
        [JsonIgnore]
        public string? Senha { get; set; }
        public PerfilGitHub? PerfilGitHub { get; set; }
        #endregion Propriedades


        #region Construtores
        public Usuario(int? id, string? nomeCompleto, string? email, string? apelido, string? senha)
        {
            Id = id;
            NomeCompleto = nomeCompleto;
            Email = email;
            Apelido = apelido;
            Senha = senha;

            DefinirPerfis();
        }
        #endregion Construtores


        #region Métodos
        private void DefinirPerfis()
        {
            if (Id == null)
            {
                return;
            }

            PerfilGitHub = new PerfilGitHub(Id);
        }

        public static Usuario? extrairObjetoDoReader(NpgsqlDataReader reader)
        {
            int? id = reader.GetInt32("id");
            string? nomeCompleto = reader.GetString("nome_completo");
            string? email = reader.GetString("email");
            string? apelido = reader.GetString("apelido");
            string? senha = reader.GetString("senha");

            if (id == null || nomeCompleto == null || email == null || apelido == null || senha == null)
            {
                return null;
            }

            Usuario? usuario = new Usuario(id, nomeCompleto, email, apelido, senha);
            return usuario;
        }

        public static Usuario? BuscarPorApelido(string apelido)
        {
            NpgsqlParameter paramApelido = new("@apelido", apelido);
            string comando = string.Concat("SELECT * FROM ", nomeDaTabela, " WHERE apelido = @apelido");

            Usuario? usuario = conexao.ExecutarUnico(comando, [paramApelido], true, extrairObjetoDoReader);
            return usuario;
        }

        public static Usuario? BuscarPorIdGitHub(string idGitHub)
        {
            NpgsqlParameter paramApelido = new("@id_github", idGitHub);
            string comando = string.Concat("SELECT * FROM ", nomeDaTabela, " WHERE id_github = @id_github");

            Usuario? usuario = conexao.ExecutarUnico(comando, [paramApelido], true, extrairObjetoDoReader);
            return usuario;
        }

        public static List<Usuario?>? BuscarTodos()
        {
            return ITabela<Usuario>.buscarTodos();
        }

        public static Usuario? Login(string apelido, string senha)
        {
            try
            {
                NpgsqlParameter paramApelido = new NpgsqlParameter("@apelido", apelido);
                NpgsqlParameter paramSenha = new NpgsqlParameter("@senha", senha);
                string comando = string.Concat("SELECT * FROM ", nomeDaTabela, " WHERE apelido = @apelido AND senha = @senha");

                Usuario? usuario = conexao.ExecutarUnico(comando, [paramApelido, paramSenha], true, extrairObjetoDoReader);
                return usuario;
            }

            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return null;
            }
        }

        // TODO: jogar no perfilgithub
        public static bool DefinirIdGitHub(int? id, string codigo)
        {
            try
            {
                if (codigo == null)
                {
                    throw new Exception("O código não pode estar nulo.");
                }

                NpgsqlParameter paramId = new NpgsqlParameter("@id", id);
                NpgsqlParameter paramIdGitHub = new NpgsqlParameter("@id_github", codigo);
                string comando = string.Concat("UPDATE ", nomeDaTabela, " SET id_github = @id_github WHERE id = @id");

                conexao.ExecutarUnico<string>(comando, [paramId, paramIdGitHub], false, default);

                return true;
            }

            catch (Exception)
            {
                Console.WriteLine("Usuário não encontrado.");
                return false;
            }
        }

        public bool DefinirIdGitHub(string? codigo)
        {
            if (Id == null || codigo == null)
            {
                return false;
            }

            return DefinirIdGitHub(Id, codigo);
        }

        public static bool Registrar(string apelido, string senha)
        {
            //Resultado<bool> resultado = new();

            //try
            //{
            //    Usuario usuario = new Usuario(apelido, senha);

            //    if (JaExiste(apelido))
            //    {
            //        throw new Exception("Este apelido já está sendo usado. Tente outro.");
            //    }

            //    Entidades.Usuario.Add(usuario);
            //    Entidades.SaveChanges();
            //    resultado.Item = true;
            //}

            //catch (Exception err)
            //{
            //    resultado.Erro = err;
            //}

            //return resultado;

            return default;
        }
        #endregion Métodos
    }
}
