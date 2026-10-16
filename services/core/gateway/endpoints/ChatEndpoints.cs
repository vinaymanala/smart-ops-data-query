using core.gateway.plugins;
using core.models;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace core.gateway.endpoints;

public static class ChatEndpoints
{
    public static void MapChatEndpoints(this WebApplication app, Kernel kernel)
    {
        var chatkernel = kernel.Clone();

        var httpClientFactory = app.Services.GetRequiredService<IHttpClientFactory>();
        var logger1 = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger<QueryInvestorFundsDataPlugin>();

        var queryInvestorFundsDataPlugin = new QueryInvestorFundsDataPlugin(httpClientFactory, logger1);

        chatkernel.Plugins.AddFromObject(queryInvestorFundsDataPlugin, "QueryInvestorFundsData");

        var openAIPromptExecutionSettings = new OpenAIPromptExecutionSettings()
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(),
            ChatSystemPrompt = """
            You are a precise internal data extraction assistant.
            Use bullet points to show data and group by type, category etc.
            A short summary of the results.            
            """,
            MaxTokens = 8000,
        };

        var group = app.MapGroup("/api/chat").WithTags("AI Chat");

        group.MapPost("/stream", async (ChatRequest request) =>
        {
            var chatCompletionService = chatkernel.GetRequiredService<IChatCompletionService>();

            var result = await chatCompletionService.GetChatMessageContentAsync(
                request.Prompt,
                executionSettings: openAIPromptExecutionSettings,
                kernel: chatkernel
                );

            return Results.Ok($"{result?.Content ?? string.Empty}");
        });
    }
}
