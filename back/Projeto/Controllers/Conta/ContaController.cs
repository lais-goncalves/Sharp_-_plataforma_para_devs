using System.Collections.Specialized;
using System.Configuration;
using System.Net.Http;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Projeto.Models;

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

        /*
        [HttpGet]
        public async Task<ActionResult> BuscarTokenDeAcessoGitHub(string codigo)
        {
            try
            {
                if (GHClientId == null || GHClientSecret == null)
                {
                    throw new Exception("Configuração de API incorreta: GitHub Client ID e/ou SECRET inexistente(s).");
                }

                NameValueCollection? query = HttpUtility.ParseQueryString(string.Empty);
                query["client_id"] = GHClientId;
                query["client_secret"] = GHClientSecret;
                query["code"] = codigo;
                string? queryString = query.ToString();

                string urlToken = "https://github.com/login/oauth/access_token?" + queryString;

                using (HttpResponseMessage resposta = await clienteHttp.PostAsync(urlToken, null))
                {
                    Console.WriteLine(resposta.StatusCode);
                    return Ok(resposta);
                }
            }

            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }
        */

        [HttpGet]
        public ActionResult RetornoLoginGitHub(string code)
        {
            try
            {
                if (code == null)
                {
                    throw new Exception("Login mal sucedido. Tente novamente.");
                }

                // TODO: buscar usuario pelo código do GitHub e logar caso não esteja logado

                return Ok("Logado com sucesso!");
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

            catch (Exception) {  }
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
