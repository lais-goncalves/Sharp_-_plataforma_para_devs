
import { useState, useEffect } from 'react';
import { supabase } from '@/integrations/supabase/client';
import { useAuth } from '@/contexts/AuthContext';
import { toast } from '@/hooks/use-toast';

interface Community {
  id: string;
  nome: string;
  descricao: string;
  imagem_url?: string;
  criador_id: string;
  publico: boolean;
  created_at: string;
  membersCount?: number;
  postsCount?: number;
}

interface Post {
  id: string;
  comunidade_id: string;
  user_id: string;
  conteudo: string;
  tipo: string;
  arquivo_url?: string;
  created_at: string;
  author?: {
    nome: string;
    foto_perfil?: string;
  };
  reactions?: any[];
  comments?: any[];
  reactionsCount?: number;
  commentsCount?: number;
}

export const useCommunity = () => {
  const { user } = useAuth();
  const [communities, setCommunities] = useState<Community[]>([]);
  const [posts, setPosts] = useState<Post[]>([]);
  const [selectedCommunity, setSelectedCommunity] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  // Buscar comunidades
  const fetchCommunities = async () => {
    try {
      setLoading(true);
      const { data, error } = await supabase
        .from('comunidades')
        .select('*')
        .eq('publico', true)
        .order('created_at', { ascending: false });

      if (error) throw error;

      // Buscar contagem de membros e posts para cada comunidade
      const processedCommunities = await Promise.all(
        data.map(async (community) => {
          const { count: membersCount } = await supabase
            .from('comunidade_membros')
            .select('*', { count: 'exact', head: true })
            .eq('comunidade_id', community.id);

          const { count: postsCount } = await supabase
            .from('postagens_comunidade')
            .select('*', { count: 'exact', head: true })
            .eq('comunidade_id', community.id);

          return {
            ...community,
            membersCount: membersCount || 0,
            postsCount: postsCount || 0
          };
        })
      );

      setCommunities(processedCommunities);
    } catch (error) {
      console.error('Erro ao buscar comunidades:', error);
    } finally {
      setLoading(false);
    }
  };

  // Buscar posts de uma comunidade
  const fetchPosts = async (communityId: string) => {
    try {
      const { data, error } = await supabase
        .from('postagens_comunidade')
        .select(`
          *,
          users:user_id (nome, foto_perfil)
        `)
        .eq('comunidade_id', communityId)
        .order('created_at', { ascending: false });

      if (error) throw error;

      // Buscar reações e comentários para cada post
      const processedPosts = await Promise.all(
        data.map(async (post) => {
          const { data: reactions } = await supabase
            .from('reacoes_postagem')
            .select('*')
            .eq('postagem_id', post.id);

          const { count: commentsCount } = await supabase
            .from('comentarios_postagem')
            .select('*', { count: 'exact', head: true })
            .eq('postagem_id', post.id);

          return {
            ...post,
            author: post.users,
            reactions: reactions || [],
            reactionsCount: reactions?.length || 0,
            commentsCount: commentsCount || 0
          };
        })
      );

      setPosts(processedPosts);
    } catch (error) {
      console.error('Erro ao buscar posts:', error);
    }
  };

  // Criar post
  const createPost = async (communityId: string, content: string) => {
    if (!user || !content.trim()) return;

    try {
      const { error } = await supabase
        .from('postagens_comunidade')
        .insert({
          comunidade_id: communityId,
          user_id: user.id,
          conteudo: content.trim(),
          tipo: 'text'
        });

      if (error) throw error;

      toast({
        title: "Sucesso",
        description: "Post criado com sucesso!"
      });

      fetchPosts(communityId);
    } catch (error) {
      console.error('Erro ao criar post:', error);
      toast({
        title: "Erro",
        description: "Não foi possível criar o post",
        variant: "destructive"
      });
    }
  };

  // Reagir a um post
  const reactToPost = async (postId: string, reactionType: string = 'like') => {
    if (!user) return;

    try {
      // Verificar se já reagiu
      const { data: existing } = await supabase
        .from('reacoes_postagem')
        .select('id')
        .eq('postagem_id', postId)
        .eq('user_id', user.id)
        .eq('tipo', reactionType)
        .single();

      if (existing) {
        // Remover reação
        await supabase
          .from('reacoes_postagem')
          .delete()
          .eq('id', existing.id);
      } else {
        // Adicionar reação
        await supabase
          .from('reacoes_postagem')
          .insert({
            postagem_id: postId,
            user_id: user.id,
            tipo: reactionType
          });
      }

      // Atualizar posts
      if (selectedCommunity) {
        fetchPosts(selectedCommunity);
      }
    } catch (error) {
      console.error('Erro ao reagir ao post:', error);
    }
  };

  // Ingressar em comunidade
  const joinCommunity = async (communityId: string) => {
    if (!user) return;

    try {
      const { error } = await supabase
        .from('comunidade_membros')
        .insert({
          comunidade_id: communityId,
          user_id: user.id,
          role: 'member'
        });

      if (error) throw error;

      toast({
        title: "Sucesso",
        description: "Você ingressou na comunidade!"
      });

      fetchCommunities();
    } catch (error) {
      console.error('Erro ao ingressar na comunidade:', error);
    }
  };

  // Configurar realtime para posts
  useEffect(() => {
    if (!selectedCommunity) return;

    const channel = supabase
      .channel(`community-${selectedCommunity}`)
      .on(
        'postgres_changes',
        {
          event: '*',
          schema: 'public',
          table: 'postagens_comunidade',
          filter: `comunidade_id=eq.${selectedCommunity}`
        },
        () => {
          fetchPosts(selectedCommunity);
        }
      )
      .subscribe();

    return () => {
      supabase.removeChannel(channel);
    };
  }, [selectedCommunity]);

  useEffect(() => {
    fetchCommunities();
  }, []);

  useEffect(() => {
    if (selectedCommunity) {
      fetchPosts(selectedCommunity);
    }
  }, [selectedCommunity]);

  return {
    communities,
    posts,
    selectedCommunity,
    setSelectedCommunity,
    createPost,
    reactToPost,
    joinCommunity,
    loading
  };
};
