using Newtonsoft.Json;
using Sharp.Models.Session;

namespace Sharp.Models.Usuarios.Perfis.TiposDePerfis;

public class PerfilDev : Perfil
{
	#region Propriedades
	[JsonIgnore] public override string NomePerfil => "dev";
	#endregion Propriedades
	
	#region Construtores
	public PerfilDev(UsuarioAtual usuario) : base(usuario) { }
	#endregion Construtores


	#region Métodos
	protected override void definirConexoes()
	{
		throw new NotImplementedException();
	}
	#endregion Métodos
}