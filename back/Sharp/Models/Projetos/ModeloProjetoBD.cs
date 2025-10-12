using Newtonsoft.Json;
using Sharp.Models.Bancos.Tabelas;
using Sharp.Models.Portfolios.Recursos;

namespace Sharp.Models.Projetos
{
    public class ModeloProjetoBD
    {
        #region Propriedades  
        [JsonIgnore]
        public string? Id { get; protected set; }

        [JsonProperty("tipo")]
        public string? Tipo { get; protected set; }
        public string? Nome { get; protected set; }
        public string? Descricao { get; protected set; }
        public string? Status { get; protected set; }
        #endregion Propriedades

        #region Construtores  
        public ModeloProjetoBD(string Id, string? Nome, string? Descricao, string? Tipo, string? Status)
        {
            this.Id = Id;
            this.Nome = Nome;
            this.Descricao = Descricao;
            this.Tipo = Tipo;
            this.Status = Status;
        }
        #endregion Construtores 
    }
}
