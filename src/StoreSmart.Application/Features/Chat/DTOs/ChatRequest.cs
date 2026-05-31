namespace StoreSmart.Application.Features.Chat.DTOs;

public record ChatRequest(
    string SessionId,
    string UserMessage
);
