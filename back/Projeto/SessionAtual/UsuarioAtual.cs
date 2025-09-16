using Microsoft.AspNetCore.Http;
using Npgsql;
using Projeto.Models.Session;
using Projeto.Models.Usuarios;
using Projeto.Models.Usuarios.Perfis;

namespace Projeto.SessionAtual
{
    public class UsuarioAtual : UsuarioLogavel
    {
        public UsuarioAtual(HttpContext httpContext) : base(httpContext) { }
    }
}
