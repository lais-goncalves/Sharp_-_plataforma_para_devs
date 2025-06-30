
import React, { useState } from 'react';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Avatar, AvatarFallback, AvatarImage } from '@/components/ui/avatar';
import { Card, CardContent, CardHeader } from '@/components/ui/card';
import { Badge } from '@/components/ui/badge';
import { Heart, MessageSquare, Send } from 'lucide-react';
import { formatDistanceToNow } from 'date-fns';
import { ptBR } from 'date-fns/locale';

interface CommunityFeedProps {
  posts: any[];
  selectedCommunity: string | null;
  onCreatePost: (communityId: string, content: string) => void;
  onReactToPost: (postId: string, reactionType: string) => void;
}

export const CommunityFeed: React.FC<CommunityFeedProps> = ({
  posts,
  selectedCommunity,
  onCreatePost,
  onReactToPost
}) => {
  const [newPost, setNewPost] = useState('');

  const handleCreatePost = () => {
    if (newPost.trim() && selectedCommunity) {
      onCreatePost(selectedCommunity, newPost);
      setNewPost('');
    }
  };

  const handleKeyPress = (e: React.KeyboardEvent) => {
    if (e.key === 'Enter' && !e.shiftKey) {
      e.preventDefault();
      handleCreatePost();
    }
  };

  if (!selectedCommunity) {
    return (
      <div className="flex-1 flex items-center justify-center">
        <div className="text-center">
          <h3 className="text-lg font-semibold mb-2">Selecione uma comunidade</h3>
          <p className="text-muted-foreground">
            Escolha uma comunidade para ver as discussões
          </p>
        </div>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      {/* New Post */}
      <Card>
        <CardContent className="p-4">
          <div className="flex space-x-3">
            <Input
              placeholder="Compartilhe algo com a comunidade..."
              value={newPost}
              onChange={(e) => setNewPost(e.target.value)}
              onKeyPress={handleKeyPress}
              className="flex-1"
            />
            <Button 
              onClick={handleCreatePost}
              disabled={!newPost.trim()}
              className="bg-gradient-to-r from-blue-600 to-purple-600"
            >
              <Send className="w-4 h-4" />
            </Button>
          </div>
        </CardContent>
      </Card>

      {/* Posts */}
      <div className="space-y-4">
        {posts.map((post) => (
          <Card key={post.id}>
            <CardHeader className="pb-3">
              <div className="flex items-start space-x-3">
                <Avatar className="w-10 h-10">
                  <AvatarImage src={post.author?.foto_perfil} alt={post.author?.nome} />
                  <AvatarFallback>
                    {post.author?.nome?.charAt(0)?.toUpperCase() || 'U'}
                  </AvatarFallback>
                </Avatar>
                <div className="flex-1">
                  <div className="flex items-center space-x-2">
                    <h4 className="font-semibold text-sm">{post.author?.nome || 'Usuário'}</h4>
                    <span className="text-xs text-muted-foreground">
                      {formatDistanceToNow(new Date(post.created_at), {
                        addSuffix: true,
                        locale: ptBR
                      })}
                    </span>
                  </div>
                </div>
              </div>
            </CardHeader>
            
            <CardContent className="pt-0">
              <p className="text-sm mb-4">{post.conteudo}</p>
              
              <div className="flex items-center space-x-4 pt-2 border-t">
                <Button
                  variant="ghost"
                  size="sm"
                  onClick={() => onReactToPost(post.id, 'like')}
                  className="flex items-center space-x-1 text-muted-foreground hover:text-red-500"
                >
                  <Heart className="w-4 h-4" />
                  <span>{post.reactionsCount || 0}</span>
                </Button>
                
                <Button
                  variant="ghost"
                  size="sm"
                  className="flex items-center space-x-1 text-muted-foreground"
                >
                  <MessageSquare className="w-4 h-4" />
                  <span>{post.commentsCount || 0}</span>
                </Button>
              </div>
            </CardContent>
          </Card>
        ))}
        
        {posts.length === 0 && (
          <div className="text-center py-8 text-muted-foreground">
            <p>Nenhuma postagem encontrada</p>
            <p className="text-sm">Seja o primeiro a postar!</p>
          </div>
        )}
      </div>
    </div>
  );
};
