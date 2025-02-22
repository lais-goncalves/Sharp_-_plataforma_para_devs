using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Projeto.Banco;
using Projeto.Config;
using Projeto.Models;

namespace Projeto.Controllers.Conta
{
    [Route("[controller]/[action]")]
    public class ContaController : Controller {
        [HttpGet]
        public ActionResult BuscarUsuario()
        {
            try
            {
                Sessao sessao = new (HttpContext);
                Usuario? usuarioLogado = sessao.BuscarUsuarioLogado();

                if (Usuario.Existe(usuarioLogado))
                {
                    return Ok(null);
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

            try
            {
                Resultado<bool> registroEfetuado = Usuario.Registrar(apelido, senha);

                if (registroEfetuado.Erro != null)
                {
                    throw registroEfetuado.Erro;
                }

                return Ok(registroEfetuado.Item);
            }

            catch(Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpPost]
        public ActionResult Logar(string apelido, string senha)
        {
            try
            {
                Usuario? usuarioLogado;
                Resultado<Usuario?> loginEfetuado = Usuario.VerificarLogin(apelido, senha);

                if (loginEfetuado.Erro != null)
                {
                    throw loginEfetuado.Erro;
                }

                usuarioLogado = loginEfetuado.Item;

                if (usuarioLogado == null)
                {
                    throw new Exception("Usuário e/ou senha incorreto(s).");
                }

                Sessao sessao = new(HttpContext);
                sessao.DefinirUsuarioLogado(usuarioLogado);
                usuarioLogado = sessao.BuscarUsuarioLogado();

                if (usuarioLogado == null)
                {
                    throw new Exception("Usuário inválido.");
                }

                return Ok(usuarioLogado);
            }

            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpPost]
        public ActionResult Logoff()
        {
            try
            {
                Sessao sessao = new (HttpContext);
                sessao.DefinirUsuarioLogado(null);

                return Ok();
            } 
            
            catch(Exception err) {
                return BadRequest(err.Message);
            }
        }
    }
}
