using Newtonsoft.Json;
using Npgsql;
using Sharp.Models.Bancos;

namespace Sharp.Models.Usuarios;

public class Usuario
{
	#region Propriedades
	[JsonIgnore] private static ConexaoBanco conexaoBanco => new ConexaoBanco();
	
	[JsonProperty("id")] public int? Id { get; set; }
	[JsonProperty("nome_completo")] public string? NomeCompleto { get; set; }
	public string? Apelido { get; set; }
	[JsonProperty("tipo_perfil")] public string? TipoPerfil { get; set; }
	#endregion Propriedades


	#region Construtores
	public Usuario(int? id, string? nomeCompleto, string? apelido, string? tipoPerfil)
	{
		Id = id;
		NomeCompleto = nomeCompleto;
		Apelido = apelido;
		TipoPerfil = tipoPerfil;
	}

	public Usuario() { }
	#endregion Construtores


	#region Métodos
	public static Usuario? VerificarLogin(string emailOuApelido, string senha)
	{
		NpgsqlParameter paramEmailOuApelido = new("@param_email_apelido", emailOuApelido);
		NpgsqlParameter paramSenha = new("@param_senha", senha);
		const string nomeFunction = "logar_usuario";

		Usuario? usuario = conexaoBanco.ExecutarFunction<Usuario>(
		                                                  nomeFunction,
		                                                  [paramEmailOuApelido, paramSenha]
		                                                 )?.First();

		return usuario;
	}

	public static bool Cadastrar(string email, string nome_completo, string apelido, string senha, string tipoPerfil)
	{
		try
		{
			NpgsqlParameter paramNomeCompleto = new("@param_nome_completo", nome_completo);
			NpgsqlParameter paramEmail = new("@param_email", email);
			NpgsqlParameter paramApelido = new("@param_apelido", apelido);
			NpgsqlParameter paramSenha = new("@param_senha", senha);
			NpgsqlParameter paramTipoPerfil = new("@param_nome_tipo_perfil", tipoPerfil);
			string procedure = string.Concat("cadastrar_usuario");

			conexaoBanco.ExecutarProcedure(
			                            procedure,
			                            [
				                            paramNomeCompleto,
				                            paramEmail,
				                            paramApelido,
				                            paramSenha,
				                            paramTipoPerfil
			                            ]);

			return true;
		}

		catch (Exception err)
		{
			Console.WriteLine(err.Message);
			return false;
		}
	}

	public static Usuario? BuscarPorId(string id)
	{
		bool podeConverter = int.TryParse(id, null, out var idInt);
		if (podeConverter)
		{
			var paramId = new NpgsqlParameter("@param_id", idInt);
			string function = string.Concat("buscar_usuario");

			Usuario? resultado = conexaoBanco.ExecutarFunction<Usuario>(
			                                                    function,
			                                                    [paramId]
			                                                   )?.First();

			return resultado;
		}

		return null;
	}
	#endregion Métodos
}