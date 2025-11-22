using Newtonsoft.Json;
using Npgsql;
using Sharp.Models.Bancos.Tabelas;
using Sharp.Models.ConexoesExternas;
using Sharp.Models.ConexoesExternas.TiposDeConexoes;

namespace Sharp.Models.Usuarios.Perfis.TiposDePerfis
{
    public class PerfilDev : Perfil
    {
        #region Propriedades
        [JsonIgnore]
        public Tabela Tabela => new Tabela("perfil_dev");

        [JsonIgnore]
        public override string NomePerfil { get; } = "dev";
        #endregion Propriedades


        #region Construtores
        public PerfilDev(UsuarioLogavel usuario) : base(usuario) { }
        #endregion Construtores


        #region Métodos
        protected override void definirConexoes()
        {
            ConexoesDoUsuario.Clear();
            ConexoesDoUsuario.Add("github", new ConexaoGitHub(UsuarioLogavel));
        }
        #endregion Métodos
    }
}
