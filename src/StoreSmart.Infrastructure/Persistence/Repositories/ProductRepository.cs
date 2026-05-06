using Microsoft.EntityFrameworkCore;
using StoreSmart.Application.Interfaces.Repositories;
using StoreSmart.Domain.Entities;

namespace StoreSmart.Infrastructure.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly SmartStoreDbContext _context;

    public ProductRepository(SmartStoreDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task<Product?> GetBySkuAsync(string sku, CancellationToken ct = default)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.SKU == sku.ToUpper(), ct);
    }

    public async Task<List<Product>> GetAllAsync(bool onlyActive = true, CancellationToken ct = default)
    {
        var query = _context.Products.AsQueryable();

        if (onlyActive)
            query = query.Where(p => p.IsActive);

        return await query.OrderBy(p => p.Name).ToListAsync(ct);
    }

    public async Task<List<Product>> GetByCategoryAsync(string categoryName, CancellationToken ct = default)
    {
        return await _context.Products
            .Where(p => p.Category.Name == categoryName && p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync(ct);
    }

    public async Task AddAsync(Product product, CancellationToken ct = default)
    {
        await _context.Products.AddAsync(product, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Product product, CancellationToken ct = default)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var product = await GetByIdAsync(id, ct);
        if (product != null)
        {
            product.Desactivate();
            await UpdateAsync(product, ct);
        }
    }

    public async Task<bool> SkuExistsAsync(string sku, CancellationToken ct = default)
    {
        return await _context.Products.AnyAsync(p => p.SKU == sku.ToUpper(), ct);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Products.AnyAsync(p => p.Id == id, ct);
    }
}