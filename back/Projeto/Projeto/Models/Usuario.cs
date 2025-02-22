using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Text.Json.Serialization;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Projeto.Banco;
using Projeto.Config;

namespace Projeto.Models
{
    [Table("usuario")]
    [NomeDaTabela("Usuario")]
    public class Usuario : Tabela<Usuario> {
        #region Propriedades
        [JsonIgnore, Key]
        public int Id { get; protected set; }

        [JsonIgnore]
        public string Senha { get; protected set; }
        public string Apelido { get; protected set; }
        #endregion Propriedades


        #region Construtores
        public Usuario(){ }

        [Newtonsoft.Json.JsonConstructor]
        public Usuario(int Id, string Apelido, string Senha)
        {
            this.Id = Id;
            this.Apelido = Apelido;
            this.Senha = Senha;
        }

        public Usuario(string Apelido, string Senha)
        {
            this.Apelido = Apelido;
            this.Senha = Senha;
        }
        #endregion Construtores


        #region Métodos
        public static bool Existe(Usuario? usuario)
        {
            return usuario != null && usuario.Existe();
        }

        public bool Existe()
        {
            return Id > 0 && !string.IsNullOrEmpty(Apelido);
        }

        public bool NaoExiste()
        {
            return !Existe();
        }

        public static bool JaExiste(string apelido)
        {
            return Entidades.Usuario.Any(u => u.Apelido == apelido);
        }

        public static Resultado<Usuario?> BuscarPorApelido(string apelido)
        {
            Resultado<Usuario?> resultado = new();

            try
            {
                Usuario? usuarioEncontrado = Entidades.Usuario.FirstOrDefault(u => u.Apelido == apelido);
                resultado.Item = usuarioEncontrado;
            }

            catch (Exception err)
            {
                resultado.Erro = err;
            }

            return resultado;
        }

        public static Resultado<Usuario?> VerificarLogin(string apelido, string senha)
        {
            Resultado<Usuario?> resultado = new();

            try
            {
                Usuario? usuarioEncontrado = Entidades.Usuario.FirstOrDefault(u => u.Apelido == apelido && u.Senha == senha);
                resultado.Item = usuarioEncontrado;
            }

            catch (Exception err)
            {
                resultado.Erro = err;
            }

            return resultado;
        }

        public static Resultado<bool> Registrar(string apelido, string senha)
        {
            Resultado<bool> resultado = new();

            try
            {
                Usuario usuario = new Usuario(apelido, senha);

                if (JaExiste(apelido))
                {
                    throw new Exception("Este apelido já está sendo usado. Tente outro.");
                }

                Entidades.Usuario.Add(usuario);
                Entidades.SaveChanges();
                resultado.Item = true;
            }

            catch (Exception err)
            {
                resultado.Erro = err;
            }

            return resultado;
        }
        #endregion Métodos
    }
}
