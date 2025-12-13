using System.Collections.Specialized;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Sharp.Models.Session;
using Sharp.Models.Paginas;
using Sharp.Models.Paginas.Controllers;
using Sharp.Models.Usuarios;

namespace Sharp.Controllers.Autenticacao
{
    [Route("[controller]/[action]")]
    public class AutenticacaoController : ControllerComSession {
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
                return Ok(resultado);
            }
        }

        [HttpPost]
        public ActionResult Cadastrar(string email, string nomeCompleto, string apelido, string senha, string tipoPerfil)
        {                
            RetornoAPI<bool> retorno = new();

            try
            {
                bool foiCadastrado = Usuario.Cadastrar(email, nomeCompleto, apelido, senha, tipoPerfil);

                if (!foiCadastrado)
                {
                    throw new Exception("Não foi possível cadastrar o usuário. Tente novamente");
                }

                return Ok();
            }

            catch (Exception err)
            {
                retorno.DefinirErro(err);
                return Ok(retorno);
            }
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

                resultado.DefinirDados(UsuarioAtual);
                return Ok(resultado);
            }

            catch (Exception err)
            {
                resultado.DefinirErro(err);
                return Ok(resultado);
            }
        }

        //[HttpGet]
        //public void LogarComGitHub()
        //{
        //    try
        //    {
        //        if (UsuarioAtual.EstaLogado())
        //        {
        //            string? idGitHub = UsuarioAtual?.Usuario.AutenticacaoGitHub?.Id;
        //            if (idGitHub != null)
        //            {
        //                return;
        //            }
        //        }

        //        string urlLogin = AutenticacaoGitHub.BuscarURLLogin();
        //        Response.Redirect(urlLogin);
        //    }

        //    catch (Exception err) 
        //    {
        //        Console.WriteLine(err);
        //    }
        //}

        //[HttpGet]
        //public async Task<ActionResult> RetornoLoginGitHub(string code)
        //{
        //    RetornoAPI<Usuario?> resultado = new RetornoAPI<Usuario?>();

        //    try
        //    {
        //        string? idInfoGitHub = await ContaGitHub.BuscarIdELogar(code);
        //        if (idInfoGitHub == null)
        //        {
        //            throw new Exception("Um erro ocorreu ao tentar buscar as informações do GitHub. Tente logar novamente.");
        //        }

        //        // caso usuário não esteja logado, logar
        //        if (!UsuarioAtual.EstaLogado())
        //        {
        //            Usuario? usuarioAutenticado = Usuario.BuscarPorIdGitHub(idInfoGitHub);
        //            if (usuarioAutenticado == null)
        //            {
        //                throw new Exception("Para logar-se com o GitHub, é necessário criar uma autenticacao primeiro.");
        //            }

        //            UsuarioAtual?.Logar(usuarioAutenticado.Apelido, usuarioAutenticado.Senha);

        //            if (!UsuarioAtual.EstaLogado())
        //            {
        //                throw new Exception("Houve um problema ao tentar logar. Tente novamente.");
        //            }
        //        }

        //        // se usuário já estiver logado
        //        else
        //        {
        //            string? idGitHubUsuario = UsuarioAtual?.Usuario.AutenticacaoGitHub?.Id;
        //            if (idGitHubUsuario == null)
        //            {
        //                // definir ID do github no banco caso não esteja definido
        //                bool idFoiDefinido = UsuarioAtual.Usuario.AutenticacaoGitHub.CadastrarId(idInfoGitHub);
        //                if (!idFoiDefinido)
        //                {
        //                    throw new Exception("Um erro ocorreu ao tentar buscar as informações do GitHub. Tente logar novamente.");
        //                }
        //            }

        //            // caso já esteja definido, mas o ID buscado é diferente do banco
        //            else if (idGitHubUsuario != idInfoGitHub)
        //            {
        //                throw new Exception("Já existe um usuário logado nesta autenticacao. Tente usar outra.");
        //            }
        //        }

        //        resultado.DefinirDados(UsuarioAtual.Usuario);
        //        return Ok(resultado);
        //    }

        //    catch (Exception err)
        //    {
        //        resultado.DefinirErro(err);
        //        return Ok(resultado);
        //    }
        //}

        [HttpGet]
        public ActionResult Deslogar()
        {
            RetornoAPI<bool?> resultado = new RetornoAPI<bool?>();

            try
            {
                UsuarioAtual?.Deslogar();
                return Ok();
            }

            catch (Exception err)
            {
                resultado.DefinirErro(err);
                return Ok(resultado);
            }
        }
    }
}
