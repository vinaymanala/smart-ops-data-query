from openai import AzureOpenAI
from azure.search.documents import SearchClient
from azure.search.documents.models import VectorizedQuery
from azure.core.credentials import AzureKeyCredential
from app.config import settings

# Initialize Clients
openai_client = AzureOpenAI(
    azure_endpoint=settings.AZURE_OPENAI_ENDPOINT,
    api_key=settings.AZURE_OPENAI_KEY,
    api_version="2024-02-01"
)

search_client = SearchClient(
    endpoint=settings.AZURE_SEARCH_ENDPOINT,
    index_name=settings.AZURE_SEARCH_INDEX_NAME,
    credential=AzureKeyCredential(settings.AZURE_SEARCH_KEY)
)

def generate_embedding(text: str) -> list[float]:
    response = openai_client.embeddings.create(
        input=text,
        model=settings.AZURE_EMBEDDING_DEPLOYMENT_NAME  # text-embedding-3-small
    )
    return response.data[0].embedding

def index_document_chunks(doc_id: str, chunks: list[str]):
    documents = []
    for idx, chunk in enumerate(chunks):
        embedding = generate_embedding(chunk)
        documents.append({
            "id": f"{doc_id}-{idx}",
            "content": chunk,
            "content_vector": embedding,
            "source_doc": doc_id
        })
    
    # Upload to Azure AI Search
    search_client.upload_documents(documents=documents)
    return len(documents)

def search_query_embeddings(query: str) -> list[dict]:
    query_vector = generate_embedding(query)

    results = search_client.search(
        search_text=None,
        vector_queries=[
            VectorizedQuery(
                vector=query_vector,
                k_nearest_neighbors=5,
                fields="text_vector"
            )
        ],
        select=["chunk", "title"]
    )    
    return list(results)