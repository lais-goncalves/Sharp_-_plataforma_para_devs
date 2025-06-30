import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import { useLocation, useNavigate } from "react-router-dom";
import { 
  User, 
  Book, 
  MessageSquare, 
  Users, 
  BookOpen, 
  Calendar
} from "lucide-react";

interface SidebarProps {
  isMobile?: boolean;
}

const Sidebar = ({ isMobile = false }: SidebarProps) => {
  const location = useLocation();
  const navigate = useNavigate();

  const menuItems = [
    { icon: User, label: 'Dashboard', path: '/dashboard' },
    { icon: Book, label: 'Portfólio', path: '/portfolio', badge: '3' },
    { icon: MessageSquare, label: 'Mensagens', path: '/chat', badge: '12' },
    { icon: Users, label: 'Comunidade', path: '/community' },
    { icon: BookOpen, label: 'Vagas', path: '/jobs', badge: 'Nova' },
    { icon: Calendar, label: 'Mentoria', path: '/mentorship' },
    { icon: User, label: 'Perfil', path: '/profile' },
  ];

  if (isMobile) {
    return (
      <div className="p-4 space-y-2">
        <div className="mb-4">
          <h2 className="text-lg font-semibold mb-2">Menu</h2>
        </div>
        
        <div className="space-y-1">
          {menuItems.map((item) => {
            const isActive = location.pathname === item.path;
            const Icon = item.icon;
            
            return (
              <button
                key={item.path}
                className={`w-full flex items-center space-x-3 p-3 rounded-lg transition-colors ${
                  isActive 
                    ? "bg-gradient-to-r from-blue-600 to-purple-600 text-white" 
                    : "hover:bg-gray-100 dark:hover:bg-gray-800"
                }`}
                onClick={() => navigate(item.path)}
              >
                <Icon className="w-5 h-5" />
                <span className="flex-1 text-left font-medium">{item.label}</span>
                {item.badge && (
                  <Badge 
                    variant={isActive ? "secondary" : "default"}
                    className="text-xs"
                  >
                    {item.badge}
                  </Badge>
                )}
              </button>
            );
          })}
        </div>
      </div>
    );
  }

  return (
    <aside className="fixed left-0 top-16 w-64 h-[calc(100vh-4rem)] bg-sidebar border-r border-sidebar-border z-40 hidden lg:block">
      <div className="p-3 lg:p-4 space-y-1 lg:space-y-2">
        {menuItems.map((item) => {
          const isActive = location.pathname === item.path;
          const Icon = item.icon;
          
          return (
            <Button
              key={item.path}
              variant={isActive ? "default" : "ghost"}
              className={`w-full justify-start text-sm lg:text-base ${
                isActive 
                  ? "bg-gradient-to-r from-blue-600 to-purple-600 text-white" 
                  : "hover:bg-sidebar-accent"
              }`}
              onClick={() => navigate(item.path)}
            >
              <Icon className="w-4 h-4 mr-2 lg:mr-3" />
              <span className="flex-1 text-left">{item.label}</span>
              {item.badge && (
                <Badge 
                  variant={isActive ? "secondary" : "default"}
                  className="text-xs"
                >
                  {item.badge}
                </Badge>
              )}
            </Button>
          );
        })}
      </div>

      <div className="absolute bottom-4 left-3 lg:left-4 right-3 lg:right-4">
        <div className="bg-gradient-to-r from-blue-50 to-purple-50 dark:from-blue-950/20 dark:to-purple-950/20 p-3 lg:p-4 rounded-lg border border-sidebar-border">
          <h4 className="font-semibold text-sm mb-2">🚀 Sharp Premium</h4>
          <p className="text-xs text-muted-foreground mb-3">
            Destaque seus projetos e acesse análises avançadas
          </p>
          <Button size="sm" className="w-full bg-gradient-to-r from-blue-600 to-purple-600 text-xs lg:text-sm">
            Upgrade
          </Button>
        </div>
      </div>
    </aside>
  );
};

export default Sidebar;
