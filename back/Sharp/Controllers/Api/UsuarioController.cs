using Microsoft.AspNetCore.Mvc;
using Sharp.Models.Paginas;
using Sharp.Models.Paginas.Controllers;
using Sharp.Models.Usuarios;

namespace Sharp.Controllers.Api
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
                // TODO: jogar isso na model
                //List<Usuario?>? usuarios = Usuario.BuscarTodos();

                List<Usuario?>? usuarios = new();
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
                // TODO: jogar isso na model
                // Usuario? usuario = Usuario.BuscarPorApelido(apelido);

                Usuario usuario = new();
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
