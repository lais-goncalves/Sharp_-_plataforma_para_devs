
import { useState, useEffect } from 'react';
import { supabase } from '@/integrations/supabase/client';
import { useAuth } from '@/contexts/AuthContext';

export interface Project {
  id: number;
  titulo: string;
  descricao: string | null;
  stars: number;
  forks: number;
  views: number;
  tags: string[] | null;
  github_url: string | null;
  demo_url: string | null;
  image_url: string | null;
  status: string | null;
  created_at: string | null;
}

export const useProjects = () => {
  const [projects, setProjects] = useState<Project[]>([]);
  const [loading, setLoading] = useState(true);
  const { profile } = useAuth();

  useEffect(() => {
    if (profile?.id) {
      fetchProjects();
    }
  }, [profile?.id]);

  const fetchProjects = async () => {
    try {
      const { data, error } = await supabase
        .from('projetos')
        .select('*')
        .eq('user_id', profile?.id)
        .order('created_at', { ascending: false });

      if (error) throw error;
      setProjects(data || []);
    } catch (error) {
      console.error('Erro ao buscar projetos:', error);
    } finally {
      setLoading(false);
    }
  };

  const updateProjectStats = async (projectId: number, type: 'stars' | 'views' | 'forks') => {
    try {
      const project = projects.find(p => p.id === projectId);
      if (!project) return;

      const newValue = (project[type] || 0) + 1;
      
      const { error } = await supabase
        .from('projetos')
        .update({ [type]: newValue })
        .eq('id', projectId);

      if (error) throw error;

      setProjects(prev => prev.map(p => 
        p.id === projectId ? { ...p, [type]: newValue } : p
      ));
    } catch (error) {
      console.error(`Erro ao atualizar ${type}:`, error);
    }
  };

  return {
    projects,
    loading,
    updateProjectStats,
    refetch: fetchProjects
  };
};
