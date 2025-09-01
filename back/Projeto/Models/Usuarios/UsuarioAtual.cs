using Microsoft.AspNetCore.Http;
using Npgsql;
using Projeto.Dados;
using Projeto.Models.Session;

namespace Projeto.Models.Usuarios
{
    public class UsuarioAtual
    {
        private string nomeSession { get; set; } = "UsuarioLogado";
        private Sessao sessao { get; set; }
        public Usuario? Usuario
        {
            get => sessao.BuscarValor<Usuario>(nomeSession);
            set => sessao.DefinirValor(value);
        }

        public UsuarioAtual(HttpContext httpContext) : base()
        {
            sessao = new Sessao(httpContext, nomeSession);
        }

        public bool EstaLogado()
        {
            Usuario? usuario = sessao.BuscarValor<Usuario>(nomeSession);
            return usuario != null && usuario.Id > 0;
        }

        protected Usuario? BuscarLoginBanco(string emailOuApelido, string senha)
        {
            try
            {
                NpgsqlParameter paramApelido = new NpgsqlParameter("@emailOuApelido", emailOuApelido);
                NpgsqlParameter paramSenha = new NpgsqlParameter("@senha", senha);
                string comando = string.Concat("SELECT * FROM ", Usuario.tabela.NomeTabela, " WHERE apelido = @emailOuApelido OR email = @emailOuApelido AND senha = @senha");

                Usuario? usuario = Usuario.tabela.conexao.ExecutarUnico<Usuario>(comando, [paramApelido, paramSenha], true);

                return usuario;
            }

            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return null;
            }
        }

        public Usuario? Logar(string emailOuApelido, string senha)
        {
            try
            {
                Usuario? usuario = BuscarLoginBanco(emailOuApelido, senha);

                if (usuario is null)
                {
                    throw new Exception("Usuário ou senha inválidos.");
                }

                Usuario = usuario;
                sessao.DefinirValor(usuario);

                return usuario;
            }

            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return null;
            }
        }

        public void Deslogar()
        {
            Usuario = null;
            sessao.DefinirValor(null);
        }

        public bool Registrar(string apelido, string senha)
        {
            //RetornoAPI<bool> resultado = new();

            //try
            //{
            //    Usuario usuario = new Usuario(apelido, senha);

            //    if (JaExiste(apelido))
            //    {
            //        throw new Exception("Este apelido já está sendo usado. Tente outro.");
            //    }

            //    Entidades.Usuario.Add(usuario);
            //    Entidades.SaveChanges();
            //    resultado.Item = true;
            //}

            //catch (Exception err)
            //{
            //    resultado.Erro = err;
            //}

            //return resultado;

            return default;
        }
    }
}
