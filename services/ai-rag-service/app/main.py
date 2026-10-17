from fastapi import FastAPI, UploadFile, File, HTTPException
from app.services.pdf_service import extract_and_chunks_pdf
from app.services.rag_service import index_document_chunks, search_query_embeddings
from app.models import schema
app = FastAPI(title="Enterprise AI Rag & Ingestion Service",version="1.0")



@app.get("/hello")
async def helloworld():
    return {
        "status": "success",
        "message": "hello world!"
    }

@app.post("/api/ai/ingest")
async def ingest_pdf(file: UploadFile = File(...)) -> schema.IngestionResponse:
    if file.filename is None or not file.filename.lower().endswith('.pdf'):
        raise HTTPException(status_code=400, detail="Only PDF files are supported")
    contents = await file.read()
    chunks = extract_and_chunks_pdf(contents)
    total_indexed = index_document_chunks(file.filename, chunks)

    return schema.IngestionResponse(
        status = "success",
        filename= file.filename,
        chunks_indexed= total_indexed,
        message= "Document successfully parsed, embedded, and indexed in Azure AI Search."
    )            

@app.post("/api/ai/searchquery")
async def query_rag(request: schema.QueryRequest) -> schema.QueryResponse:

    context = search_query_embeddings(request.question)
    print(context)
    return schema.QueryResponse(context=context)

    
