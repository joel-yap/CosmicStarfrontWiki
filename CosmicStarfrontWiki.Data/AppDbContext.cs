using CosmicStarfrontWiki.Model;
using Microsoft.EntityFrameworkCore;

namespace CosmicStarfrontWiki.Data
{
    public class AppDbContext : DbContext
    { 
        public DbSet<WikiPage> WikiPages { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Content> Contents { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { optionsBuilder.UseSqlite("Data Source=..\\CosmicStarfrontWiki.Data\\app.db"); }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { // Configure relationships
          modelBuilder.Entity<Section>() .HasOne(s => s.WikiPage) .WithMany(wp => wp.Sections) .HasForeignKey(s => s.WikiPageId); 
          modelBuilder.Entity<Content>() .HasOne(c => c.Section) .WithMany(s => s.Contents) .HasForeignKey(c => c.SectionId); 
        }
    }
}
