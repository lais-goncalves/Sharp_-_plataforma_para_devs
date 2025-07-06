using System.Data;
using Npgsql;
using NpgsqlTypes;

namespace Projeto.Dados
{
    public class Conexao
    {
        #region Propriedades
        private static readonly string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DEFAULT_CONNECTION"].ConnectionString;
        public static readonly Conexao instancia = new Conexao();
        #endregion Propriedades

        #region Construtores
        private Conexao() { }
        #endregion Construtores


        #region Métodos
        public delegate T? ExtrairDados<T>(NpgsqlDataReader reader);
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

        private string? _executarProcedure(string procedure, List<NpgsqlParameter>? parametros, bool temOutput, NpgsqlDbType tipoOutput)
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
                    NpgsqlParameter? parametroOutput;

                    if (temOutput)
                    {
                        parametroOutput = new NpgsqlParameter("@retorno", "");
                        parametroOutput.NpgsqlDbType = tipoOutput;
                        parametroOutput.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(parametroOutput);
                    }

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();

                    if (temOutput)
                    {
                        resultado = cmd.Parameters["@retorno"]?.Value?.ToString();
                    }
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

        private List<T?>? _executarComando<T>(string comando, List<NpgsqlParameter>? parametros, bool buscarUnico, bool retornarResultados, ExtrairDados<T>? extrairObjetoDoReader)
        {
            NpgsqlConnection? conn = null;
            NpgsqlCommand? cmd = null;
            NpgsqlDataReader? reader = null;
            List<T?>? resultado = default;

            try
            {
                if (retornarResultados && extrairObjetoDoReader == null)
                {
                    throw new Exception("Para retornar resultados, é necessário definir um método de extração.");
                }

                using (NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString))
                {
                    conn = dataSource.OpenConnection();
                    cmd = new NpgsqlCommand(comando, conn);
                    cmd.CommandText = comando;

                    criarListaDeParametros(cmd, parametros);
                    resultado = [];

                    reader = cmd.ExecuteReader();

                    if (retornarResultados)
                    {
                        while (reader != null && reader.Read())
                        {
                            T? objExtraido = extrairObjetoDoReader(reader);

                            if (objExtraido != null)
                            {
                                resultado.Add(objExtraido);

                                if (buscarUnico)
                                {
                                    reader = null;
                                }
                            }
                        }
                    }

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

        public string? ExecutarProcedure(string comando, List<NpgsqlParameter>? parametros = null, bool temOutput = true, NpgsqlDbType tipoOutput = NpgsqlDbType.Varchar)
        {
            try
            {
                return _executarProcedure(comando, parametros, temOutput, tipoOutput);
            }

            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return null;
            }
        }

        public List<T?>? Executar<T>(string comando, List<NpgsqlParameter>? parametros = null, bool retornarResultados = false, ExtrairDados<T>? extrairObjetoDoReader = null)
        {
            try
            {
                return _executarComando(comando, parametros, false, retornarResultados, extrairObjetoDoReader);
            }

            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return null;
            }
        }

        public T? ExecutarUnico<T>(string comando, List<NpgsqlParameter>? parametros = null, bool retornarResultados = false, ExtrairDados<T>? extrairObjetoDoReader = null)
        {
            try
            {
                List<T?>? resultado = _executarComando(comando, parametros, true, retornarResultados, extrairObjetoDoReader);

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

        public static string? ExtrairString(NpgsqlDataReader reader)
        {
            return reader.GetString(0);
        }

        public static int? ExtrairInt32(NpgsqlDataReader reader)
        {
            return reader.GetInt32(0);
        }
        #endregion Métodos
    }
}
