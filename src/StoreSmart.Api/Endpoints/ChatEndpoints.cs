using StoreSmart.Application.Features.Chat.DTOs;
using StoreSmart.Application.Interfaces;

namespace StoreSmart.Api.Endpoints;

public static class ChatEndpoints
{
    public static void RegisterChatEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/chat")
            .WithTags("Chat");

        group.MapPost("/", async (ChatRequest request, IStoreAgentService agentService) =>
            {
                var response = await agentService.ProcessUserMessageAsync(
                    request.SessionId, 
                    request.UserMessage,
                    request.UserId);

                return Results.Ok(response);
            })
            .WithName("ChatWithAgent")
            .WithOpenApi();
    }
}