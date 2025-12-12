using Sharp.Models.Session;

namespace Sharp.Models.Usuarios.Login;

public class LoginAtual
{
	#region Construtores
	public LoginAtual(HttpContext httpContext)
	{
		Usuario = new UsuarioAtual(httpContext);
	}
	#endregion Construtores


	#region Propriedades
	public Sessao Sessao { get; protected set; }
	public UsuarioAtual Usuario { get; protected set; }
	#endregion Propriedades
}