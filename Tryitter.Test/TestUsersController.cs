using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Tryitter.DTOs;
using Tryitter.Models;
using Tryitter.Services;

namespace Tryitter.Test
{
    public class TestUsersController : IClassFixture<TryitterTestContext<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        public class UserId
        {
            public int id;
        }

        public TestUsersController(TryitterTestContext<Program> factory)
        {
            _factory = factory;
        }

        public readonly static TheoryData<UserDTO, UserDTO> TestGetAllUsersData =
        new()
        {
            {
                new UserDTO ()
                {
                    Name = "Romeo",
                    Email = "romeo@mail.com",
                    Password = "Romeo1",
                    Admin = false,
                },
                new UserDTO ()
                {
                    Name = "Julieta",
                    Email = "julieta@mail.com",
                    Password = "Julieta2",
                    Admin = false,
                }
            },
            {
                new UserDTO ()
                {
                    Name = "Sr Smith",
                    Email = "sr.smith@mail.com",
                    Password = "smiths",
                    Admin = false,
                },
                new UserDTO ()
                {
                    Name = "Sra Smith",
                    Email = "sra.smith@mail.com",
                    Password = "smiths",
                    Admin = false,
                }
            },
        };

        [Theory]
        [MemberData(nameof(TestGetAllUsersData))]
        public async Task TestGetAllUsers(UserDTO user1, UserDTO user2)
        {
            var client = _factory.CreateClient();

            await client.PostAsJsonAsync("register", user1);
            await client.PostAsJsonAsync("register", user2);

            using HttpResponseMessage response = await client.GetAsync("users");
            var responseBody = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseBody.Contains(user1.Name).Should().BeTrue();
            responseBody.Contains(user2.Name).Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(TestGetAllUsersData))]
        public async Task TestGetUserById(UserDTO user1, UserDTO user2)
        {
            var client = _factory.CreateClient();

            await client.PostAsJsonAsync("register", user1);
            await client.PostAsJsonAsync("register", user2);

            var instance = new TokenGenerator();
            var token = instance.Generate(user1.Email!, user1.Admin);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using HttpResponseMessage response = await client.GetAsync($"users/1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [MemberData(nameof(TestGetAllUsersData))]
        public async Task TestUpdateUsersByAdmin(UserDTO user1, UserDTO user2)
        {
            var client = _factory.CreateClient();

            await client.PostAsJsonAsync("register", user1);

            var instance = new TokenGenerator();
            var token = instance.Generate(user1.Email!, true);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using HttpResponseMessage response = await client.PutAsJsonAsync("users/1", user2);
            var responseBody = await response.Content.ReadFromJsonAsync<User>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseBody.Name.Should().Be(user2.Name);
        }

        [Theory]
        [MemberData(nameof(TestGetAllUsersData))]
        public async Task TestUpdateUsersByUser(UserDTO user1, UserDTO user2)
        {
            var client = _factory.CreateClient();

            await client.PostAsJsonAsync("register", user1);

            var instance = new TokenGenerator();
            var token = instance.Generate(user1.Email!, user1.Admin);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using HttpResponseMessage response = await client.PutAsJsonAsync("users/me", user2);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [MemberData(nameof(TestGetAllUsersData))]
        public async Task TestDeleteUsersByAdmin(UserDTO user1, UserDTO user2)
        {
            var client = _factory.CreateClient();

            user1.Email = Faker.Internet.Email("Antonieto");
            user2.Email = Faker.Internet.Email("jurema");

            await client.PostAsJsonAsync("register", user1);
            using HttpResponseMessage responsePost = await client.PostAsJsonAsync("register", user2);
            var resultPost = await responsePost.Content.ReadFromJsonAsync<UserDTO>();

            var instance = new TokenGenerator();
            var token = instance.Generate("aloha@gmail.com", true);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using HttpResponseMessage response = await client.DeleteAsync($"users/{resultPost.id}");

            var getUserResponse = await client.GetAsync($"users/{resultPost.id}");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            getUserResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Theory]
        [MemberData(nameof(TestGetAllUsersData))]
        public async Task Test6DeleteUsersByUser(UserDTO user1, UserDTO user2)
        {
            var client = _factory.CreateClient();

            await client.PostAsJsonAsync("register", user1);
            await client.PostAsJsonAsync("register", user2);

            var instance = new TokenGenerator();
            var token = instance.Generate(user1.Email!, user1.Admin);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using HttpResponseMessage response = await client.DeleteAsync("users/me");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
