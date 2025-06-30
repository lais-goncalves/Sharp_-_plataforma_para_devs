
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import Layout from "@/components/layout/Layout";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { useAuth } from "@/contexts/AuthContext";
import { useProjects } from "@/hooks/useProjects";
import EditProfileDialog from "@/components/EditProfileDialog";
import { Github, Linkedin, Calendar, MapPin, Mail, Edit } from "lucide-react";

const Profile = () => {
  const { profile } = useAuth();
  const { projects, updateProjectStats } = useProjects();
  const navigate = useNavigate();
  const [editDialogOpen, setEditDialogOpen] = useState(false);

  const handleGitHubClick = () => {
    if (profile?.github_username) {
      window.open(`https://github.com/${profile.github_username}`, '_blank');
    } else {
      window.open('https://github.com', '_blank');
    }
  };

  const handleLinkedInClick = () => {
    window.open('https://linkedin.com', '_blank');
  };

  const totalStats = {
    projects: projects.length,
    stars: projects.reduce((sum, p) => sum + (p.stars || 0), 0),
    views: projects.reduce((sum, p) => sum + (p.views || 0), 0),
    connections: 89 // Mantido temporariamente até implementarmos sistema de conexões
  };

  if (!profile) {
    return (
      <Layout>
        <div className="p-4 md:p-6">
          <div className="text-center">Carregando perfil...</div>
        </div>
      </Layout>
    );
  }

  return (
    <Layout>
      <div className="p-4 md:p-6 space-y-6 md:space-y-8">
        {/* Profile Header */}
        <Card className="shadow-sm">
          <CardContent className="p-4 md:p-6">
            <div className="flex flex-col space-y-6 md:space-y-0 md:flex-row md:items-start md:space-x-6">
              <div className="flex justify-center md:justify-start">
                <Avatar className="w-24 h-24 md:w-32 md:h-32">
                  <AvatarImage src={profile.foto_perfil || ''} alt={profile.nome} />
                  <AvatarFallback className="text-xl md:text-2xl">
                    {profile.nome?.split(' ').map(n => n[0]).join('') || 'U'}
                  </AvatarFallback>
                </Avatar>
              </div>
              
              <div className="flex-1 text-center md:text-left space-y-4">
                <div className="flex flex-col space-y-4 md:space-y-0 md:flex-row md:items-center md:justify-between">
                  <div className="space-y-2">
                    <h1 className="text-2xl md:text-3xl font-bold text-foreground">{profile.nome}</h1>
                    <p className="text-muted-foreground text-sm md:text-base">Desenvolvedor • São Paulo, SP</p>
                  </div>
                  <Button 
                    className="w-full md:w-auto"
                    onClick={() => setEditDialogOpen(true)}
                  >
                    <Edit className="w-4 h-4 mr-2" />
                    Editar Perfil
                  </Button>
                </div>
                
                <p className="text-foreground text-sm md:text-base leading-relaxed">
                  {profile.descricao || 'Desenvolvedor apaixonado por criar experiências digitais incríveis.'}
                </p>
                
                <div className="flex flex-col space-y-2 md:flex-row md:flex-wrap md:gap-4 md:space-y-0 text-sm text-muted-foreground">
                  <div className="flex items-center justify-center md:justify-start space-x-1">
                    <MapPin className="w-4 h-4" />
                    <span>São Paulo, SP</span>
                  </div>
                  <div className="flex items-center justify-center md:justify-start space-x-1">
                    <Mail className="w-4 h-4" />
                    <span className="truncate">{profile.email}</span>
                  </div>
                  <div className="flex items-center justify-center md:justify-start space-x-1">
                    <Calendar className="w-4 h-4" />
                    <span>Entrou em {profile.created_at ? new Date(profile.created_at).toLocaleDateString('pt-BR', { month: 'long', year: 'numeric' }) : 'Janeiro 2024'}</span>
                  </div>
                </div>
                
                {profile.skills && profile.skills.length > 0 && (
                  <div className="flex flex-wrap gap-2 justify-center md:justify-start">
                    {profile.skills.map((skill, index) => (
                      <Badge key={index} variant="secondary" className="text-xs">{skill}</Badge>
                    ))}
                  </div>
                )}
                
                <div className="flex flex-col space-y-2 md:flex-row md:items-center md:space-x-4 md:space-y-0">
                  <Badge className="bg-gradient-to-r from-blue-600 to-purple-600 text-white w-full md:w-auto justify-center">
                    {profile.points || 0} pontos
                  </Badge>
                  {profile.status && (
                    <Badge className={`w-full md:w-auto justify-center ${
                      profile.status === 'open-to-work' ? 'bg-green-100 text-green-800' : 'bg-gray-100 text-gray-800'
                    }`}>
                      {profile.status === 'open-to-work' ? 'Aberto a oportunidades' : profile.status}
                    </Badge>
                  )}
                </div>
                
                <div className="flex flex-col space-y-2 md:flex-row md:space-x-4 md:space-y-0">
                  <Button variant="outline" size="sm" className="w-full md:w-auto" onClick={handleGitHubClick}>
                    <Github className="w-4 h-4 mr-2" />
                    GitHub
                  </Button>
                  <Button variant="outline" size="sm" className="w-full md:w-auto" onClick={handleLinkedInClick}>
                    <Linkedin className="w-4 h-4 mr-2" />
                    LinkedIn
                  </Button>
                </div>
              </div>
            </div>
          </CardContent>
        </Card>

        {/* Stats Cards - Mobile friendly */}
        <div className="grid grid-cols-2 md:grid-cols-4 gap-3 md:gap-4">
          <Card className="shadow-sm">
            <CardContent className="p-3 md:p-4 text-center">
              <div className="text-xl md:text-2xl font-bold text-blue-600">{totalStats.projects}</div>
              <div className="text-xs md:text-sm text-muted-foreground">Projetos</div>
            </CardContent>
          </Card>
          <Card className="shadow-sm">
            <CardContent className="p-3 md:p-4 text-center">
              <div className="text-xl md:text-2xl font-bold text-green-600">{totalStats.connections}</div>
              <div className="text-xs md:text-sm text-muted-foreground">Conexões</div>
            </CardContent>
          </Card>
          <Card className="shadow-sm">
            <CardContent className="p-3 md:p-4 text-center">
              <div className="text-xl md:text-2xl font-bold text-purple-600">{totalStats.stars}</div>
              <div className="text-xs md:text-sm text-muted-foreground">Estrelas</div>
            </CardContent>
          </Card>
          <Card className="shadow-sm">
            <CardContent className="p-3 md:p-4 text-center">
              <div className="text-xl md:text-2xl font-bold text-orange-600">{totalStats.views}</div>
              <div className="text-xs md:text-sm text-muted-foreground">Visualizações</div>
            </CardContent>
          </Card>
        </div>

        {/* Tabs - Mobile responsive */}
        <Tabs defaultValue="projects" className="space-y-4 md:space-y-6">
          <TabsList className="grid w-full grid-cols-4 h-auto">
            <TabsTrigger value="projects" className="text-xs md:text-sm p-2 md:p-3">Projetos</TabsTrigger>
            <TabsTrigger value="contributions" className="text-xs md:text-sm p-2 md:p-3">Contribuições</TabsTrigger>
            <TabsTrigger value="achievements" className="text-xs md:text-sm p-2 md:p-3">Conquistas</TabsTrigger>
            <TabsTrigger value="activity" className="text-xs md:text-sm p-2 md:p-3">Atividade</TabsTrigger>
          </TabsList>
          
          <TabsContent value="projects" className="space-y-4">
            <div className="grid gap-4 md:gap-6 md:grid-cols-2">
              {projects.length > 0 ? projects.map((project) => (
                <Card key={project.id} className="shadow-sm">
                  <CardHeader className="pb-3">
                    <CardTitle className="text-base md:text-lg text-foreground">{project.titulo}</CardTitle>
                  </CardHeader>
                  <CardContent className="space-y-4">
                    <p className="text-muted-foreground text-sm md:text-base">{project.descricao}</p>
                    <div className="flex flex-col space-y-3 md:space-y-0 md:flex-row md:items-center md:justify-between">
                      <div className="flex items-center space-x-4 text-sm text-muted-foreground">
                        <button
                          onClick={() => updateProjectStats(project.id, 'stars')}
                          className="hover:text-yellow-600"
                        >
                          ⭐ {project.stars || 0}
                        </button>
                        <span>🍴 {project.forks || 0}</span>
                        <span>👁️ {project.views || 0}</span>
                      </div>
                      <Button 
                        size="sm" 
                        variant="outline" 
                        className="w-full md:w-auto"
                        onClick={() => updateProjectStats(project.id, 'views')}
                      >
                        Ver Projeto
                      </Button>
                    </div>
                  </CardContent>
                </Card>
              )) : (
                <div className="col-span-full text-center py-8 text-muted-foreground">
                  <p>Nenhum projeto encontrado</p>
                  <p className="text-sm">Crie seu primeiro projeto!</p>
                </div>
              )}
            </div>
          </TabsContent>
          
          <TabsContent value="contributions">
            <Card className="shadow-sm">
              <CardHeader>
                <CardTitle className="text-lg md:text-xl">Contribuições Recentes</CardTitle>
              </CardHeader>
              <CardContent>
                <div className="text-center py-8 text-muted-foreground">
                  <p>Contribuições serão mostradas aqui</p>
                  <p className="text-sm">Faça sua primeira contribuição!</p>
                </div>
              </CardContent>
            </Card>
          </TabsContent>
          
          <TabsContent value="achievements">
            <Card className="shadow-sm">
              <CardHeader>
                <CardTitle className="text-lg md:text-xl">Conquistas</CardTitle>
              </CardHeader>
              <CardContent>
                <div className="text-center py-8 text-muted-foreground">
                  <p>Conquistas serão mostradas aqui</p>
                  <p className="text-sm">Complete ações para ganhar conquistas!</p>
                </div>
              </CardContent>
            </Card>
          </TabsContent>
          
          <TabsContent value="activity">
            <Card className="shadow-sm">
              <CardHeader>
                <CardTitle className="text-lg md:text-xl">Atividade Recente</CardTitle>
              </CardHeader>
              <CardContent>
                <div className="text-center py-8 text-muted-foreground">
                  <p>Atividades recentes serão mostradas aqui</p>
                  <p className="text-sm">Sua atividade na plataforma aparecerá aqui!</p>
                </div>
              </CardContent>
            </Card>
          </TabsContent>
        </Tabs>

        <EditProfileDialog 
          open={editDialogOpen} 
          onOpenChange={setEditDialogOpen} 
        />
      </div>
    </Layout>
  );
};

export default Profile;
