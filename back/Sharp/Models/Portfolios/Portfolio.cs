using Newtonsoft.Json;
using Npgsql;
using Sharp.Models.Bancos;
using Sharp.Models.Projetos;
using Sharp.Models.Session;
using Sharp.Models.Usuarios;

namespace Sharp.Models.Portfolios;

public class Portfolio
{
	#region Construtores
	public Portfolio(UsuarioAtual usuarioAtual)
	{
		UsuarioAtual = usuarioAtual;
	}
	#endregion Construtores


	#region Propriedades
	[JsonIgnore] private ConexaoBanco conexaoBanco => new ConexaoBanco();
	[JsonIgnore] private UsuarioAtual UsuarioAtual { get; }

	public string? Id { get; set; }
	public string? Descricao { get; set; } = string.Empty;

	private readonly FabricaDeProjetos fabrica = new();
	#endregion Propriedades


	#region Métodos
	protected List<dynamic> BuscarInfoProjetosDoBanco()
	{
		try
		{
			var paramIdUsuario = new NpgsqlParameter("@param_id_usuario", UsuarioAtual.Id);
			var nomeFuncao = "buscar_projetos_usuario";

			List<dynamic> projetos =
				conexaoBanco.ExecutarFunction<dynamic>(nomeFuncao, [paramIdUsuario]) ?? [];

			return projetos;
		}

		catch (Exception err)
		{
			throw new Exception(err.Message);
		}
	}

	protected List<BaseProjeto> ConverterListaDynamicParaProjeto(List<dynamic> listaProjetosDynamic)
	{
		var projetosFinalizados = new List<BaseProjeto>();
		foreach (var projetoDynamic in listaProjetosDynamic)
		{
			BaseProjeto? projeto = fabrica.CriarECarregarDadosProjeto(projetoDynamic);
			if (projeto != null) projetosFinalizados.Add(projeto);
		}

		return projetosFinalizados;
	}

	public List<BaseProjeto> BuscarProjetos()
	{
		try
		{
			var projetosEncontrados = BuscarInfoProjetosDoBanco();

			var projetosFinalizados = ConverterListaDynamicParaProjeto(projetosEncontrados);
			projetosEncontrados.Clear();

			projetosFinalizados.ForEach(p => p.BuscarTodasAsInformacoes());

			return projetosFinalizados;
		}

		catch (Exception)
		{
			throw new Exception("Ocorreu um problema ao buscar os projetos do usuário.");
		}
	}
	#endregion Métodos
}