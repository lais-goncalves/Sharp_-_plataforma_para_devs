using Projeto.Models;

namespace Projeto.Config
{
    public abstract class Session<T>
    {
        protected abstract SessionSetter sessao { get; set; }
        public abstract string nomeString { get; }

        public Session(SessionSetter sessao)
        {
            this.sessao = sessao;
        }

        public T? Buscar()
        {
            return sessao.Buscar<T>(nomeString);
        }

        protected void Definir(T? novoUsuarioLogado)
        {
            sessao.Definir(nomeString, novoUsuarioLogado);
        }
    }
}
