using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<KlineData> KlineData { get; set; }
    public DbSet<BidAskTotal> BidAskTotal { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<KlineData>()
            .HasIndex(k => new { k.Symbol, k.Interval, k.OpenTime })
            .IsUnique();

        modelBuilder.Entity<BidAskTotal>()
           .HasIndex(k => new { k.TimeStamp })
           .IsUnique();
    }
}
