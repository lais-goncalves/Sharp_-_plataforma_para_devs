namespace Projeto.Models.Perfil
{
    public interface IPerfil
    {
        protected abstract string? Id { get; set; }
        protected abstract string? Apelido { get; }
        protected abstract string? NomeCompleto { get; }
        protected abstract HttpClient ClienteHttp { get; }
        protected virtual static string? UrlSite { get; } 

        protected abstract void BuscarInfoDaFonte();

        protected abstract void BuscarInfoDoBanco(int? idPerfilSharp);

        protected void BuscarTodasAsInfomacoes(int? idPerfilSharp)
        {
            BuscarInfoDoBanco(idPerfilSharp);
            BuscarInfoDaFonte();
        }
    }
}
