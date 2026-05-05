using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StoreSmart.Application.Interfaces;
using StoreSmart.Application.Interfaces.Repositories;
using StoreSmart.Application.Services;
using StoreSmart.Infrastructure.Auth;
using StoreSmart.Infrastructure.Persistence;
using StoreSmart.Infrastructure.Persistence.Repositories;

namespace StoreSmart.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddNpgSql(configuration.GetConnectionString("DefaultConnection")!);
        services.AddDbContext<SmartStoreDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), npgsql =>
                npgsql.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorCodesToAdd: null)));

        
        services.AddScoped<IStoreAgentService, StoreAgentService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        
        return services;
    }
}