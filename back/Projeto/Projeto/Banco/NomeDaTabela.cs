namespace Projeto.Banco
{
    public class NomeDaTabela : Attribute
    {
        public string Nome { get; set; }

        public NomeDaTabela(string Nome)
        {
            this.Nome = Nome;
        }
    }
}
