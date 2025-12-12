using Sharp.Models.Projetos.TiposDeProjetos;

namespace Sharp.Models.Projetos;

public class FabricaDeProjetos
{
	#region Métodos
	public BaseProjeto? CriarProjeto(dynamic p)
	{
		try
		{
			BaseProjeto projeto;
			string tipoProjeto = p["tipo"];

			if (tipoProjeto == ProjetoGitHub.TipoProjeto)
				projeto = new ProjetoGitHub();

			else
				projeto = new ProjetoComum();

			projeto.Id = p["id"];
			projeto.Nome = p["nome"];
			projeto.Tipo = p["tipo"];
			projeto.Descricao = p["descricao"];
			projeto.Status = p["status"];

			return projeto;
		}

		catch (Exception err)
		{
			Console.WriteLine(err.Message);
			return null;
		}
	}

	public BaseProjeto? CriarECarregarDadosProjeto(dynamic p)
	{
		BaseProjeto? projeto = CriarProjeto(p);
		projeto?.BuscarTodasAsInformacoes();

		return projeto;
	}
	#endregion Métodos
}