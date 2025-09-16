using Microsoft.AspNetCore.Mvc;
using Projeto.Models.Paginas;
using Projeto.Models.Paginas.Controllers;
using Projeto.Models.Usuarios;

namespace Projeto.Controllers.Autenticacao.AutenticacoesDev
{
    [Route("[controller]/[action]")]
    public class AutenticacaoGitHubController : ControllerComSession
    {
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

            throw new NotImplementedException();
        }

        [HttpGet]
        public ActionResult Logar()
        {
            //if (UsuarioAtual?.Perfil.NomePerfil != "dev")
            //{
            //    return BadRequest(); // TODO: arrumar
            //}

            //PerfilDev perfilUsuario = (PerfilDev) UsuarioAtual.Perfil;
            //ContaGitHub conta = (ContaGitHub) perfilUsuario.ContasDoUsuario["github"];

            //string idGitHub = ContaGitHub.BuscarTokenEID();

            return default;
        }
    }
}
