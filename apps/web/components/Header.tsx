import { ActiveTab } from "@/types/types"
import { Search, Sparkles } from "lucide-react"


export default function Header({ activeTab }: { activeTab: ActiveTab }) {
    return (
        <header className="flex items-center justify-between border-b border-[#e7e6e1] px-5 py-4 sm:px-8 sm:py-5">
            <div className="flex items-center gap-3">
                {activeTab == 'chat' ? (
                    <>
                        <div className="flex size-9 items-center justify-center rounded-xl bg-[#242321] text-[#fbfbf9]">
                            <Sparkles aria-hidden="true" className="size-4" />
                        </div>
                        <div>
                            <p className="text-sm font-semibold tracking-tight">Clarity</p>
                            <p className="text-xs text-[#85837d]">Your thoughtful AI assistant</p>
                        </div></>)
                    : activeTab == 'search' && (
                        <>
                            <div className="flex size-9 items-center justify-center rounded-xl bg-[#242321] text-[#fbfbf9]">
                                <Search aria-hidden="true" className="size-4" />
                            </div>
                            <div>
                                <p className="text-sm font-semibold tracking-tight">AI Search</p>
                                <p className="text-xs text-[#85837d]">Your AI search assistant</p>
                            </div></>
                    )}
            </div>
            <span className="hidden rounded-full border border-[#deddd8] px-3 py-1 text-[11px] font-medium text-[#85837d] sm:inline-flex">Private workspace</span>
        </header>
    )
}