import { ActiveTab } from "@/types/types";
import { ArrowUp, MessageSquare, Paperclip, Search } from "lucide-react";
import { Dispatch, SetStateAction } from "react";


type TextAreaSectionProps = {
    message: string
    setMessage: (value: SetStateAction<string>) => void
    activeTab: ActiveTab,
    fileName: string,
    setFileName: (value: SetStateAction<string>) => void
    submitMessage: () => void
    setActiveTab: Dispatch<SetStateAction<ActiveTab>>
}

const starterPrompts = ['Summarize a document', 'Draft an email', 'Explain a concept']
export default function TextAreaSection({
    message, setMessage, fileName, setFileName, activeTab, submitMessage, setActiveTab }: TextAreaSectionProps) {
    return (

        <div className="mt-auto">
            <div className="mb-4 flex flex-wrap gap-2">
                {starterPrompts.map((prompt) => (
                    <button key={prompt} type="button" onClick={() => setMessage(prompt)} className="rounded-full border border-[#deddd8] bg-white px-3 py-2 text-xs text-[#65625c] transition-colors hover:border-[#aaa79f] hover:text-[#242321]">{prompt}</button>
                ))}
            </div>
            <div className="rounded-2xl border border-[#d8d7d1] bg-white p-2 shadow-[0_8px_25px_rgba(36,35,33,0.05)]">
                <div className="flex items-center gap-1 border-b border-[#eeeDE9] px-1 pb-2">
                    <button type="button" aria-pressed={activeTab === 'chat'} onClick={() => setActiveTab('chat')} className={`flex items-center gap-2 rounded-lg px-3 py-2 text-xs font-medium transition-colors ${activeTab === 'chat' ? 'bg-[#f1f0ec] text-[#242321]' : 'text-[#99968f] hover:text-[#242321]'}`}>
                        <MessageSquare aria-hidden="true" className="size-3.5" /> AI Chat</button>
                    <button type="button" aria-pressed={activeTab === 'search'} onClick={() => setActiveTab('search')} className={`flex items-center gap-2 rounded-lg px-3 py-2 text-xs font-medium transition-colors ${activeTab === 'search' ? 'bg-[#f1f0ec] text-[#242321]' : 'text-[#99968f] hover:text-[#242321]'}`}>
                        <Search aria-hidden="true" className="size-3.5" /> AI Search</button>
                </div>

                <div className="flex min-h-2 flex-col p-2">
                    <textarea
                        value={message}
                        onChange={(event) => setMessage(event.target.value)}
                        onKeyDown={(event) => { if (event.key === 'Enter' && !event.shiftKey && !event.nativeEvent.isComposing && event.keyCode !== 229) { event.preventDefault(); submitMessage() } }}
                        placeholder={activeTab === 'chat' ? 'Message Clarity...' : 'Search your sources...'}
                        aria-label={activeTab === 'chat' ? 'Message Clarity' : 'Search your sources'}
                        className="min-h-2 resize-none bg-transparent px-2 py-1 text-sm outline-none placeholder:text-[#aaa79f]" />
                    <div className="flex items-center justify-between gap-2 pt-2">
                        <div className="flex items-center gap-2">
                            {activeTab === 'search' && (
                                <label className="flex cursor-pointer items-center gap-1.5 rounded-lg px-2 py-1.5 text-[11px] text-[#85837d] transition-colors hover:bg-[#f7f7f4] hover:text-[#242321]">
                                    <input type="file" className="sr-only" onChange={(event) => setFileName(event.target.files?.[0]?.name ?? '')} />
                                    <Paperclip aria-hidden="true" className="size-3.5" />
                                    {fileName || 'Add file'}
                                </label>
                            )}
                            <span className="hidden px-1 text-[11px] text-[#aaa79f] sm:inline">Enter to send · Shift + Enter for a new line</span>
                        </div>
                        <button type="button" onClick={submitMessage} aria-label={activeTab === 'chat' ? 'Send message' : 'Search sources'} className="flex size-9 shrink-0 items-center justify-center rounded-xl bg-[#242321] text-white transition-transform hover:scale-105 disabled:cursor-not-allowed disabled:opacity-35" disabled={!message.trim()}><ArrowUp aria-hidden="true" className="size-4" /></button>
                    </div>
                </div>
            </div>
        </div>
    )
}