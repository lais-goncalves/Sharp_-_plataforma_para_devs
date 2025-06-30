
import React from 'react';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { Users, MessageSquare } from 'lucide-react';

interface CommunityListProps {
  communities: any[];
  selectedCommunity: string | null;
  onSelectCommunity: (id: string) => void;
  onJoinCommunity: (id: string) => void;
  loading: boolean;
}

export const CommunityList: React.FC<CommunityListProps> = ({
  communities,
  selectedCommunity,
  onSelectCommunity,
  onJoinCommunity,
  loading
}) => {
  if (loading) {
    return (
      <div className="grid md:grid-cols-2 gap-4">
        {[...Array(4)].map((_, i) => (
          <div key={i} className="border rounded-lg p-4 animate-pulse">
            <div className="flex items-start justify-between mb-3">
              <div className="flex items-center space-x-3">
                <div className="w-12 h-12 bg-gray-300 rounded-lg"></div>
                <div>
                  <div className="h-4 bg-gray-300 rounded w-32 mb-2"></div>
                  <div className="h-3 bg-gray-200 rounded w-20"></div>
                </div>
              </div>
            </div>
            <div className="h-3 bg-gray-200 rounded w-full mb-3"></div>
            <div className="h-8 bg-gray-300 rounded w-24"></div>
          </div>
        ))}
      </div>
    );
  }

  return (
    <div className="grid md:grid-cols-2 gap-4">
      {communities.map((community) => (
        <div 
          key={community.id} 
          className={`border rounded-lg p-4 hover:shadow-md transition-shadow cursor-pointer ${
            selectedCommunity === community.id ? 'border-blue-500 bg-blue-50' : ''
          }`}
          onClick={() => onSelectCommunity(community.id)}
        >
          <div className="flex items-start justify-between mb-3">
            <div className="flex items-center space-x-3">
              <div className="w-12 h-12 bg-gradient-to-r from-blue-600 to-purple-600 rounded-lg flex items-center justify-center">
                <span className="text-white font-bold text-lg">
                  {community.nome.charAt(0).toUpperCase()}
                </span>
              </div>
              <div>
                <h4 className="font-semibold text-lg">{community.nome}</h4>
                <p className="text-sm text-muted-foreground">
                  {community.membersCount} membros
                </p>
              </div>
            </div>
          </div>
          
          <p className="text-sm text-muted-foreground mb-3 line-clamp-2">
            {community.descricao}
          </p>
          
          <div className="flex items-center justify-between">
            <div className="flex items-center space-x-4">
              <div className="flex items-center space-x-1 text-sm text-muted-foreground">
                <Users className="w-4 h-4" />
                <span>{community.membersCount}</span>
              </div>
              <div className="flex items-center space-x-1 text-sm text-muted-foreground">
                <MessageSquare className="w-4 h-4" />
                <span>{community.postsCount}</span>
              </div>
            </div>
            
            <Button 
              size="sm" 
              variant="outline"
              onClick={(e) => {
                e.stopPropagation();
                onJoinCommunity(community.id);
              }}
            >
              Participar
            </Button>
          </div>
        </div>
      ))}
      
      {communities.length === 0 && !loading && (
        <div className="col-span-2 text-center py-8 text-muted-foreground">
          <p>Nenhuma comunidade encontrada</p>
        </div>
      )}
    </div>
  );
};
