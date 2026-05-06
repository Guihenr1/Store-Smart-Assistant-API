namespace StoreSmart.Domain.ValueObjects;

public class Category
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    private Category() { }

    public static Category Create(string name, string? description = null)
    {
        return new Category 
        { 
            Name = name.Trim(), 
            Description = description?.Trim() 
        };
    }
}