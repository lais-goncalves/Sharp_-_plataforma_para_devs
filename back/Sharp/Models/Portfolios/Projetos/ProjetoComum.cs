using Newtonsoft.Json;
using Sharp.Models.Bancos.Tabelas;
using Sharp.Models.ConexoesExternas.TiposDeConexoes;
using Sharp.Models.Portfolios.Recursos;
using Sharp.Models.Usuarios;

namespace Sharp.Models.Portfolios.Projetos
{
    public class Projeto : BaseProjeto
    {
        #region Construtores
        public Projeto(string Id, string? Nome, string? Descricao, string? Status)
            : base(Id, Nome, Descricao, null, Status) {}
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

        public virtual void BuscarTodasAsInformacoes()
        {
            BuscarDemaisInformacoes();
            BuscarFerramentas();
        }
        #endregion Métodos
    }
}
