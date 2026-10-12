using Microsoft.SemanticKernel;

namespace core.extensions;

public static class SemanticKernelService
{
    public static Kernel UseSemanticKernelService(this WebApplication app)
    {        
        var skBuilder = Kernel.CreateBuilder();
        skBuilder.Services.AddLogging();
        skBuilder.Services.AddHttpClient();        

        var config = new ConfigurationBuilder()
        .AddUserSecrets<Program>()
        .AddJsonFile("appsettings.json", optional: true)
        .AddJsonFile("appsettings.Development.json", optional: true)
        .Build();

        var groqApiKey = config["GROQ_API_KEY"];
        var groqModelEndpoint = config["GROQ_MODEL_ENDPOINT"] ?? "https://api.groq.com/openai/v1";
        var groqModelName = config["GROQ_MODEL_NAME"] ?? "openai/gpt-oss-20b";
        skBuilder.AddOpenAIChatCompletion(
            modelId: groqModelName,
            apiKey: groqApiKey,
            endpoint: new Uri(groqModelEndpoint)
        );
        
        // Build the kernel
        return skBuilder.Build();

    }
}