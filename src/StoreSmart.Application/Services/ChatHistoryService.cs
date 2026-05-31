using Microsoft.Extensions.Caching.Memory;
using Microsoft.SemanticKernel.ChatCompletion;
using StoreSmart.Application.Interfaces;

namespace StoreSmart.Application.Services;

public class ChatHistoryService : IChatHistoryService
{
    private readonly IMemoryCache _cache;
    private readonly TimeSpan _cacheDuration = TimeSpan.FromHours(12);

    public ChatHistoryService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public Task<ChatHistory> GetOrCreateHistoryAsync(string sessionId)
    {
        if (!_cache.TryGetValue(sessionId, out ChatHistory? history) || history == null)
        {
            history = new ChatHistory();
            history.AddSystemMessage(GetSystemPrompt());
            _cache.Set(sessionId, history, _cacheDuration);
        }

        return Task.FromResult(history);
    }

    private string GetSystemPrompt() => """
                                        You are StoreSmart, a helpful and friendly AI shopping assistant.
                                        You help customers find products, answer questions about specifications, pricing, and availability.
                                        Always base your answers on real products from the catalog.
                                        Be concise, professional, and enthusiastic.
                                        If you cannot find relevant products, be honest about it.
                                        """;

    public Task SaveHistoryAsync(string sessionId, ChatHistory history)
    {
        _cache.Set(sessionId, history, _cacheDuration);
        return Task.CompletedTask;
    }

    public Task ClearHistoryAsync(string sessionId)
    {
        _cache.Remove(sessionId);
        return Task.CompletedTask;
    }
}