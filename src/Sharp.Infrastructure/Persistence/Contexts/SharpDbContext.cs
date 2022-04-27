using Microsoft.EntityFrameworkCore;
using Sharp.Infrastructure.Persistence.Models;

namespace Sharp.Infrastructure.Persistence.Contexts;

/// <summary>
///     Simple DB Context for the whole player.
/// </summary>
public class SharpDbContext : DbContext
{
    private readonly string _dbPath;

    public SharpDbContext()
    {
        // var folder = Environment.SpecialFolder.LocalApplicationData;
        // var path = Environment.GetFolderPath(folder);
        // DbPath = Path.Join(path, "database.db");
        _dbPath = "database.db";
    }

    public DbSet<PlayerDetails> PlayerDetails { get; set; }
    public DbSet<GameRegistration> GameRegistrations { get; set; }
    public DbSet<CommandTransaction> CommandTransactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // We use SQLite because we have a semi-peristent store this way.
        // A memory store would also be thinkable but wouldn't offer much
        // benefit when the player crashes while a whole external database
        // system would be a overkill.
        optionsBuilder
            .UseSqlite($"Data Source={_dbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // We simply use a composite key of name and email because the player Id is fired at a later point...
        // TODO: This will probably be refactored in the game service, therefore we need to touch this again.
        modelBuilder.Entity<PlayerDetails>()
            .HasKey(nameof(Models.PlayerDetails.PlayerId));
        modelBuilder.Entity<PlayerDetails>()
            .HasIndex(p => new { p.Email, p.Name })
            .IsUnique(true);

        modelBuilder.Entity<GameRegistration>()
            .HasKey(nameof(GameRegistration.GameId));

        modelBuilder.Entity<CommandTransaction>()
            .HasKey(nameof(CommandTransaction.TransactionId));
    }
}