using Npgsql;

namespace Sharp.Models.Bancos.Tabelas
{
    public class TabelaComTipo<T> : Tabela
    {
        public TabelaComTipo(string nomeTabela) : base(nomeTabela) { }

        public T? BuscarPorId(int id)
        {
            try
            {
                NpgsqlParameter paramId = new("@id", id);
                paramId.DbType = System.Data.DbType.Int32;
                string query = string.Concat("SELECT * FROM ", NomeTabela, " WHERE id = @id");

                T? post = conexao.ExecutarUnico<T>(query, new List<NpgsqlParameter> { paramId });

                return post;
            }

            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return default;
            }
        }

        public T? BuscarPorId(string id)
        {
            try
            {
                int idInt = int.Parse(id);
                return BuscarPorId(idInt);
            }

            catch (Exception)
            {
                return default;
            }
        }

        public List<T?>? BuscarTodos()
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
