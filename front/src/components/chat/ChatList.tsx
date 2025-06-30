
import React from 'react';
import { Avatar, AvatarFallback, AvatarImage } from '@/components/ui/avatar';
import { Badge } from '@/components/ui/badge';
import { formatDistanceToNow } from 'date-fns';
import { ptBR } from 'date-fns/locale';

interface ChatListProps {
  conversations: any[];
  selectedConversation: string | null;
  onSelectConversation: (id: string) => void;
  loading: boolean;
}

export const ChatList: React.FC<ChatListProps> = ({
  conversations,
  selectedConversation,
  onSelectConversation,
  loading
}) => {
  if (loading) {
    return (
      <div className="space-y-3">
        {[...Array(3)].map((_, i) => (
          <div key={i} className="flex items-center space-x-3 p-3 animate-pulse">
            <div className="w-10 h-10 bg-gray-300 rounded-full"></div>
            <div className="flex-1">
              <div className="h-4 bg-gray-300 rounded w-3/4 mb-2"></div>
              <div className="h-3 bg-gray-200 rounded w-1/2"></div>
            </div>
          </div>
        ))}
      </div>
    );
  }

  return (
    <div className="space-y-1">
      {conversations.map((conversation) => (
        <div
          key={conversation.id}
          className={`p-3 cursor-pointer hover:bg-muted transition-colors rounded-lg ${
            selectedConversation === conversation.id ? 'bg-muted border border-blue-500' : ''
          }`}
          onClick={() => onSelectConversation(conversation.id)}
        >
          <div className="flex items-start space-x-3">
            <div className="relative">
              <Avatar className="w-10 h-10">
                <AvatarImage 
                  src={conversation.otherUser?.foto_perfil} 
                  alt={conversation.otherUser?.nome} 
                />
                <AvatarFallback>
                  {conversation.otherUser?.nome?.charAt(0)?.toUpperCase() || 'U'}
                </AvatarFallback>
              </Avatar>
              <div className="absolute -bottom-1 -right-1 w-3 h-3 bg-green-500 rounded-full border-2 border-white"></div>
            </div>
            
            <div className="flex-1 min-w-0">
              <div className="flex items-center justify-between">
                <h4 className="font-semibold text-sm truncate">
                  {conversation.otherUser?.nome || 'Usuário'}
                </h4>
                <div className="flex items-center space-x-2">
                  {conversation.lastMessage && (
                    <span className="text-xs text-muted-foreground">
                      {formatDistanceToNow(new Date(conversation.lastMessage.created_at), {
                        addSuffix: true,
                        locale: ptBR
                      })}
                    </span>
                  )}
                  {conversation.unreadCount > 0 && (
                    <Badge className="text-xs px-1.5 py-0.5 bg-blue-500">
                      {conversation.unreadCount}
                    </Badge>
                  )}
                </div>
              </div>
              
              {conversation.lastMessage && (
                <p className="text-sm text-muted-foreground truncate">
                  {conversation.lastMessage.conteudo}
                </p>
              )}
            </div>
          </div>
        </div>
      ))}
      
      {conversations.length === 0 && !loading && (
        <div className="text-center py-8 text-muted-foreground">
          <p>Nenhuma conversa encontrada</p>
          <p className="text-sm">Inicie uma nova conversa!</p>
        </div>
      )}
    </div>
  );
};
