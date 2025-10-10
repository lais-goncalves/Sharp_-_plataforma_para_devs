using Sharp.Models.ConexoesExternas.TiposDeConexoes;
using Sharp.Models.Portfolios.Recursos;
using Sharp.Models.Usuarios;

namespace Sharp.Models.Portfolios.Projetos.TiposDeProjetos
{
    public class ProjetoGitHub : BaseProjeto
    {
        #region Propriedades
        public static string? CLIENT_ID = ConexaoGitHub.CLIENT_ID;
        public static string? CLIENT_SECRET = ConexaoGitHub.CLIENT_SECRET;

        public static new string? _Tipo = "github";
        #endregion Propriedades


        #region Construtores
        public ProjetoGitHub(string Id, string? Nome, string? Descricao, string? Status) : base(Id, Nome, Descricao, _Tipo, Status) {}
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
