using StoreSmart.Api.Endpoints;
using StoreSmart.Application.Interfaces;
using StoreSmart.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add services
builder.Services.AddScoped<IStoreAgentService, StoreAgentService>();

// Register Semantic Kernel + Plugins (in extension method)
// builder.Services.AddSemanticKernelServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.RegisterChatEndpoints();

app.UseAuthorization();

app.MapControllers();

app.Run();