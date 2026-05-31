using StoreSmart.Application.Features.Chat.DTOs;

namespace StoreSmart.Application.Interfaces;

public interface IStoreAgentService
{
    Task<ChatResponse> ProcessUserMessageAsync(
        string sessionId, 
        string userMessage, 
        CancellationToken ct = default);
}