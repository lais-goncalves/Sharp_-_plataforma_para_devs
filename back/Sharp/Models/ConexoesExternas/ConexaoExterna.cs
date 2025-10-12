using System.Reflection;
using System.Text.Json.Serialization;
using Npgsql;
using Sharp.Models.Bancos;
using Sharp.Models.Bancos.Tabelas;
using Sharp.Models.Usuarios;
using Sharp.Models.Session.SessionAtual;

namespace Sharp.Models.ConexoesExternas
{
    public abstract class ConexaoExterna
    {
        #region Propriedades
        [JsonIgnore]
        static TabelaComTipo<ConexaoExterna> Tabela = new TabelaComTipo<ConexaoExterna>("Tipo_Conexao");
        [JsonIgnore]
        static TabelaComTipo<ConexaoExterna> TabelaRelacionamento = new TabelaComTipo<ConexaoExterna>("Conexao_Usuario");
        [JsonIgnore]
        protected UsuarioLogavel UsuarioLogavel { get; }
        [JsonIgnore]
        public string? IdLogin { get; protected set; }

        public abstract string? NomeDaConexao { get; protected set; }
        #endregion Propriedades


        #region Construtores
        public ConexaoExterna(UsuarioLogavel usuarioLogavel)
        {
            UsuarioLogavel = usuarioLogavel;

            if (UsuarioLogavel.EstaLogado())
            {
                BuscarInformacoesDoBanco();
            }
        }
        #endregion Construtores


        #region Métodos
        public virtual bool Existe()
        {
            return !string.IsNullOrEmpty(IdLogin);
        }

        public Usuario? BuscarUsuarioPorIdDeLogin(string id)
        {
            try
            {
                NpgsqlParameter paramId = new NpgsqlParameter("@param_id_login_conexao", id);
                NpgsqlParameter paramNomeConta = new NpgsqlParameter("@param_nome_conexao", NomeDaConexao);
                string nomeProcedure = "buscar_usuario_por_id_login_conexao";

                dynamic? resultadoId = Tabela.conexao.ExecutarFunctionUnica<dynamic>(nomeProcedure, [paramId, paramNomeConta]);
                if (resultadoId == null)
                {
                    return null;
                }

                string idString = ConexaoBanco.BuscarPropriedadeDynamic<string>(resultadoId, "id");
                Usuario? usuarioEncontrado = Usuario.BuscarPorId(idString);
                return usuarioEncontrado;
            }

            catch (Exception err)
            {
                Console.WriteLine(err);
                return null;
            }
        }

        public virtual bool DefinirIdNoBanco(string id)
        {
            try
            {
                NpgsqlParameter paramId = new NpgsqlParameter("@param_id_usuario", UsuarioLogavel?.Id);
                NpgsqlParameter paramNomeConta = new NpgsqlParameter("@param_nome_conexao", NomeDaConexao);
                NpgsqlParameter paramIdConta = new NpgsqlParameter("@param_id_login_conexao", id);

                string procedure = "inserir_id_login_conexao";
                Tabela.conexao.ExecutarProcedure(procedure, [paramId, paramNomeConta, paramIdConta]);

                BuscarTodasAsInfomacoes();
                return true;
            }

            catch (Exception)
            {
                Console.WriteLine("Um erro ocorreu ao tentar buscar as informações da ConexaoExterna. Tente logar novamente.");
                return false;
            }
        }

        public abstract void BuscarInformacoesDaFonte();

        public virtual void BuscarInformacoesDoBanco()
        {
            NpgsqlParameter paramIdUsuario = new("@param_id_usuario", UsuarioLogavel.Id);
            NpgsqlParameter paramNomeConta = new("@param_nome_conexao", NomeDaConexao);

            string function = "buscar_id_login_conexao";
            dynamic? resultado = TabelaRelacionamento.conexao.ExecutarFunctionUnica<dynamic>(function, [paramIdUsuario, paramNomeConta]);

            IdLogin = ConexaoBanco.BuscarPropriedadeDynamic<string>(resultado, "id_conexao");
        }

        public virtual ConexaoExterna BuscarTodasAsInfomacoes()
        {
            if (UsuarioLogavel.EstaLogado())
            {
                BuscarInformacoesDoBanco();
                BuscarInformacoesDaFonte();
            }

            return this;
        }
        #endregion Métodos
    }
}
