namespace StoreSmart.Application.Features.Chat.DTOs;

public record ChatRequest(
    string SessionId,
    string UserMessage,
    string? UserId = null
);

// public record ChatResponse(
    // string Message
    //List<ProductDto>? CitedProducts = null,
    // ShoppingListSummary? ShoppingList = null
// );