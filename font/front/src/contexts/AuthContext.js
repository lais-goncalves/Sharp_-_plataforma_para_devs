
import React, { createContext, useContext, useEffect, useState } from 'react';
import Api from '../config/Api'

// TODO: trocar toda essa página pela autenticação via backend

// interface Profile {
//   id;
//   email;
//   nome: string | null;
//   foto_perfil: string | null;
//   github_username: string | null;
//   status: string | null;
//   points: number | null;
//   skills: string[] | null;
//   descricao: string | null;
//   created_at: string | null;
// }

// interface AuthContextType {
//   user: User | null;
//   profile: Profile | null;
//   loading: boolean;
//   estaAutenticado: boolean;
//   signOut: () => Promise<void>;
//   login: (email: string, password: string) => Promise<{ error: any }>;
//   signUp: (email: string, password: string, nome: string) => Promise<{ error: any }>;
//   loginWithGitHub: () => Promise<{ error: any }>;
//   fetchProfile: () => Promise<void>;
// }


const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(undefined);
  const [loading, setLoading] = useState(true);

  const buscarUsuarioLogado = async () => {
    try {
      setLoading(true);
      const resposta = await Api.fetch('/Conta/BuscarUsuarioLogado');

      setUser(resposta.dados);
      setLoading(false);

      return resposta;
    } 
    
    catch (error) {
      console.error('Erro ao buscar usuário logado: ', error); 
      setLoading(false);
    }
  };

  const realizarLogin = async (emailOuApelido, password) => {
    try {
      const query = {
        email_ou_apelido: emailOuApelido,
        senha: password
      }

      const metadata = {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json'
        },
      }

      setLoading(true);
      const resposta = await Api.fetch('/Conta/Logar', query, metadata);

      setUser(resposta.dados);
      setLoading(false);

      return resposta;
    } 
    
    catch (error) {
      return error;
    }
  };

  const signUp = async (email, password, nome) => {
    // try {
    //   const { data, error } = await supabase.auth.signUp({
    //     email,
    //     password,
    //     options: {
    //       data: {
    //         name: nome,
    //       },
    //     },
    //   });
    //   return { error };
    // } catch (error) {
    //   return { error };
    // }

    // TODO: continuar daqui
  };

  const loginWithGitHub = async () => {
    // try {
    //   const { data, error } = await supabase.auth.signInWithOAuth({
    //     provider: 'github',
    //     options: {
    //       redirectTo: `${window.location.origin}/dashboard`,
    //     },
    //   });
    //   return { error };
    // } catch (error) {
    //   return { error };
    // }

    // TODO: continuar daqui
  };

  const signOut = async () => {
    // await supabase.auth.signOut();
    try {
      setLoading(true);
      const resposta = await Api.fetch('/Conta/Deslogar');

      setUser(null);
      setLoading(false);

      return resposta;
    } 
    
    catch (error) {
      console.error('Erro ao buscar usuário logado: ', error); 
      setLoading(false);
    }
    
    // TODO: continuar daqui
  };

  // useEffect(() => {
  //   // const { data: { subscription } } = supabase.auth.onAuthStateChange(
  //   //   async (event, session) => {
  //   //     setUser(session?.user ?? null);
  //   //     if (session?.user) {
  //   //       await fetchProfile();
  //   //     } else {
  //   //       setProfile(null);
  //   //     }
  //   //     setLoading(false);
  //   //   }
  //   // );

  //   //return () => subscription.unsubscribe();
  // }, []);

  useEffect(() => {
    if (user === undefined) {
      buscarUsuarioLogado().then(() => setLoading(false));
    }
  }, [user]);

  const estaAutenticado = !!user;

  return (
    <AuthContext.Provider value={{ 
      user,  
      loading, 
      estaAutenticado,
      signOut, 
      realizarLogin,
      signUp,
      loginWithGitHub,
      buscarUsuarioLogado
    }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};