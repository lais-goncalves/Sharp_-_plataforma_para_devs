namespace Projeto.Models.Perfil.Autenticacao
{
    public interface IAutenticacao
    {
        abstract static int IdPerfilSharp { get; set; }
        bool EstaAutenticado { get; set; }

        abstract bool Autenticar();
    }
}
