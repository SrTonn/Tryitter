using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Tryitter.DTOs;
using Tryitter.Models;
using Tryitter.Services;

namespace Tryitter.Test
{
    public class TestPostsController : IClassFixture<TryitterTestContext<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public UserDTO user1 = new UserDTO()
        {
            Name = "jonas",
            Email = "jonas@mail.com",
            Password = "Jonas1",
            Admin = false,
        };
        public TestPostsController(TryitterTestContext<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task TestGetAllPosts()
        {
            var client = _factory.CreateClient();

            using HttpResponseMessage response = await client
                .GetAsync("posts");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }       
        
        [Fact]
        public async Task TestGetPostById()
        {
            var client = _factory.CreateClient();
            var post = new Post() {
                UserId = 1,
                Title = Faker.Lorem.Sentence(),
                ImageUrl = Faker.Lorem.Sentence(),
            };

            user1.Name = Faker.Name.First();
            user1.Name = Faker.Internet.Email(user1.Name);

            var instance = new TokenGenerator();
            var token = instance.Generate(user1.Email!, false);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            await client.PostAsJsonAsync("register", user1);

            await client.PostAsJsonAsync("posts", post);

            using HttpResponseMessage response = await client
                .GetAsync("posts/1");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        
        [Fact]
        public async Task TestGetLastPost()
        {
            var client = _factory.CreateClient();
            var post = new Post() {
                UserId = 1,
                Title = Faker.Lorem.Sentence(),
                ImageUrl = Faker.Lorem.Sentence(),
            };

            user1.Name = Faker.Name.First();
            user1.Name = Faker.Internet.Email(user1.Name);

            var instance = new TokenGenerator();
            var token = instance.Generate(user1.Email!, false);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            await client.PostAsJsonAsync("register", user1);

            await client.PostAsJsonAsync("posts", post);

            using HttpResponseMessage response = await client
                .GetAsync("posts/last");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        
        [Fact]
        public async Task TestGetLastPostByUserId()
        {
            var client = _factory.CreateClient();
            var post = new Post() {
                UserId = 1,
                Title = Faker.Lorem.Sentence(),
                ImageUrl = Faker.Lorem.Sentence(),
            };

            user1.Name = Faker.Name.First();
            user1.Name = Faker.Internet.Email(user1.Name);

            var instance = new TokenGenerator();
            var token = instance.Generate(user1.Email!, false);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            await client.PostAsJsonAsync("register", user1);

            await client.PostAsJsonAsync("posts", post);

            using HttpResponseMessage response = await client
                .GetAsync("posts/last/1");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
