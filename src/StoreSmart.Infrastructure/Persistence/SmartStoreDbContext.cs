using Microsoft.EntityFrameworkCore;
using StoreSmart.Domain.Entities;

namespace StoreSmart.Infrastructure.Persistence;

public class SmartStoreDbContext: DbContext
{
    public SmartStoreDbContext(DbContextOptions<SmartStoreDbContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SmartStoreDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}