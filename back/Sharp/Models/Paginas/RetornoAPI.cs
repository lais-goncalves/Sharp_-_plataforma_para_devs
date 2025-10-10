using Newtonsoft.Json;

namespace Sharp.Models.Paginas
{
    public class RetornoAPI<T>(T? dados = default, string? erro = null)
    {
        public T? Dados { get; protected set; } = dados;

        public string? Erro { get; protected set; } = erro;

        public bool TemErro()
        {
            return Erro != null && Erro != string.Empty;
        }

        public bool TemDados()
        {
            return Dados != null;
        }

        public string DefinirErro(string mensagem)
        {
            Erro = mensagem;
            return Erro;
        }

        public string DefinirErro(Exception erro)
        {
            Erro = erro.Message;
            return Erro;
        }

        public T? DefinirDados(T? dados)
        {
            Dados = dados;
            return Dados;
        }
    }
}
