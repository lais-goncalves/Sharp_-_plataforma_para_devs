using Microsoft.AspNetCore.Mvc;
using Projeto.Banco;
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
                Resultado<List<Usuario>?> resultadoBusca = Usuario.BuscarTodos();

                if (resultadoBusca.Erro != null)
                {
                    return BadRequest(resultadoBusca.Erro.Message);
                }

                if (resultadoBusca.Item?.Count <= 0)
                {
                    return Ok("Não foram encontrados usuários.");
                }

                return Ok(resultadoBusca.Item);
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
                Resultado<Usuario?> resultadoBusca = Usuario.BuscarPorApelido(apelido);

                if (resultadoBusca.Erro != null)
                {
                    throw resultadoBusca.Erro;
                }

                if (resultadoBusca.Item == null)
                {
                    return Ok("Usuário não encontrado.");
                }

                return Ok(resultadoBusca.Item.Apelido);
            }

            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }
    }
}
