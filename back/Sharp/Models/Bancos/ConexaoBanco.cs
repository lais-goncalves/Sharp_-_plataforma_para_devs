using System.Data;
using Newtonsoft.Json;
using Npgsql;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace Sharp.Models.Bancos;

public class ConexaoBanco
{
	#region Construtores
	public ConexaoBanco() { }
	#endregion Construtores


	#region Propriedades
	private readonly string _connectionString = buscarConnectionString();
	#endregion Propriedades


	#region Métodos
	#region Utils
	private static string buscarConnectionString()
	{
		return ConfigurationManager.ConnectionStrings["DEFAULT_CONNECTION"].ConnectionString;
	}

	private void criarListaDeParametros(NpgsqlCommand cmd, List<NpgsqlParameter>? parametros)
	{
		if (parametros is null) return;

		foreach (var param in parametros) cmd.Parameters.Add(param);
	}

	private void fecharConexao(NpgsqlCommand? cmd, NpgsqlConnection? conn)
	{
		conn?.Close();
		conn?.Dispose();
		cmd?.Dispose();
		cmd?.Connection?.Close();
	}
	#endregion Utils


	private void configurarComando(NpgsqlCommand cmd, string comando, List<NpgsqlParameter>? parametros,
	                               CommandType tipoComando = CommandType.StoredProcedure)
	{
		cmd.CommandText = comando;
		cmd.CommandType = tipoComando;
		criarListaDeParametros(cmd, parametros);
	}

	private T? executarComandoGenerico<T>(string comando, List<NpgsqlParameter>? parametros, CommandType tipoComando,
	                                      Func<NpgsqlCommand, T?> executar)
	{
		NpgsqlConnection? conn = null;
		NpgsqlCommand? cmd = null;

		try
		{
			using var dataSource = NpgsqlDataSource.Create(_connectionString);

			conn = dataSource.OpenConnection();
			cmd = new NpgsqlCommand(comando, conn);
			configurarComando(cmd, comando, parametros, tipoComando);

			return executar(cmd);
		}
		catch (Exception err)
		{
			Console.WriteLine(err);
			return default;
		}
		finally
		{
			fecharConexao(cmd, conn);
		}
	}

	public void ExecutarProcedure(string comando, List<NpgsqlParameter>? parametros)
	{
		List<object>? MetodoProcedure(NpgsqlCommand cmd)
		{
			cmd.ExecuteNonQuery();
			return null;
		}

		executarComandoGenerico<object>(comando, parametros, CommandType.StoredProcedure, MetodoProcedure);
	}

	public List<T>? ExecutarFunction<T>(string comando, List<NpgsqlParameter>? parametros)
	{
		// TODO: quebrar em métodos menores
		List<T>? MetodoFunction(NpgsqlCommand cmd)
		{
			List<T>? resultado = null;

			NpgsqlDataReader reader = cmd.ExecuteReader();
				
			DataTable dataTable = new();
			using (var adapter = new NpgsqlDataAdapter(cmd))
			{
				adapter.Fill(dataTable);
			}

			string objSerializadoParaJson = JsonConvert.SerializeObject(dataTable);
			resultado = JsonConvert.DeserializeObject<List<T>?>(objSerializadoParaJson);
			
			reader.Close();
			return resultado;
		}
		
		string nomesParametros = "";
		if (parametros is not null)
		{
			List<string> listaNomesParametros = parametros.Select(p => p.ParameterName).ToList();
			nomesParametros = string.Join(", ", listaNomesParametros);
		}

		string novoComando = $"SELECT * FROM {comando}({nomesParametros})";
		
		return executarComandoGenerico(novoComando, parametros, CommandType.Text, MetodoFunction);
	}
	#endregion Métodos
}