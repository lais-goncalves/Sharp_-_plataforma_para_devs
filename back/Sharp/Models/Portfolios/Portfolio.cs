using Newtonsoft.Json;
using Sharp.Models.Usuarios;
using Sharp.Models.Portfolios.Projetos;
using Sharp.Models.Bancos.Tabelas;
using Npgsql;

namespace Sharp.Models.Portfolios
{
    public class Portfolio
    {
        #region Propriedades
        [JsonIgnore]
        public static TabelaComTipo<Portfolio> Tabela = new TabelaComTipo<Portfolio>("portfolio");

        [JsonIgnore]
        UsuarioLogavel UsuarioLogavel { get; set; }

        public string? Id { get; set; }
        public string? Descricao { get; set; } = string.Empty;
        #endregion Propriedades


        #region Construtores
        public Portfolio(UsuarioLogavel usuarioLogavel)
        {
            UsuarioLogavel = usuarioLogavel;  
        }
        #endregion Construtores


        #region Métodos
        //public static Portfolio? BuscarPorUsuario(string idUsuario)
        //{
        //    NpgsqlParameter paramIdUsuario = new NpgsqlParameter("@id_usuario", idUsuario);
        //    string query = string.Concat("SELECT * FROM ", Tabela.NomeTabela, "WHERE id_usuario = ", paramIdUsuario.ParameterName);

        //    Portfolio? resultado = Tabela.BuscarUnico<Portfolio>(query);
        //    return resultado;
        //}

        //public Portfolio BuscarInformacoesDoBanco()
        //{
        //    // TODO: continuar
        //    return this;
        //}

        protected List<dynamic?>? BuscarInfoProjetosDoBanco()
        {
            try
            {
                NpgsqlParameter paramIdUsuario = new NpgsqlParameter("@param_id_usuario", UsuarioLogavel.Id);
                string nomeFuncao = "buscar_projetos_usuario";

                List<dynamic?>? projetos = Projeto.Tabela.conexao.ExecutarFunction<dynamic>(nomeFuncao, [paramIdUsuario]);

                return projetos;
            }

            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public List<BaseProjeto?> BuscarProjetos()
        {
            try
            {
                List<dynamic?>? projetosEncontrados = BuscarInfoProjetosDoBanco();
                List<BaseProjeto?> projetosFinalizados = new List<BaseProjeto?>();

                FabricaDeProjetos fabrica = new FabricaDeProjetos();

                while (projetosEncontrados?.Count > 0)
                {
                    dynamic? projetoEncontrado = projetosEncontrados[0];

                    if (projetoEncontrado != null)
                    {
                        BaseProjeto? projeto = fabrica.CriarProjeto(projetoEncontrado);

                        if (projeto != null)
                        {
                            projetosFinalizados.Add(projeto);
                        }
                    }
                    
                    projetosEncontrados.RemoveAt(0);
                }

                return projetosFinalizados;
            }

            catch (Exception err)
            {
                throw new Exception("Ocorreu um problema ao buscar os projetos do usuário.");
            }
        }
        #endregion Métodos
    }
}
