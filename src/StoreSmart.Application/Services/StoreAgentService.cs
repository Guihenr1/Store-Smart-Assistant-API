using StoreSmart.Application.Interfaces;

namespace StoreSmart.Application.Services;

public class StoreAgentService : IStoreAgentService
{
    public Task<object?> ProcessUserMessageAsync(string requestSessionId, string requestUserMessage, string? requestUserId)
    {
        throw new NotImplementedException();
    }
}