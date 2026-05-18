using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using StoreSmart.Api;
using StoreSmart.Api.Endpoints;
using StoreSmart.Application;
using StoreSmart.Application.Settings;
using StoreSmart.Infrastructure;
using StoreSmart.Infrastructure.Data;
using StoreSmart.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi("v1", options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddHttpContextAccessor();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });


// Register Semantic Kernel + Plugins (in extension method)
// builder.Services.AddSemanticKernelServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || 
    !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("ENABLE_SCALAR")))
{
    app.MapOpenApi();
    
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Store Smart API Documentation")
            .WithTheme(ScalarTheme.DeepSpace) 
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

if (app.Environment.IsDevelopment() || 
    !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("AUTO_MIGRATE")))
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<SmartStoreDbContext>();

    Console.WriteLine("🔄 Applying database migrations...");
    await dbContext.Database.MigrateAsync();

    Console.WriteLine("🌱 Checking database seed...");
    await ProductSeeder.SeedAsync(dbContext);
}

app.RegisterChatEndpoints();
app.RegisterAuthEndpoints();
app.RegisterProductEndpoints();

app.MapHealthChecks("/health");
app.MapGet("/health", () => Results.Ok(new { status = "Healthy", time = DateTime.UtcNow }));

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();