using Newtonsoft.Json;
using Sharp.Models.ConexoesExternas;
using Sharp.Models.ConexoesExternas.TiposDeConexoes;

namespace Sharp.Models.Usuarios.Perfis
{
    public abstract class Perfil
    {
        #region Propriedades
        [JsonIgnore]
        public Dictionary<string, ConexaoExterna> ConexoesDoUsuario { get; } = [];

        protected UsuarioLogavel UsuarioLogavel { get; set; }
        public abstract string NomePerfil { get; }
        #endregion Propriedades


        #region Construtores
        public Perfil(UsuarioLogavel usuario)
        {
            UsuarioLogavel = usuario;
            definirConexoes();
        }
        #endregion Construtores


        #region Métodos
        protected abstract void definirConexoes();
        public abstract Usuario? Registrar();
        #endregion Métodos
    }
}
