using System.Data;
using Microsoft.Data.SqlClient;

namespace Projeto.Dados
{
    public class Conexao
    {
        #region Propriedades
        private static readonly string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public static readonly Conexao instancia = new Conexao();
        #endregion Propriedades

        #region Construtores
        private Conexao() { }
        #endregion Construtores


        #region Métodos
        public delegate T? ExtrairDados<T>(SqlDataReader reader);
        private void criarListaDeParametros(SqlCommand cmd, List<SqlParameter>? parametros)
        {
            if (parametros is null)
            {
                return;
            }

            foreach (SqlParameter param in parametros)
            {
                cmd.Parameters.Add(param);
            }
        }

        private void fecharConexao(SqlCommand? cmd, SqlConnection? conn)
        {
            conn?.Close();
            conn?.Dispose();
            cmd?.Dispose();
            cmd?.Connection.Close();
        }

        private string? _executarProcedure(string procedure, List<SqlParameter>? parametros, bool temOutput, SqlDbType tipoOutput)
        {
            SqlConnection? conn = null;
            SqlCommand? cmd = null;
            string? resultado = null;

            try
            {
                using (conn = new SqlConnection() { ConnectionString = connectionString })
                {
                    conn.Open();
                    cmd = conn.CreateCommand();
                    cmd.CommandText = procedure;

                    criarListaDeParametros(cmd, parametros);
                    SqlParameter? parametroOutput;

                    if (temOutput)
                    {
                        parametroOutput = new SqlParameter("@retorno", "");
                        parametroOutput.SqlDbType = tipoOutput;
                        parametroOutput.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(parametroOutput);
                    }

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();

                    if (temOutput)
                    {
                        resultado = cmd.Parameters["@retorno"].Value.ToString();
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

        private List<T?>? _executarComando<T>(string comando, List<SqlParameter>? parametros, bool buscarUnico, bool retornarResultados, ExtrairDados<T>? extrairObjetoDoReader)
        {
            SqlConnection? conn = null;
            SqlCommand? cmd = null;
            SqlDataReader? reader = null;
            List<T?>? resultado = null;

            try
            {
                if (retornarResultados && extrairObjetoDoReader == null)
                {
                    throw new Exception("Para retornar resultados, é necessário definir um método de extração.");
                }

                using (conn = new SqlConnection() { ConnectionString = connectionString })
                {
                    conn.Open();
                    cmd = conn.CreateCommand();
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

        public string? ExecutarProcedure(string comando, List<SqlParameter>? parametros = null, bool temOutput = true, SqlDbType tipoOutput = SqlDbType.NVarChar)
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

        public List<T?>? Executar<T>(string comando, List<SqlParameter>? parametros = null, bool retornarResultados = false, ExtrairDados<T>? extrairObjetoDoReader = null)
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

        public T? ExecutarUnico<T>(string comando, List<SqlParameter>? parametros = null, bool retornarResultados = false, ExtrairDados<T>? extrairObjetoDoReader = null)
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

        public static string? ExtrairString(SqlDataReader reader)
        {
            return reader[0]?.ToString();
        }


        #endregion Métodos
    }
}
