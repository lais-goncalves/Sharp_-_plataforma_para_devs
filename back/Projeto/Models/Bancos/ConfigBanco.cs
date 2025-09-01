using Projeto.Models.Usuarios;

namespace Projeto.Models.Bancos
{
    public static class ConfigBanco
    {
        public static Dictionary<string, ITabelaBanco> Tabelas = DefinirTabelas();

        private static Dictionary<string, ITabelaBanco> DefinirTabelas()
        {
            Dictionary<string, ITabelaBanco> tabelas = new()
               {
                   { "usuario", new TabelaBanco<Usuario>("Usuario") },
                   { "post", new TabelaBanco<Usuario>("Post") }
               };

            return tabelas;
        }

        public static TabelaBanco<T> BuscarTabela<T>(string nomeTabela) where T : class
        {
            if (Tabelas.TryGetValue(nomeTabela, out ITabelaBanco? value))
            {
                return (TabelaBanco<T>) value;
            }
            else
            {
                throw new Exception($"Tabela '{nomeTabela}' não encontrada.");
            }
        }
    }
}
