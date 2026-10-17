import axios from "axios"
import { client } from "./client";

const urlChatPrefix = "/api/chat"

export const AIChatEndpoint = {
    getChatStream : async (prompt: string) => {
        if (prompt == null || !prompt.length){
            return {
                message: 'Check your prompt again..'
            }
        }
        const response = await client.post(`${urlChatPrefix}/stream`, {Prompt: prompt});
        if (response == null){
            return {
                message: 'No response found.'
            }
        }
        if (response.status !== 200){
            return {
                message: 'Something went wrong. Kindly try again in some time.'
            }
        }
        return {message: response.data};
    }
}

const urlSearchPrefix = "/api/search"

type AISearchRequest = {
    file: File,
    prompt: string  
}
export const AISearchEndpoint = {
    getChatStream : async (request: AISearchRequest) => {

        const formData = new FormData()
        if (request.file){
            formData.append("file", request.file);
        }
        formData.append("prompt",request.prompt || "");

        if (request == null){
            return {
                message: 'Check your prompt again..'
            }
        }
        const response = await client.post(`${urlSearchPrefix}/stream`, formData);
        if (response == null){
            return {
                message: 'No response found.'
            }
        }

        return response;
    }
}
