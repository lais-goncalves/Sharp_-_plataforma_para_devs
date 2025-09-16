using Npgsql;
using Projeto.Models.Bancos.Tabelas;

namespace Projeto.Models.Portfolios.Recursos.TiposDeRecursos
{
    public abstract class Linguagem : Recurso
    {
        #region Propriedades
        public static Tabela Tabela => new TabelaComTipo<Linguagem>("linguagem");
        public string Id { get; }
        public string Nome { get; }
        #endregion Propriedades

        #region Construtores
        public Linguagem(string id, string nome) 
        {
            Id = id;
            Nome = nome;
        }
        #endregion Construtores
    }
}
