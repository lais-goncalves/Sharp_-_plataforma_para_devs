using Sharp.Models.Conexoes.TiposDeConexoes;

namespace Sharp.Models.Conexoes.FabricaDeConexoes;

public class FabricaDeConexoes
{
	#region Métodos
	public Conexao CriarConexao(string tipo)
	{
		string tipoFormatado = tipo.ToLower().Trim();
		switch (tipoFormatado)
		{
			case "github": return new ConexaoGitHub();
			case "sharp":  return new ConexaoSharp();
			default:       throw new Exception($"Tipo de conexão '{tipo}' não localizado.");
		}
	}
	#endregion Métodos
}