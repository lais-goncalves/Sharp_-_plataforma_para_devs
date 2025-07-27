using Projeto.Dados;
using Projeto.Models.Perfil.Autenticacao;

namespace Projeto.Models.Perfil
{
    public interface IPerfil
    {
        protected int IdPerfilSharp { get; set; }
        protected string? Id { get; set; }
        protected string? Apelido { get; }

        protected abstract void BuscarInfoDaFonte();

        protected abstract void BuscarInfoDoBanco();

        protected virtual void BuscarTodasAsInfomacoes()
        {
            BuscarInfoDoBanco();
            BuscarInfoDaFonte();
        }

        protected abstract bool DefinirInfoNoBanco(string idPerfil);
    }
}
