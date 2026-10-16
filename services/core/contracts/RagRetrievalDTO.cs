
using System.Text.Json.Serialization;

class SearchResultResponse
{
    [JsonPropertyName("context")]
    public List<SearchContextItem> Context {get; set;} = new();
}

public class SearchContextItem
{
    [JsonPropertyName("chunk")]
    public string Chunk {get; set;} = string.Empty;
    [JsonPropertyName("@search.score")]
    public double SearchScore {get; set;}
}