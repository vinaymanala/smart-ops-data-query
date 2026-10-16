import { ActiveTab, Messages, Message } from "@/types/types";
import { LucideZodiacCancer, Search, Sparkles } from "lucide-react";
import Hero from "./Hero";
import remarkGfm from "remark-gfm";
import ReactMarkdown from 'react-markdown';

type AiResponseProps = { messages: Message[] }

type ChatMessagesAreaProps = { activeTab: ActiveTab, messages: Messages, loading: boolean }

const AiChatDescription = () => {
    return (
        <div className="mb-6 flex max-w-[85%] items-start gap-3">
            <div className="mt-1 flex size-7 shrink-0 items-center justify-center rounded-lg bg-[#eeede8] text-[#68655e]">
                <Sparkles aria-hidden="true" className="size-3.5" />
            </div>
            <div>
                <p className="mb-1 text-[11px] font-medium text-[#9a9890]">Clarity</p>
                <p className="text-sm leading-6 text-[#65625c]">I&apos;m ready to help you explore an idea, answer a question, or find something in your sources.</p>
            </div>
        </div>
    )
}

const AiSearchDescription = () => {
    return (
        <div className="mb-6 flex max-w-[85%] items-start gap-3">
            <div className="mt-1 flex size-7 shrink-0 items-center justify-center rounded-lg bg-[#eeede8] text-[#68655e]">
                <Search aria-hidden="true" className="size-3.5" />
            </div>
            <div>
                <p className="mb-1 text-[11px] font-medium text-[#9a9890]">Search</p>
                <p className="text-sm leading-6 text-[#65625c]">I&apos;m ready to help you explore an idea, answer a question, or find something in your sources.</p>
            </div>
        </div>
    )
}

const AiChatResponse = ({ messages }: AiResponseProps) => {
    return (
        <>
            {messages.map(m => (
                <div className="mb-6 flex  items-start gap-3">
                    {m.type == 'ai' ? (
                        <div key={m.message}>
                            <div className="mt-1 flex size-7 shrink-0 items-center justify-center rounded-lg bg-[#eeede8] text-[#68655e]">
                                <Sparkles aria-hidden="true" className="size-3.5" />
                            </div>
                            <div>
                                <p className="mb-1 text-[11px] font-medium text-[#9a9890]">Clarity</p>
                                <p className="text-sm leading-6 text-[#65625c]">
                                    <ReactMarkdown remarkPlugins={[remarkGfm]}>
                                        {m.message}
                                    </ReactMarkdown>
                                </p>
                            </div>
                        </div>
                    ) : m.type == 'user' &&
                    (
                        <div key={m.message} className="mb-4 ml-auto max-w-[85%] rounded-2xl rounded-br-md bg-[#242321] px-4 py-3 text-sm leading-6 text-white">{m.message}</div>
                    )
                    }
                </div>
            ))}
        </>)
}


const AiSearchResponse = ({ messages }: AiResponseProps) => {
    return (
        <>
            {messages.map(m => (
                <div className="mb-6 flex  items-start gap-3">
                    {m.type == 'ai' ?
                        (
                            <div key={m.message}>
                                <div className="mt-1 flex size-7 shrink-0 items-center justify-center rounded-lg bg-[#eeede8] text-[#68655e]">
                                    <Search aria-hidden="true" className="size-3.5" />
                                </div>
                                <div>
                                    <p className="mb-1 text-[11px] font-medium text-[#9a9890]">AI Search</p>
                                    <p className="text-sm leading-6 text-[#65625c]">{m.message}</p>
                                </div>
                            </div>
                        ) : m.type == 'user' &&
                        (
                            <div key={m.message} className="mb-4 ml-auto max-w-[85%] rounded-2xl rounded-br-md bg-[#242321] px-4 py-3 text-sm leading-6 text-white">{m.message}</div>
                        )
                    }
                </div>
            ))}
        </>)
}

export default function ChatMessagesArea({ activeTab, messages, loading }: ChatMessagesAreaProps) {
    return (
        <>

            {activeTab == 'chat' ? (
                !messages.chat.length ?
                    <>
                        <Hero />
                        <AiChatDescription />
                    </> :
                    <>
                        <AiChatResponse messages={messages.chat} />
                        {loading &&
                            (<div>
                                <p className="text-sm leading-6 text-[#65625c]">
                                    <LucideZodiacCancer className="animate-spin" />
                                </p>
                            </div>)}
                    </>
            ) : activeTab == 'search' && (
                !messages.search.length ?
                    <>
                        <Hero />
                        <AiSearchDescription />
                    </>
                    : <AiSearchResponse messages={messages.search} />
            )
            }
        </>
    )
}