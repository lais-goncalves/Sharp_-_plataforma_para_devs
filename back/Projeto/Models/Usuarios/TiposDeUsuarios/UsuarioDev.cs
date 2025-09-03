using Newtonsoft.Json;
using Projeto.Models.Bancos.Tabelas;
using Projeto.Models.Usuarios.Contas;
using Projeto.Models.Usuarios.Perfis;
using Projeto.Models.Usuarios.Perfis.TiposDePerfis;

namespace Projeto.Models.Usuarios.TiposDeUsuarios
{
    public class UsuarioDev : UsuarioLogavel
    {
        public override IPerfil Perfil { get; set; } = new PerfilDev();

        public UsuarioDev(int id, string? nomeCompleto, string? email, string? apelido, string? senha)
        {
            Id = id;
            NomeCompleto = nomeCompleto;
            Email = email;
            Apelido = apelido;
            Senha = senha;
        }

        public override bool Logar()
        {
            throw new NotImplementedException();
        }

        public override bool Registrar()
        {
            throw new NotImplementedException();
        }
    }
}
