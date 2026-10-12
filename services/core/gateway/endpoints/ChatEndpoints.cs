using core.gateway.plugins;
using core.models;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace core.gateway.Endpoints;

public interface IChatEndpoints
{
    void MapChatEndpoints(IEndpointRouteBuilder app, Kernel kernel);    
}

public static class ChatEndpoints
{
    private static Kernel? _kernel;
    private static OpenAIPromptExecutionSettings? _openAIPromptExecutionSettings;
    private static IChatCompletionService? _chatCompletionService;    
    public static void MapChatEndpoints(this WebApplication app, Kernel kernel)
    {
        _kernel = kernel;

        _chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();
        var httpClientFactory = app.Services.GetRequiredService<IHttpClientFactory>();
        var logger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger<QueryInvestorFundsDataPlugin>();

        var queryInvestorFundsDataPlugin = new QueryInvestorFundsDataPlugin(httpClientFactory, logger);
        _kernel.Plugins.AddFromObject(queryInvestorFundsDataPlugin, "QueryInvestorFundsData");
        
        _openAIPromptExecutionSettings = new OpenAIPromptExecutionSettings()
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(),
            ChatSystemPrompt = "You are a precise internal data extraction assistant.",
            MaxTokens = 8000,            
            
        };

        var group = app.MapGroup("/api/chat").WithTags("AI Chat");        

        group.MapPost("/stream", StreamChatAsync);
    }

    private static async Task<IResult> StreamChatAsync(ChatRequest request)
    {

        var result = await _chatCompletionService!.GetChatMessageContentAsync(
            request.Prompt,
            executionSettings: _openAIPromptExecutionSettings,
            kernel: _kernel);

        Console.WriteLine($"RESULT={result}");
        Console.WriteLine($"CONTENT={result?.Content}");
        return Results.Ok($"{result?.Content ?? string.Empty}");
        // var stream = _kernel?.InvokePromptStreamingAsync(request.Prompt);

        // await foreach (var chunk in stream)
        // {
        //     yield return chunk.ToString();
        // }
    }
}
