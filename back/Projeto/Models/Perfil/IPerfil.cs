using Projeto.Dados;

namespace Projeto.Models.Perfil
{
    public interface IPerfil
    {
        protected int IdPerfilSharp { get; set; }
        protected string? Id { get; set; }
        protected string? Apelido { get; }
        protected abstract static string? UrlSite { get; }
        protected static HttpClient ClienteHttp = new HttpClient();
        protected static Conexao Conexao = Conexao.instancia;

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
