using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Projeto.Banco;

namespace Projeto.Models
{
    [NomeDaTabela("Post")]
    public class Post : Tabela<Post>
    {
        #region Propriedades
        [System.Text.Json.Serialization.JsonIgnore, Key]
        public string Id { get; set; }
        public string? Tipo { get; set; }
        public string? Titulo { get; set; }
        public string Texto { get; set; }
        public int PostadoPor { get; set; }
        public DateTime PostadoEm { get; set; }
        #endregion Propriedades


        #region Construtores
        [JsonConstructor]
        public Post(
            string id, 
            string tipo, 
            string titulo, 
            string texto, 
            int postado_por, 
            DateTime postado_em
        ) : base()
        {
            Id = id;
            Tipo = tipo;
            Titulo = titulo;
            Texto = texto;
            PostadoPor = postado_por;
            PostadoEm = postado_em;
        }

        public Post(
            string tipo,
            string titulo,
            string texto,
            int postado_por,
            DateTime postado_em
        ) : base()
        {
            Tipo = tipo;
            Titulo = titulo;
            Texto = texto;
            PostadoPor = postado_por;
            PostadoEm = postado_em;
        }

        public Post() : base() { }
        #endregion Construtores


        #region Métodos
        public static bool JaExiste(int id)
        {
            try
            {
                Resultado<Post?> resultadoBusca = BuscarPorId(id);

                if (resultadoBusca.Erro != null)
                {
                    return true;
                }

                Post? postEncontrado = resultadoBusca.Item;

                return postEncontrado != null && !string.IsNullOrEmpty(postEncontrado.Id);
            }

            catch (Exception)
            {
                return true;
            }
        }

        public Resultado<string?> Postar(Usuario usuarioLogado)
        {
            try
            {
                // 1 - verificar se usuário está logado
                // 2 - verificar se parâmetros estão de acordo com o banco
                // 3 - postar no banco


                // 1
                if (usuarioLogado == null)
                {
                    throw new Exception("Você deve estar logado para poder postar.");
                }


                // 2
                if (string.IsNullOrEmpty(Tipo) || Titulo == null)
                {
                    throw new Exception("Os campos 'Tipo' e 'Titulo' não podem estar vazios.");
                }


                // 3
                SqlParameter paramTipo = new SqlParameter("@p_tipo", Tipo);
                SqlParameter paramTitulo = new SqlParameter("@p_titulo", Titulo);
                SqlParameter paramTexto = new SqlParameter("@p_texto", Texto ?? "");
                SqlParameter paramPostadoPor = new SqlParameter("@p_postado_por", usuarioLogado.Id);
                SqlParameter paramRetorno = new SqlParameter("@retorno", System.Data.SqlDbType.Int);
                paramRetorno.Direction = System.Data.ParameterDirection.Output;

                string procedure = "POSTAR";

                Resultado<string?> resultadoPost = Conexao.ExecutarUnico<string>(
                    procedure,
                    [paramTipo, paramTitulo, paramTexto, paramPostadoPor, paramRetorno], 
                    true
                );

                if (resultadoPost.Erro != null)
                {
                    return resultadoPost;
                }

                resultadoPost.Item = paramRetorno.SqlValue.ToString();
                return resultadoPost;
            }

            catch (Exception err)  
            {
                Resultado<string?> resultado = new();
                resultado.Erro = err;
                return resultado;
            }
        }
        #endregion Métodos
    }
}
