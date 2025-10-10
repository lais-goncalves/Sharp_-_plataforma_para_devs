using Newtonsoft.Json;
using Npgsql;
using Sharp.Models.Bancos.Tabelas;
using Sharp.Models.Portfolios.Recursos;
using Sharp.Models.Usuarios;

namespace Sharp.Models.Portfolios.Projetos
{
    public abstract class BaseProjeto
    {
        #region Propriedades  
        [JsonIgnore]
        public static TabelaComTipo<BaseProjeto> Tabela => new TabelaComTipo<BaseProjeto>("projeto");
        [JsonIgnore]
        public string? Id { get; }
        [JsonIgnore]
        public static string? _Tipo => null;

        public string? Tipo { get; protected set; }
        public string? Nome { get; }
        public string? Descricao { get; }
        public string? Status { get; }
        public List<Recurso>? Ferramentas { get; }
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
        #endregion Construtores  

        #region Métodos  
        protected static List<BaseProjeto>? BuscarProjetosDoTipo(Usuario usuario)
        {
            NpgsqlParameter paramIdUsuario = new("@param_id_usuario", usuario.Id);
            NpgsqlParameter paramTipoProjeto = new("@param_tipo_projeto", _Tipo);

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
