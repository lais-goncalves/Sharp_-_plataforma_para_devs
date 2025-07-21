using System.Collections.Specialized;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Projeto.Models;
using System.Net.Http.Headers;
using Newtonsoft.Json;

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
                if (code == null)
                {
                    throw new Exception("Login mal sucedido. Tente novamente.");
                }

                if (GHClientId == null || GHClientSecret == null)
                {
                    throw new Exception("Configuração de API incorreta: GitHub Client ID e/ou SECRET inexistente(s).");
                }

                // PASSO 1 -> BUSCAR TOKEN NO GITHUB USANDO CODIGO DE ACESSO (CODE)
                NameValueCollection? query = HttpUtility.ParseQueryString(string.Empty);
                query["client_id"] = GHClientId;
                query["client_secret"] = GHClientSecret;
                query["code"] = code;
                string? queryString = query.ToString();

                string urlToken = "https://github.com/login/oauth/access_token?" + queryString;
                HttpResponseMessage postToken = await clienteHttp.PostAsync(urlToken, null);

                if (!postToken.IsSuccessStatusCode)
                {
                    throw new Exception("Login mal sucedido. Tente novamente.");
                }

                // PASSO 2 -> PEGAR TOKEN NA STRING DE RETORNO DO GITHUB
                string queryStringToken = postToken.Content.ReadAsStringAsync().Result;

                NameValueCollection? queryToken = HttpUtility.ParseQueryString(queryStringToken);
                string? token = queryToken["access_token"];

                if (token == null)
                {
                    throw new Exception("Login mal sucedido. Tente novamente.");
                }

                // PASSO 3 -> USAR TOKEN PRA PEGAR INFORMAÇÕES DO USUÁRIO
                using (HttpRequestMessage getInfoUsuario = new (HttpMethod.Get, "https://api.github.com/user"))
                {
                    getInfoUsuario.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    getInfoUsuario.Headers.Add("Accept", "application/json");
                    getInfoUsuario.Headers.UserAgent.ParseAdd("Sharp");

                    var retornoInfoUsuario = clienteHttp.Send(getInfoUsuario);

                    if (!retornoInfoUsuario.IsSuccessStatusCode)
                    {
                        throw new Exception("Login mal sucedido. Tente novamente.");
                    }

                    // verificar se usuário já está conectado ao GitHub
                    string? infoGitHubUsuario = retornoInfoUsuario.Content.ReadAsStringAsync().Result;

                    if (infoGitHubUsuario == null)
                    {
                        throw new Exception("Um erro ocorreu ao tentar buscar as informações do GitHub. Tente logar novamente.");
                    }

                    dynamic? objInfoGitHubUsuario = JsonConvert.DeserializeObject(infoGitHubUsuario);
                    string? idInfoGitHub = objInfoGitHubUsuario?.id;

                    if (idInfoGitHub == null)
                    {
                        throw new Exception("Um erro ocorreu ao tentar buscar as informações do GitHub. Tente logar novamente.");
                    }

                    // caso usuário não esteja logado, logar
                    if (!usuarioEstaLogado)
                    {
                        Usuario? usuario = Usuario.BuscarPorIdGitHub(idInfoGitHub);

                        if (usuario == null)
                        {
                            throw new Exception("Para logar-se com o GitHub, é necessário criar uma conta primeiro.");
                        }

                        UsuarioLogado = usuario;
                    } 
                    
                    else
                    {
                        string? idGitHubUsuario = UsuarioLogado?.PerfilGitHub?.Id;

                        if (idGitHubUsuario == null)
                        {
                            bool idFoiDefinido = (bool) UsuarioLogado?.PerfilGitHub?.DefinirInfoNoBanco(idInfoGitHub);

                            if (idFoiDefinido == false)
                            {
                                throw new Exception("Um erro ocorreu ao tentar buscar as informações do GitHub. Tente logar novamente.");
                            }
                        } else if (idGitHubUsuario != idInfoGitHub)
                        {
                            throw new Exception("Já existe um usuário logado nesta conta. Tente usar outra conta.");
                        }
                    }

                    return Ok("Login efetuado com sucesso.");
                }
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
