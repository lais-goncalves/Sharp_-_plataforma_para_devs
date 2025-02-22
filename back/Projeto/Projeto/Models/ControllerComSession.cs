using Microsoft.AspNetCore.Mvc;
using Projeto.Config;

namespace Projeto.Models
{
    public class ControllerComSession : Controller
    {
        protected Sessao sessao => buscarSessao();
        protected Usuario? usuarioLogado
        {
            get => sessao.BuscarUsuarioLogado();
            set => sessao.DefinirUsuarioLogado(value);
        }

        private Sessao buscarSessao()
        {
            return new Sessao(HttpContext);
        }

        protected bool UsuarioEstaLogado()
        {
            return usuarioLogado != null && usuarioLogado.Id > 0;
        }

        protected bool UsuarioNaoLogado()
        {
            return !UsuarioEstaLogado();
        }
    }
}
