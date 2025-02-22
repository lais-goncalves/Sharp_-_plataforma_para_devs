using Microsoft.Data.SqlClient;

namespace Projeto.Banco
{
    public class Resultado<T>
    {
        public T? Item { get; set; } = default;
        public Exception? Erro { get; set; } = null;

        public Resultado() { }

        public Resultado(T? item, Exception? erro = null)
        {
            Item = item;
            Erro = erro;
        }

        public Resultado(T? item, string? msgErro = null)
        {
            Item = item;
            Erro = msgErro != null ? new Exception(msgErro) : null;
        }
    }
}
