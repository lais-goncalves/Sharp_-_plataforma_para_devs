using System.Text.Json.Serialization;
using Projeto.Dados;

namespace Projeto.Models.Perfil
{
    public interface IPerfil
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
