using Projeto.Models.Bancos.Tabelas;

namespace Projeto.Models.Portfolios.Recursos.TiposDeRecursos
{
    public abstract class Ferramenta : Recurso
    {
        #region Propriedades
        public Tabela Tabela => new TabelaComTipo<Linguagem>("ferramenta");
        public string Id { get; }
        public string Nome { get; }
        #endregion Propriedades
    }
}
