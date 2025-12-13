using Newtonsoft.Json;
using Npgsql;
using Sharp.Models.Bancos.Tabelas;
using Sharp.Models.Projetos;
using Sharp.Models.Usuarios;

namespace Sharp.Models.Portfolios
{
    public class Portfolio
    {
        #region Propriedades  
        [JsonIgnore] public static TabelaComTipo<Portfolio> Tabela = new TabelaComTipo<Portfolio>("portfolio");
        [JsonIgnore] UsuarioLogavel UsuarioLogavel { get; set; }

        public string? Id { get; set; }
        public string? Descricao { get; set; } = string.Empty;

        FabricaDeProjetos fabrica = new FabricaDeProjetos();
        #endregion Propriedades  


        #region Construtores  
        public Portfolio(UsuarioLogavel usuarioLogavel)
        {
            UsuarioLogavel = usuarioLogavel;
        }
        #endregion Construtores  
        
        #region Construtores
        
        #endregion Construtores

        #region Métodos
        protected List<dynamic> BuscarInfoProjetosDoBanco()
        {
            try
            {
                NpgsqlParameter paramIdUsuario = new NpgsqlParameter("@param_id_usuario", UsuarioLogavel.Id);
                string nomeFuncao = "buscar_projetos_usuario";

                List<dynamic> projetos = BaseProjeto.Tabela.conexao.ExecutarFunction<dynamic>(nomeFuncao, [paramIdUsuario]) ?? [];

                return projetos;
            }

            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        protected List<BaseProjeto> ConverterListaDynamicParaProjeto(List<dynamic> listaProjetosDynamic)
        {
            List<BaseProjeto> projetosFinalizados = new List<BaseProjeto>();
            foreach (dynamic projetoDynamic in listaProjetosDynamic)
            {
                BaseProjeto? projeto = fabrica.CriarECarregarDadosProjeto(projetoDynamic);
                if (projeto != null)
                {
                    projetosFinalizados.Add(projeto);
                }
            }

            return projetosFinalizados;
        }

        public List<BaseProjeto> BuscarProjetos()
        {
            try
            {
                List<dynamic>? projetosEncontrados = BuscarInfoProjetosDoBanco();

                List<BaseProjeto> projetosFinalizados = ConverterListaDynamicParaProjeto(projetosEncontrados);
                projetosEncontrados.Clear();

                projetosFinalizados.ForEach(p => p.BuscarTodasAsInformacoes());

                return projetosFinalizados;
            }

            catch (Exception)
            {
                throw new Exception("Ocorreu um problema ao buscar os projetos do usuário.");
            }
        }
        #endregion Métodos  
    }
}
