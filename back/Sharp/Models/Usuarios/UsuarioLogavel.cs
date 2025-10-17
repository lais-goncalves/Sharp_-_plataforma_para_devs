using Sharp.Models.Session;
using Sharp.Models.Usuarios.Perfis;
using Sharp.Models.Usuarios.Perfis.TiposDePerfis;

namespace Sharp.Models.Usuarios
{
    public class UsuarioLogavel : Usuario, ItemComSessao
    {
        public Perfil Perfil { get; set; }
        public Sessao Sessao { get; protected set; }

        public UsuarioLogavel(HttpContext httpContext) : base()
        {
            definirInformacoes(httpContext);
        }

        private void definirInformacoes(HttpContext httpContext)
        {
            Sessao = new Sessao(httpContext, "UsuarioLogavel");
            buscarUsuario();

            // TODO: criar verificador de perfil do usuário para definir de acordo com o que vem do bacno
            Perfil = new PerfilDev(this);
        }

        private void copiarNovoUsuario(Usuario? novoUsuario)
        {
            Id = novoUsuario?.Id;
            NomeCompleto = novoUsuario?.NomeCompleto;
            Email = novoUsuario?.Email;
            Senha = novoUsuario?.Senha;
            Apelido = novoUsuario?.Apelido;
        }

        private void definirNovoUsuario(Usuario? novoUsuario)
        {
            Sessao.DefinirValor(novoUsuario);
            copiarNovoUsuario(novoUsuario);
        }

        private Usuario? buscarUsuario()
        {
            Usuario? usuario = Sessao.BuscarValor<Usuario?>();
            copiarNovoUsuario(usuario);
            return usuario;
        }

        public bool EstaLogado()
        {
            return Id != null && Id > 0;
        }

        public Usuario? Logar(string? emailOuApelido, string? senha)
        {
            try
            {
                if (emailOuApelido is null || senha is null)
                {
                    return null;
                }


                Usuario? usuario = VerificarLogin(emailOuApelido, senha);
                definirNovoUsuario(usuario);

                return usuario;
            }

            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return null;
            }
        }

        public Usuario? Logar(Usuario? novoUsuario)
        {
            if (novoUsuario is null)
            {
                return null;
            }

            string? credencial = novoUsuario.Email ?? novoUsuario.Apelido;
            if (credencial is null)
            {
                return null;
            }

            return Logar(credencial, novoUsuario.Senha);
        }

        public void Deslogar()
        {
            definirNovoUsuario(null);
        }
    }
}
