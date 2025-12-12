using Sharp.Models.Session;

namespace Sharp.Models.Projetos.TiposDeProjetos;

public class ProjetoGitHub : BaseProjeto
{
	#region Propriedades
	// TODO: trazer conexao aqui

	public new static string TipoProjeto => "github";
	#endregion Propriedades


	#region Construtores
	public ProjetoGitHub(string id, string? nome, string? descricao, string? status, string? tipo = null) :
		base(id, nome, descricao, tipo, status) { }

	public ProjetoGitHub() { }
	#endregion Construtores


	// TODO: refatorar isso tudo
	#region Métodos
	public static List<ProjetoGitHub> BuscarTodosOsProjetos(UsuarioAtual usuario)
	{
		/*var projetos = new List<ProjetoGitHub>();

		var conexaoGitHub = (ConexaoGitHub)usuario.Perfil.ConexoesDoUsuario["github"];
		conexaoGitHub.BuscarTodasAsInfomacoes();
		string? apelidoUsuarioGitHub = conexaoGitHub.Apelido;


		HttpClient clienteHttp = new();
		var urlBusca = $"{_urlApiGitHub}/users/{apelidoUsuarioGitHub}/repos";
		HttpRequestMessage buscaPerfil = new(HttpMethod.Get, urlBusca);

		buscaPerfil.Headers.Add("Accept", "application/json");
		buscaPerfil.Headers.UserAgent.ParseAdd("Sharp");

		var retornoInfoUsuario = clienteHttp.Send(buscaPerfil);
		var resultadoBusca = retornoInfoUsuario.Content?.ReadAsStringAsync().Result;

		if (resultadoBusca == null) return projetos;

		var objResultado = HttpUtility.ParseQueryString(resultadoBusca);
		Console.WriteLine(objResultado.ToString());*/

		return new List<ProjetoGitHub>();
	}

	protected override void BuscarFerramentas()
	{
		// TODO: continuar
	}

	protected override void BuscarDemaisInformacoes()
	{
		// TODO: continuar
	}
	#endregion Métodos
}