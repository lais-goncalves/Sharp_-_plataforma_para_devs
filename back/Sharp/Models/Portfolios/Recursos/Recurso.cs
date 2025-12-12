using Newtonsoft.Json;

namespace Sharp.Models.Portfolios.Recursos;

public interface Recurso
{
	#region Propriedades
	[JsonIgnore] string Id { get; }

	string Nome { get; }
	string Tipo { get; }
	#endregion Propriedades
}