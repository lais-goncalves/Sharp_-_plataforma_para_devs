using Microsoft.AspNetCore.Mvc;
using Projeto.Models;
using Projeto.Models.Usuarios;

namespace Projeto.Models.Paginas.Controllers
{
    public class ControllerComSession : Controller
    {
        protected UsuarioAtual? UsuarioAtual => new UsuarioAtual(HttpContext);
    }
}