using Newtonsoft.Json;
using Projeto.Models.Usuarios.Contas;

namespace Projeto.Models.Usuarios.TiposDeUsuarios
{
    public class UsuarioDev : Usuario
    {
        private int? _id;

        [JsonIgnore]
        public int? Id
        {
            get => _id;
            set
            {
                _id = value;
                DefinirPerfis();
            }
        }

        [JsonIgnore]
        public ContaGitHub PerfilGitHub { get; set; }

        public UsuarioDev(int id, string? nomeCompleto, string? email, string? apelido, string? senha)
            : base(id, nomeCompleto, email, apelido, senha)
        {
            DefinirPerfis();
        }

        public UsuarioDev() : base()
        {
            DefinirPerfis();
        }

        public void DefinirPerfis()
        {
            PerfilGitHub = new ContaGitHub(Id);
        }
    }
}
