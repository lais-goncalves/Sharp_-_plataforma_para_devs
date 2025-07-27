using System.Collections.Specialized;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Projeto.Models;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Projeto.Models.Perfil;

namespace Projeto.Controllers.Conta
{
    [Route("[controller]/[action]")]
    public class ContaController : ControllerComSession {
        private string? GHClientId = System.Configuration.ConfigurationManager.AppSettings["GITHUB_CLIENT_ID"];
        private string? GHClientSecret = System.Configuration.ConfigurationManager.AppSettings["GITHUB_CLIENT_SECRET"];
        protected HttpClient clienteHttp = new();

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
        public async Task<ActionResult> RetornoLoginGitHub(string code)
        {
            try
            {
                Usuario? usuarioAutenticadoComGitHub = usuarioEstaLogado ? UsuarioLogado : new Usuario();

                string? tokenDeAcesso = await usuarioAutenticadoComGitHub?.PerfilGitHub?.BuscarTokenAutenticacao(code);

                if (tokenDeAcesso == null)
                {
                    throw new Exception("Login mal sucedido. Tente novamente.");
                }

                // PASSO 3 -> USAR TOKEN PRA PEGAR INFORMAÇÕES DO USUÁRIO
                string? idInfoGitHub = usuarioAutenticadoComGitHub.PerfilGitHub.BuscarId(tokenDeAcesso);

                if (idInfoGitHub == null)
                {
                  throw new Exception("Um erro ocorreu ao tentar buscar as informações do GitHub. Tente logar novamente.");
                }

                // caso usuário não esteja logado, logar
                if (!usuarioEstaLogado)
                {
                    usuarioAutenticadoComGitHub = Usuario.BuscarPorIdGitHub(idInfoGitHub);

                    if (usuarioAutenticadoComGitHub == null)
                    {
                        throw new Exception("Para logar-se com o GitHub, é necessário criar uma conta primeiro.");
                    }

                    UsuarioLogado = usuarioAutenticadoComGitHub;
                }

                else
                {
                    string? idGitHubUsuario = UsuarioLogado?.PerfilGitHub?.Id;

                    // definir ID do github no banco caso não esteja definido
                    if (idGitHubUsuario == null)
                    {
                        bool idFoiDefinido = (bool) UsuarioLogado?.PerfilGitHub?.DefinirInfoNoBanco(idInfoGitHub);

                        if (idFoiDefinido == false)
                        {
                            throw new Exception("Um erro ocorreu ao tentar buscar as informações do GitHub. Tente logar novamente.");
                        }
                    } 
                    
                    // caso já esteja definido, mas o ID buscado é diferente do banco
                    else if (idGitHubUsuario != idInfoGitHub)
                    {
                        throw new Exception("Já existe um usuário logado nesta conta. Tente usar outra conta.");
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


                if (GHClientId == null || GHClientSecret == null)
                {
                    throw new Exception("Configuração de API incorreta: GitHub Client ID e/ou SECRET inexistente(s).");
                }

                NameValueCollection? query = HttpUtility.ParseQueryString(string.Empty);
                query["client_id"] = GHClientId;
                string? queryString = query.ToString();

                string urlLogin = "https://github.com/login/oauth/authorize?" + queryString;
                Response.Redirect(urlLogin);
            }

            catch (Exception) { }
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
