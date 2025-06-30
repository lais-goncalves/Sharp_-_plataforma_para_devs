
import { useState, useEffect } from 'react';
import { supabase } from '@/integrations/supabase/client';
import { useAuth } from '@/contexts/AuthContext';

export interface MentorshipMeeting {
  id: string;
  convidado_nome: string;
  data_reuniao: string;
  horario: string;
  duracao: number;
  status: string;
  topico: string | null;
  created_at: string;
}

export const useMentorshipMeetings = () => {
  const [meetings, setMeetings] = useState<MentorshipMeeting[]>([]);
  const [loading, setLoading] = useState(true);
  const { profile } = useAuth();

  useEffect(() => {
    if (profile?.id) {
      fetchMeetings();
    }
  }, [profile?.id]);

  const fetchMeetings = async () => {
    try {
      const { data, error } = await supabase
        .from('reunioes_mentoria')
        .select('*')
        .eq('organizador_id', profile?.id)
        .eq('status', 'agendada')
        .order('data_reuniao', { ascending: true });

      if (error) throw error;
      setMeetings(data || []);
    } catch (error) {
      console.error('Erro ao buscar reuniões:', error);
    } finally {
      setLoading(false);
    }
  };

  const createMeeting = async (meetingData: {
    convidado_nome: string;
    data_reuniao: string;
    horario: string;
    topico?: string;
  }) => {
    try {
      const { data, error } = await supabase
        .from('reunioes_mentoria')
        .insert([{
          organizador_id: profile?.id,
          ...meetingData
        }])
        .select()
        .single();

      if (error) throw error;
      
      setMeetings(prev => [...prev, data]);
      return { success: true, data };
    } catch (error) {
      console.error('Erro ao criar reunião:', error);
      return { success: false, error };
    }
  };

  const updateMeeting = async (id: string, updates: Partial<MentorshipMeeting>) => {
    try {
      const { data, error } = await supabase
        .from('reunioes_mentoria')
        .update(updates)
        .eq('id', id)
        .select()
        .single();

      if (error) throw error;
      
      setMeetings(prev => prev.map(m => m.id === id ? data : m));
      return { success: true, data };
    } catch (error) {
      console.error('Erro ao atualizar reunião:', error);
      return { success: false, error };
    }
  };

  const deleteMeeting = async (id: string) => {
    try {
      const { error } = await supabase
        .from('reunioes_mentoria')
        .delete()
        .eq('id', id);

      if (error) throw error;
      
      setMeetings(prev => prev.filter(m => m.id !== id));
      return { success: true };
    } catch (error) {
      console.error('Erro ao deletar reunião:', error);
      return { success: false, error };
    }
  };

  return {
    meetings,
    loading,
    createMeeting,
    updateMeeting,
    deleteMeeting,
    refetch: fetchMeetings
  };
};
