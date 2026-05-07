using StoreSmart.Domain.ValueObjects;

namespace StoreSmart.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public string SKU { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    
    public string Brand { get; private set; } = string.Empty;
    public Category Category { get; private set; } = null!;
    
    public Money Price { get; private set; } = null!;
    public int StockQuantity { get; private set; }
    public bool IsActive { get; private set; }

    public string? ImageUrl { get; private set; }
    public List<string> Tags { get; private set; } = new();
    
    
    public string? Specifications { get; private set; }   
    public string? Features { get; private set; }
    public string? TechnicalDetails { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Product() { }

    public static Product Create(
        string sku,
        string name,
        string description,
        string brand,
        Category category,
        Money price,
        int stockQuantity,
        string? imageUrl = null,
        string? specifications = null,
        string? features = null,
        string? technicalDetails = null)
    {
        return new Product
        {
            Id = Guid.NewGuid(),
            SKU = sku.Trim().ToUpper(),
            Name = name.Trim(),
            Description = description.Trim(),
            Brand = brand.Trim(),
            Category = category,
            Price = price,
            StockQuantity = stockQuantity,
            IsActive = true,
            ImageUrl = imageUrl,
            Specifications = specifications,
            Features = features,
            TechnicalDetails = technicalDetails,
            CreatedAt = DateTime.UtcNow,
            Tags = new List<string>()
        };
    }

    public void Update(
        string? name = null,
        string? description = null,
        string? brand = null,
        Category? category = null,
        Money? price = null,
        int? stockQuantity = null,
        string? imageUrl = null,
        string? specifications = null,
        string? features = null,
        string? technicalDetails = null,
        List<string>? tags = null)
    {
        if (name != null) Name = name.Trim();
        if (description != null) Description = description.Trim();
        if (brand != null) Brand = brand.Trim();
        if (category != null) Category = category;
        if (price != null) Price = price;
        if (stockQuantity.HasValue) StockQuantity = stockQuantity.Value;
        if (imageUrl != null) ImageUrl = imageUrl;
        if (specifications != null) Specifications = specifications;
        if (features != null) Features = features;
        if (technicalDetails != null) TechnicalDetails = technicalDetails;

        if (tags != null)
        {
            Tags = tags.Select(t => t.Trim().ToLower()).Distinct().ToList();
        }

        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStock(int newQuantity)
    {
        StockQuantity = newQuantity;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddTag(string tag)
    {
        var normalized = tag.Trim().ToLower();
        if (!Tags.Contains(normalized))
            Tags.Add(normalized);
    }
    
    public void Desactivate()
    {
        IsActive = false;
    }
}