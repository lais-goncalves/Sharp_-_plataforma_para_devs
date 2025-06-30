
import Layout from "@/components/layout/Layout";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { MessageSquare, Search } from "lucide-react";
import { useChat } from "@/hooks/useChat";
import { ChatList } from "@/components/chat/ChatList";
import { ChatWindow } from "@/components/chat/ChatWindow";

const Chat = () => {
  const {
    conversations,
    currentMessages,
    selectedConversation,
    setSelectedConversation,
    sendMessage,
    loading
  } = useChat();

  const selectedConversationData = conversations.find(
    conv => conv.id === selectedConversation
  );

  return (
    <Layout>
      <div className="p-6 h-[calc(100vh-7rem)]">
        <div className="grid grid-cols-1 lg:grid-cols-4 gap-6 h-full">
          {/* Chat List */}
          <Card className="lg:col-span-1 h-full">
            <CardHeader className="pb-3">
              <CardTitle className="flex items-center justify-between">
                Mensagens
                <Button size="sm" variant="outline">
                  <MessageSquare className="w-4 h-4" />
                </Button>
              </CardTitle>
              <div className="relative">
                <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400 w-4 h-4" />
                <Input
                  placeholder="Buscar conversas..."
                  className="pl-10"
                />
              </div>
            </CardHeader>
            <CardContent className="p-0 overflow-y-auto max-h-[calc(100vh-16rem)]">
              <ChatList
                conversations={conversations}
                selectedConversation={selectedConversation}
                onSelectConversation={setSelectedConversation}
                loading={loading}
              />
            </CardContent>
          </Card>

          {/* Chat Window */}
          <Card className="lg:col-span-3 h-full flex flex-col">
            <ChatWindow
              messages={currentMessages}
              selectedConversation={selectedConversation}
              conversationData={selectedConversationData}
              onSendMessage={sendMessage}
            />
          </Card>
        </div>
      </div>
    </Layout>
  );
};

export default Chat;
