using Microsoft.AspNetCore.Http;
using Npgsql;
using Sharp.Models.Session;
using Sharp.Models.Usuarios;
using Sharp.Models.Usuarios.Perfis;

namespace Sharp.Models.Session.SessionAtual
{
    public class UsuarioAtual : UsuarioLogavel
    {
        public UsuarioAtual(HttpContext httpContext) : base(httpContext) { }
    }
}
