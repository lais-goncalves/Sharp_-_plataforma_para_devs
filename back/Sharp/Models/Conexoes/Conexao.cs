using Newtonsoft.Json;
using Sharp.Models.Bancos;

namespace Sharp.Models.Conexoes;

public abstract class Conexao
{
	#region Propriedades
	[JsonIgnore] private readonly ConexaoBanco _conexaoBanco = new ConexaoBanco();
	#endregion Propriedades


	#region Métodos
	public abstract bool EstabelecerConexao();
	// TODO: VERIFICAR public abstract bool UsuarioPossuiConexao(UsuarioAtual usuarioAtual);
	#endregion Métodos
}