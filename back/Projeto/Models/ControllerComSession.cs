using Microsoft.AspNetCore.Mvc;
using Projeto.Config;

namespace Projeto.Models
{
    public class ControllerComSession : Controller
    {
        protected Sessao Sessao => buscarSessao();
        protected Usuario? UsuarioLogado
        {
            get => Sessao.BuscarUsuarioLogado();
            private set => Sessao.DefinirUsuarioLogado(value);
        }

        public bool usuarioEstaLogado => UsuarioLogado != null;

        private Sessao buscarSessao()
        {
            return new Sessao(HttpContext);
        }


        protected void RealizarLogoff()
        {
            UsuarioLogado = null;
        }

        protected bool RealizarLogin(Usuario novoUsuario)
        {
            return RealizarLogin(novoUsuario.Apelido, novoUsuario.Senha);
        }

        protected bool RealizarLogin(string apelido, string senha)
        {
            try
            {
                Usuario? usuario = Usuario.Login(apelido, senha);

                if (usuario == null)
                {
                    return false;
                }

                UsuarioLogado = usuario;
                return true;
            }

            catch (Exception)
            {
                return false;
            }
        }
    }
}
