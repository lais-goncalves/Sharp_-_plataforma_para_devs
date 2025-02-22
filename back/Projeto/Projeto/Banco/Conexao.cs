using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;

namespace Projeto.Banco
{
    public class Conexao<TItem> where TItem : Tabela<TItem>, new ()
    {
        // TODO: deixar @p_texto com valor default no SQL Server
        #region Propriedades
        private static readonly string strConexao = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        #endregion Propriedades


        #region Métodos
        private static T? ConverterLinhaPara<T>(DataRow linha)
        {
            try
            {
                // 1 - verificar se linha é válida
                // 2 - verificar se o tipo é uma tabela (TItem)
                //      2.1 - se sim: converter para T usando conversor da tabela
                //      2.2 - senão: converter para objeto, e então para T

                T? item = default;
                Type tipo = typeof(T);

                // 1
                if (linha == null || linha.ItemArray.Length <= 0)
                {
                    return item;
                }


                // 2
                if (tipo == typeof(TItem))
                {
                    // 2.1
                    TItem? objetoTabela = Tabela<TItem>.ExtrairObjetoDaLinha(linha);

                    if (objetoTabela != null)
                    {
                        item = (T)(object)objetoTabela;
                    }
                }

                else
                {
                    // 2.2
                    string serializado = JsonConvert.SerializeObject(linha);
                    item = JsonConvert.DeserializeObject<T>(serializado);
                }

                return item;
            }

            catch (Exception err)
            {
                return default;
            }
        }

        public Resultado<List<T?>?> ExecutarComando<T>(string comando, List<SqlParameter>? parametros = null, bool ehProcedure = false)
        {
            // 1 - inicializar variáveis do banco
            // 2 - definir conexão
            // 3 - adicionar parâmetros ao comando
            //      3.1 - verificar se comando recebebparâmetro de retorno (output)
            // 4 - adicionar dados recebidos do comando a uma DataTable
            //      *: procedures e functions/queries têm modos diferentes de...
            //      ... adicionar esses dados à DataTable, por isso verifica se ...
            //      ... é procedure ou não
            // 5 - extrai os dados da DataTable e os converte para o tipo desejado


            // 1
            Resultado<List<T?>?> resultado = new();
            SqlConnection? conn = null;
            SqlCommand? cmd = null;

            try
            {
                // 2
                using (conn = new SqlConnection() { ConnectionString = strConexao })
                {
                    conn.Open();
                    cmd = conn.CreateCommand();
                    SqlParameter? parametroOutput = null;
                    cmd.CommandText = comando;


                    // 3
                    if (parametros is not null)
                    {
                        foreach (SqlParameter param in parametros)
                        {
                            cmd.Parameters.Add(param);

                            if (param.Direction == ParameterDirection.Output)
                            {
                                // 3.1
                                parametroOutput = param;
                                break;
                            }
                        }
                    }


                    // 4
                    DataRowCollection? linhas = null;
                    DataTable resultados = new();


                    // 4 *
                    if (ehProcedure)
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataReader dataReader = cmd.ExecuteReader();
                        resultados.Load(dataReader);
                    }

                    else
                    {
                        SqlDataAdapter adaptador = new(cmd);
                        adaptador.Fill(resultados);
                    }


                    // 5
                    linhas = resultados.Rows;

                    if (linhas is null || linhas.Count <= 0)
                    {
                        resultado.Item = null;
                    }

                    else
                    {
                        resultado.Item = [];

                        foreach (DataRow linha in linhas)
                        {
                            T? item = ConverterLinhaPara<T?>(linha);

                            if (item != null)
                            {
                                resultado.Item.Add(item);
                            }
                        }
                    }
                }
            }

            catch (Exception err)
            {
                resultado.Erro = err;
            }

            finally
            {
                conn?.Close();
                conn?.Dispose();
                cmd?.Dispose();
            }

            return resultado;
        }

        public Resultado<List<T?>?> Executar<T>(string query, List<SqlParameter>? parametros = null, bool ehProcedure = false)
        {
            Resultado<List<T?>?> resultado = new();

            try
            {
                resultado = ExecutarComando<T?>(query, parametros, ehProcedure);
            }

            catch (Exception err) {
                resultado.Erro = err;
            }

            return resultado;
        }

        public Resultado<T?> ExecutarUnico<T>(string query, List<SqlParameter>? parametros = null, bool ehProcedure = false)
        {
            Resultado<T?> resultadoUnico = new();

            try
            {
                Resultado<List<T?>?> resultadoComando = ExecutarComando<T>(query, parametros, ehProcedure);
                T? itemDoResultado = resultadoComando.Item?.Count > 0 ? resultadoComando.Item[0] : default;

                resultadoUnico = new Resultado<T?>((T?)(itemDoResultado), resultadoComando.Erro);
            }

            catch (Exception err) {
                resultadoUnico.Erro = err;
            }

            return resultadoUnico;
        }
        #endregion Métodos
    }
}
