using System.Collections.Specialized;
using System.Data;
using System.Net.Http.Headers;
using System.Web;
using Npgsql;
using Sharp.Models.Conexoes;
using Sharp.Models.Session;
using Sharp.Models.Usuarios;

namespace Sharp.Models.Autenticacao;

public abstract class AutenticacaoComConexao : Autenticacao
{
	#region Propriedades
	public abstract Conexao Conexao { get; set; }
	protected abstract string TipoConexao { get; }
	#endregion Propriedades
	
	
	#region Construtores
	public AutenticacaoComConexao(UsuarioAtual usuarioAtual) : base(usuarioAtual) { }
	#endregion Construtores


	#region Métodos
	#region UtilsHttp
	protected string CriarUrlComQuery(string url, NameValueCollection? query)
	{
		if (query is null)
		{
			return url;
		}
		
		string urlFormatada = url;
		
		if (url[^1] != '?')
		{
			urlFormatada += "?";
		}

		string urlComQuery = $"{urlFormatada}{query}";
		return urlComQuery;
	}
	
	protected HttpRequestMessage CriarRequisicaoGet(string url, string? token = null)
	{
		HttpRequestMessage requisicao = new(HttpMethod.Get, url);

		requisicao.Headers.Add("Accept", "application/json");
		requisicao.Headers.UserAgent.ParseAdd("Sharp");
		
		if (token is not null)
		{
			requisicao.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
		}

		return requisicao;
	}
	
	protected HttpResponseMessage _EnviarRequisicaoGet(HttpRequestMessage requisicao)
	{
		HttpClient clienteHttp = new();
		var resposta = clienteHttp.Send(requisicao);
		
		return resposta;
	}
	
	protected async Task<HttpResponseMessage> _EnviarRequisicaoPost(string url, HttpContent? conteudo = null)
	{
		HttpClient clienteHttp = new();
		var resposta = await clienteHttp.PostAsync(url, conteudo);
		
		return resposta;
	}

	protected NameValueCollection ParsearResposta(HttpResponseMessage resposta)
	{
		var conteudoString = resposta.Content.ReadAsStringAsync().Result;
		NameValueCollection conteudoParseado = HttpUtility.ParseQueryString(conteudoString);

		return conteudoParseado;
	}
	
	// Compilado dos métodos acima - GET
	protected NameValueCollection Get(string url, NameValueCollection? query = null, string? token = null, string? msgErro = null, bool parsear = true)
	{
		string urlComQuery = CriarUrlComQuery(url, query);
		
		HttpRequestMessage requisicao = CriarRequisicaoGet(urlComQuery, token);
		HttpResponseMessage resposta = _EnviarRequisicaoGet(requisicao);
		
		if (msgErro != null && !resposta.IsSuccessStatusCode)
		{
			throw new Exception(msgErro);
		}

		NameValueCollection resultado;

		if (!parsear)
		{
			resultado = new ();
			resultado["resultado"] = resposta.Content.ReadAsStringAsync().Result;
			
			return resultado;
		}
		
		resultado = ParsearResposta(resposta);
		return resultado;
	}
	
	// Compilado dos métodos acima - POST
	protected async Task<NameValueCollection> Post(string url, NameValueCollection query, string? msgErro = null)
	{
		string urlComQuery = CriarUrlComQuery(url, query);
		
		HttpResponseMessage resposta = await _EnviarRequisicaoPost(urlComQuery);
		NameValueCollection resultado = ParsearResposta(resposta);
		
		if (msgErro != null && !resposta.IsSuccessStatusCode)
		{
			throw new Exception(msgErro);
		}

		return resultado;
	}
	#endregion UtilsHttp
	
	
	#region UtilsBanco
	public Usuario? BuscarUsuarioPorIdConexao(string idConexao)
	{
		try
		{
			var function = "buscar_usuario_por_id_login_conexao";
			var paramNomeConta = new NpgsqlParameter("@param_nome_conexao", TipoConexao);
			var paramId = new NpgsqlParameter("@param_id_usuario", idConexao);

			dynamic? resultadoId = ConexaoBanco.ExecutarFunction<dynamic>(function, [paramId, paramNomeConta]);
			if (resultadoId == null || resultadoId?.Count <= 0)
			{
				return null;
			}
			
			string? idString = resultadoId?[0]["id"].ToString(); 
			Usuario? usuarioEncontrado = Usuario.BuscarPorId(idString);
			
			return usuarioEncontrado;
		}

		catch (Exception err)
		{
			Console.WriteLine(err);
			return null;
		}
	}

	public string? BuscarIdConexaoPorUsuario(Usuario usuario)
	{
		int? idUsuario = usuario.Id;
		if (idUsuario is null)
		{
			return null;
		}

		var function = "buscar_id_login_conexao";
		var paramNomeConexao = new NpgsqlParameter("@param_nome_conexao", TipoConexao);
		var paramIdUsuario = new NpgsqlParameter("@param_id_usuario", idUsuario)
		{
			DbType = DbType.Int16
		};

		List<string>? resultado = ConexaoBanco.ExecutarFunction<string>(function, [
			paramIdUsuario, paramNomeConexao
		])?.ToList();

		return resultado?.Count <= 0 ? null : resultado?.First();
	}

	public void CadastrarIdConexaoDoUsuario(Usuario usuario, string idLoginConexao)
	{
		var paramId = new NpgsqlParameter("@param_id_usuario", usuario.Id);
		var paramNomeConta = new NpgsqlParameter("@param_nome_conexao", TipoConexao);
		var paramIdConta = new NpgsqlParameter("@param_id_login_conexao", idLoginConexao);
	
		// TODO: remover apelido -> não serve pra nada
		var paramApelido = new NpgsqlParameter("@param_apelido_usuario_conexao", "");

		const string procedure = "cadastrar_conexao_usuario";
		ConexaoBanco.ExecutarProcedure(procedure, [paramId, paramNomeConta, paramIdConta, paramApelido]);
	}
	#endregion UtilsBanco
	
	
	protected Usuario? AutenticarUsuario(string idUsuarioConexao)
	{
		if (UsuarioAtual.EstaLogado)
		{
			string? idConexao = BuscarIdConexaoPorUsuario(UsuarioAtual);
			if (idConexao is null) // quer dizer que o usuário não possui uma conexão cadastrada
			{
				CadastrarIdConexaoDoUsuario(UsuarioAtual, idUsuarioConexao);
			}

			return UsuarioAtual;
		}

		Usuario? usuarioConexao = BuscarUsuarioPorIdConexao(idUsuarioConexao);
		LogarUsuario(usuarioConexao);
		
		return usuarioConexao;
	}
	#endregion Métodos
}