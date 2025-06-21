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

        public bool UsuarioEstaLogado { get => usuarioLogado != null && usuarioLogado.Id > 0; }

        private Sessao buscarSessao()
        {
            return new Sessao(HttpContext);
        }

        protected void Logoff()
        {
            usuarioLogado = null;
        }
    }
}
