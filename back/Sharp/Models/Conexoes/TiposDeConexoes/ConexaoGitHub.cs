using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace Sharp.Models.Conexoes.TiposDeConexoes;

public class ConexaoGitHub : Conexao
{
	#region Propriedades
	public static readonly string? UrlApi = "https://api.github.com";
	
	public readonly string? ClientId = buscarClientId();
	public readonly string? ClientSecret = buscarClientSecret();
	#endregion Propriedades


	#region Métodos
	#region Utils
	private static string? buscarClientId()
	{
		return ConfigurationManager.AppSettings["GITHUB_CLIENT_ID"];
	}

	private static string? buscarClientSecret()
	{
		return ConfigurationManager.AppSettings["GITHUB_CLIENT_SECRET"];
	}
	#endregion Utils


	public override bool EstabelecerConexao()
	{
		// TODO: continuar esse cara
		return true;
	}
	#endregion Métodos
}