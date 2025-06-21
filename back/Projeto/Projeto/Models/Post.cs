using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Projeto.Dados;

namespace Projeto.Models
{
    public class Post : ITabela<Post>
    {
        #region Propriedades
        public static Conexao conexao { get; } = Conexao.instancia;
        public static string nomeDaTabela { get; } = "Post";
        public string? Id { get; set; }
        public string? Tipo { get; set; }
        public string? Titulo { get; set; }
        public string? Texto { get; set; }
        public int? PostadoPor { get; set; }
        public DateTime? PostadoEm { get; set; }
        #endregion Propriedades


        #region Construtores
        [JsonConstructor]
        public Post(
            string? id,
            string? tipo,
            string? titulo,
            string? texto,
            int? postado_por,
            DateTime? postado_em
        ) : base()
        {
            Id = id;
            Tipo = tipo;
            Titulo = titulo;
            Texto = texto;
            PostadoPor = postado_por;
            PostadoEm = postado_em;
        }
        #endregion Construtores


        #region Métodos
        public static Post? extrairObjetoDoReader(SqlDataReader reader)
        {
            string? id = reader["id"]?.ToString();
            string? tipo = reader["tipo"]?.ToString();
            string? titulo = reader["titulo"]?.ToString();
            string? texto = reader["texto"]?.ToString();
            int postado_por = 0;
            DateTime postado_em = default;

            try
            {
                string? _postado_por = reader["postado_por"].ToString();

                if (_postado_por != null)
                {
                    postado_por = int.Parse(_postado_por);
                }
            }

            catch(Exception) 
            {
                return null;
            }

            try
            {
                string? _postado_em = reader["postado_em"].ToString();

                if (_postado_em != null)
                {
                    postado_em = DateTime.Parse(_postado_em);
                }
            }

            catch (Exception) {
                return null;
            }

            Post? post = new Post(id, tipo, titulo, texto, postado_por, postado_em);

            return post;
        }

        public static Post? BuscarPorId(int id)
        {
            return ITabela<Post>.buscarPorId(id);
        }

        public static List<Post?>? BuscarTodos()
        {
            return ITabela<Post>.buscarTodos();
        }

        public string? Postar(Usuario usuarioLogado)
        {
            return Postar(Tipo, Texto, Titulo, usuarioLogado);
        }

        public static string? Postar(string? tipo, string? texto, string? titulo, Usuario? usuarioLogado)
        {
            if (usuarioLogado == null || usuarioLogado?.Id == null)
            {
                throw new Exception("O usuário deve estar logado para poder postar.");
            }

            if (tipo == null || titulo == null)
            {
                throw new Exception("As propriedades 'tipo' e 'título' devem estar preenchidas para poder postar.");
            }

            try
            {
                SqlParameter _tipo = new SqlParameter("@p_tipo", tipo);
                SqlParameter _texto = new SqlParameter("@p_texto", texto);
                SqlParameter _titulo = new SqlParameter("@p_titulo", titulo);
                SqlParameter _postado_por = new SqlParameter("@p_postado_por", usuarioLogado.Id);
                List<SqlParameter> parametros = [_tipo, _texto, _titulo, _postado_por];

                string comando = string.Concat("POSTAR");
                string? resultado = conexao.ExecutarProcedure(comando, parametros, true);

                return resultado;
            }

            catch(Exception err)
            {
                Console.WriteLine(err.Message);
                return null;
            }
        }
        #endregion Métodos
    }
}
