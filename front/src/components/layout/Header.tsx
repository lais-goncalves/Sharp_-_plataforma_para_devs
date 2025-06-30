
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { Badge } from "@/components/ui/badge";
import { Drawer, DrawerContent, DrawerTrigger } from "@/components/ui/drawer";
import { useAuth } from "@/contexts/AuthContext";
import { useNavigate } from "react-router-dom";
import { Search, Bell, MessageSquare, Github, Menu } from "lucide-react";
import ThemeToggle from "@/components/ThemeToggle";
import Sidebar from "./Sidebar";

const Header = () => {
  const { profile } = useAuth();
  const navigate = useNavigate();

  const handleGitHubClick = () => {
    if (profile?.github_username) {
      window.open(`https://github.com/${profile.github_username}`, '_blank');
    } else {
      window.open('https://github.com', '_blank');
    }
  };

  const handleNotificationsClick = () => {
    console.log('Abrir notificações');
  };

  const handleMessagesClick = () => {
    navigate('/chat');
  };

  const handleProfileClick = () => {
    navigate('/profile');
  };

  const LogoContent = () => (
    <div className="flex items-center space-x-2">
      <div className="w-6 h-6 lg:w-8 lg:h-8 bg-gradient-to-r from-blue-600 to-purple-600 rounded-lg flex items-center justify-center">
        <span className="text-white font-bold text-xs lg:text-sm">#</span>
      </div>
      <span className="text-lg lg:text-xl font-bold bg-gradient-to-r from-blue-600 to-purple-600 bg-clip-text text-transparent">
        Sharp
      </span>
    </div>
  );

  return (
    <header className="bg-background border-b border-border px-4 lg:px-6 py-3 fixed top-0 left-0 right-0 z-50">
      <div className="flex items-center justify-between max-w-7xl mx-auto">
        <div className="flex items-center space-x-2 lg:space-x-4">
          {/* Logo para mobile - abre sidebar com drawer */}
          <div className="lg:hidden">
            <Drawer>
              <DrawerTrigger asChild>
                <button className="flex items-center space-x-2 cursor-pointer">
                  <LogoContent />
                </button>
              </DrawerTrigger>
              <DrawerContent>
                <div className="h-[80vh] overflow-y-auto">
                  <Sidebar isMobile={true} />
                </div>
              </DrawerContent>
            </Drawer>
          </div>

          {/* Logo para desktop - navega para dashboard */}
          <div className="hidden lg:flex items-center space-x-2 cursor-pointer" onClick={() => navigate('/dashboard')}>
            <LogoContent />
          </div>
        </div>

        {/* Search - escondido em mobile */}
        <div className="hidden md:flex flex-1 max-w-xl mx-8">
          <div className="relative w-full">
            <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-muted-foreground w-4 h-4" />
            <Input
              placeholder="Buscar projetos, desenvolvedores, empresas..."
              className="pl-10 bg-muted/50 border-border"
            />
          </div>
        </div>

        <div className="flex items-center space-x-2 lg:space-x-4">
          {/* GitHub - escondido em mobile pequeno */}
          <Button variant="ghost" size="icon" onClick={handleGitHubClick} className="hidden sm:flex">
            <Github className="w-5 h-5" />
          </Button>

          <ThemeToggle />

          {/* Notifications - escondido em mobile pequeno */}
          <Button variant="ghost" size="icon" className="relative hidden sm:flex" onClick={handleNotificationsClick}>
            <Bell className="w-5 h-5" />
            <div className="absolute -top-1 -right-1 w-2 h-2 bg-red-500 rounded-full"></div>
          </Button>
          
          <Button variant="ghost" size="icon" className="relative" onClick={handleMessagesClick}>
            <MessageSquare className="w-5 h-5" />
            <div className="absolute -top-1 -right-1 w-2 h-2 bg-green-500 rounded-full"></div>
          </Button>

          {profile && (
            <div className="flex items-center space-x-2 lg:space-x-3 cursor-pointer" onClick={handleProfileClick}>
              {/* Info do usuário - escondido em mobile */}
              <div className="text-right hidden md:block">
                <div className="text-sm font-medium">{profile.nome}</div>
                <div className="flex items-center space-x-1">
                  <Badge variant="secondary" className="text-xs">
                    {profile.points} pts
                  </Badge>
                  {profile.status && (
                    <Badge 
                      variant={profile.status === 'open-to-work' ? 'default' : 'secondary'}
                      className="text-xs"
                    >
                      {profile.status}
                    </Badge>
                  )}
                </div>
              </div>
              <Avatar className="w-7 h-7 lg:w-8 lg:h-8">
                <AvatarImage src={profile.foto_perfil || ''} alt={profile.nome} />
                <AvatarFallback>{profile.nome?.split(' ').map(n => n[0]).join('')}</AvatarFallback>
              </Avatar>
            </div>
          )}
        </div>
      </div>
    </header>
  );
};

export default Header;
