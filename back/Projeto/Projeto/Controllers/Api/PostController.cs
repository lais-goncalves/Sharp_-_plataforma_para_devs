using Microsoft.AspNetCore.Mvc;
using Projeto.Models;

namespace Projeto.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    public class PostController : ControllerComSession
    {
        [HttpGet]
        public IActionResult BuscarPorId(int id)
        {
            try
            {
                Post? post = Post.BuscarPorId(id);

                if (post == null)
                {
                    return Ok();
                }

                return Ok(post);
            }

            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpGet]
        public IActionResult BuscarTodos()
        {
            try
            {
                List<Post?>? posts = Post.BuscarTodos();

                if (posts == null || posts.Count <= 0)
                {
                    return Ok();
                }

                return Ok(posts);
            }

            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpPost]
        public IActionResult Postar(string tipo, string titulo, string texto)
        {
            try
            {
                if (!UsuarioEstaLogado)
                {
                    throw new Exception("Você deve estar logado para poder postar.");
                }

                string? idResultado = Post.Postar(tipo, titulo, texto, usuarioLogado);

                if (idResultado == null)
                {
                    throw new Exception("Não foi possível postar. Tente Novamente.");
                }

                return Ok(idResultado);
            }

            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }
    }
}
