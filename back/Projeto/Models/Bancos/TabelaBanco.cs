using Npgsql;
using Projeto.Models.Usuarios;

namespace Projeto.Models.Bancos
{
    // Essa interface serve para garantir que a ConfigBanco consiga usar o dictionary com tabelas de diferentes tipos (simplesmente usar TabelaBanco<T> não funcionaria)
    public interface ITabelaBanco
    {
        Conexao conexao { get; set; }
        string NomeTabela { get; set; }
    }

    public class TabelaBanco<T>(string nomeTabela) : ITabelaBanco where T : class
    {
        public Conexao conexao { get; set; } = Conexao.instancia;
        public string NomeTabela { get; set; } = nomeTabela;

        public void Teste()
        {
            conexao.Executar<T>("SELECT * FROM Usuario", null, true);
        }

        public T? buscarPorId(int id)
        {
            try
            {
                NpgsqlParameter paramId = new("@id", id);
                paramId.DbType = System.Data.DbType.Int32;
                string query = string.Concat("SELECT * FROM ", NomeTabela, " WHERE id = @id");

                T? post = conexao.ExecutarUnico<T>(query, [paramId], true);

                return post;
            }

            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return default;
            }
        }

        public T? buscarPorId(string id)
        {
            try
            {
                int idInt = int.Parse(id);
                return buscarPorId(idInt);
            }

            catch (Exception)
            {
                return default;
            }

        }

        public List<T?>? buscarTodos()
        {
            try
            {
                string query = string.Concat("SELECT * FROM ", NomeTabela);
                var resultado = conexao.Executar<T>(query, null, true);

                return resultado;
            }

            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return null;
            }
        }
    }
}
