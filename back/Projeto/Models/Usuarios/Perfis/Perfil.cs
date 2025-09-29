using Newtonsoft.Json;
using Projeto.Models.Usuarios.Contas;
using Projeto.Models.Usuarios.Contas.TiposDeContas;

namespace Projeto.Models.Usuarios.Perfis
{
    public abstract class Perfil
    {
        #region Propriedades
        [JsonIgnore]
        public Dictionary<string, Conta> ContasDoUsuario { get; } = [];

        protected UsuarioLogavel UsuarioLogavel { get; set; }
        public abstract string NomePerfil { get; }
        #endregion Propriedades


        #region Construtores
        public Perfil(UsuarioLogavel usuario)
        {
            UsuarioLogavel = usuario;
            definirContas();
        }
        #endregion Construtores


        #region Métodos
        protected abstract void definirContas();
        public abstract Usuario? Registrar();
        #endregion Métodos
    }
}
