using Newtonsoft.Json;
using Sharp.Models.Bancos.Tabelas;

namespace Sharp.Models.Portfolios.Recursos
{
    public interface Recurso
    {
        #region Propriedades
        [JsonIgnore]
        static Tabela Tabela { get; }
        [JsonIgnore]
        string Id { get; }

        string Nome { get; }
        string Tipo { get; }
        #endregion Propriedades
    }
}
