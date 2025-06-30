
import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import Layout from "@/components/layout/Layout";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { useAuth } from "@/contexts/AuthContext";
import { supabase } from "@/integrations/supabase/client";
import { BookOpen, MessageSquare, Users, TrendingUp } from "lucide-react";
import { useQuery } from "@tanstack/react-query";

const Dashboard = () => {
  const { profile } = useAuth();
  const navigate = useNavigate();

  // Fetch user's projects
  const { data: userProjects = [] } = useQuery({
    queryKey: ['userProjects', profile?.id],
    queryFn: async () => {
      if (!profile?.id) return [];
      const { data, error } = await supabase
        .from('projetos')
        .select('*')
        .eq('user_id', profile.id)
        .order('created_at', { ascending: false })
        .limit(3);
      
      if (error) throw error;
      return data;
    },
    enabled: !!profile?.id,
  });

  // Fetch recent community activity - fixed the relationship query
  const { data: recentMessages = [] } = useQuery({
    queryKey: ['recentMessages', profile?.id],
    queryFn: async () => {
      if (!profile?.id) return [];
      const { data, error } = await supabase
        .from('mensagens')
        .select(`
          *,
          sender:users!sender_id(nome, foto_perfil),
          receiver:users!receiver_id(nome, foto_perfil)
        `)
        .or(`sender_id.eq.${profile.id},receiver_id.eq.${profile.id}`)
        .order('created_at', { ascending: false })
        .limit(3);
      
      if (error) throw error;
      return data;
    },
    enabled: !!profile?.id,
  });

  // Calculate stats
  const stats = [
    { 
      label: 'Projetos', 
      value: userProjects.length.toString(), 
      icon: BookOpen, 
      color: 'from-blue-500 to-cyan-500' 
    },
    { 
      label: 'Mensagens', 
      value: recentMessages.length.toString(), 
      icon: MessageSquare, 
      color: 'from-purple-500 to-pink-500' 
    },
    { 
      label: 'Pontos', 
      value: profile?.points?.toString() || '0', 
      icon: TrendingUp, 
      color: 'from-green-500 to-emerald-500' 
    },
    { 
      label: 'Conexões', 
      value: '0', 
      icon: Users, 
      color: 'from-orange-500 to-red-500' 
    },
  ];

  const handleNewProject = () => {
    navigate('/portfolio');
  };

  const handleSendMessage = () => {
    navigate('/chat');
  };

  const handleFindDevs = () => {
    navigate('/community');
  };

  const handleViewStats = () => {
    navigate('/profile');
  };

  if (!profile) {
    return (
      <Layout>
        <div className="p-6">
          <div className="animate-pulse space-y-6">
            <div className="h-32 bg-gray-200 rounded-xl"></div>
            <div className="grid grid-cols-4 gap-6">
              {[...Array(4)].map((_, i) => (
                <div key={i} className="h-24 bg-gray-200 rounded-lg"></div>
              ))}
            </div>
          </div>
        </div>
      </Layout>
    );
  }

  return (
    <Layout>
      <div className="p-6 space-y-6">
        {/* Welcome Section */}
        <div className="bg-gradient-to-r from-blue-600 to-purple-600 rounded-xl p-6 text-white">
          <h1 className="text-2xl font-bold mb-2">Bem-vindo de volta, {profile.nome}! 👋</h1>
          <p className="opacity-90">
            Você tem {userProjects.length} projetos e {recentMessages.length} mensagens recentes.
          </p>
        </div>

        {/* Stats Grid */}
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
          {stats.map((stat, index) => {
            const Icon = stat.icon;
            return (
              <Card key={index} className="hover:shadow-lg transition-shadow">
                <CardContent className="p-6">
                  <div className="flex items-center justify-between">
                    <div>
                      <p className="text-sm text-muted-foreground">{stat.label}</p>
                      <p className="text-2xl font-bold text-foreground">{stat.value}</p>
                    </div>
                    <div className={`w-12 h-12 bg-gradient-to-r ${stat.color} rounded-lg flex items-center justify-center`}>
                      <Icon className="w-6 h-6 text-white" />
                    </div>
                  </div>
                </CardContent>
              </Card>
            );
          })}
        </div>

        <div className="grid lg:grid-cols-3 gap-6">
          {/* Recent Projects */}
          <Card className="lg:col-span-2">
            <CardHeader>
              <CardTitle className="flex items-center justify-between">
                Projetos Recentes
                <Button variant="outline" size="sm">Ver Todos</Button>
              </CardTitle>
            </CardHeader>
            <CardContent className="space-y-4">
              {userProjects.length > 0 ? (
                userProjects.map((project) => (
                  <div key={project.id} className="flex items-center justify-between p-4 border rounded-lg hover:bg-muted transition-colors">
                    <div className="flex-1">
                      <h4 className="font-semibold text-foreground">{project.titulo}</h4>
                      <p className="text-sm text-muted-foreground mt-1">{project.descricao}</p>
                      {project.tags && project.tags.length > 0 && (
                        <div className="flex space-x-2 mt-2">
                          {project.tags.slice(0, 3).map((tag, i) => (
                            <Badge key={i} variant="secondary" className="text-xs">{tag}</Badge>
                          ))}
                        </div>
                      )}
                    </div>
                    <div className="text-right text-sm text-muted-foreground">
                      <div className="text-foreground">{project.views} visualizações</div>
                      <div className="text-foreground">{project.stars} curtidas</div>
                    </div>
                  </div>
                ))
              ) : (
                <div className="text-center py-8 text-muted-foreground">
                  <BookOpen className="w-12 h-12 mx-auto mb-4 opacity-50" />
                  <p>Nenhum projeto encontrado</p>
                  <p className="text-sm">Comece criando seu primeiro projeto!</p>
                </div>
              )}
            </CardContent>
          </Card>

          {/* Recent Activity */}
          <Card>
            <CardHeader>
              <CardTitle>Atividade Recente</CardTitle>
            </CardHeader>
            <CardContent className="space-y-4">
              {recentMessages.length > 0 ? (
                recentMessages.map((message) => (
                  <div key={message.id} className="flex items-start space-x-3">
                    <Avatar className="w-8 h-8">
                      <AvatarImage src={message.sender?.foto_perfil || ''} />
                      <AvatarFallback>
                        {message.sender?.nome?.split(' ').map(n => n[0]).join('') || 'U'}
                      </AvatarFallback>
                    </Avatar>
                    <div className="flex-1">
                      <p className="text-sm text-foreground">
                        <span className="font-semibold">{message.sender?.nome}</span> enviou uma mensagem
                      </p>
                      <p className="text-xs text-muted-foreground">
                        {new Date(message.created_at).toLocaleDateString()}
                      </p>
                    </div>
                  </div>
                ))
              ) : (
                <div className="text-center py-8 text-muted-foreground">
                  <MessageSquare className="w-12 h-12 mx-auto mb-4 opacity-50" />
                  <p>Nenhuma atividade recente</p>
                </div>
              )}
            </CardContent>
          </Card>
        </div>

        {/* Quick Actions */}
        <Card>
          <CardHeader>
            <CardTitle>Ações Rápidas</CardTitle>
          </CardHeader>
          <CardContent>
            <div className="grid grid-cols-2 md:grid-cols-4 gap-4">
              <Button 
                className="h-20 flex flex-col space-y-2 bg-gradient-to-r from-blue-500 to-cyan-500"
                onClick={handleNewProject}
              >
                <BookOpen className="w-6 h-6" />
                <span>Novo Projeto</span>
              </Button>
              <Button 
                variant="outline" 
                className="h-20 flex flex-col space-y-2"
                onClick={handleSendMessage}
              >
                <MessageSquare className="w-6 h-6" />
                <span>Enviar Mensagem</span>
              </Button>
              <Button 
                variant="outline" 
                className="h-20 flex flex-col space-y-2"
                onClick={handleFindDevs}
              >
                <Users className="w-6 h-6" />
                <span>Encontrar Devs</span>
              </Button>
              <Button 
                variant="outline" 
                className="h-20 flex flex-col space-y-2"
                onClick={handleViewStats}
              >
                <TrendingUp className="w-6 h-6" />
                <span>Ver Estatísticas</span>
              </Button>
            </div>
          </CardContent>
        </Card>
      </div>
    </Layout>
  );
};

export default Dashboard;
