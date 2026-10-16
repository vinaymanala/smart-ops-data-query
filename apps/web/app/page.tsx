'use client'

import { useEffect, useRef, useState } from 'react'
import Header from '@/components/Header'
import { ActiveTab, Messages } from '@/types/types'
import TextAreaSection from '@/components/TextAreaSection'
import Footer from '@/components/Footer'
import ChatMessagesArea from '@/components/ChatMessagesArea'
import { AIChatEndpoint } from '@/services/SmartOpsService'

const messages: Messages = {
  chat: [],
  search: []
}

export default function Page() {
  const [activeTab, setActiveTab] = useState<ActiveTab>('chat')
  const [message, setMessage] = useState('')
  const [sentMessages, setSentMessages] = useState<Messages>(messages)
  const [fileName, setFileName] = useState('')
  const [loading, setLoading] = useState<boolean>(false)
  const scrollContainerRef = useRef<HTMLDivElement | null>(null)

  useEffect(() => {
    const container = scrollContainerRef.current
    if (!container) return

    container.scrollTo({
      top: container.scrollHeight,
      behavior: 'smooth'
    })
  }, [sentMessages, activeTab, loading])

  function submitMessage() {
    const trimmedMessage = message.trim()
    if (!trimmedMessage) return

    setLoading(true)

    if (activeTab === 'chat') {
      const userMessage = {
        type: 'user',
        message: trimmedMessage,
        timestamp: ''
      }

      setSentMessages((prev) => ({
        chat: [...prev.chat, userMessage],
        search: [...prev.search]
      }))
      setMessage('')

      ;(async () => {
        const { message: aiMessageText } = await AIChatEndpoint.getChatStream(trimmedMessage)
        setLoading(false)

        const aiMessage = {
          type: 'ai',
          message: aiMessageText,
          timestamp: ''
        }

        setSentMessages((prev) => ({
          chat: [...prev.chat, aiMessage],
          search: [...prev.search]
        }))
      })()
      return
    }

    if (activeTab === 'search') {
      setSentMessages((prev) => ({
        search: [...prev.search, {
          type: 'user',
          message: trimmedMessage,
          timestamp: ''
        }],
        chat: [...prev.chat]
      }))
      setMessage('')
      return
    }
  }

  return (
    <main className="min-h-screen bg-[#f5f5f3] px-4 py-2 text-[#242321] sm:px-8 sm:py-10">
      <section className="mx-auto flex w-full min-h-[calc(100vh-2.5rem)] max-w-3xl flex-col overflow-hidden rounded-[1.75rem] border border-[#deddd8] bg-[#fbfbf9] shadow-[0_20px_70px_rgba(36,35,33,0.08)] sm:min-h-[calc(100vh-5rem)]">
        <Header activeTab={activeTab} />
        <div className="flex flex-1 flex-col px-5 sm:px-12 sm:py-8">
          <div ref={scrollContainerRef} className="flex flex-col h-100 overflow-y-auto px-4">
            <ChatMessagesArea activeTab={activeTab} messages={sentMessages as Messages} loading={loading} />
          </div>
          <TextAreaSection
            message={message}
            setMessage={setMessage}
            fileName={fileName}
            setFileName={setFileName}
            activeTab={activeTab}
            submitMessage={submitMessage}
            setActiveTab={setActiveTab}
          />
        </div>
        <Footer />
      </section>
    </main>
  )
}

