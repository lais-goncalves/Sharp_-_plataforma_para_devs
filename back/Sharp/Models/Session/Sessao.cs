using Newtonsoft.Json;

namespace Sharp.Models.Session;

public class Sessao(HttpContext httpContext, string rotuloItem)
{
	#region Propriedades
	private HttpContext HttpContext { get; } = httpContext;
	protected string RotuloItem { get; set; } = rotuloItem;
	#endregion Propriedades


	#region Métodos
	public void DefinirValor(object? value)
	{
		HttpContext.Session.SetString(RotuloItem, JsonConvert.SerializeObject(value));
	}

	public T? BuscarValor<T>()
	{
		var value = HttpContext.Session.GetString(RotuloItem);
		return value == null ? default : JsonConvert.DeserializeObject<T>(value);
	}
	#endregion Métodos
}