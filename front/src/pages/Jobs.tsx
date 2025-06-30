import Layout from "@/components/layout/Layout";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Badge } from "@/components/ui/badge";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { Search, Filter, MapPin, Clock, DollarSign, Briefcase } from "lucide-react";

const Jobs = () => {
  const jobs = [
    {
      id: 1,
      title: 'Desenvolvedor React Junior',
      company: 'TechCorp',
      logo: 'https://api.dicebear.com/7.x/identicon/svg?seed=techcorp',
      location: 'São Paulo, SP',
      type: 'CLT',
      experience: 'Junior',
      salary: 'R$ 4.000 - R$ 6.000',
      description: 'Procuramos desenvolvedor React para integrar nossa equipe...',
      tags: ['React', 'JavaScript', 'TypeScript'],
      posted: '2 dias atrás',
      applications: 23
    },
    {
      id: 2,
      title: 'Fullstack Developer',
      company: 'StartupXYZ',
      logo: 'https://api.dicebear.com/7.x/identicon/svg?seed=startupxyz',
      location: 'Remoto',
      type: 'PJ',
      experience: 'Pleno',
      salary: 'R$ 8.000 - R$ 12.000',
      description: 'Oportunidade em startup inovadora para desenvolvedor fullstack...',
      tags: ['Node.js', 'React', 'MongoDB', 'AWS'],
      posted: '1 dia atrás',
      applications: 45
    },
    {
      id: 3,
      title: 'Frontend Engineer',
      company: 'Design Studio',
      logo: 'https://api.dicebear.com/7.x/identicon/svg?seed=designstudio',
      location: 'Rio de Janeiro, RJ',
      type: 'Híbrido',
      experience: 'Pleno/Senior',
      salary: 'R$ 10.000 - R$ 15.000',
      description: 'Procuramos frontend engineer para projetos de alta qualidade...',
      tags: ['Vue.js', 'Nuxt.js', 'Sass', 'Figma'],
      posted: '3 dias atrás',
      applications: 67
    }
  ];

  const companies = [
    { name: 'TechCorp', logo: 'https://api.dicebear.com/7.x/identicon/svg?seed=techcorp', jobs: 5 },
    { name: 'StartupXYZ', logo: 'https://api.dicebear.com/7.x/identicon/svg?seed=startupxyz', jobs: 3 },
    { name: 'Design Studio', logo: 'https://api.dicebear.com/7.x/identicon/svg?seed=designstudio', jobs: 8 },
  ];

  const getTypeColor = (type: string) => {
    switch (type) {
      case 'CLT': return 'bg-green-100 text-green-800';
      case 'PJ': return 'bg-blue-100 text-blue-800';
      case 'Híbrido': return 'bg-purple-100 text-purple-800';
      case 'Remoto': return 'bg-orange-100 text-orange-800';
      default: return 'bg-gray-100 text-gray-800';
    }
  };

  const getExperienceColor = (level: string) => {
    switch (level) {
      case 'Junior': return 'bg-yellow-100 text-yellow-800';
      case 'Pleno': return 'bg-blue-100 text-blue-800';
      case 'Senior': return 'bg-purple-100 text-purple-800';
      case 'Pleno/Senior': return 'bg-indigo-100 text-indigo-800';
      default: return 'bg-gray-100 text-gray-800';
    }
  };

  return (
    <Layout>
      <div className="p-3 md:p-4 space-y-4">
        {/* Header */}
        <div className="flex flex-col md:flex-row md:items-center justify-between space-y-3 md:space-y-0">
          <div>
            <h1 className="text-2xl md:text-3xl font-bold">Vagas de Emprego</h1>
            <p className="text-gray-600 text-sm md:text-base">Encontre oportunidades perfeitas para sua carreira</p>
          </div>
          <div className="flex flex-col sm:flex-row space-y-2 sm:space-y-0 sm:space-x-3">
            <Button variant="outline" size="sm">
              <Briefcase className="w-4 h-4 mr-2" />
              Minhas Candidaturas
            </Button>
            <Button className="bg-gradient-to-r from-blue-600 to-purple-600" size="sm">
              Publicar Vaga
            </Button>
          </div>
        </div>

        {/* Search and Filters */}
        <Card>
          <CardContent className="p-3 md:p-4">
            <div className="flex flex-col md:flex-row space-y-3 md:space-y-0 md:space-x-4">
              <div className="flex-1 relative">
                <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400 w-4 h-4" />
                <Input
                  placeholder="Buscar vagas por cargo, empresa ou tecnologia..."
                  className="pl-10"
                />
              </div>
              <div className="flex space-x-2">
                <Button variant="outline" size="sm">
                  <Filter className="w-4 h-4 mr-2" />
                  Filtros
                </Button>
                <Button variant="outline" size="sm">
                  <MapPin className="w-4 h-4 mr-2" />
                  Localização
                </Button>
              </div>
            </div>
            
            <div className="flex flex-wrap gap-2 mt-3">
              <Badge variant="secondary">React</Badge>
              <Badge variant="secondary">TypeScript</Badge>
              <Badge variant="secondary">Node.js</Badge>
              <Badge variant="secondary">Remote</Badge>
              <Badge variant="secondary">Junior</Badge>
            </div>
          </CardContent>
        </Card>

        <div className="grid lg:grid-cols-4 gap-4">
          {/* Job Listings */}
          <div className="lg:col-span-3 space-y-3">
            {jobs.map((job) => (
              <Card key={job.id} className="hover:shadow-lg transition-shadow">
                <CardContent className="p-4 md:p-6">
                  <div className="flex items-start justify-between mb-3">
                    <div className="flex items-start space-x-3">
                      <Avatar className="w-10 h-10 md:w-12 md:h-12">
                        <AvatarImage src={job.logo} alt={job.company} />
                        <AvatarFallback>{job.company[0]}</AvatarFallback>
                      </Avatar>
                      <div>
                        <h3 className="text-lg md:text-xl font-semibold mb-1">{job.title}</h3>
                        <p className="text-gray-600 font-medium text-sm md:text-base">{job.company}</p>
                        <div className="flex flex-wrap items-center gap-2 md:gap-4 text-xs md:text-sm text-gray-500 mt-2">
                          <div className="flex items-center space-x-1">
                            <MapPin className="w-3 h-3 md:w-4 md:h-4" />
                            <span>{job.location}</span>
                          </div>
                          <div className="flex items-center space-x-1">
                            <Clock className="w-3 h-3 md:w-4 md:h-4" />
                            <span>{job.posted}</span>
                          </div>
                          <div className="flex items-center space-x-1">
                            <DollarSign className="w-3 h-3 md:w-4 md:h-4" />
                            <span className="hidden sm:inline">{job.salary}</span>
                          </div>
                        </div>
                      </div>
                    </div>
                    <div className="text-right">
                      <Badge className={getTypeColor(job.type)} variant="secondary">
                        {job.type}
                      </Badge>
                    </div>
                  </div>

                  <p className="text-gray-700 mb-3 text-sm md:text-base">{job.description}</p>

                  <div className="flex flex-col md:flex-row md:items-center justify-between space-y-3 md:space-y-0">
                    <div className="space-y-2">
                      <div className="flex flex-wrap gap-1">
                        {job.tags.map((tag, index) => (
                          <Badge key={index} variant="outline" className="text-xs">
                            {tag}
                          </Badge>
                        ))}
                      </div>
                      <div className="flex items-center space-x-3">
                        <Badge className={getExperienceColor(job.experience)} variant="secondary">
                          {job.experience}
                        </Badge>
                        <span className="text-xs md:text-sm text-gray-500">
                          {job.applications} candidatos
                        </span>
                      </div>
                    </div>
                    <div className="flex space-x-2">
                      <Button variant="outline" size="sm">
                        Salvar
                      </Button>
                      <Button className="bg-gradient-to-r from-blue-600 to-purple-600" size="sm">
                        Candidatar-se
                      </Button>
                    </div>
                  </div>
                </CardContent>
              </Card>
            ))}

            <div className="text-center py-6">
              <Button variant="outline">Carregar Mais Vagas</Button>
            </div>
          </div>

          {/* Sidebar */}
          <div className="space-y-4">
            {/* Job Alerts */}
            <Card>
              <CardHeader className="pb-3">
                <CardTitle className="text-lg">Alertas de Vaga</CardTitle>
              </CardHeader>
              <CardContent>
                <p className="text-xs md:text-sm text-gray-600 mb-3">
                  Receba notificações sobre novas vagas que combinam com seu perfil.
                </p>
                <Button className="w-full" size="sm">Criar Alerta</Button>
              </CardContent>
            </Card>

            {/* Companies */}
            <Card>
              <CardHeader className="pb-3">
                <CardTitle className="text-lg">Empresas em Destaque</CardTitle>
              </CardHeader>
              <CardContent className="space-y-2">
                {companies.map((company, index) => (
                  <div key={index} className="flex items-center justify-between">
                    <div className="flex items-center space-x-2">
                      <Avatar className="w-6 h-6 md:w-8 md:h-8">
                        <AvatarImage src={company.logo} alt={company.name} />
                        <AvatarFallback>{company.name[0]}</AvatarFallback>
                      </Avatar>
                      <span className="font-medium text-xs md:text-sm">{company.name}</span>
                    </div>
                    <Badge variant="secondary" className="text-xs">
                      {company.jobs} vagas
                    </Badge>
                  </div>
                ))}
              </CardContent>
            </Card>

            {/* Career Tips */}
            <Card>
              <CardHeader className="pb-3">
                <CardTitle className="text-lg">Dicas de Carreira</CardTitle>
              </CardHeader>
              <CardContent className="space-y-2">
                <div className="text-xs md:text-sm">
                  <h4 className="font-semibold mb-1">📝 Como escrever um bom README</h4>
                  <p className="text-gray-600 text-xs">Aprenda a documentar seus projetos</p>
                </div>
                <div className="text-xs md:text-sm">
                  <h4 className="font-semibold mb-1">💼 Preparação para entrevistas</h4>
                  <p className="text-gray-600 text-xs">Dicas para se sair bem nas entrevistas</p>
                </div>
                <div className="text-xs md:text-sm">
                  <h4 className="font-semibold mb-1">🚀 Portfolio que impressiona</h4>
                  <p className="text-gray-600 text-xs">Como destacar seus melhores projetos</p>
                </div>
              </CardContent>
            </Card>
          </div>
        </div>
      </div>
    </Layout>
  );
};

export default Jobs;
