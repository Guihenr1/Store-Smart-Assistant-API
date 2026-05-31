using Microsoft.EntityFrameworkCore;
using StoreSmart.Domain.Entities;
using StoreSmart.Domain.ValueObjects;
using StoreSmart.Infrastructure.Persistence;

namespace StoreSmart.Infrastructure.Data;

public static class ProductSeeder
{
    public static async Task SeedAsync(SmartStoreDbContext context)
    {
        // Only seed if there are no products
        if (await context.Products.AnyAsync())
            return;

        Console.WriteLine("🌱 Seeding initial products...");

        var products = new List<Product>
        {
            Product.Create(
                sku: "LAPTOP001",
                name: "Dell XPS 15",
                description: "Premium ultrabook with 15.6-inch 4K OLED display, Intel Core i7, 32GB RAM and 1TB SSD. Excellent for developers and content creators.",
                brand: "Dell",
                category: Category.Create("Laptops"),
                price: Money.Create(1899.99m),
                stockQuantity: 25,
                imageUrl: "https://example.com/images/dell-xps15.jpg",
                specifications: "Intel i7-13700H, 32GB DDR5, 1TB NVMe SSD, NVIDIA RTX 4060",
                features: "4K OLED Display, Thunderbolt 4, Excellent battery life"
            ),

            Product.Create(
                sku: "HEAD001",
                name: "Sony WH-1000XM5",
                description: "Industry-leading noise cancelling wireless headphones with exceptional sound quality and 30-hour battery life.",
                brand: "Sony",
                category: Category.Create("Headphones"),
                price: Money.Create(399.99m),
                stockQuantity: 45,
                specifications: "Noise Cancelling, 30h battery, Hi-Res Audio, Multipoint Connection",
                features: "Best-in-class ANC, Touch controls, Foldable design"
            ),

            Product.Create(
                sku: "PHONE001",
                name: "iPhone 16 Pro",
                description: "Latest Apple iPhone with A18 Pro chip, titanium design and advanced camera system.",
                brand: "Apple",
                category: Category.Create("Smartphones"),
                price: Money.Create(1199.99m),
                stockQuantity: 30,
                specifications: "6.3-inch Super Retina XDR, 256GB, A18 Pro, 48MP Camera",
                features: "Action Button, USB-C, Excellent video recording"
            ),

            Product.Create(
                sku: "WATCH001",
                name: "Samsung Galaxy Watch 7",
                description: "Advanced health and fitness smartwatch with AI-powered insights.",
                brand: "Samsung",
                category: Category.Create("Smartwatches"),
                price: Money.Create(329.99m),
                stockQuantity: 40,
                specifications: "BioActive Sensor, Sleep Coaching, 5ATM + IP68",
                features: "ECG, Blood Oxygen, Advanced Sleep Analysis"
            ),

            Product.Create(
                sku: "TSHIRT-COTTON-BLK",
                name: "Essential Organic Cotton T-Shirt",
                description: "Premium organic cotton t-shirt. Ultra-soft, breathable and eco-friendly.",
                brand: "Everlane",
                category: Category.Create("T-Shirts"),
                price: Money.Create(29.99m),
                stockQuantity: 150,
                specifications: "100% Organic Cotton, Regular Fit, Pre-shrunk",
                features: "Sustainable, Super soft, Durable stitching"
            ),

            Product.Create(
                sku: "TSHIRT-POLY-BLK",
                name: "Performance Polyester T-Shirt",
                description: "Moisture-wicking polyester t-shirt perfect for sports and active use.",
                brand: "Nike",
                category: Category.Create("T-Shirts"),
                price: Money.Create(34.99m),
                stockQuantity: 80,
                specifications: "100% Polyester, Athletic Fit, Quick-dry fabric",
                features: "Breathable, Odor resistant, UV protection"
            ),

            Product.Create(
                sku: "TSHIRT-WOOL-GRY",
                name: "Merino Wool Blend T-Shirt",
                description: "Luxurious merino wool t-shirt that regulates temperature naturally.",
                brand: "Icebreaker",
                category: Category.Create("T-Shirts"),
                price: Money.Create(79.99m),
                stockQuantity: 35,
                specifications: "60% Merino Wool, 40% Polyester, Slim Fit",
                features: "Natural odor resistance, Temperature regulating, Ultra comfortable"
            ),

            Product.Create(
                sku: "TSHIRT-BAMBOO-GRN",
                name: "Bamboo Fabric T-Shirt",
                description: "Ultra-soft bamboo viscose t-shirt with excellent moisture management.",
                brand: "Bamboo Earth",
                category: Category.Create("T-Shirts"),
                price: Money.Create(39.99m),
                stockQuantity: 65,
                specifications: "95% Bamboo Viscose, 5% Spandex, Regular Fit",
                features: "Antibacterial, Hypoallergenic, Very soft"
            ),
        };

        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();

        Console.WriteLine($"✅ Seeded {products.Count} products successfully.");
    }
}