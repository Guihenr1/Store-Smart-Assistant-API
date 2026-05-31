using System.ComponentModel;
using Microsoft.SemanticKernel;
using StoreSmart.Application.Interfaces.Repositories;
using StoreSmart.Domain.Entities;

namespace StoreSmart.Infrastructure.Plugins;

public class ProductSearchPlugin(IProductRepository productRepository)
{
    [KernelFunction("search_products")]
    [Description("Search for products in the catalog")]
    public async Task<List<Product>> SearchProductsAsync(string query, int maxResults = 6)
    {
        return await productRepository.SearchSemanticAsync(query, maxResults);
    }
}