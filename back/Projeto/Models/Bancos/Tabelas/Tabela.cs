using System.Linq;
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
            return conexao.Executar<T>(query, parametros);
        }

        public T? BuscarUnico<T>(string query, List<NpgsqlParameter>? parametros = null)
        {
            return conexao.ExecutarUnico<T>(query, parametros);
        }

        public List<T?>? Executar<T>(string query, List<NpgsqlParameter>? parametros = null)
        {
            return conexao.Executar<T>(query, parametros, false);
        }

        public List<T?>? ExecutarFunction<T>(string nomeFunction, List<NpgsqlParameter>? parametros = null)
        {
            string nomesParametros = "";
            if (parametros is not null)
            {
                List<string> listaNomesParametros = parametros.Select(p => p.ParameterName).ToList();
                nomesParametros = string.Join(", ", listaNomesParametros);
            }

            string novaQuery = string.Concat("SELECT * FROM ", nomeFunction, "(", nomesParametros, ")");

            List<T?>? resultado = conexao.Executar<T>(novaQuery, parametros, false);
            return resultado;
        }

        public T? ExecutarFunctionUnica<T>(string nomeFunction, List<NpgsqlParameter>? parametros = null)
        {
            List<T?>? resultado = ExecutarFunction<T>(nomeFunction, parametros);
            if (resultado is null)
            {
                return default;
            }

            T? resultadoUnico = resultado.FirstOrDefault();
            return resultadoUnico;
        }
    }
}
