using Microsoft.EntityFrameworkCore;

namespace StoreSmart.Infrastructure.Persistence;

public class SmartStoreDbContext: DbContext
{
    public SmartStoreDbContext(DbContextOptions<SmartStoreDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SmartStoreDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}