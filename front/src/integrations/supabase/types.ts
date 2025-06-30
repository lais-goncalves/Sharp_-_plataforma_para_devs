export type Json =
  | string
  | number
  | boolean
  | null
  | { [key: string]: Json | undefined }
  | Json[]

export type Database = {
  public: {
    Tables: {
      comentarios_postagem: {
        Row: {
          conteudo: string
          created_at: string
          id: string
          postagem_id: string
          user_id: string
        }
        Insert: {
          conteudo: string
          created_at?: string
          id?: string
          postagem_id: string
          user_id: string
        }
        Update: {
          conteudo?: string
          created_at?: string
          id?: string
          postagem_id?: string
          user_id?: string
        }
        Relationships: [
          {
            foreignKeyName: "comentarios_postagem_postagem_id_fkey"
            columns: ["postagem_id"]
            isOneToOne: false
            referencedRelation: "postagens_comunidade"
            referencedColumns: ["id"]
          },
        ]
      }
      comunidade_membros: {
        Row: {
          comunidade_id: string
          id: string
          joined_at: string
          role: string | null
          user_id: string
        }
        Insert: {
          comunidade_id: string
          id?: string
          joined_at?: string
          role?: string | null
          user_id: string
        }
        Update: {
          comunidade_id?: string
          id?: string
          joined_at?: string
          role?: string | null
          user_id?: string
        }
        Relationships: [
          {
            foreignKeyName: "comunidade_membros_comunidade_id_fkey"
            columns: ["comunidade_id"]
            isOneToOne: false
            referencedRelation: "comunidades"
            referencedColumns: ["id"]
          },
        ]
      }
      comunidades: {
        Row: {
          created_at: string
          criador_id: string
          descricao: string | null
          id: string
          imagem_url: string | null
          nome: string
          publico: boolean | null
        }
        Insert: {
          created_at?: string
          criador_id: string
          descricao?: string | null
          id?: string
          imagem_url?: string | null
          nome: string
          publico?: boolean | null
        }
        Update: {
          created_at?: string
          criador_id?: string
          descricao?: string | null
          id?: string
          imagem_url?: string | null
          nome?: string
          publico?: boolean | null
        }
        Relationships: []
      }
      conversas: {
        Row: {
          created_at: string
          id: string
          updated_at: string
          user1_id: string
          user2_id: string
        }
        Insert: {
          created_at?: string
          id?: string
          updated_at?: string
          user1_id: string
          user2_id: string
        }
        Update: {
          created_at?: string
          id?: string
          updated_at?: string
          user1_id?: string
          user2_id?: string
        }
        Relationships: []
      }
      denuncias: {
        Row: {
          created_at: string
          id: string
          item_id: string
          motivo: string
          reportado_por: string
          status: string | null
          tipo: string
        }
        Insert: {
          created_at?: string
          id?: string
          item_id: string
          motivo: string
          reportado_por: string
          status?: string | null
          tipo: string
        }
        Update: {
          created_at?: string
          id?: string
          item_id?: string
          motivo?: string
          reportado_por?: string
          status?: string | null
          tipo?: string
        }
        Relationships: []
      }
      grupo_membros: {
        Row: {
          grupo_id: number | null
          id: number
          joined_at: string | null
          role: string | null
          user_id: string | null
        }
        Insert: {
          grupo_id?: number | null
          id?: number
          joined_at?: string | null
          role?: string | null
          user_id?: string | null
        }
        Update: {
          grupo_id?: number | null
          id?: number
          joined_at?: string | null
          role?: string | null
          user_id?: string | null
        }
        Relationships: [
          {
            foreignKeyName: "grupo_membros_grupo_id_fkey"
            columns: ["grupo_id"]
            isOneToOne: false
            referencedRelation: "grupos"
            referencedColumns: ["id"]
          },
          {
            foreignKeyName: "grupo_membros_user_id_fkey"
            columns: ["user_id"]
            isOneToOne: false
            referencedRelation: "users"
            referencedColumns: ["id"]
          },
        ]
      }
      grupos: {
        Row: {
          created_at: string | null
          criador_id: string | null
          descricao: string | null
          id: number
          imagem_url: string | null
          nome: string
          publico: boolean | null
        }
        Insert: {
          created_at?: string | null
          criador_id?: string | null
          descricao?: string | null
          id?: number
          imagem_url?: string | null
          nome: string
          publico?: boolean | null
        }
        Update: {
          created_at?: string | null
          criador_id?: string | null
          descricao?: string | null
          id?: number
          imagem_url?: string | null
          nome?: string
          publico?: boolean | null
        }
        Relationships: [
          {
            foreignKeyName: "grupos_criador_id_fkey"
            columns: ["criador_id"]
            isOneToOne: false
            referencedRelation: "users"
            referencedColumns: ["id"]
          },
        ]
      }
      mensagens: {
        Row: {
          conteudo: string
          created_at: string | null
          id: number
          read_at: string | null
          receiver_id: string | null
          sender_id: string | null
          tipo: string | null
        }
        Insert: {
          conteudo: string
          created_at?: string | null
          id?: number
          read_at?: string | null
          receiver_id?: string | null
          sender_id?: string | null
          tipo?: string | null
        }
        Update: {
          conteudo?: string
          created_at?: string | null
          id?: number
          read_at?: string | null
          receiver_id?: string | null
          sender_id?: string | null
          tipo?: string | null
        }
        Relationships: [
          {
            foreignKeyName: "mensagens_receiver_id_fkey"
            columns: ["receiver_id"]
            isOneToOne: false
            referencedRelation: "users"
            referencedColumns: ["id"]
          },
          {
            foreignKeyName: "mensagens_sender_id_fkey"
            columns: ["sender_id"]
            isOneToOne: false
            referencedRelation: "users"
            referencedColumns: ["id"]
          },
        ]
      }
      mensagens_conversa: {
        Row: {
          arquivo_url: string | null
          conteudo: string
          conversa_id: string
          created_at: string
          id: string
          lida: boolean | null
          sender_id: string
          tipo: string | null
        }
        Insert: {
          arquivo_url?: string | null
          conteudo: string
          conversa_id: string
          created_at?: string
          id?: string
          lida?: boolean | null
          sender_id: string
          tipo?: string | null
        }
        Update: {
          arquivo_url?: string | null
          conteudo?: string
          conversa_id?: string
          created_at?: string
          id?: string
          lida?: boolean | null
          sender_id?: string
          tipo?: string | null
        }
        Relationships: [
          {
            foreignKeyName: "mensagens_conversa_conversa_id_fkey"
            columns: ["conversa_id"]
            isOneToOne: false
            referencedRelation: "conversas"
            referencedColumns: ["id"]
          },
        ]
      }
      notificacoes: {
        Row: {
          conteudo: string | null
          created_at: string | null
          id: number
          read_at: string | null
          tipo: string
          titulo: string
          user_id: string | null
        }
        Insert: {
          conteudo?: string | null
          created_at?: string | null
          id?: number
          read_at?: string | null
          tipo: string
          titulo: string
          user_id?: string | null
        }
        Update: {
          conteudo?: string | null
          created_at?: string | null
          id?: number
          read_at?: string | null
          tipo?: string
          titulo?: string
          user_id?: string | null
        }
        Relationships: [
          {
            foreignKeyName: "notificacoes_user_id_fkey"
            columns: ["user_id"]
            isOneToOne: false
            referencedRelation: "users"
            referencedColumns: ["id"]
          },
        ]
      }
      postagens_comunidade: {
        Row: {
          arquivo_url: string | null
          comunidade_id: string
          conteudo: string
          created_at: string
          id: string
          tipo: string | null
          user_id: string
        }
        Insert: {
          arquivo_url?: string | null
          comunidade_id: string
          conteudo: string
          created_at?: string
          id?: string
          tipo?: string | null
          user_id: string
        }
        Update: {
          arquivo_url?: string | null
          comunidade_id?: string
          conteudo?: string
          created_at?: string
          id?: string
          tipo?: string | null
          user_id?: string
        }
        Relationships: [
          {
            foreignKeyName: "postagens_comunidade_comunidade_id_fkey"
            columns: ["comunidade_id"]
            isOneToOne: false
            referencedRelation: "comunidades"
            referencedColumns: ["id"]
          },
        ]
      }
      projetos: {
        Row: {
          created_at: string | null
          demo_url: string | null
          descricao: string | null
          forks: number | null
          github_url: string | null
          id: number
          image_url: string | null
          stars: number | null
          status: string | null
          tags: string[] | null
          titulo: string
          updated_at: string | null
          url: string | null
          user_id: string | null
          views: number | null
        }
        Insert: {
          created_at?: string | null
          demo_url?: string | null
          descricao?: string | null
          forks?: number | null
          github_url?: string | null
          id?: number
          image_url?: string | null
          stars?: number | null
          status?: string | null
          tags?: string[] | null
          titulo: string
          updated_at?: string | null
          url?: string | null
          user_id?: string | null
          views?: number | null
        }
        Update: {
          created_at?: string | null
          demo_url?: string | null
          descricao?: string | null
          forks?: number | null
          github_url?: string | null
          id?: number
          image_url?: string | null
          stars?: number | null
          status?: string | null
          tags?: string[] | null
          titulo?: string
          updated_at?: string | null
          url?: string | null
          user_id?: string | null
          views?: number | null
        }
        Relationships: [
          {
            foreignKeyName: "projetos_user_id_fkey"
            columns: ["user_id"]
            isOneToOne: false
            referencedRelation: "users"
            referencedColumns: ["id"]
          },
        ]
      }
      reacoes_postagem: {
        Row: {
          created_at: string
          id: string
          postagem_id: string
          tipo: string | null
          user_id: string
        }
        Insert: {
          created_at?: string
          id?: string
          postagem_id: string
          tipo?: string | null
          user_id: string
        }
        Update: {
          created_at?: string
          id?: string
          postagem_id?: string
          tipo?: string | null
          user_id?: string
        }
        Relationships: [
          {
            foreignKeyName: "reacoes_postagem_postagem_id_fkey"
            columns: ["postagem_id"]
            isOneToOne: false
            referencedRelation: "postagens_comunidade"
            referencedColumns: ["id"]
          },
        ]
      }
      reunioes_mentoria: {
        Row: {
          convidado_nome: string
          created_at: string
          data_reuniao: string
          duracao: number | null
          horario: string
          id: string
          organizador_id: string
          status: string | null
          topico: string | null
          updated_at: string
        }
        Insert: {
          convidado_nome: string
          created_at?: string
          data_reuniao: string
          duracao?: number | null
          horario: string
          id?: string
          organizador_id: string
          status?: string | null
          topico?: string | null
          updated_at?: string
        }
        Update: {
          convidado_nome?: string
          created_at?: string
          data_reuniao?: string
          duracao?: number | null
          horario?: string
          id?: string
          organizador_id?: string
          status?: string | null
          topico?: string | null
          updated_at?: string
        }
        Relationships: []
      }
      users: {
        Row: {
          created_at: string | null
          descricao: string | null
          email: string
          foto_perfil: string | null
          github_username: string | null
          id: string
          nome: string | null
          points: number | null
          skills: string[] | null
          status: string | null
          updated_at: string | null
        }
        Insert: {
          created_at?: string | null
          descricao?: string | null
          email: string
          foto_perfil?: string | null
          github_username?: string | null
          id: string
          nome?: string | null
          points?: number | null
          skills?: string[] | null
          status?: string | null
          updated_at?: string | null
        }
        Update: {
          created_at?: string | null
          descricao?: string | null
          email?: string
          foto_perfil?: string | null
          github_username?: string | null
          id?: string
          nome?: string | null
          points?: number | null
          skills?: string[] | null
          status?: string | null
          updated_at?: string | null
        }
        Relationships: []
      }
      vagas: {
        Row: {
          ativa: boolean | null
          created_at: string | null
          descricao: string | null
          empresa_id: string | null
          id: number
          localizacao: string | null
          remoto: boolean | null
          requisitos: string | null
          salario_max: number | null
          salario_min: number | null
          tipo: string | null
          titulo: string
          updated_at: string | null
        }
        Insert: {
          ativa?: boolean | null
          created_at?: string | null
          descricao?: string | null
          empresa_id?: string | null
          id?: number
          localizacao?: string | null
          remoto?: boolean | null
          requisitos?: string | null
          salario_max?: number | null
          salario_min?: number | null
          tipo?: string | null
          titulo: string
          updated_at?: string | null
        }
        Update: {
          ativa?: boolean | null
          created_at?: string | null
          descricao?: string | null
          empresa_id?: string | null
          id?: number
          localizacao?: string | null
          remoto?: boolean | null
          requisitos?: string | null
          salario_max?: number | null
          salario_min?: number | null
          tipo?: string | null
          titulo?: string
          updated_at?: string | null
        }
        Relationships: [
          {
            foreignKeyName: "vagas_empresa_id_fkey"
            columns: ["empresa_id"]
            isOneToOne: false
            referencedRelation: "users"
            referencedColumns: ["id"]
          },
        ]
      }
    }
    Views: {
      [_ in never]: never
    }
    Functions: {
      [_ in never]: never
    }
    Enums: {
      [_ in never]: never
    }
    CompositeTypes: {
      [_ in never]: never
    }
  }
}

type DefaultSchema = Database[Extract<keyof Database, "public">]

export type Tables<
  DefaultSchemaTableNameOrOptions extends
    | keyof (DefaultSchema["Tables"] & DefaultSchema["Views"])
    | { schema: keyof Database },
  TableName extends DefaultSchemaTableNameOrOptions extends {
    schema: keyof Database
  }
    ? keyof (Database[DefaultSchemaTableNameOrOptions["schema"]]["Tables"] &
        Database[DefaultSchemaTableNameOrOptions["schema"]]["Views"])
    : never = never,
> = DefaultSchemaTableNameOrOptions extends { schema: keyof Database }
  ? (Database[DefaultSchemaTableNameOrOptions["schema"]]["Tables"] &
      Database[DefaultSchemaTableNameOrOptions["schema"]]["Views"])[TableName] extends {
      Row: infer R
    }
    ? R
    : never
  : DefaultSchemaTableNameOrOptions extends keyof (DefaultSchema["Tables"] &
        DefaultSchema["Views"])
    ? (DefaultSchema["Tables"] &
        DefaultSchema["Views"])[DefaultSchemaTableNameOrOptions] extends {
        Row: infer R
      }
      ? R
      : never
    : never

export type TablesInsert<
  DefaultSchemaTableNameOrOptions extends
    | keyof DefaultSchema["Tables"]
    | { schema: keyof Database },
  TableName extends DefaultSchemaTableNameOrOptions extends {
    schema: keyof Database
  }
    ? keyof Database[DefaultSchemaTableNameOrOptions["schema"]]["Tables"]
    : never = never,
> = DefaultSchemaTableNameOrOptions extends { schema: keyof Database }
  ? Database[DefaultSchemaTableNameOrOptions["schema"]]["Tables"][TableName] extends {
      Insert: infer I
    }
    ? I
    : never
  : DefaultSchemaTableNameOrOptions extends keyof DefaultSchema["Tables"]
    ? DefaultSchema["Tables"][DefaultSchemaTableNameOrOptions] extends {
        Insert: infer I
      }
      ? I
      : never
    : never

export type TablesUpdate<
  DefaultSchemaTableNameOrOptions extends
    | keyof DefaultSchema["Tables"]
    | { schema: keyof Database },
  TableName extends DefaultSchemaTableNameOrOptions extends {
    schema: keyof Database
  }
    ? keyof Database[DefaultSchemaTableNameOrOptions["schema"]]["Tables"]
    : never = never,
> = DefaultSchemaTableNameOrOptions extends { schema: keyof Database }
  ? Database[DefaultSchemaTableNameOrOptions["schema"]]["Tables"][TableName] extends {
      Update: infer U
    }
    ? U
    : never
  : DefaultSchemaTableNameOrOptions extends keyof DefaultSchema["Tables"]
    ? DefaultSchema["Tables"][DefaultSchemaTableNameOrOptions] extends {
        Update: infer U
      }
      ? U
      : never
    : never

export type Enums<
  DefaultSchemaEnumNameOrOptions extends
    | keyof DefaultSchema["Enums"]
    | { schema: keyof Database },
  EnumName extends DefaultSchemaEnumNameOrOptions extends {
    schema: keyof Database
  }
    ? keyof Database[DefaultSchemaEnumNameOrOptions["schema"]]["Enums"]
    : never = never,
> = DefaultSchemaEnumNameOrOptions extends { schema: keyof Database }
  ? Database[DefaultSchemaEnumNameOrOptions["schema"]]["Enums"][EnumName]
  : DefaultSchemaEnumNameOrOptions extends keyof DefaultSchema["Enums"]
    ? DefaultSchema["Enums"][DefaultSchemaEnumNameOrOptions]
    : never

export type CompositeTypes<
  PublicCompositeTypeNameOrOptions extends
    | keyof DefaultSchema["CompositeTypes"]
    | { schema: keyof Database },
  CompositeTypeName extends PublicCompositeTypeNameOrOptions extends {
    schema: keyof Database
  }
    ? keyof Database[PublicCompositeTypeNameOrOptions["schema"]]["CompositeTypes"]
    : never = never,
> = PublicCompositeTypeNameOrOptions extends { schema: keyof Database }
  ? Database[PublicCompositeTypeNameOrOptions["schema"]]["CompositeTypes"][CompositeTypeName]
  : PublicCompositeTypeNameOrOptions extends keyof DefaultSchema["CompositeTypes"]
    ? DefaultSchema["CompositeTypes"][PublicCompositeTypeNameOrOptions]
    : never

export const Constants = {
  public: {
    Enums: {},
  },
} as const
