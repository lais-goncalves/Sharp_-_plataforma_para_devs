using Projeto.Models.Usuarios.Login;

namespace Projeto.Models.Usuarios.Perfis
{
    public interface IPerfil : ILogavel
    {
        #region Propriedades
        string NomePerfil { get; }
        #endregion Propriedades
    }
}
