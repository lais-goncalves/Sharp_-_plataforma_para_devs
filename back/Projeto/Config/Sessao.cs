using Newtonsoft.Json;
using Projeto.Models;
using Microsoft.AspNetCore.Http;

namespace Projeto.Config
{
    public class Sessao(HttpContext httpContext, string nome)
    {
        public HttpContext HttpContext { get; set; } = httpContext;
        public string Nome { get; set; } = nome;

        public void DefinirValor(object? value)
        {
            HttpContext.Session.SetString(Nome, JsonConvert.SerializeObject(value));
        }

        public T? BuscarValor<T>(string key)
        {
            var value = HttpContext.Session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }
    }
}