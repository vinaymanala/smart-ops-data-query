import axios from "axios";


export const PORT = process.env.NEXT_PUBLIC_PORT || '8081'
export const client = axios.create({
    baseURL: process.env.NEXT_PUBLIC_BASE_URL || `http://localhost:${PORT}`,
    headers: {
        'Content-Type': 'application/json',
        // Accept: 'application/json',
        AllowedOrigins: '*',
    },
    // withCredentials: true,
})