using Microsoft.AspNetCore.Mvc;
using Projeto.Models.Paginas;
using Projeto.Models.Paginas.Controllers;
using Projeto.Models.Usuarios;

namespace Projeto.Controllers.Api
{
    [Route("[controller]/[action]")]
    public class UsuarioController : ControllerComSession
    {
        [HttpGet]
        public IActionResult BuscarTodos()
        {
            RetornoAPI<List<Usuario?>?> resultado = new RetornoAPI<List<Usuario?>?>();

            try
            {
                List<Usuario?>? usuarios = Usuario.BuscarTodos();
                resultado.DefinirDados(usuarios);

                return Ok(resultado);
            }

            catch (Exception err)
            {
                resultado.DefinirErro(err);
                return BadRequest(resultado);
            }
        }

        [HttpGet]
        public IActionResult BuscarPorApelido(string apelido)
        {
            RetornoAPI<Usuario?> resultado = new RetornoAPI<Usuario?>();

            try
            {
                Usuario? usuario = Usuario.BuscarPorApelido(apelido);
                resultado.DefinirDados(usuario);

                return Ok(resultado);
            }

            catch (Exception err)
            {
                resultado.DefinirErro(err);
                return BadRequest(resultado);
            }
        }
    }
}
