using System.Data;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using Projeto.Models;
using Projeto.Config;
using System.Reflection;
using System.Dynamic;
using System.ComponentModel;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Projeto.Banco
{
    public abstract class Tabela<TItem> where TItem: Tabela<TItem>, new()
    {
        #region Propriedades
        public static string NomeDaTabela { get => PegarNomeDaTabela(); }
        public static Conexao<TItem> Conexao { get; protected set; } = new Conexao<TItem>();
        public static ProjetoDbContext Entidades { get; protected set; } = new ProjetoDbContext();
        #endregion Propriedades


        #region Construtores
        protected Tabela() { }
        #endregion Construtores


        #region Métodos
        protected static object? PegarAtributo<A>() where A : Attribute
        {
            Type tipo = typeof(TItem);
            object[] atributos = tipo.GetCustomAttributes(typeof(A), true);

            if (atributos.Length > 0)
            {
                return atributos[0];
            }

            else
            {
                return default;
            }
        }

        private static string PegarNomeDaTabela()
        {
            object? atributo = PegarAtributo<NomeDaTabela>();

            if (atributo == null)
            {
                throw new Exception("A tabela deve conter o atributo NomeDaTabela");
            }

            try
            {
                return ((NomeDaTabela)atributo).Nome;
            }

            catch (Exception)
            {
                throw;
            }
        }

        private static ParameterInfo[] PegarParametrosDoConstrutor()
        {
            ParameterInfo[] listaParametros = Array.Empty<ParameterInfo>();

            try
            {
                ConstructorInfo[] construtores = typeof(TItem).GetConstructors();

                if (construtores.Length <= 0)
                {
                    return listaParametros;
                }


                ConstructorInfo? construtor = null;

                foreach (ConstructorInfo infoConstrutor in construtores)
                {
                    object? atributoJsonConstrutor = infoConstrutor.GetCustomAttribute(typeof(Newtonsoft.Json.JsonConstructorAttribute), true);

                    if (atributoJsonConstrutor == null)
                    {
                        continue;
                    }

                    construtor = infoConstrutor;
                    break;
                }

                if (construtor == null)
                {
                    construtor = construtores[0];
                }


                listaParametros = construtor.GetParameters();
            }

            catch (Exception err) {
                Debug.WriteLine(err.Message);
            }

            return listaParametros;
        }

        public static TItem? ExtrairObjetoDaLinha(DataRow linha)
        {
            if (linha == null)
            {
                return null;
            }

            ParameterInfo[] parametros = PegarParametrosDoConstrutor();
            Dictionary<string, object> dicionario = new ();
            TItem? resultado = null;

            foreach (ParameterInfo param in parametros)
            {
                string? nomeDoParametro = param.Name;

                if (nomeDoParametro == null)
                {
                    continue;
                }

                bool existeColunaComMesmoNome = linha.Table.Columns.Contains(nomeDoParametro);

                if (!existeColunaComMesmoNome)
                {
                    continue;
                }

                Type tipoDoParametro = param.ParameterType;
                string? valorDoParametroStr = linha[nomeDoParametro].ToString();

                if (string.IsNullOrEmpty(valorDoParametroStr))
                {
                    dicionario[nomeDoParametro] = null;
                }

                else if (tipoDoParametro == typeof(DateTime))
                {
                    DateTime novaData = default;
                    bool conseguiuConverter = DateTime.TryParse(valorDoParametroStr, out novaData);

                    dicionario[nomeDoParametro] = conseguiuConverter ? novaData : new DateTime();
                }

                else
                {
                    dicionario[nomeDoParametro] = valorDoParametroStr;
                }
            }

            string dicionarioSerializado = JsonConvert.SerializeObject(dicionario);
            resultado = JsonConvert.DeserializeObject<TItem>(dicionarioSerializado);

            return resultado;
        }

        public static Resultado<List<TItem>?> BuscarTodos()
        {
            Resultado<List<TItem>?> resultado = new();

            try
            {
                string query = string.Concat("SELECT * FROM ", NomeDaTabela);

                Resultado<List<TItem>?> resultadoBusca = Conexao.Executar<TItem>(query, null);

                return resultadoBusca;
            }

            catch (Exception err)
            {
                resultado.Erro = err;
            }

            return resultado;
        }

        private static Resultado<TItem?> BaseBuscarPorId(string id)
        {
            Resultado<TItem?> resultado = new();

            try
            {
                SqlParameter paramId = new("@p_param_id", id);
                string query = string.Concat("SELECT * FROM ", NomeDaTabela, " WHERE id = ", paramId.ParameterName);

                Resultado<TItem?> resultadoBusca = Conexao.ExecutarUnico<TItem>(query, [paramId]);

                return resultadoBusca;
            }

            catch (Exception err)
            {
                resultado.Erro = err;
            }

            return resultado;
        }

        public static Resultado<TItem?> BuscarPorId(int id)
        {
            return BaseBuscarPorId(id.ToString());
        }

        public static Resultado<TItem?> BuscarPorId(string id)
        {
            return BaseBuscarPorId(id);
        }
        #endregion Métodos
    }
}
