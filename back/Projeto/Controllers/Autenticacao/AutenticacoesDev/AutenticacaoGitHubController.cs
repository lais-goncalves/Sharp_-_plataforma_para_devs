using System.Collections.Specialized;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Projeto.Models.Paginas;
using Projeto.Models.Paginas.Controllers;
using Projeto.Models.Usuarios;
using Projeto.Models.Usuarios.Contas.TiposDeContas;
using Projeto.Models.Usuarios.Perfis.TiposDePerfis;

namespace Projeto.Controllers.Autenticacao.AutenticacoesDev
{
    [Route("[controller]/[action]")]
    public class AutenticacaoGitHubController : ControllerComSession
    {
        ContaGitHub? ContaGitHub => (ContaGitHub)UsuarioAtual.Perfil.ContasDoUsuario["github"];

        [HttpGet]
        public async Task<ActionResult> RetornoLoginGitHub(string code)
        {
            RetornoAPI<UsuarioLogavel?> resultado;

            try {
                // se perfil disponibilizar conta GitHub
                if (ContaGitHub is not null)
                {
                    resultado = await ContaGitHub.RetornoLoginGitHub(code);
                    return Ok(resultado);
                }

                // senão
                throw new Exception("");
            }

            catch (Exception err)
            {
                resultado = new RetornoAPI<UsuarioLogavel?>();
                resultado.DefinirErro(err);

                return BadRequest(resultado);
            }
        }

        [HttpGet]
        public void LogarComGitHub()
        {
            try
            {
                if (UsuarioAtual.EstaLogado())
                {
                    // caso o usuário já tenha conta e já esteja logado...
                    string? idGitHub = ContaGitHub?.IdLogin;
                    if (idGitHub != null)
                    {
                        return; 
                    }
                }

                // a partir desse ponto, caso o usuário não tenha entrado com o GitHub ainda, cadastrar
                if (ContaGitHub.CLIENT_ID == null || ContaGitHub.CLIENT_SECRET == null)
                {
                    throw new Exception("Configuração de API incorreta.");
                }

                // redirecionar para API do GitHub para que possa ser cadastrado
                string urlLogin = ContaGitHub.BuscarURLLogin();
                Response.Redirect(urlLogin);
            }

            catch (Exception err) 
            {
                Console.WriteLine(err.Message);
            }
        }
    }
}
