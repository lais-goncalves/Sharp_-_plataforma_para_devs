using Microsoft.AspNetCore.Mvc;
using Projeto.Models;

namespace Projeto.Controllers.Conta
{
    [Route("[controller]/[action]")]
    public class ContaController : ControllerComSession {
        [HttpGet]
        public ActionResult BuscarUsuario()
        {
            try
            {
                if (!UsuarioEstaLogado)
                {
                    return Ok("Usuário não logado.");
                }

                return Ok(usuarioLogado);
            }

            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpPost]
        public ActionResult Registrar(string apelido, string senha)
        {
            // TODO: ESCAPAR CARACTERES DO APELIDO E DA SENHA

            //try
            //{
            //    Resultado<bool> registroEfetuado = Usuario.Registrar(apelido, senha);

            //    if (registroEfetuado.Erro != null)
            //    {
            //        throw registroEfetuado.Erro;
            //    }

            //    return Ok(registroEfetuado.Item);
            //}

            //catch(Exception err)
            //{
            //    return BadRequest(err.Message);
            //}

            return Ok();
        }

        [HttpPost]
        public ActionResult Logar(string apelido, string senha)
        {
            try
            {
                Usuario? usuario = Usuario.LoginOk(apelido, senha);

                if (usuario == null)
                {
                    throw new Exception("Usuário e/ou senha incorreto(s).");
                }

                usuarioLogado = usuario;
                return Ok(usuarioLogado);
            }

            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpPost]
        public new ActionResult Logoff()
        {
            try
            {
                Logoff();
                return Ok("Logoff efetuado com sucesso.");
            }

            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }
    }
}
