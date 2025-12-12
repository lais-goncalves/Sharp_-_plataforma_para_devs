namespace Sharp.Models.Projetos.TiposDeProjetos;

public class ProjetoComum : BaseProjeto
{
	#region Construtores
	public ProjetoComum(string id, string? nome, string? descricao, string? status, string? tipo = null) :
		base(id, nome, descricao, tipo, status) { }

	public ProjetoComum() { }
	#endregion Construtores


	#region Métodos
	protected override void BuscarFerramentas()
	{
		// TODO: continuar
	}

	protected override void BuscarDemaisInformacoes()
	{
		// TODO: continuar
	}

	public virtual void BuscarTodasAsInformacoes()
	{
		BuscarDemaisInformacoes();
		BuscarFerramentas();
	}
	#endregion Métodos
}