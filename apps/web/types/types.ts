
export type ActiveTab = 'chat' | 'search'

export type Message = {
    type: string,
    message: string,
    timestamp: string
}

export type Messages = {
    chat: Message[]
    search: Message[]
}