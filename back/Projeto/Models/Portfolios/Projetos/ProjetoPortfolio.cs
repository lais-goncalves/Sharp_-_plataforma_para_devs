using Projeto.Models.Bancos.Tabelas;
using Projeto.Models.Portfolios.Recursos.TiposDeRecursos;

namespace Projeto.Models.Portfolios.Projetos
{
    public class ProjetoPortfolio
    {
        #region Propriedades
        public TabelaComTipo<ProjetoPortfolio> Tabela => new TabelaComTipo<ProjetoPortfolio>("projeto");

        public string Id { get; }
        public string Nome { get; }
        public string Descricao { get; }
        public List<Linguagem> Linguagens { get; }
        public List<Ferramenta> Ferramentas { get; }
        // TODO: implementar links
        #endregion Propriedades


        #region Construtores
        public ProjetoPortfolio(string id, string nome, string descricao, List<Linguagem> linguagens, List<Ferramenta> ferramentas)
        {
            Id = id;
            Nome = nome;
            Descricao = descricao;
            Linguagens = linguagens;
            Ferramentas = ferramentas;
        }
        #endregion Construtores


        #region Métodos

        #endregion Métodos
    }
}
