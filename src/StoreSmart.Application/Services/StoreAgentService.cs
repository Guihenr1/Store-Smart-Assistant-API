using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using StoreSmart.Application.Features.Chat.DTOs;
using StoreSmart.Application.Features.Products.Commands.Mappings;
using StoreSmart.Application.Features.Products.DTOs;
using StoreSmart.Application.Interfaces;
using StoreSmart.Application.Interfaces.Repositories;

namespace StoreSmart.Application.Services;

public class StoreAgentService : IStoreAgentService
{
    private readonly Kernel _kernel;
    private readonly IChatHistoryService _chatHistoryService;
    private readonly IProductRepository _productRepository; 

    public StoreAgentService(
        Kernel kernel,
        IChatHistoryService chatHistoryService,
        IProductRepository productRepository)
    {
        _kernel = kernel;
        _chatHistoryService = chatHistoryService;
        _productRepository = productRepository;
    }

    public async Task<ChatResponse> ProcessUserMessageAsync(
        string sessionId,
        string userMessage,
        CancellationToken ct = default)
    {
        var chatHistory = await _chatHistoryService.GetOrCreateHistoryAsync(sessionId);
        chatHistory.AddUserMessage(userMessage);

        var chatCompletion = _kernel.GetRequiredService<IChatCompletionService>();

        var executionSettings = new OpenAIPromptExecutionSettings
        {
            ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
            Temperature = 0.7,
            MaxTokens = 1000
        };

        var result = await chatCompletion.GetChatMessageContentAsync(
            chatHistory,
            executionSettings: executionSettings,
            cancellationToken: ct);

        chatHistory.AddAssistantMessage(result.Content ?? "");

        await _chatHistoryService.SaveHistoryAsync(sessionId, chatHistory);

        var citedProducts = await ExtractCitedProductsAsync(result.Content ?? "");

        return new ChatResponse
        {
            Message = result.Content ?? "Sorry, I couldn't process your request.",
            SessionId = sessionId,
            CitedProducts = citedProducts
        };
    }

    /// <summary>
    /// Extracts product references from AI response and fetches full details
    /// </summary>
    private async Task<List<ProductResponse>> ExtractCitedProductsAsync(string aiResponse)
    {
        if (string.IsNullOrWhiteSpace(aiResponse))
            return new List<ProductResponse>();

        var products = new List<ProductResponse>();

        var allProducts = await _productRepository.GetAllAsync(onlyActive: true);

        foreach (var product in allProducts)
        {
            if (aiResponse.Contains(product.SKU, StringComparison.OrdinalIgnoreCase) ||
                aiResponse.Contains(product.Name, StringComparison.OrdinalIgnoreCase))
            {
                products.Add(product.ToResponse());
            }
        }

        return products.Take(5).ToList();
    }
}