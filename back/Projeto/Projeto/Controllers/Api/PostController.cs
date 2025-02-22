using Microsoft.AspNetCore.Mvc;
using Projeto.Banco;
using Projeto.Config;
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
                Resultado<Post?> resultadoBusca = Post.BuscarPorId(id);

                if (resultadoBusca.Erro != null)
                {
                    return BadRequest(resultadoBusca.Erro.Message);
                }

                if (resultadoBusca.Item == null)
                {
                    return Ok("Nenhum post encontrado.");
                }

                return Ok(resultadoBusca.Item);
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
                Resultado<List<Post>?> resultadoBusca = Post.BuscarTodos();

                if (resultadoBusca.Erro != null)
                {
                    return BadRequest("Ocorreu um erro ao tentar buscar os posts.");
                }

                if (resultadoBusca.Item == null || resultadoBusca.Item.Count <= 0)
                {
                    return Ok("Nenhum post foi encontrado.");
                }

                return Ok(resultadoBusca.Item);
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
                // 1 - verificar se usuário está logado
                // 2 - adicionar ao banco
                // 3 - verificar se postou com sucesso


                // 1
                if (UsuarioNaoLogado())
                {
                    throw new Exception("Você deve estar logado para poder postar.");
                }


                // 2
                Post post = new() { Tipo = tipo, Titulo = titulo, Texto = texto };
                Resultado<string?> resultadoPostarNoBanco = post.Postar(usuarioLogado);


                // 3
                if (resultadoPostarNoBanco.Item == null)
                {
                    return BadRequest("Ocorreu um problema ao tentar postar. Por favor, recarregue a página.");
                }

                if (resultadoPostarNoBanco.Item == null)
                {
                    return BadRequest("Não foi possível postar.");
                }

                return Ok(resultadoPostarNoBanco.Item);
            }

            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }
    }
}
