using Projeto.Models.Bancos.Tabelas;

namespace Projeto.Models.Portfolios.Recursos
{
    public interface Recurso
    {
        #region Propriedades
        static Tabela Tabela { get; }
        string Id { get; }
        string Nome { get; }
        #endregion Propriedades
    }
}
