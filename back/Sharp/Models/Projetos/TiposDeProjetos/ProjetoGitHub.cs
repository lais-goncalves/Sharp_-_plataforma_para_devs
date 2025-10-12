using Sharp.Models.ConexoesExternas.TiposDeConexoes;
using Sharp.Models.Portfolios.Recursos;
using Sharp.Models.Usuarios;

namespace Sharp.Models.Projetos.TiposDeProjetos
{
    public class ProjetoGitHub : BaseProjeto
    {
        #region Propriedades
        public static string? CLIENT_ID = ConexaoGitHub.CLIENT_ID;
        public static string? CLIENT_SECRET = ConexaoGitHub.CLIENT_SECRET;

        public new static string TipoProjeto => "github";
        #endregion Propriedades


        #region Construtores
        public ProjetoGitHub(string Id, string? Nome, string? Descricao, string? Status, string? Tipo = null) : base(Id, Nome, Descricao, Tipo, Status) { }

        public ProjetoGitHub() : base() { }
        #endregion Construtores


        #region Métodos
        protected override void BuscarFerramentas()
        {
            // TODO: continuar
        }

        protected override void BuscarDemaisInformacoes()
        {
            // TODO: continuar
        }
        #endregion Métodos
    }
}
