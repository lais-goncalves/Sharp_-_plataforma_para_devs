using Newtonsoft.Json;
using Sharp.Models.Conexoes.FabricaDeConexoes;
using Sharp.Models.Usuarios;

namespace Sharp.Models.Session;

public class UsuarioAtual : Usuario, ItemComSessao
{
	#region Propriedades
	[JsonIgnore] public string RotuloItem => "UsuarioAtual";
	[JsonIgnore] public Sessao Sessao { get; }
	[JsonIgnore] public FabricaDeConexoes FabricaDeConexoes { get; } = new();
	
	public bool EstaLogado => VerificarEstaLogado();
	#endregion Propriedades
	
	
	#region Construtores
	public UsuarioAtual(HttpContext httpContext)
	{
		Sessao = new Sessao(httpContext, RotuloItem);
		Usuario? usuario = buscarUsuarioAtual();
		DefinirUsuarioAtual(usuario);
	}
	#endregion Construtores


	#region Métodos
	private bool VerificarEstaLogado()
	{
		Usuario? usuario = buscarUsuarioAtual();
		return usuario?.Id != null;
	}
	
	private void copiarUsuarioAtual(Usuario? novoUsuario)
	{
		Id = novoUsuario?.Id ?? null;
		NomeCompleto = novoUsuario?.NomeCompleto ?? null;
		Apelido = novoUsuario?.Apelido ?? null;
	}

	private Usuario? buscarUsuarioAtual()
	{
		var usuario = Sessao.BuscarValor<Usuario?>();
		return usuario;
	}

	public void DefinirUsuarioAtual(Usuario? novoUsuario)
	{
		Sessao.DefinirValor(novoUsuario);
		copiarUsuarioAtual(novoUsuario);
	}

	public void ExcluirUsuarioAtual()
	{
		Usuario usuarioVazio = new Usuario();
		DefinirUsuarioAtual(usuarioVazio);
	}
	#endregion Métodos
}