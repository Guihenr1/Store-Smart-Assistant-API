using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using StoreSmart.Infrastructure.Plugins;

namespace StoreSmart.Infrastructure.SemanticKernel;

public static class SemanticKernelExtensions
{
    public static IServiceCollection AddSemanticKernel(this IServiceCollection services, IConfiguration configuration)
    {
        var kernelBuilder = services.AddKernel();

        kernelBuilder.AddAzureOpenAIChatCompletion(
            deploymentName: configuration["AzureOpenAI:ChatDeployment"]!,
            endpoint: configuration["AzureOpenAI:Endpoint"]!,
            apiKey: configuration["AzureOpenAI:ApiKey"]!);

        kernelBuilder.AddAzureOpenAITextEmbeddingGeneration(
            deploymentName: configuration["AzureOpenAI:EmbeddingDeployment"]!,
            endpoint: configuration["AzureOpenAI:Endpoint"]!,
            apiKey: configuration["AzureOpenAI:ApiKey"]!);

        // services.AddTransient<ProductSearchPlugin>();
        
        // kernelBuilder.Plugins.AddFromType<ProductSearchPlugin>();

        return services;
    }
}