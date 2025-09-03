using Projeto.Models.Bancos.Tabelas;
using Projeto.Models.Usuarios.Login;
using Projeto.Models.Usuarios.Perfis;

namespace Projeto.Models.Usuarios
{
    public abstract class UsuarioLogavel : Usuario, ILogavel
    {
        public abstract IPerfil Perfil { get; set; }

        public abstract bool Logar();

        public abstract bool Registrar();
    }
}
