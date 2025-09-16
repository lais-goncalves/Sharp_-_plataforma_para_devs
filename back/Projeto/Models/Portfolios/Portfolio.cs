using Newtonsoft.Json;
using Projeto.Models.Usuarios;
using Projeto.Models.Portfolios.Projetos;
using Projeto.Models.Bancos.Tabelas;
using Npgsql;

namespace Projeto.Models.Portfolios
{
    public class Portfolio
    {
        #region Propriedades
        [JsonIgnore]
        public static TabelaComTipo<Portfolio> Tabela = new TabelaComTipo<Portfolio>("portfolio");

        [JsonIgnore]
        UsuarioLogavel? UsuarioLogavel { get; set; }

        public string Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        #endregion Propriedades


        #region Construtores
        public Portfolio(UsuarioLogavel usuarioLogavel)
        {
            UsuarioLogavel = usuarioLogavel;  
        }

        public Portfolio(string id, string descricao)
        {
            Id = id;
            Descricao = descricao;
        }
        #endregion Construtores


        #region Métodos
        public static Portfolio? BuscarPorUsuario(string idUsuario)
        {
            NpgsqlParameter paramIdUsuario = new NpgsqlParameter("@id_usuario", idUsuario);
            string query = string.Concat("SELECT * FROM ", Tabela.NomeTabela, "WHERE id_usuario = ", paramIdUsuario.ParameterName);

            Portfolio? resultado = Tabela.BuscarUnico<Portfolio>(query);
            return resultado;
        }

        public Portfolio BuscarInformacoesDoBanco()
        {
            // TODO: continuar
            return this;
        }

        public List<ProjetoPortfolio> BuscarProjetos()
        {
            return default;
        }
        #endregion Métodos
    }
}
