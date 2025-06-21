using System.Data;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.Data.SqlClient;
using Projeto.Models;

namespace Projeto.Dados
{
    public interface ITabela<T> where T : ITabela<T>
    {
        static virtual Conexao conexao { get; }
        public virtual static string nomeDaTabela { get; }


        public static abstract T? extrairObjetoDoReader(SqlDataReader reader);

        public static T? buscarPorId(string id)
        {
            try
            {
                SqlParameter paramId = new("@id", id);
                string query = string.Concat("SELECT * FROM ", T.nomeDaTabela, " WHERE id = @id");

                T? post = T.conexao.ExecutarUnico(query, [paramId], true, T.extrairObjetoDoReader);

                return post;
            }

            catch (Exception err) 
            {
                Console.WriteLine(err.Message);
                return default;
            }
        }

        public static T? buscarPorId(int id)
        {
            string idStr = id.ToString();
            return buscarPorId(idStr);
        }

        public static List<T?>? buscarTodos()
        {
            try
            {
                string query = string.Concat("SELECT * FROM ", T.nomeDaTabela);
                var resultado = T.conexao.Executar<T>(query, null, true, T.extrairObjetoDoReader);

                return resultado;
            }

            catch (Exception err) {
                Console.WriteLine(err.Message);
                return null;
            }
        }

        //protected static string? inserir();

        //protected abstract bool excluir<P>();
    }
}
