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
            set => Sessao.DefinirUsuarioLogado(value);
        }

        public bool usuarioEstaLogado { get => UsuarioLogado != null && UsuarioLogado.Id > 0; }

        private Sessao buscarSessao()
        {
            return new Sessao(HttpContext);
        }

        protected void RealizarLogoff()
        {
            UsuarioLogado = null;
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
