using Projeto.Models.Bancos.Tabelas;

namespace Projeto.Models.Usuarios.Perfis.TiposDePerfis
{
    public class PerfilDev : IPerfil
    {
        #region Propriedades
        public Tabela Tabela => new Tabela("perfil_dev");
        public string NomePerfil => "dev";

        public bool Logar()
        {
            throw new NotImplementedException();
        }

        public bool Registrar()
        {
            throw new NotImplementedException();
        }
        #endregion Propriedades


        #region Métodos
        #endregion Métodos
    }
}
