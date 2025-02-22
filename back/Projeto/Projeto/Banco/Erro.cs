namespace Projeto.Banco
{
    public class Erro : Exception
    {
        public bool DoBanco { get; set; }

        public Erro (string? mensagem) : base(mensagem) { }
    }
}
