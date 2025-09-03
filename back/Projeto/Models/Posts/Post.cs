﻿using System.Data;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Npgsql;
using Projeto.Models.Usuarios;

namespace Projeto.Models.Posts
{
    public class Post
    {
        #region Propriedades
        static TabelaBanco<Post> tabela = ListaTabelas.BuscarTabela<Post>("Post");

        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? PostadoPor { get; set; }

        public string? Titulo { get; set; }
        public string? Texto { get; set; }
        public DateTime? PostadoEm { get; set; }
        #endregion Propriedades


        #region Construtores
        [JsonConstructor]
        public Post(
            int? id,
            string? titulo,
            string? texto,
            int? postado_por,
            DateTime? postado_em
        ) : base()
        {
            Id = id;
            Titulo = titulo;
            Texto = texto;
            PostadoPor = postado_por;
            PostadoEm = postado_em;
        }
        #endregion Construtores


        #region Métodos
        public static Post? extrairObjetoDoReader(NpgsqlDataReader reader)

        {
            int? id = reader.GetInt32("id");
            string? titulo = reader.GetString("titulo");
            string? texto = reader.GetString("texto");
            int? postado_por = reader.GetInt32("postado_por");
            DateTime? postado_em = reader.GetDateTime("postado_em");

            Post? post = new Post(id, titulo, texto, postado_por, postado_em);

            return post;
        }

        public static Post? BuscarPorId(int id)
        {
            return tabela.BuscarPorId(id);
        }

        public static List<Post?>? BuscarTodos()
        {
            return tabela.BuscarTodos();
        }

        public int? Postar(Usuario UsuarioLogado)
        {
            return Postar(Texto, Titulo, UsuarioLogado);
        }

        public static int? Postar(string? texto, string? titulo, Usuario? UsuarioLogado)
        {
            if (UsuarioLogado == null || UsuarioLogado?.Id == null)
            {
                throw new Exception("O usuário deve estar logado para poder postar.");
            }

            if (titulo == null)
            {
                throw new Exception("As propriedades 'tipo' e 'título' devem estar preenchidas para poder postar.");
            }

            try
            {
                NpgsqlParameter _texto = new NpgsqlParameter("texto", texto);
                NpgsqlParameter _titulo = new NpgsqlParameter("titulo", titulo);
                NpgsqlParameter _postado_por = new NpgsqlParameter("@postado_por", UsuarioLogado.Id);
                List<NpgsqlParameter> parametros = [_texto, _titulo, _postado_por];

                string comando = string.Concat("SELECT postar(@titulo, @texto, @postado_por)");

                int? resultado = tabela.conexao.ExecutarUnico<int>(comando, parametros, true);
                return resultado;
            }

            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return null;
            }
        }
        #endregion Métodos
    }
}
