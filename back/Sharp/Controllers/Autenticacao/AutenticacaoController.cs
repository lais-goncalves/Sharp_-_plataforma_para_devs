using Microsoft.AspNetCore.Mvc;
using Sharp.Models.Autenticacao;
using Sharp.Models.Paginas;
using Sharp.Models.Paginas.Controllers;
using Sharp.Models.Usuarios;

namespace Sharp.Controllers.Autenticacao
{
    [Route("[controller]/[action]")]
    public class AutenticacaoController : ControllerComSession
    {
        private AutenticacaoSharp Auth => new(UsuarioAtual);
        
        [HttpGet]
        public ActionResult BuscarUsuarioLogado()
        {
            RetornoAPI<Usuario?> resultado = new RetornoAPI<Usuario?>();

            try
            {
                resultado.DefinirDados(UsuarioAtual);
                return Ok(resultado);
            }

            catch (Exception err)
            {
                resultado.DefinirErro(err);
                return Unauthorized(resultado);
            }
        }

        // TODO: testar
        [HttpPost]
        public ActionResult Cadastrar(string email, string nomeCompleto, string apelido, string senha, string tipoPerfil)
        {         
            RetornoAPI<bool> retorno = new();

            try
            {
                Auth.CadastrarUsuario(email, nomeCompleto, apelido, senha, tipoPerfil);
                return Ok();
            }

            catch (Exception err)
            {
                retorno.DefinirErro(err);
                return BadRequest(retorno);
            }
        }

        [HttpGet]
        public ActionResult Logar(string email_ou_apelido, string senha)
        {
            RetornoAPI<Usuario?> resultado = new RetornoAPI<Usuario?>();
            
            try
            {
                Auth.BuscarUsuarioEAutenticar(email_ou_apelido, senha);
                resultado.DefinirDados(UsuarioAtual);
                return Ok(resultado);
            }
            
            catch (Exception err)
            {
                resultado.DefinirErro(err);
                return Unauthorized(resultado);
            }
        }

        [HttpGet]
        public ActionResult Deslogar()
        {
            RetornoAPI<bool?> resultado = new RetornoAPI<bool?>();

            try
            {
                UsuarioAtual.ExcluirUsuarioAtual();
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
