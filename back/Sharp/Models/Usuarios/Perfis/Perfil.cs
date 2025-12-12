using Sharp.Models.Session;

namespace Sharp.Models.Usuarios.Perfis;

public abstract class Perfil
{
	#region Construtores
	public Perfil(UsuarioAtual usuario)
	{
		UsuarioAtual = usuario;
		definirConexoes();
	}
	#endregion Construtores


	#region Métodos
	protected abstract void definirConexoes();
	#endregion Métodos


	#region Propriedades
	protected UsuarioAtual UsuarioAtual { get; set; }
	public abstract string NomePerfil { get; }
	#endregion Propriedades
}