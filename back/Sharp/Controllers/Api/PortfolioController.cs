using Microsoft.AspNetCore.Mvc;
using Sharp.Models.Paginas;
using Sharp.Models.Paginas.Controllers;
using Sharp.Models.Portfolios;
using Sharp.Models.Projetos;
using Sharp.Models.Projetos.TiposDeProjetos;

namespace Sharp.Controllers.Api
{
    [Route("[controller]/[action]")]
    public class PortfolioController : ControllerComSession
    {
        [HttpGet]
        public IActionResult BuscarProjetosUsuario()
        {
            RetornoAPI<List<BaseProjeto>?> resultado = new ();

            try
            {
                if (!UsuarioAtual.EstaLogado)
                {
                    resultado.DefinirErro("O usuário deve estar logado.");
                    return Ok(resultado);
                }

                Portfolio portfolio = new Portfolio(UsuarioAtual);
                List<BaseProjeto>? projetos = portfolio.BuscarProjetos();

                resultado.DefinirDados(projetos);
                return Ok(resultado);
            }

            catch (Exception ex)
            {
                resultado.DefinirErro(ex);
                return BadRequest(resultado);
            }
        }

        [HttpGet]
        public IActionResult BuscarProjetosGitHub()
        {
            RetornoAPI<List<ProjetoGitHub>?> resultado = new();

            try
            {
                if (!UsuarioAtual.EstaLogado)
                {
                    resultado.DefinirErro("O usuário deve estar logado.");
                    return Ok(resultado);
                }

                List<ProjetoGitHub> projetos = ProjetoGitHub.BuscarTodosOsProjetos(UsuarioAtual);

                resultado.DefinirDados(projetos);
                return Ok(resultado);
            }

            catch (Exception ex)
            {
                resultado.DefinirErro(ex);
                return BadRequest(resultado);
            }
        }
    }
}
