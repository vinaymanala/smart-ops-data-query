
using core.gateway.plugins;
using core.models;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace core.gateway.endpoints;


public static class SearchEndpoints
{

    public static void MapSearchEndpoints(this WebApplication app, Kernel kernel)
    {
        var searchkernel = kernel.Clone();

        var httpClientFactory = app.Services.GetRequiredService<IHttpClientFactory>();

        var logger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger<RagServicePlugin>();

        var ragServicePlugin = new RagServicePlugin(httpClientFactory, logger);

        searchkernel.Plugins.AddFromObject(ragServicePlugin, "RagServicePlugin");

        var openAIPromptExecutionSettings = new OpenAIPromptExecutionSettings()
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(),
            ChatSystemPrompt = "You are an enterprise document RAG search assistant. Always use the RagServicePlugin to answer questions about uploaded documents.",
            MaxTokens = 8000,
        };

        var group = app.MapGroup("/api/search").WithTags("AI Search");

        group.MapPost("/stream", async (HttpRequest httpRequest) =>
        {
            string userPrompt = string.Empty;
            string? savedFilePath = null;

            if (httpRequest.HasFormContentType)
            {
                var form = await httpRequest.ReadFormAsync();
                var file = form.Files["file"];
                userPrompt = form["prompt"].ToString();

                if (file != null && file.Length > 0)
                {
                    // Save the uploaded file temporarily to disk so the plugin can read its path
                    var tempDir = Path.Combine(Path.GetTempPath(), "rag_uploads");
                    Directory.CreateDirectory(tempDir);
                    savedFilePath = Path.Combine(tempDir, file.FileName);

                    using (var stream = new FileStream(savedFilePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // If the user didn't type a prompt, auto-generate one for the plugin
                    if (string.IsNullOrEmpty(userPrompt))
                    {
                        userPrompt = $"Please ingest and upload this PDF document: {savedFilePath}";
                    }
                }
            }
            else
            {
                // Fallback for standard JSON text requests
                var request = await httpRequest.ReadFromJsonAsync<ChatRequest>();
                userPrompt = request?.Prompt ?? string.Empty;
            }

            if (string.IsNullOrEmpty(userPrompt))
            {
                return Results.BadRequest("Prompt or file is required.");
            }
            var chatCompletionService = searchkernel.GetRequiredService<IChatCompletionService>();

            var result = await chatCompletionService.GetChatMessageContentAsync(
                userPrompt,
                executionSettings: searchkernel.Plugins.Any() ? null : null,
                kernel: searchkernel
                );

            return Results.Ok($"{result?.Content ?? string.Empty}");
        });
    }
}