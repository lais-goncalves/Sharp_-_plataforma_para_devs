using Microsoft.AspNetCore.Mvc;
using Projeto.Models;

namespace Projeto.Controllers.Conta
{
    [Route("[controller]/[action]")]
    public class ContaController : ControllerComSession {
        [HttpGet]
        public ActionResult BuscarUsuarioLogado()
        {
            try
            {
                if (!usuarioEstaLogado)
                {
                    return Ok("Usuário não logado.");
                }

                return Ok(UsuarioLogado);
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

        [HttpGet]
        public ActionResult Logar(string apelido, string senha)
        {
            try
            {
                bool logadoComSucesso = RealizarLogin(apelido, senha);

                if (!logadoComSucesso)
                {
                    throw new Exception("Usuário e/ou senha incorreto(s).");
                }

                return Ok(UsuarioLogado);
            }

            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpGet]
        public ActionResult Logoff()
        {
            try
            {
                RealizarLogoff();
                return Ok("Logoff efetuado com sucesso.");
            }

            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }
    }
}
