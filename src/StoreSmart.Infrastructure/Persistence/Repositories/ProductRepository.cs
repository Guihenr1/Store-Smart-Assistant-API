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
    
    public async Task<(List<Product> Products, int TotalCount)> GetPaginatedAsync(
        int pageNumber,
        int pageSize,
        bool onlyActive = true,
        string? category = null,
        string? searchTerm = null,
        CancellationToken ct = default)
    {
        var query = _context.Products.AsQueryable();

        if (onlyActive)
            query = query.Where(p => p.IsActive);

        if (!string.IsNullOrWhiteSpace(category))
            query = query.Where(p => p.Category.Name == category);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.ToLower();
            query = query.Where(p =>
                p.Name.ToLower().Contains(term) ||
                p.Description.ToLower().Contains(term) ||
                p.Brand.ToLower().Contains(term) ||
                p.SKU.ToLower().Contains(term));
        }

        var totalCount = await query.CountAsync(ct);

        var products = await query
            .OrderBy(p => p.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (products, totalCount);
    }
    
    public async Task<List<Product>> SearchSemanticAsync(
        string query, 
        int limit = 6, 
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return await GetAllAsync(onlyActive: true, ct);
        }

        var searchTerm = query.ToLower().Trim();

        return await _context.Products
            .Where(p => p.IsActive)
            .Where(p =>
                EF.Functions.ILike(p.Name, $"%{searchTerm}%") ||
                EF.Functions.ILike(p.Description, $"%{searchTerm}%") ||
                EF.Functions.ILike(p.Brand, $"%{searchTerm}%") ||
                EF.Functions.ILike(p.Category.Name, $"%{searchTerm}%") ||
                (p.Specifications != null && EF.Functions.ILike(p.Specifications, $"%{searchTerm}%")) ||
                (p.Features != null && EF.Functions.ILike(p.Features, $"%{searchTerm}%")) ||
                (!string.IsNullOrEmpty(p.SKU) && EF.Functions.ILike(p.SKU, $"%{searchTerm}%"))
            )
            .OrderByDescending(p => p.StockQuantity)
            .ThenBy(p => p.Name)
            .Take(limit)
            .ToListAsync(ct);
    }
}