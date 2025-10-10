using Microsoft.AspNetCore.Mvc;
using Sharp.Models.Paginas;
using Sharp.Models.Paginas.Controllers;
using Sharp.Models.Posts;

namespace Sharp.Controllers.Api
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

                int? idRetornoAPI = Post.Postar(titulo, texto, UsuarioAtual);
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
