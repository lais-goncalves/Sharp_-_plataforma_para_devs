using System.Data;
using System.Text.Json.Serialization;
using Microsoft.Data.SqlClient;
using Npgsql;
using Projeto.Dados;

namespace Projeto.Models
{
    public class Usuario : ITabela<Usuario> {
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
        [JsonIgnore]
        public string? IdGitHub { get; set; }
        #endregion Propriedades


        #region Construtores
        public Usuario(int? id, string? nomeCompleto, string? email, string? apelido, string? senha, string? idGitHub = null)
        {
            Id = id;
            NomeCompleto = nomeCompleto;
            Email = email;
            Apelido = apelido;
            Senha = senha;
            IdGitHub = idGitHub;
        }
        #endregion Construtores


        #region Métodos
        public static Usuario? extrairObjetoDoReader(NpgsqlDataReader reader)
        {
            int? id = reader.GetInt32("id");
            string? nomeCompleto = reader.GetString("nome_completo");
            string? email = reader.GetString("email");
            string? apelido = reader.GetString("apelido");
            string? senha = reader.GetString("senha");
            string? idGitHub = reader.GetString("github_id");

            if (id == null || nomeCompleto == null || email == null || apelido == null || senha == null)
            {
                return null;
            }

            Usuario? usuario = new Usuario(id, nomeCompleto, email, apelido, senha, idGitHub);
            return usuario;
        }

        public static Usuario? BuscarPorApelido(string apelido)
        {
            NpgsqlParameter paramApelido = new ("@apelido", apelido);
            string comando = string.Concat("SELECT * FROM ", nomeDaTabela, " WHERE apelido = @apelido");

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

        public static string? BuscarIdGitHub(int? id)
        {
            try
            {
                NpgsqlParameter paramId = new NpgsqlParameter("@id", id);
                string comando = string.Concat("SELECT id_github FROM ", nomeDaTabela, " WHERE id = @id");

                string? codigo = conexao.ExecutarUnico(comando, [paramId], true, Conexao.ExtrairString);
                return codigo;
            }

            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return null;
            }
        }

        public string? BuscarIdGitHub(Usuario usuario)
        {
            if (usuario == null || usuario.Id == null)
            {
                return null;
            }

            return BuscarIdGitHub(usuario.Id);
        }

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

        public bool DefinirIdGitHub(Usuario usuario, string codigo)
        {
            if (usuario == null || usuario.Id == null || codigo == null)
            {
                return false;
            }

            return DefinirIdGitHub(usuario.Id, codigo);
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
