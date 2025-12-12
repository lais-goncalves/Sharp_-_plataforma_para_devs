using System.Collections.Specialized;
using System.Web;
using Sharp.Models.Conexoes.TiposDeConexoes;
using Sharp.Models.Session;
using Sharp.Models.Usuarios;
using System.Text.Json.Nodes;
using Sharp.Models.Conexoes;

namespace Sharp.Models.Autenticacao.TiposDeAutenticacao;

public class AutenticacaoGitHub : AutenticacaoComConexao
{
	#region Propriedades
	private static readonly string urlLogin = "https://github.com/login/oauth";
	private static readonly string urlAutorizacao = $"{urlLogin}/authorize";
	private static readonly string urlToken = $"{urlLogin}/access_token";
	private static readonly string urlInfoUsuario = $"{ConexaoGitHub.UrlApi}/user";
	
	protected override string TipoConexao => "GitHub";
	public override Conexao Conexao { get; set; }
	#endregion Propriedades
	
	
	#region Construtores
	public AutenticacaoGitHub(UsuarioAtual usuarioAtual) : base(usuarioAtual)
	{
		Conexao = new ConexaoGitHub();
	}
	#endregion Construtores
	
	
	#region Métodos
	#region ChamadosPelaController
	public string? BuscarUrlRedirecionamentoLogin()
	{
		string? idConexao = BuscarIdConexaoPorUsuario(UsuarioAtual);
		if (idConexao is not null)
		{
			return null;
		}

		string urlRedirecionarLogin = CriarUrlAutenticacao();
		return urlRedirecionarLogin;
	}
	
	public async Task<Usuario> BuscarUsuarioEAutenticar(string codigoDoUsuario)
	{
		string? token = await BuscarTokenAutenticacao(codigoDoUsuario);
		if (token is null)
		{
			throw new Exception(MsgErros.Auth);
		}
		
		string? idUsuarioGitHub = BuscarInfoDoUsuarioNaConexao(token);
		if (idUsuarioGitHub is null)
		{
			throw new Exception(MsgErros.Fonte);
		}

		Usuario? usuarioAutenticado = AutenticarUsuario(idUsuarioGitHub);
		if (usuarioAutenticado is null)
		{
			throw new Exception(MsgErros.Usuario);
		}
		
		return usuarioAutenticado;
	}
	#endregion ChamadosPelaController
	
	
	protected NameValueCollection BuscarPerfilUsuarioConectado()
	{
		try
		{
			var resultado = Get(urlInfoUsuario);
			return resultado;
		}

		catch (Exception err)
		{
			Console.WriteLine(err.Message);
			return null;
		}
	}
	
	protected string CriarUrlAutenticacao()
	{
		var query = HttpUtility.ParseQueryString(string.Empty);
		query["client_id"] = ((ConexaoGitHub) Conexao).ClientId;

		string urlAutenticacao = CriarUrlComQuery(urlAutorizacao, query);
		return urlAutenticacao;
	}

	private async Task<string?> BuscarTokenAutenticacao(string codigoDoUsuario)
	{
		NameValueCollection query = HttpUtility.ParseQueryString(string.Empty);
		query["client_id"] = ((ConexaoGitHub) Conexao).ClientId;
		query["client_secret"] = ((ConexaoGitHub) Conexao).ClientSecret;
		query["code"] = codigoDoUsuario;

		NameValueCollection resultadoPost = await Post(urlToken, query, MsgErros.Auth);
		string? token = resultadoPost["access_token"];
			
		return token;
	}

	private string? BuscarInfoDoUsuarioNaConexao(string token)
	{
		NameValueCollection? resultadoNV = Get(urlInfoUsuario, null, token, MsgErros.Auth, false);
		string? resultadoStr = resultadoNV?["resultado"];
		var resultadoSeparado = JsonNode.Parse(resultadoStr);

		string? idUsuarioGitHub = ((int?)resultadoSeparado?["id"]).ToString();
		return idUsuarioGitHub;
	}
	
	// TODO: criar classe para as infos do usuário que podem vir da conexão?
	#endregion Métodos
}