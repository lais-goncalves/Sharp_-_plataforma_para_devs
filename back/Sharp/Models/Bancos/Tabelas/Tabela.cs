using System.Linq;
using Npgsql;
using Sharp.Models.Usuarios;

namespace Sharp.Models.Bancos.Tabelas
{
    public class Tabela(string nomeTabela)
    {
        public ConexaoBanco conexao { get; set; } = ConexaoBanco.instancia;
        public string NomeTabela { get; set; } = nomeTabela;
    }
}
