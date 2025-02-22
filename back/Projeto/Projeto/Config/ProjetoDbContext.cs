using Microsoft.EntityFrameworkCore;
using Projeto.Models;

namespace Projeto.Config
{
    public class ProjetoDbContext : DbContext
    {
        public ProjetoDbContext() : base() { }

        public ProjetoDbContext(DbContextOptions<ProjetoDbContext> opcoes) : base(opcoes) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        }

        public virtual DbSet<Usuario> Usuario { get; set; }
    }
}
