using Microsoft.AspNetCore.Mvc;
using Sharp.Models;
using Sharp.Models.Usuarios;

namespace Sharp.Models.Paginas.Controllers
{
    public class ControllerComSession : Controller
    {
        protected UsuarioLogavel UsuarioAtual => new UsuarioLogavel(HttpContext);
    }
}