
import { useState, useEffect } from 'react';
import { supabase } from '@/integrations/supabase/client';
import { useAuth } from '@/contexts/AuthContext';
import { toast } from '@/hooks/use-toast';

interface Message {
  id: string;
  conversa_id: string;
  sender_id: string;
  conteudo: string;
  tipo: string;
  arquivo_url?: string;
  lida: boolean;
  created_at: string;
}

interface Conversation {
  id: string;
  user1_id: string;
  user2_id: string;
  created_at: string;
  updated_at: string;
  otherUser?: {
    id: string;
    nome: string;
    foto_perfil: string;
  };
  lastMessage?: Message;
  unreadCount?: number;
}

export const useChat = () => {
  const { user } = useAuth();
  const [conversations, setConversations] = useState<Conversation[]>([]);
  const [currentMessages, setCurrentMessages] = useState<Message[]>([]);
  const [selectedConversation, setSelectedConversation] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  // Buscar conversas do usuário
  const fetchConversations = async () => {
    if (!user) return;

    try {
      setLoading(true);
      const { data, error } = await supabase
        .from('conversas')
        .select(`
          *,
          mensagens_conversa!inner (
            id,
            conteudo,
            created_at,
            sender_id,
            lida
          )
        `)
        .or(`user1_id.eq.${user.id},user2_id.eq.${user.id}`)
        .order('updated_at', { ascending: false });

      if (error) throw error;

      // Processar conversas para incluir informações do outro usuário
      const processedConversations = await Promise.all(
        data.map(async (conv) => {
          const otherUserId = conv.user1_id === user.id ? conv.user2_id : conv.user1_id;
          
          // Buscar dados do outro usuário
          const { data: userData } = await supabase
            .from('users')
            .select('id, nome, foto_perfil')
            .eq('id', otherUserId)
            .single();

          // Buscar última mensagem
          const { data: lastMsg } = await supabase
            .from('mensagens_conversa')
            .select('*')
            .eq('conversa_id', conv.id)
            .order('created_at', { ascending: false })
            .limit(1)
            .single();

          // Contar mensagens não lidas
          const { count: unreadCount } = await supabase
            .from('mensagens_conversa')
            .select('*', { count: 'exact', head: true })
            .eq('conversa_id', conv.id)
            .eq('lida', false)
            .neq('sender_id', user.id);

          return {
            ...conv,
            otherUser: userData,
            lastMessage: lastMsg,
            unreadCount: unreadCount || 0
          };
        })
      );

      setConversations(processedConversations);
    } catch (error) {
      console.error('Erro ao buscar conversas:', error);
      toast({
        title: "Erro",
        description: "Não foi possível carregar as conversas",
        variant: "destructive"
      });
    } finally {
      setLoading(false);
    }
  };

  // Buscar mensagens de uma conversa
  const fetchMessages = async (conversationId: string) => {
    try {
      const { data, error } = await supabase
        .from('mensagens_conversa')
        .select('*')
        .eq('conversa_id', conversationId)
        .order('created_at', { ascending: true });

      if (error) throw error;

      setCurrentMessages(data || []);
      
      // Marcar mensagens como lidas
      await supabase
        .from('mensagens_conversa')
        .update({ lida: true })
        .eq('conversa_id', conversationId)
        .neq('sender_id', user?.id);

    } catch (error) {
      console.error('Erro ao buscar mensagens:', error);
    }
  };

  // Enviar mensagem
  const sendMessage = async (conversationId: string, content: string) => {
    if (!user || !content.trim()) return;

    try {
      const { error } = await supabase
        .from('mensagens_conversa')
        .insert({
          conversa_id: conversationId,
          sender_id: user.id,
          conteudo: content.trim(),
          tipo: 'text'
        });

      if (error) throw error;

      // Atualizar timestamp da conversa
      await supabase
        .from('conversas')
        .update({ updated_at: new Date().toISOString() })
        .eq('id', conversationId);

    } catch (error) {
      console.error('Erro ao enviar mensagem:', error);
      toast({
        title: "Erro",
        description: "Não foi possível enviar a mensagem",
        variant: "destructive"
      });
    }
  };

  // Criar nova conversa
  const createConversation = async (otherUserId: string) => {
    if (!user) return null;

    try {
      // Verificar se já existe conversa
      const { data: existing } = await supabase
        .from('conversas')
        .select('id')
        .or(`and(user1_id.eq.${user.id},user2_id.eq.${otherUserId}),and(user1_id.eq.${otherUserId},user2_id.eq.${user.id})`)
        .single();

      if (existing) {
        return existing.id;
      }

      // Criar nova conversa
      const { data, error } = await supabase
        .from('conversas')
        .insert({
          user1_id: user.id,
          user2_id: otherUserId
        })
        .select()
        .single();

      if (error) throw error;

      fetchConversations();
      return data.id;
    } catch (error) {
      console.error('Erro ao criar conversa:', error);
      return null;
    }
  };

  // Configurar realtime para mensagens
  useEffect(() => {
    if (!user || !selectedConversation) return;

    const channel = supabase
      .channel(`messages-${selectedConversation}`)
      .on(
        'postgres_changes',
        {
          event: 'INSERT',
          schema: 'public',
          table: 'mensagens_conversa',
          filter: `conversa_id=eq.${selectedConversation}`
        },
        (payload) => {
          const newMessage = payload.new as Message;
          setCurrentMessages(prev => [...prev, newMessage]);

          // Marcar como lida se não for do usuário atual
          if (newMessage.sender_id !== user.id) {
            supabase
              .from('mensagens_conversa')
              .update({ lida: true })
              .eq('id', newMessage.id);
          }
        }
      )
      .subscribe();

    return () => {
      supabase.removeChannel(channel);
    };
  }, [user, selectedConversation]);

  // Configurar realtime para conversas
  useEffect(() => {
    if (!user) return;

    const channel = supabase
      .channel(`conversations-${user.id}`)
      .on(
        'postgres_changes',
        {
          event: '*',
          schema: 'public',
          table: 'conversas'
        },
        () => {
          fetchConversations();
        }
      )
      .subscribe();

    return () => {
      supabase.removeChannel(channel);
    };
  }, [user]);

  useEffect(() => {
    if (user) {
      fetchConversations();
    }
  }, [user]);

  useEffect(() => {
    if (selectedConversation) {
      fetchMessages(selectedConversation);
    }
  }, [selectedConversation]);

  return {
    conversations,
    currentMessages,
    selectedConversation,
    setSelectedConversation,
    sendMessage,
    createConversation,
    loading
  };
};
