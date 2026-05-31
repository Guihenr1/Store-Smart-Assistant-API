using StoreSmart.Application.Features.Products.DTOs;

namespace StoreSmart.Application.Features.Chat.DTOs;

public class ChatResponse
{
    public string Message { get; set; } = string.Empty;
    public string SessionId { get; set; } = string.Empty;

    /// <summary>
    /// Products cited/referenced in the AI response (for transparency and RAG)
    /// </summary>
    public List<ProductResponse>? CitedProducts { get; set; }

    /// <summary>
    /// Optional metadata (e.g., confidence, sources, etc.)
    /// </summary>
    public Dictionary<string, object>? Metadata { get; set; }

    public ChatResponse()
    {
        CitedProducts = new List<ProductResponse>();
        Metadata = new Dictionary<string, object>();
    }

    public ChatResponse(string message, string sessionId)
    {
        Message = message;
        SessionId = sessionId;
        CitedProducts = new List<ProductResponse>();
        Metadata = new Dictionary<string, object>();
    }
}