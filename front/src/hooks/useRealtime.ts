
import { useEffect, useState } from 'react';
import { supabase } from '@/integrations/supabase/client';
import { useAuth } from '@/contexts/AuthContext';

export const useRealtime = () => {
  const { user } = useAuth();
  const [isConnected, setIsConnected] = useState(false);

  useEffect(() => {
    if (!user) return;

    const channel = supabase.channel('realtime-updates')
      .on('presence', { event: 'sync' }, () => {
        setIsConnected(true);
      })
      .subscribe();

    return () => {
      supabase.removeChannel(channel);
      setIsConnected(false);
    };
  }, [user]);

  return { isConnected };
};
