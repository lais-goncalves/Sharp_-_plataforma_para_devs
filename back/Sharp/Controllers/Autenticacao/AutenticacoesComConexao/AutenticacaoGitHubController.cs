using Microsoft.AspNetCore.Mvc;
using Sharp.Models.Autenticacao.TiposDeAutenticacao;
using Sharp.Models.Paginas;
using Sharp.Models.Paginas.Controllers;
using Sharp.Models.Usuarios;

namespace Sharp.Controllers.Autenticacao.AutenticacoesDev;

[Route("[controller]/[action]")]
public class AutenticacaoGitHubController : ControllerComSession
{
    private AutenticacaoGitHub Auth => new (UsuarioAtual);
        
    [HttpGet]
    public void LogarComGitHub()
    {
        try
        {
            // redirecionar para API do GitHub para que possa ser cadastrado
            string? urlLogin = Auth.BuscarUrlRedirecionamentoLogin();
            if (urlLogin is not null)
            {
                Response.Redirect(urlLogin);
            }
        }

        catch (Exception err) 
        {
            Console.WriteLine(err.Message);
        }
    }
        
    [HttpGet]
    public async Task<ActionResult> RetornoLoginGitHub(string code)
    {
        RetornoAPI<Usuario?> resultado = new();

        try
        {
            Usuario usuarioAutenticado = await Auth.BuscarUsuarioEAutenticar(code);
            resultado.DefinirDados(usuarioAutenticado);

            return Ok(resultado);
        }

        catch (Exception err)
        {
            resultado.DefinirErro(err);
            return BadRequest(resultado);
        }
    }
}