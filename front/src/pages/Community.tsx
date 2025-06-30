
import Layout from "@/components/layout/Layout";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Badge } from "@/components/ui/badge";
import { Search, Users, MessageSquare, TrendingUp } from "lucide-react";
import { useCommunity } from "@/hooks/useCommunity";
import { CommunityList } from "@/components/community/CommunityList";
import { CommunityFeed } from "@/components/community/CommunityFeed";

const Community = () => {
  const {
    communities,
    posts,
    selectedCommunity,
    setSelectedCommunity,
    createPost,
    reactToPost,
    joinCommunity,
    loading
  } = useCommunity();

  const selectedCommunityData = communities.find(
    comm => comm.id === selectedCommunity
  );

  return (
    <Layout>
      <div className="p-6 space-y-6">
        {/* Header */}
        <div className="flex flex-col md:flex-row md:items-center justify-between space-y-4 md:space-y-0">
          <div>
            <h1 className="text-3xl font-bold">Comunidade</h1>
            <p className="text-muted-foreground">Conecte-se, aprenda e compartilhe conhecimento</p>
          </div>
          <div className="flex space-x-3">
            <Button variant="outline">
              <Users className="w-4 h-4 mr-2" />
              Encontrar Grupos
            </Button>
            <Button className="bg-gradient-to-r from-blue-600 to-purple-600">
              <MessageSquare className="w-4 h-4 mr-2" />
              Nova Comunidade
            </Button>
          </div>
        </div>

        {/* Search */}
        <Card>
          <CardContent className="p-4">
            <div className="flex space-x-4">
              <div className="flex-1 relative">
                <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400 w-4 h-4" />
                <Input
                  placeholder="Buscar comunidades, posts, pessoas..."
                  className="pl-10"
                />
              </div>
              <div className="flex space-x-2">
                <Badge variant="secondary">Mais Ativas</Badge>
                <Badge variant="outline">Recentes</Badge>
                <Badge variant="outline">Populares</Badge>
              </div>
            </div>
          </CardContent>
        </Card>

        <div className="grid lg:grid-cols-4 gap-6">
          {/* Main Content */}
          <div className="lg:col-span-3 space-y-6">
            {!selectedCommunity ? (
              <Card>
                <CardHeader>
                  <CardTitle>Comunidades Recomendadas</CardTitle>
                </CardHeader>
                <CardContent>
                  <CommunityList
                    communities={communities}
                    selectedCommunity={selectedCommunity}
                    onSelectCommunity={setSelectedCommunity}
                    onJoinCommunity={joinCommunity}
                    loading={loading}
                  />
                </CardContent>
              </Card>
            ) : (
              <div className="space-y-6">
                <Card>
                  <CardHeader>
                    <div className="flex items-center justify-between">
                      <div className="flex items-center space-x-3">
                        <div className="w-12 h-12 bg-gradient-to-r from-blue-600 to-purple-600 rounded-lg flex items-center justify-center">
                          <span className="text-white font-bold text-lg">
                            {selectedCommunityData?.nome.charAt(0).toUpperCase()}
                          </span>
                        </div>
                        <div>
                          <CardTitle>{selectedCommunityData?.nome}</CardTitle>
                          <p className="text-sm text-muted-foreground">
                            {selectedCommunityData?.membersCount} membros
                          </p>
                        </div>
                      </div>
                      <Button 
                        variant="outline" 
                        onClick={() => setSelectedCommunity(null)}
                      >
                        Voltar
                      </Button>
                    </div>
                  </CardHeader>
                </Card>

                <CommunityFeed
                  posts={posts}
                  selectedCommunity={selectedCommunity}
                  onCreatePost={createPost}
                  onReactToPost={reactToPost}
                />
              </div>
            )}
          </div>

          {/* Sidebar */}
          <div className="space-y-6">
            <Card>
              <CardHeader>
                <CardTitle className="flex items-center">
                  <TrendingUp className="w-4 h-4 mr-2" />
                  Trending
                </CardTitle>
              </CardHeader>
              <CardContent className="space-y-3">
                {['React Hooks', 'TypeScript', 'Next.js', 'Tailwind CSS'].map((topic, index) => (
                  <div key={index} className="flex items-center justify-between">
                    <span className="font-medium">#{topic}</span>
                    <Badge variant="secondary">{Math.floor(Math.random() * 50) + 10} posts</Badge>
                  </div>
                ))}
              </CardContent>
            </Card>

            <Card>
              <CardHeader>
                <CardTitle>Ações Rápidas</CardTitle>
              </CardHeader>
              <CardContent className="space-y-2">
                <Button variant="outline" className="w-full justify-start">
                  <MessageSquare className="w-4 h-4 mr-2" />
                  Nova Discussão
                </Button>
                <Button variant="outline" className="w-full justify-start">
                  <Users className="w-4 h-4 mr-2" />
                  Encontrar Pessoas
                </Button>
              </CardContent>
            </Card>
          </div>
        </div>
      </div>
    </Layout>
  );
};

export default Community;
