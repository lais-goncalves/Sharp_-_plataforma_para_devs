using System.Collections.Specialized;
using System.Net.Http.Headers;
using System.Web;
using Newtonsoft.Json;
using Sharp.Models.ConexoesExternas.TiposDeConexoes;
using Sharp.Models.Portfolios.Recursos;
using Sharp.Models.Usuarios;

namespace Sharp.Models.Projetos.TiposDeProjetos
{
    public class ProjetoGitHub : BaseProjeto
    {
        #region Propriedades
        public static string? CLIENT_ID = ConexaoGitHub.CLIENT_ID;
        public static string? CLIENT_SECRET = ConexaoGitHub.CLIENT_SECRET;
        public static string? urlApiGitHub = ConexaoGitHub.urlSite;

        public new static string TipoProjeto => "github";
        #endregion Propriedades


        #region Construtores
        public ProjetoGitHub(string Id, string? Nome, string? Descricao, string? Status, string? Tipo = null) : base(Id, Nome, Descricao, Tipo, Status) { }

        public ProjetoGitHub() : base() { }
        #endregion Construtores


        #region Métodos
        private static NameValueCollection? BuscarProjetosDaFonte(UsuarioLogavel usuario)
        {
            ConexaoGitHub conexaoGitHub = (ConexaoGitHub)usuario.Perfil.ConexoesDoUsuario["github"];
            conexaoGitHub.BuscarTodasAsInfomacoes();
            string? apelidoUsuarioGitHub = conexaoGitHub.Apelido;


            HttpClient clienteHttp = new();
            string urlBusca = $"{urlApiGitHub}/users/{apelidoUsuarioGitHub}/repos";
            HttpRequestMessage buscaPerfil = new(HttpMethod.Get, urlBusca);

            buscaPerfil.Headers.Add("Accept", "application/json");
            buscaPerfil.Headers.UserAgent.ParseAdd("Sharp");

            var retornoInfoUsuario = clienteHttp.Send(buscaPerfil);
            string resultadoBusca = retornoInfoUsuario.Content?.ReadAsStringAsync().Result ?? "";

            NameValueCollection? objResultado = HttpUtility.ParseQueryString(resultadoBusca);
            return objResultado;
        }

        public static List<ProjetoGitHub> BuscarTodosOsProjetos(UsuarioLogavel usuario)
        {
            List<ProjetoGitHub> projetos = new List<ProjetoGitHub>();
            NameValueCollection? resultadoBusca = BuscarProjetosDaFonte(usuario);

            if (resultadoBusca == null)
            {
                return projetos;
            }

            foreach (var objResultado in resultadoBusca)
            {
                var objetoParseado = JsonConvert.SerializeObject(objResultado);

                Console.WriteLine(objResultado.ToString());
            }

            return projetos;
        }

        protected override void BuscarFerramentas()
        {
            // TODO: continuar
        }

        protected override void BuscarDemaisInformacoes()
        {
            // TODO: continuar
        }
        #endregion Métodos
    }
}
