using Microsoft.EntityFrameworkCore;
using Sharp.Data.Model;

namespace Sharp.Data.Context;

public class SharpDbContext : DbContext
{
    public DbSet<PlayerDetails> PlayerDetails { get; set; }
    public string DbPath { get; }
    
    public SharpDbContext()
    {
        // var folder = Environment.SpecialFolder.LocalApplicationData;
        // var path = Environment.GetFolderPath(folder);
        // DbPath = Path.Join(path, "database.db");
        DbPath = "database.db";
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={DbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // TODO: We simply use a composite key of name and email until we figured out WHAT exactly the ID is? Is it
        //  maybe provided in an event?
        modelBuilder.Entity<PlayerDetails>()
            .HasKey(nameof(Data.Model.PlayerDetails.Name), nameof(Data.Model.PlayerDetails.Email));
    }
}