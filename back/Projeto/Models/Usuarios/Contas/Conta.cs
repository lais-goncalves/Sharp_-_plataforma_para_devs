using System.Reflection;
using System.Text.Json.Serialization;
using Projeto.SessionAtual;

namespace Projeto.Models.Usuarios.Contas
{
    public abstract class Conta
    {
        [JsonIgnore]
        protected UsuarioLogavel UsuarioLogavel { get; }
        [JsonIgnore]
        protected abstract string ColunaIdBanco { get; set; }
        [JsonIgnore]
        protected string? Id { get; set; }
        protected string? Apelido { get; set; }

        public Conta(UsuarioLogavel usuarioLogavel)
        {
            UsuarioLogavel = usuarioLogavel;
        }

        public bool Existe()
        {
            return !string.IsNullOrEmpty(Id);
        }

        public abstract void BuscarInformacoesDaFonte();

        public abstract void BuscarInformacoesDoBanco();

        public virtual Conta BuscarTodasAsInfomacoes()
        {
            BuscarInformacoesDoBanco();
            BuscarInformacoesDaFonte();

            return this;
        }
    }
}
