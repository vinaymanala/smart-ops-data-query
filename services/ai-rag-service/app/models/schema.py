from pydantic import BaseModel, Field

class QueryRequest(BaseModel):
    question: str = Field(
        min_length=1,
        max_length=1000,
        description="User question"
    )

class QueryResponse(BaseModel):
    context: list[dict]


class IngestionResponse(BaseModel):
    status: str
    filename: str
    chunks_indexed: int
    message: str