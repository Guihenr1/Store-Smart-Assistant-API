using StoreSmart.Domain.Entities;

namespace StoreSmart.Application.Interfaces.Repositories;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Product?> GetBySkuAsync(string sku, CancellationToken ct = default);
    Task<List<Product>> GetAllAsync(bool onlyActive = true, CancellationToken ct = default);
    Task<List<Product>> GetByCategoryAsync(string categoryName, CancellationToken ct = default);
    
    Task AddAsync(Product product, CancellationToken ct = default);
    Task UpdateAsync(Product product, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
    
    Task<bool> SkuExistsAsync(string sku, CancellationToken ct = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken ct = default);
}