using Projeto.Models;

namespace Projeto.Config
{
    public class SessionUsuario : Session<Usuario>
    {
        protected override SessionSetter sessao { get; set; }
        protected override string nomeString => "UsuarioLogado";

        public Usuario? UsuarioLogado
        {
            get => Buscar();
            set => Definir(value);
        }

        public SessionUsuario(SessionSetter sessao) : base(sessao)
        {
            this.sessao = sessao;
        }

        public bool EstaLogado()
        {
            return UsuarioLogado != null && UsuarioLogado?.Id != null;
        }
    }
}
