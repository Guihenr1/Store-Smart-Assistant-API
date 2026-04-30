namespace StoreSmart.Application.Interfaces;

public interface IStoreAgentService
{
    Task<object?> ProcessUserMessageAsync(string requestSessionId, string requestUserMessage, string? requestUserId);
}