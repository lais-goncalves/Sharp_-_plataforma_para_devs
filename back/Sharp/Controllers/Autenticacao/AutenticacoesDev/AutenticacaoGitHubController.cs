using System.Collections.Specialized;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Sharp.Models.ConexoesExternas.TiposDeConexoes;
using Sharp.Models.Paginas;
using Sharp.Models.Paginas.Controllers;
using Sharp.Models.Usuarios;
using Sharp.Models.Usuarios.Perfis.TiposDePerfis;

namespace Sharp.Controllers.Autenticacao.AutenticacoesDev
{
    [Route("[controller]/[action]")]
    public class AutenticacaoGitHubController : ControllerComSession
    {
        ConexaoGitHub? ContaGitHub => (ConexaoGitHub)UsuarioAtual.Perfil.ConexoesDoUsuario["github"];

        [HttpGet]
        public async Task<ActionResult> RetornoLoginGitHub(string code)
        {
            RetornoAPI<UsuarioLogavel?> resultado;

            try {
                // se perfil disponibilizar conexaoExterna GitHub
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
                    // caso o usuário já tenha conexaoExterna e já esteja logado...
                    string? idGitHub = ContaGitHub?.IdLogin;
                    if (idGitHub != null)
                    {
                        return; 
                    }
                }

                // a partir desse ponto, caso o usuário não tenha entrado com o GitHub ainda, cadastrar
                if (ConexaoGitHub.CLIENT_ID == null || ConexaoGitHub.CLIENT_SECRET == null)
                {
                    throw new Exception("Configuração de API incorreta.");
                }

                // redirecionar para API do GitHub para que possa ser cadastrado
                string urlLogin = ConexaoGitHub.BuscarURLLogin();
                Response.Redirect(urlLogin);
            }

            catch (Exception err) 
            {
                Console.WriteLine(err.Message);
            }
        }
    }
}
