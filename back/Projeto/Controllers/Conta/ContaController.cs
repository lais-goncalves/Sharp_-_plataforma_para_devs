using System.Collections.Specialized;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Projeto.Models;
using Projeto.Models.Perfil;

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

                return Ok(UsuarioLogado?.Id);
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
        public void LogarComGitHub()
        {
            try
            {
                if (usuarioEstaLogado)
                {
                    string? idGitHub = UsuarioLogado?.PerfilGitHub?.Id;
                    if (idGitHub != null)
                    {
                        return;
                    }
                }

                string urlLogin = PerfilGitHub.BuscarURLLogin();
                Response.Redirect(urlLogin);
            }

            catch (Exception err) 
            {
               Console.WriteLine(err.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult> RetornoLoginGitHub(string code)
        {
            try
            {
                string? idInfoGitHub = await PerfilGitHub.LogarEBuscarId(code);
                if (idInfoGitHub == null)
                {
                    throw new Exception("Um erro ocorreu ao tentar buscar as informações do GitHub. Tente logar novamente.");
                }

                // caso usuário não esteja logado, logar
                if (!usuarioEstaLogado)
                {
                    Usuario? usuarioAutenticado = Usuario.BuscarPorIdGitHub(idInfoGitHub);
                    if (usuarioAutenticado == null)
                    {
                        throw new Exception("Para logar-se com o GitHub, é necessário criar uma conta primeiro.");
                    }

                    bool logadoComSucesso = RealizarLogin(usuarioAutenticado);
                    if (!logadoComSucesso)
                    {
                        throw new Exception("Houve um problema ao tentar logar. Tente novamente.");
                    }
                }

                else
                {
                    string? idGitHubUsuario = UsuarioLogado?.PerfilGitHub?.Id;
                    if (idGitHubUsuario == null)
                    {
                        // definir ID do github no banco caso não esteja definido
                        bool idFoiDefinido = UsuarioLogado.PerfilGitHub.CadastrarId(idInfoGitHub);
                        if (!idFoiDefinido)
                        {
                            throw new Exception("Um erro ocorreu ao tentar buscar as informações do GitHub. Tente logar novamente.");
                        }
                    }

                    // caso já esteja definido, mas o ID buscado é diferente do banco
                    else if (idGitHubUsuario != idInfoGitHub)
                    {
                        throw new Exception("Já existe um usuário logado nesta conta. Tente usar outra.");
                    }
                }

                return Ok("Login efetuado com sucesso.");
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
