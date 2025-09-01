using Microsoft.AspNetCore.Mvc;
using Projeto.Models.Paginas;
using Projeto.Models.Paginas.Controllers;
using Projeto.Models.Posts;

namespace Projeto.Controllers.Api
{
    [Route("[controller]/[action]")]
    public class PostController : ControllerComSession
    {
        [HttpGet]
        public IActionResult BuscarPorId(int id)
        {
            RetornoAPI<Post?> resultado = new RetornoAPI<Post?>();

            try
            {
                Post? post = Post.BuscarPorId(id);
                resultado.DefinirDados(post);

                return Ok(resultado);
            }

            catch (Exception err)
            {
                resultado.DefinirErro(err);
                return BadRequest(resultado);
            }
        }

        [HttpGet]
        public IActionResult BuscarTodos()
        {
            RetornoAPI<List<Post?>?> resultado = new RetornoAPI<List<Post?>?>();

            try
            {
                List<Post?>? posts = Post.BuscarTodos();
                resultado.DefinirDados(posts);

                return Ok(resultado);
            }

            catch (Exception err)
            {
                resultado.DefinirErro(err);
                return BadRequest(resultado);
            }
        }

        [HttpPost]
        public IActionResult Postar(string titulo, string texto)
        {
            RetornoAPI<Post?> resultado = new RetornoAPI<Post?>();

            try
            {
                if (!UsuarioAtual.EstaLogado())
                {
                    throw new Exception("Você deve estar logado para poder postar.");
                }

                int? idRetornoAPI = Post.Postar(titulo, texto, UsuarioAtual.Usuario);
                if (idRetornoAPI == null)
                {
                    throw new Exception("Não foi possível postar. Tente Novamente.");
                }

                return Ok(idRetornoAPI);
            }

            catch (Exception err)
            {
                resultado.DefinirErro(err);
                return BadRequest(resultado);
            }
        }
    }
}
