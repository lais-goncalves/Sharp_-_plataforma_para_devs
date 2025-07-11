﻿using Microsoft.AspNetCore.Mvc;
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
                return Ok(posts);
            }

            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpPost]
        public IActionResult Postar(string titulo, string texto)
        {
            try
            {
                if (!usuarioEstaLogado)
                {
                    throw new Exception("Você deve estar logado para poder postar.");
                }

                int? idResultado = Post.Postar(titulo, texto, UsuarioLogado);

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
