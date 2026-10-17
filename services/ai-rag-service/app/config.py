from pydantic_settings import BaseSettings

class Setting(BaseSettings):
    AZURE_OPENAI_ENDPOINT : str = ""
    AZURE_OPENAI_KEY : str = ""
    AZURE_SEARCH_ENDPOINT: str = ""
    AZURE_SEARCH_INDEX_NAME : str = ""
    AZURE_SEARCH_KEY : str = ""
    AZURE_EMBEDDING_DEPLOYMENT_NAME : str = ""


settings = Setting()