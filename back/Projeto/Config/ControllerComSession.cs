using Microsoft.AspNetCore.Mvc;
using Projeto.Models;

namespace Projeto.Config
{
    public class ControllerComSession : Controller
    {
        protected UsuarioAtual? UsuarioAtual => new UsuarioAtual(HttpContext);
    }
}