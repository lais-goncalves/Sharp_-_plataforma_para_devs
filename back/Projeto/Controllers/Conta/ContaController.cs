using System.Collections.Specialized;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Projeto.Config;
using Projeto.Models;
using Projeto.Models.Perfil;

namespace Projeto.Controllers.Conta
{
    [Route("[controller]/[action]")]
    public class ContaController : ControllerComSession {
        [HttpGet]
        public ActionResult BuscarUsuarioLogado()
        {
            RetornoAPI<Usuario?> resultado = new RetornoAPI<Usuario?>();

            try
            {
                resultado.DefinirDados(UsuarioAtual?.Usuario);
                return Ok(resultado);
            }

            catch (Exception err)
            {
                resultado.DefinirErro(err);
                return Unauthorized(resultado);
            }
        }

        [HttpPost]
        public ActionResult Registrar(string apelido, string senha)
        {
            // TODO: ESCAPAR CARACTERES DO APELIDO E DA SENHA

            //try
            //{
            //    RetornoAPI<bool> registroEfetuado = Usuario.Registrar(apelido, senha);

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
        public ActionResult Logar(string email_ou_apelido, string senha)
        {
            RetornoAPI<Usuario?> resultado = new RetornoAPI<Usuario?>();

            try
            {
                UsuarioAtual?.Logar(email_ou_apelido, senha);

                if (!UsuarioAtual.EstaLogado())
                {
                    throw new Exception("Usuário e/ou senha incorreto(s).");
                }

                resultado.DefinirDados(UsuarioAtual.Usuario);
                return Ok(resultado);
            }

            catch (Exception err)
            {
                resultado.DefinirErro(err);
                return Unauthorized(resultado);
            }
        }

        [HttpGet]
        public void LogarComGitHub()
        {
            try
            {
                if (UsuarioAtual.EstaLogado())
                {
                    string? idGitHub = UsuarioAtual?.Usuario.PerfilGitHub?.Id;
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
                Console.WriteLine(err);
            }
        }

        [HttpGet]
        public async Task<ActionResult> RetornoLoginGitHub(string code)
        {
            RetornoAPI<Usuario?> resultado = new RetornoAPI<Usuario?>();

            try
            {
                string? idInfoGitHub = await PerfilGitHub.BuscarIdELogar(code);
                if (idInfoGitHub == null)
                {
                    throw new Exception("Um erro ocorreu ao tentar buscar as informações do GitHub. Tente logar novamente.");
                }

                // caso usuário não esteja logado, logar
                if (!UsuarioAtual.EstaLogado())
                {
                    Usuario? usuarioAutenticado = Usuario.BuscarPorIdGitHub(idInfoGitHub);
                    if (usuarioAutenticado == null)
                    {
                        throw new Exception("Para logar-se com o GitHub, é necessário criar uma conta primeiro.");
                    }

                    UsuarioAtual?.Logar(usuarioAutenticado.Apelido, usuarioAutenticado.Senha);

                    if (!UsuarioAtual.EstaLogado())
                    {
                        throw new Exception("Houve um problema ao tentar logar. Tente novamente.");
                    }
                }

                // se usuário já estiver logado
                else
                {
                    string? idGitHubUsuario = UsuarioAtual?.Usuario.PerfilGitHub?.Id;
                    if (idGitHubUsuario == null)
                    {
                        // definir ID do github no banco caso não esteja definido
                        bool idFoiDefinido = UsuarioAtual.Usuario.PerfilGitHub.CadastrarId(idInfoGitHub);
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

                resultado.DefinirDados(UsuarioAtual.Usuario);
                return Ok(resultado);
            }

            catch (Exception err)
            {
                resultado.DefinirErro(err);
                return BadRequest(resultado);
            }
        }

        [HttpGet]
        public ActionResult Deslogar()
        {
            RetornoAPI<bool?> resultado = new RetornoAPI<bool?>();

            try
            {
                UsuarioAtual.Deslogar();
                return Ok();
            }

            catch (Exception err)
            {
                resultado.DefinirErro(err);
                return BadRequest(resultado);
            }
        }
    }
}
