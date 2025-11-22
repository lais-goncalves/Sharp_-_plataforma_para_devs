namespace Sharp.Models.Projetos.TiposDeProjetos
{
    public class ProjetoComum : BaseProjeto
    {
        #region Construtores
        public ProjetoComum(string Id, string? Nome, string? Descricao, string? Status, string? Tipo = null) : base(Id, Nome, Descricao, Tipo, Status) { }

        public ProjetoComum() : base() { }
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
