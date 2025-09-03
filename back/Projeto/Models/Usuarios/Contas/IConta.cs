using System.Text.Json.Serialization;

namespace Projeto.Models.Usuarios.Contas
{
    public interface IConta
    {
        [JsonIgnore]
        protected int IdPerfilSharp { get; }
        [JsonIgnore]
        protected string? Id { get; }
        protected string? Apelido { get; }

        public bool Existe => Id is not null;

        protected abstract void BuscarInfoDaFonte();

        protected abstract void BuscarInfoDoBanco();

        protected virtual void BuscarTodasAsInfomacoes()
        {
            BuscarInfoDoBanco();
            BuscarInfoDaFonte();
        }
    }
}
