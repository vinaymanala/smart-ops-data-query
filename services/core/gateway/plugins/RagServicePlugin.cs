
using System.ComponentModel;
using System.Net.Http.Headers;
using Microsoft.SemanticKernel;

namespace core.gateway.plugins;

public class RagServicePlugin(
IHttpClientFactory httpClientFactory,
ILogger<RagServicePlugin> logger
)
{
    [KernelFunction("uploads_pdf_file_rag_ai_search")]    
    [Description("Uploads and ingests a PDF document into Azure AI Search for vector RAG retrieval.")]
    public async Task<string> IngestPdfDocumentAsync(
        [Description("The full file path to the PDF document on disk")] string filePath
    )
    {
        if (!File.Exists(filePath))
        {
            return $"Error: File not found at path {filePath}";
        }

        HttpClient client = httpClientFactory.CreateClient("coreapiclient");
        using var content = new MultipartFormDataContent();
        using var fileStream = File.OpenRead(filePath);
        using var streamContent = new StreamContent(fileStream);

        streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
        content.Add(streamContent, "file", Path.GetFileName(filePath));

        // Send POST request to your Python FastAPI endpoint
        var url ="http://localhost:8000/api/ai/ingest";
        var response = await client.PostAsync(url, content);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            logger.LogInformation($"Document successfully processed by Python RAG service: {result}");
            return result;
        }
        logger.LogError($"Failed to ingest document. Status Code: {response.StatusCode}");
        return $"Failed to ingest document. Status Code: {response.StatusCode}";
    }

    [KernelFunction("query_pdf_file_rag_ai_search")]
    [Description("Searches indexed PDF documents for relevant investor and fund information.")]
    public async Task<string> QueryPdfDocumentsAsync(
        [Description("The search query string, e.g., 'Get me japan investor'")] string searchQuery)
    {
        HttpClient client = httpClientFactory.CreateClient("coreapiclient");
        // 1. Call your Python FastAPI search endpoint
        var url = $"http://localhost:8000/api/ai/searchquery";
        var response = await client.PostAsJsonAsync(url, new { question = searchQuery });

        if (!response.IsSuccessStatusCode)
        {
            return "Error: Failed to retrieve data from the Python AI Search service.";
        }

        // 2. Parse the JSON response containing the "context" array
        var searchResult = await response.Content.ReadFromJsonAsync<SearchResultResponse>();

        if (searchResult?.Context == null || searchResult.Context.Count == 0)
        {
            return "No relevant documents found matching your query.";
        }

        // 3. Extract and combine the text chunks from the search results
        return string.Join("\n---\n", searchResult.Context.Select(c => c.Chunk));
    }
}