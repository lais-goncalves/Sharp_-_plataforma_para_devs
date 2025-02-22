using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Projeto.Controllers.Api;
using Projeto.Models;

namespace TesteProjeto.Controllers
{
    internal class TestePostController
    {
        [Test]
        public void BuscarPorId()
        {
            int id = 1;

            PostController controller = new PostController();
            ObjectResult resultado = (ObjectResult)controller.BuscarPorId(id);

            Post? post = (Post?)resultado.Value;

            if (post == null)
            {
                Assert.Fail();
                return;
            }

            Post postEsperado = new("1", "texto", "titulo 1", "texto 1", 3, DateTime.Parse("2025-01-23T22:24:44"));

            string jsonPost = JsonConvert.SerializeObject(post);
            string jsonPostEsperado = JsonConvert.SerializeObject(postEsperado);

            Assert.That(jsonPost, Is.EqualTo(jsonPostEsperado));
        }

        [Test]
        public void BuscarTodos()
        {
            PostController controller = new PostController();
            ObjectResult resultado = (ObjectResult)controller.BuscarTodos();

            List<Post?>? posts = (List<Post?>?)resultado.Value;

            if (posts == null || posts.Count <= 0)
            {
                Assert.Fail();
                return;
            }

            Post? primeiroPost = posts[0];
            Assert.That(primeiroPost != null && !string.IsNullOrEmpty(primeiroPost.Id));
        }
    }
}
