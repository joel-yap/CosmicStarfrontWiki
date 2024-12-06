using CosmicStarfrontWiki.Model;
using Microsoft.EntityFrameworkCore;

namespace CosmicStarfrontWiki.Data
{
    public class AppDbContext : DbContext
    { 
        public DbSet<Faction> Factions { get; set; } 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { optionsBuilder.UseSqlite("Data Source=..\\CosmicStarfrontWiki.Data\\app.db"); } 
    }
}
