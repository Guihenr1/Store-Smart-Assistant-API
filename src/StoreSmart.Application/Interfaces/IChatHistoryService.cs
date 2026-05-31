using Microsoft.SemanticKernel.ChatCompletion;

namespace StoreSmart.Application.Interfaces;

public interface IChatHistoryService
{
    Task<ChatHistory> GetOrCreateHistoryAsync(string sessionId);
    Task SaveHistoryAsync(string sessionId, ChatHistory history);
    Task ClearHistoryAsync(string sessionId);
}