using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Projeto.Models;

namespace TesteProjeto.Models
{
    internal class TesteUsuario
    {
        [SetUp]
        public void Setup() {  }

        [Test]
        public void ExisteVazio()
        {
            Usuario usuario = new Usuario();

            Assert.That(usuario.Existe(), Is.False);
        }

        [Test]
        public void ExisteUsuario()
        {
            Usuario usuario = new Usuario(1, "teste", "teste");

            Assert.That(usuario.Existe(), Is.True);
        }

        [Test]
        public void ExisteEstaticoNull()
        {
            Usuario? usuario = null;

            Assert.That(Usuario.Existe(usuario), Is.False);
        }

        [Test]
        public void ExisteEstaticoUsuario()
        {
            Usuario usuario = new Usuario(1, "teste", "teste");

            Assert.That(Usuario.Existe(usuario), Is.True);
        }
    }
}
