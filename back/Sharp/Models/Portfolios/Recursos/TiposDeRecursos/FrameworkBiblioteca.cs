using Projeto.Models.Bancos.Tabelas;

namespace Projeto.Models.Portfolios.Recursos.TiposDeRecursos
{
    public abstract class FrameworkBiblioteca : Recurso
    {
        #region Propriedades
        public Tabela Tabela => new TabelaComTipo<Linguagem>("framework_lib");
        public string Id { get; }
        public string Nome { get; }
        public string IdLinguagem { get; }
        #endregion Propriedades
    }
}
