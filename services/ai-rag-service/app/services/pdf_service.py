import fitz

def extract_and_chunks_pdf(file_bytes: bytes, chunk_size: int = 500, overlap: int = 50) -> list[str]:
    doc = fitz.open(stream=file_bytes, filetype="pdf",)
    full_text = ""

    try:
        for page in doc:
            # Extract plain text cleanly per page
            page_text = page.get_text("text")
            if isinstance(page_text, str) and page_text:
                full_text += page_text.strip() + " "
    finally:
        doc.close()

    chunks: list[str] = []

    start = 0
    while start < len(full_text):
        end = start + chunk_size
        chunks.append(full_text[start:end])
        start += chunk_size - overlap
    return chunks
