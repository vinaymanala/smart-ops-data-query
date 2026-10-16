using Microsoft.SemanticKernel;

namespace core.extensions;

public static class SemanticKernelService
{
    public static Kernel AddSemanticKernelService(this IServiceCollection services, IConfiguration configuration)
    {
        var skBuilder = Kernel.CreateBuilder();
        skBuilder.Services.AddLogging();
        skBuilder.Services.AddHttpClient();

        var groqApiKey = configuration["GROQ_API_KEY"];
        var groqModelEndpoint = configuration["GROQ_MODEL_ENDPOINT"] ?? "https://api.groq.com/openai/v1";
        var groqModelName = configuration["GROQ_MODEL_NAME"] ?? "openai/gpt-oss-20b";

        skBuilder.AddOpenAIChatCompletion(
            modelId: groqModelName,
            apiKey: groqApiKey,
            endpoint: new Uri(groqModelEndpoint)
        );

        var kernel = skBuilder.Build();
        services.AddSingleton(kernel);
        return kernel;
    }
}