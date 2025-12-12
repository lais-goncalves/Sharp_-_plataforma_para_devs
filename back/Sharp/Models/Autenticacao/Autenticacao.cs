using System.Dynamic;
using Sharp.Models.Bancos;
using Sharp.Models.Session;
using Sharp.Models.Usuarios;

namespace Sharp.Models.Autenticacao;

public abstract class Autenticacao
{
	#region Propriedades
	protected ConexaoBanco ConexaoBanco => new ConexaoBanco();
	protected UsuarioAtual UsuarioAtual { get; }
	protected readonly dynamic MsgErros = criarMsgErro();
	#endregion Propriedades
	
	
	#region Construtores
	public Autenticacao(UsuarioAtual usuarioAtual)
	{
		UsuarioAtual = usuarioAtual;
	}
	#endregion Construtores


	#region Métodos
	// @LogarUsuario não deve ser chamado fora de outros métodos de autentitação,
	// somente loga na session
	protected void LogarUsuario(Usuario? novoUsuario)
	{
		UsuarioAtual.DefinirUsuarioAtual(novoUsuario);
	}
	
	private static dynamic criarMsgErro()
	{
		dynamic erros = new ExpandoObject();
    
		erros.Auth = "Houve um erro ao tentar autenticar. Tente novamente.";
		erros.Fonte = "Um erro ocorreu ao tentar buscar as informações da fonte. Tente logar novamente.";
		erros.Usuario = "Não foi possível autenticar. Usuário não encontrado.";
    		
		return erros;
	}
	#endregion Métodos
}