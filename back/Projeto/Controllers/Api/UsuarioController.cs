using Microsoft.AspNetCore.Mvc;
using Projeto.Models;

namespace Projeto.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    public class UsuarioController : ControllerComSession
    {
        [HttpGet]
        public IActionResult BuscarTodos()
        {
            try
            {
                List<Usuario?>? usuarios = Usuario.BuscarTodos();

                if (usuarios == null || usuarios.Count <= 0)
                {
                    return Ok("Não foram encontrados usuários.");
                }

                return Ok(usuarios);
            }

            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpGet]
        public IActionResult BuscarPorApelido(string apelido)
        {
            try
            {
                Usuario? usuario = Usuario.BuscarPorApelido(apelido);

                if (usuario == null)
                {
                    return Ok("Usuário não encontrado.");
                }

                return Ok("Apelido: " + usuario.Apelido);
            }

            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpPost]
        public IActionResult DefinirCodigoGitHubUsuario(int id, string codigo)
        {
            try
            {
                bool funcionou = Usuario.DefinirCodigoGitHub(id, codigo);

                if (funcionou)
                {
                    return Ok();
                }

                throw new Exception("Não funcionou.");
            }

            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }
    }
}
