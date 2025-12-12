using Npgsql;
using Sharp.Models.Bancos;
using Sharp.Models.Session;
using Sharp.Models.Usuarios;

namespace Sharp.Models.Autenticacao;

public class AutenticacaoSharp : Autenticacao
{
	#region Construtores
	public AutenticacaoSharp(UsuarioAtual usuarioAtual) : base(usuarioAtual) { }
	#endregion Construtores
	
	#region Métodos
	public Usuario? BuscarUsuarioEAutenticar(string emailOuApelido, string senha)
	{
		if (UsuarioAtual.EstaLogado)
		{
			return UsuarioAtual;
		}
		
		const string function = "logar_usuario";
		NpgsqlParameter paramEmailOuApelido = new("@param_email_ou_apelido", emailOuApelido);
		NpgsqlParameter paramSenha = new("@param_senha", senha);

		List<Usuario>? listaUsuarioAutenticado = ConexaoBanco.ExecutarFunction<Usuario>(function, [
			paramEmailOuApelido, paramSenha
		])?.ToList();

		Usuario? usuarioAutenticado = listaUsuarioAutenticado?.FirstOrDefault();
		if (usuarioAutenticado?.Id == null)
		{
			throw new Exception("Apelido/Email ou senha incorretos.");
		}
		
		LogarUsuario(usuarioAutenticado);
		return usuarioAutenticado;
	}
	
	public void CadastrarUsuario(string email, string nome_completo, string apelido, string senha, string tipoPerfil)
	{
		try
		{
			NpgsqlParameter paramNomeCompleto = new("@param_nome_completo", nome_completo);
			NpgsqlParameter paramEmail = new("@param_email", email);
			NpgsqlParameter paramApelido = new("@param_apelido", apelido);
			NpgsqlParameter paramSenha = new("@param_senha", senha);
			NpgsqlParameter paramTipoPerfil = new("@param_nome_tipo_perfil", tipoPerfil);
			string procedure = string.Concat("cadastrar_usuario");

			ConexaoBanco.ExecutarProcedure(
			                            procedure,
			                            [
				                            paramNomeCompleto,
				                            paramEmail,
				                            paramApelido,
				                            paramSenha,
				                            paramTipoPerfil
			                            ]);
		}

		catch (Exception)
		{
			throw new Exception("Não foi possível cadastrar. Tente novamente.");
		}
	}
	#endregion Métodos
}