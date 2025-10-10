using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace Sharp.Models.Session
{
    public class Sessao(HttpContext httpContext, string nome)
    {
        private HttpContext HttpContext { get; set; } = httpContext;
        protected string Nome { get; set; } = nome;

        public void DefinirValor(object? value)
        {
            HttpContext.Session.SetString(Nome, JsonConvert.SerializeObject(value));
        }

        public T? BuscarValor<T>()
        {
            var value = HttpContext.Session.GetString(Nome);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }

        public T? BuscarValor<T>(string key)
        {
            var value = HttpContext.Session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }
    }
}