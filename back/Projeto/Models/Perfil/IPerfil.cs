using Projeto.Dados;
using Projeto.Models.Perfil.Autenticacao;

namespace Projeto.Models.Perfil
{
    public interface IPerfil
    {
        protected int IdPerfilSharp { get; }
        protected string? Id { get; }
        protected string? Apelido { get; }

        public bool Existe => Id is not null;

        protected abstract void BuscarInfoDaFonte();

        protected abstract void BuscarInfoDoBanco();

        protected virtual void BuscarTodasAsInfomacoes()
        {
            BuscarInfoDoBanco();
            BuscarInfoDaFonte();
        }
    }
}
