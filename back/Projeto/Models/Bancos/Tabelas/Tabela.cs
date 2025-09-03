using Npgsql;
using Projeto.Models.Usuarios;

namespace Projeto.Models.Bancos.Tabelas
{
    public class Tabela(string nomeTabela)
    {
        public Conexao conexao { get; set; } = Conexao.instancia;
        public string NomeTabela { get; set; } = nomeTabela;

        public List<T?>? Buscar<T>(string query, List<NpgsqlParameter>? parametros = null)
        {
            return conexao.Executar<T>(query, parametros, true);
        }

        public T? BuscarUnico<T>(string query, List<NpgsqlParameter>? parametros = null)
        {
            return conexao.ExecutarUnico<T>(query, parametros, true);
        }

        public List<T?>? Executar<T>(string query, List<NpgsqlParameter>? parametros = null)
        {
            return conexao.Executar<T>(query, parametros, false);
        }
    }
}
