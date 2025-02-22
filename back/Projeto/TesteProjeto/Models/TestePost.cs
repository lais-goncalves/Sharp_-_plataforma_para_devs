using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using NUnit.Framework.Internal;
using Projeto.Banco;
using Projeto.Models;

namespace TesteProjeto.Models
{
    public class TestePost
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void PostNaoExiste(int id)
        {
            bool jaExiste = Post.JaExiste(id);

            Assert.That(jaExiste, Is.False);
        }

        [Test]
        [TestCase(1)]
        public void PostJaExiste(int id)
        {
            bool jaExiste = Post.JaExiste(id);

            Assert.That(jaExiste);
        }
    }
}
