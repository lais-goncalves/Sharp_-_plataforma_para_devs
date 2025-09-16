using Newtonsoft.Json;
using Npgsql;
using Projeto.Models.Bancos.Tabelas;
using Projeto.Models.Usuarios.Contas;
using Projeto.Models.Usuarios.Contas.TiposDeContas;

namespace Projeto.Models.Usuarios.Perfis.TiposDePerfis
{
    public class PerfilDev : Perfil
    {
        #region Propriedades
        [JsonIgnore]
        public Tabela Tabela => new Tabela("perfil_dev");
        [JsonIgnore]
        public override string NomePerfil { get; } = "dev";
        [JsonIgnore]
        public Dictionary<string, Conta> ContasDoUsuario { get; } = [];

        #endregion Propriedades

        #region Construtores
        public PerfilDev(UsuarioLogavel usuario) : base(usuario)
        {
            definirContas();
        }
        #endregion Construtores


        #region Métodos
        private void definirContas()
        {
            ContasDoUsuario.Clear();
            ContasDoUsuario.Add("github", new ContaGitHub(UsuarioLogavel));
        }

        public override UsuarioLogavel? Registrar()
        {
            // TODO: implementar registro
            return default;
        }

        public static Usuario? BuscarPorIdGitHub(string idGitHub)
        {
            NpgsqlParameter paramApelido = new("@id_github", idGitHub);
            string comando = string.Concat("SELECT * FROM ", Usuario.Tabela.NomeTabela, " WHERE id_github = @id_github");

            Usuario? usuario = Usuario.Tabela.conexao.ExecutarUnico<Usuario>(comando, [paramApelido]);
            return usuario;
        }
        #endregion Métodos
    }
}
