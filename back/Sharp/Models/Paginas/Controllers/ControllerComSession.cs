using Microsoft.AspNetCore.Mvc;
using Sharp.Models.Session;
using Sharp.Models.Usuarios;

namespace Sharp.Models.Paginas.Controllers;

public class ControllerComSession : Controller
{
	protected UsuarioAtual UsuarioAtual => new(HttpContext);
}