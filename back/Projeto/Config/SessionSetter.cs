using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Projeto.Models;

namespace Projeto.Config
{
    public class Sessao
    {
        private HttpContext HttpContext { get; set; }
        public static string UsuarioLogadoStr => "UsuarioLogado";

        public Sessao (HttpContext HttpContext)
        {
            this.HttpContext = HttpContext;
        }

        public void Definir(string key, object? value)
        {
            HttpContext.Session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public T? Buscar<T>(string key)
        {
            var value = HttpContext.Session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }

        public Usuario? BuscarUsuarioLogado()
        {
            return Buscar<Usuario>(UsuarioLogadoStr);
        }

        public void DefinirUsuarioLogado(Usuario? novoUsuario)
        {
            Definir(UsuarioLogadoStr, novoUsuario);
        }
    }
}
