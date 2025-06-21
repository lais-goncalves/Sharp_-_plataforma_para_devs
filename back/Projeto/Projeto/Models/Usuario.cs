using System.Text.Json.Serialization;
using Microsoft.Data.SqlClient;
using Projeto.Dados;

namespace Projeto.Models
{
    public class Usuario : ITabela<Usuario> {
        #region Propriedades
        public static string nomeDaTabela { get; } = "Usuario";
        public static Conexao conexao { get; } = Conexao.instancia;
        [JsonIgnore]
        public int Id { get; set; }
        [JsonIgnore]
        public string Senha { get; set; }
        public string Apelido { get; set; }
        #endregion Propriedades


        #region Construtores
        public Usuario(int id, string apelido, string senha)
        {
            Id = id;
            Apelido = apelido;
            Senha = senha;
        }
        #endregion Construtores


        #region Métodos
        public static Usuario? extrairObjetoDoReader(SqlDataReader reader)
        {
            int id = 0;
            string? apelido = reader["apelido"]?.ToString();
            string? senha = reader["senha"]?.ToString();

            try
            {
                string? _id = reader["id"].ToString();

                if (_id != null)
                {
                    id = int.Parse(_id);
                }
            }

            catch (Exception) {
                return null;
            }

            if (apelido == null || senha == null)
            {
                return null;
            }

            Usuario? usuario = new Usuario(id, apelido, senha);
            return usuario;
        }

        public static Usuario? BuscarPorApelido(string apelido)
        {
            SqlParameter paramApelido = new ("@apelido", apelido);
            string comando = string.Concat("SELECT * FROM ", nomeDaTabela, "WHERE apelido = @apelido");

            Usuario? usuario = conexao.ExecutarUnico(comando, [paramApelido], true, extrairObjetoDoReader);
            return usuario;
        }

        public static List<Usuario?>? BuscarTodos()
        {
            return ITabela<Usuario>.buscarTodos();
        }

        public static Usuario? LoginOk(string apelido, string senha)
        {
            try
            {
                SqlParameter paramApelido = new SqlParameter("@apelido", apelido);
                SqlParameter paramSenha = new SqlParameter("@senha", senha);
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
