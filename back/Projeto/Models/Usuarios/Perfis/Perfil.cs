namespace Projeto.Models.Usuarios.Perfis
{
    public abstract class Perfil
    {
        #region Propriedades
        protected UsuarioLogavel UsuarioLogavel { get; set; }
        public abstract string NomePerfil { get; }

        public Perfil(UsuarioLogavel usuario)
        {
            UsuarioLogavel = usuario;
        }

        //public abstract Usuario? Logar();

        public abstract Usuario? Registrar();
        #endregion Propriedades
    }
}
