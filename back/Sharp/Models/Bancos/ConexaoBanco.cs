using System.Data;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;

namespace Sharp.Models.Bancos
{
    public class ConexaoBanco
    {
        #region Propriedades
        private readonly string connectionString = buscarConnectionString();
        public static readonly ConexaoBanco instancia = new ConexaoBanco();
        #endregion Propriedades


        #region Construtores
        private ConexaoBanco() { }
        #endregion Construtores


        #region Métodos
        #region Utils
        private static string buscarConnectionString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["DEFAULT_CONNECTION"].ConnectionString;
        }

        private void criarListaDeParametros(NpgsqlCommand cmd, List<NpgsqlParameter>? parametros)
        {
            if (parametros is null)
            {
                return;
            }

            foreach (NpgsqlParameter param in parametros)
            {
                cmd.Parameters.Add(param);
            }
        }

        private void fecharConexao(NpgsqlCommand? cmd, NpgsqlConnection? conn)
        {
            conn?.Close();
            conn?.Dispose();
            cmd?.Dispose();
            cmd?.Connection?.Close();
        }

        public T? dataReaderParaJson<T>(NpgsqlDataReader dataReader)
        {
            DataTable dataTable = new();
            dataTable.Load(dataReader);

            var objSerializadoParaJson = JsonConvert.SerializeObject(dataTable);
            T? objDesserializado = JsonConvert.DeserializeObject<T>(objSerializadoParaJson);

            return objDesserializado;
        }

        public static T? BuscarPropriedadeDynamic<T>(dynamic? objDynamic, string nomePropriedadeBuscada)
        {
            T? prop = objDynamic?[nomePropriedadeBuscada];
            return prop;
        }
        #endregion Utils


        #region Procedures
        private string? _executarProcedure(string procedure, List<NpgsqlParameter>? parametros)
        {
            NpgsqlConnection? conn = null;
            NpgsqlCommand? cmd = null;
            string? resultado = null;

            try
            {
                using (NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString))
                {
                    conn = dataSource.OpenConnection();
                    cmd = new NpgsqlCommand(procedure, conn);
                    cmd.CommandText = procedure;

                    criarListaDeParametros(cmd, parametros);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
            }

            catch (Exception err)
            {
                Console.WriteLine(err);
                resultado = null;
            }

            finally
            {
                fecharConexao(cmd, conn);
            }

            return resultado;
        }

        public string? ExecutarProcedure(string comando, List<NpgsqlParameter>? parametros = null)
        {
            try
            {
                return _executarProcedure(comando, parametros);
            }

            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return null;
            }
        }
        #endregion Procedures


        #region Comandos
        private List<T>? _executarComando<T>(string comando, List<NpgsqlParameter>? parametros)
        {
            NpgsqlConnection? conn = null;
            NpgsqlCommand? cmd = null;
            NpgsqlDataReader? reader = null;
            List<T?>? resultado = default;

            try
            {
                using (NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString))
                {
                    conn = dataSource.OpenConnection();
                    cmd = new NpgsqlCommand(comando, conn);
                    cmd.CommandText = comando;

                    criarListaDeParametros(cmd, parametros);

                    reader = cmd.ExecuteReader();
                    resultado = dataReaderParaJson<List<T>?>(reader);

                    reader?.Close();
                }
            }

            catch (Exception err)
            {
                Console.WriteLine(err);
                resultado = null;
            }

            finally
            {
                fecharConexao(cmd, conn);
            }

            return resultado;
        }

        public List<T>? Executar<T>(string comando, List<NpgsqlParameter>? parametros = null, bool retornarRetornoAPIs = false)
        {
            try
            {
                return _executarComando<T>(comando, parametros);
            }

            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return null;
            }
        }

        public T? ExecutarUnico<T>(string comando, List<NpgsqlParameter>? parametros = null)
        {
            try
            {
                // Explicitly specify the type argument for _executarComando<T>
                List<T?>? resultado = _executarComando<T>(comando, parametros);

                if (resultado != null && resultado?.Count > 0)
                {
                    return resultado[0];
                }

                return default;
            }

            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return default;
            }
        }

        public List<T>? Executar<T>(string query, List<NpgsqlParameter>? parametros = null)
        {
            return Executar<T>(query, parametros, false);
        }

        public List<T>? ExecutarFunction<T>(string nomeFunction, List<NpgsqlParameter>? parametros = null)
        {
            string nomesParametros = "";
            if (parametros is not null)
            {
                List<string> listaNomesParametros = parametros.Select(p => p.ParameterName).ToList();
                nomesParametros = string.Join(", ", listaNomesParametros);
            }

            string novaQuery = string.Concat("SELECT * FROM ", nomeFunction, "(", nomesParametros, ")");

            List<T>? resultado = Executar<T>(novaQuery, parametros, false);
            return resultado;
        }

        public T? ExecutarFunctionUnica<T>(string nomeFunction, List<NpgsqlParameter>? parametros = null)
        {
            List<T>? resultado = ExecutarFunction<T>(nomeFunction, parametros);
            if (resultado is null)
            {
                return default;
            }

            T? resultadoUnico = resultado.FirstOrDefault();
            return resultadoUnico;
        }
        #endregion Comandos
        #endregion Métodos
    }
}
