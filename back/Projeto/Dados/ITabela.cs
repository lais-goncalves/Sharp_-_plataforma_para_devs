﻿using Microsoft.Data.SqlClient;
using Npgsql;

namespace Projeto.Dados
{
    public interface ITabela<T> where T : ITabela<T>
    {
        public abstract static Conexao conexao { get; set; }
        public abstract static string nomeDaTabela { get; set; }


        public static abstract T? extrairObjetoDoReader(NpgsqlDataReader reader);

        public static T? buscarPorId(int id)
        {
            try
            {
                NpgsqlParameter paramId = new("@id", id);
                paramId.DbType = System.Data.DbType.Int32;
                string query = string.Concat("SELECT * FROM ", T.nomeDaTabela, " WHERE id = @id");

                T? post = T.conexao.ExecutarUnico(query, [paramId], true, T.extrairObjetoDoReader);

                return post;
            }

            catch (Exception err) 
            {
                Console.WriteLine(err.Message);
                return default;
            }
        }

        public static T? buscarPorId(string id)
        {
            try
            {
                int idInt = int.Parse(id);
                return buscarPorId(idInt);
            }

            catch (Exception)
            {
                return default;
            }

        }

        public static List<T?>? buscarTodos()
        {
            try
            {
                string query = string.Concat("SELECT * FROM ", T.nomeDaTabela);
                var resultado = T.conexao.Executar<T>(query, null, true, T.extrairObjetoDoReader);

                return resultado;
            }

            catch (Exception err) {
                Console.WriteLine(err.Message);
                return null;
            }
        }

        //protected static string? inserir();

        //protected abstract bool excluir<P>();
    }
}
