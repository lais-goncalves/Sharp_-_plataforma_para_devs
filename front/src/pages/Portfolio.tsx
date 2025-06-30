import Layout from "@/components/layout/Layout";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Github, BookOpen, Search, Filter } from "lucide-react";

const Portfolio = () => {
  const projects = [
    {
      id: 1,
      name: 'E-commerce Dashboard',
      description: 'Dashboard completo para e-commerce com React e TypeScript',
      image: 'https://images.unsplash.com/photo-1551288049-bebda4e38f71?w=400&h=300&fit=crop',
      tech: ['React', 'TypeScript', 'Tailwind CSS', 'Chart.js'],
      stars: 45,
      forks: 12,
      views: 234,
      status: 'deployed',
      github: 'https://github.com/user/ecommerce-dashboard',
      demo: 'https://ecommerce-dashboard.vercel.app'
    },
    {
      id: 2,
      name: 'Chat App Real-time',
      description: 'Aplicativo de chat em tempo real com Socket.io',
      image: 'https://images.unsplash.com/photo-1577563908411-5077b6dc7624?w=400&h=300&fit=crop',
      tech: ['Node.js', 'Socket.io', 'React', 'MongoDB'],
      stars: 67,
      forks: 23,
      views: 189,
      status: 'development',
      github: 'https://github.com/user/chat-app',
      demo: null
    },
    {
      id: 3,
      name: 'Mobile Game UI',
      description: 'Interface de usuário para jogo mobile com React Native',
      image: 'https://images.unsplash.com/photo-1511512578047-dfb367046420?w=400&h=300&fit=crop',
      tech: ['React Native', 'Expo', 'Styled Components'],
      stars: 23,
      forks: 8,
      views: 156,
      status: 'completed',
      github: 'https://github.com/user/mobile-game-ui',
      demo: 'https://expo.dev/@user/mobile-game-ui'
    }
  ];

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'deployed': return 'bg-green-100 text-green-800';
      case 'development': return 'bg-yellow-100 text-yellow-800';
      case 'completed': return 'bg-blue-100 text-blue-800';
      default: return 'bg-gray-100 text-gray-800';
    }
  };

  const getStatusText = (status: string) => {
    switch (status) {
      case 'deployed': return 'Deployed';
      case 'development': return 'Em Desenvolvimento';
      case 'completed': return 'Completo';
      default: return 'Status';
    }
  };

  return (
    <Layout>
      <div className="p-3 md:p-4 space-y-4">
        {/* Header */}
        <div className="flex flex-col md:flex-row md:items-center justify-between space-y-3 md:space-y-0">
          <div>
            <h1 className="text-2xl md:text-3xl font-bold">Meu Portfólio</h1>
            <p className="text-gray-600 text-sm md:text-base">Gerencie e exiba seus projetos</p>
          </div>
          <div className="flex flex-col sm:flex-row space-y-2 sm:space-y-0 sm:space-x-3">
            <Button className="bg-gradient-to-r from-blue-600 to-purple-600 text-sm">
              <Github className="w-4 h-4 mr-2" />
              Importar do GitHub
            </Button>
            <Button variant="outline" className="text-sm">
              <BookOpen className="w-4 h-4 mr-2" />
              Novo Projeto
            </Button>
          </div>
        </div>

        {/* Filters and Search */}
        <Card>
          <CardContent className="p-3 md:p-4">
            <div className="flex flex-col md:flex-row space-y-3 md:space-y-0 md:space-x-4">
              <div className="flex-1 relative">
                <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-muted-foreground w-4 h-4" />
                <Input
                  placeholder="Buscar projetos..."
                  className="pl-10"
                />
              </div>
              <Button variant="outline" size="sm">
                <Filter className="w-4 h-4 mr-2" />
                Filtros
              </Button>
            </div>
          </CardContent>
        </Card>

        {/* Projects Grid */}
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
          {projects.map((project) => (
            <Card key={project.id} className="group hover:shadow-xl transition-all duration-300">
              <div className="relative overflow-hidden rounded-t-lg">
                <img 
                  src={project.image} 
                  alt={project.name}
                  className="w-full h-36 md:h-48 object-cover group-hover:scale-105 transition-transform duration-300"
                />
                <div className="absolute top-3 right-3">
                  <Badge className={getStatusColor(project.status)}>
                    {getStatusText(project.status)}
                  </Badge>
                </div>
              </div>
              
              <CardHeader className="pb-2">
                <CardTitle className="text-base md:text-lg">{project.name}</CardTitle>
                <p className="text-xs md:text-sm text-gray-600">{project.description}</p>
              </CardHeader>
              
              <CardContent className="space-y-3">
                <div className="flex flex-wrap gap-1">
                  {project.tech.map((tech, index) => (
                    <Badge key={index} variant="secondary" className="text-xs">
                      {tech}
                    </Badge>
                  ))}
                </div>
                
                <div className="flex justify-between text-xs md:text-sm text-gray-600">
                  <span>⭐ {project.stars}</span>
                  <span>🍴 {project.forks}</span>
                  <span>👁️ {project.views}</span>
                </div>
                
                <div className="flex space-x-2">
                  <Button size="sm" variant="outline" className="flex-1 text-xs">
                    <Github className="w-3 h-3 mr-1" />
                    Código
                  </Button>
                  {project.demo && (
                    <Button size="sm" className="flex-1 bg-gradient-to-r from-blue-600 to-purple-600 text-xs">
                      Ver Demo
                    </Button>
                  )}
                </div>
              </CardContent>
            </Card>
          ))}
        </div>

        {/* Stats Summary */}
        <Card>
          <CardHeader className="pb-3">
            <CardTitle className="text-lg">Estatísticas do Portfólio</CardTitle>
          </CardHeader>
          <CardContent>
            <div className="grid grid-cols-2 md:grid-cols-4 gap-4">
              <div className="text-center">
                <div className="text-xl md:text-2xl font-bold text-blue-600">12</div>
                <div className="text-xs md:text-sm text-gray-600">Total de Projetos</div>
              </div>
              <div className="text-center">
                <div className="text-xl md:text-2xl font-bold text-green-600">135</div>
                <div className="text-xs md:text-sm text-gray-600">Total de Stars</div>
              </div>
              <div className="text-center">
                <div className="text-xl md:text-2xl font-bold text-purple-600">43</div>
                <div className="text-xs md:text-sm text-gray-600">Total de Forks</div>
              </div>
              <div className="text-center">
                <div className="text-xl md:text-2xl font-bold text-orange-600">1.2k</div>
                <div className="text-xs md:text-sm text-gray-600">Visualizações</div>
              </div>
            </div>
          </CardContent>
        </Card>
      </div>
    </Layout>
  );
};

export default Portfolio;
