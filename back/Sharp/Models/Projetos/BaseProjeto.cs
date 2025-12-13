using Newtonsoft.Json;
using Npgsql;
using Sharp.Models.Bancos.Tabelas;
using Sharp.Models.Portfolios.Recursos;
using Sharp.Models.Usuarios;

namespace Sharp.Models.Projetos
{
    public abstract class BaseProjeto
    {
        #region Propriedades  
        [JsonIgnore]
        public static TabelaComTipo<BaseProjeto> Tabela => new TabelaComTipo<BaseProjeto>("projeto");

        [JsonIgnore]
        public string? Id { get; set; }

        [JsonIgnore]
        protected static string TipoProjeto => "";

        [JsonProperty("tipo")]
        public string? Tipo { get; set; }
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public string? Status { get; set; }
        public int? Estrelas { get; set; } = 5;
        public string[] Tecnologias { get; set; } = ["C#", "React.js", ".NET", "JavaScript", "PostgreSQL"];
        #endregion Propriedades  

        #region Construtores  
        public BaseProjeto(string Id, string? Nome, string? Descricao, string? Tipo, string? Status)
        {
            this.Id = Id;
            this.Nome = Nome;
            this.Descricao = Descricao;
            this.Tipo = Tipo;
            this.Status = Status;
        }

        public BaseProjeto() { }
        #endregion Construtores  

        #region Métodos  
        protected static List<BaseProjeto>? BuscarProjetosCadastradosDoTipo(Usuario usuario)
        {
            NpgsqlParameter paramIdUsuario = new("@param_id_usuario", usuario.Id);
            NpgsqlParameter paramTipoProjeto = new("@param_tipo_projeto", TipoProjeto);

            string function = "buscar_projetos_usuario_por_tipo";

            List<BaseProjeto>? resultado = Tabela.conexao.ExecutarFunction<BaseProjeto>(function, new List<NpgsqlParameter> { paramIdUsuario, paramTipoProjeto });

            return resultado;
        }

        protected abstract void BuscarFerramentas();

        protected abstract void BuscarDemaisInformacoes();

        public virtual void BuscarTodasAsInformacoes()
        {
            BuscarDemaisInformacoes();
            BuscarFerramentas();
        }
        #endregion Métodos  
    }
}
