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
                    Name = "Test1",
                    Email = "aloha1@mail.com",
                    Password = "password1",
                    Admin = false,
                },
                new UserDTO ()
                {
                    Name = "Test11",
                    Email = "aloha11@mail.com",
                    Password = "password11",
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

            var instance = new TokenGenerator();
            var token = instance.Generate(user1.Email!, user1.Admin);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using HttpResponseMessage response = await client.GetAsync("users/1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseBody = await response.Content.ReadFromJsonAsync<User>();

            responseBody.Email.Should().Be(user1.Email);
            responseBody.Name.Should().Be(user1.Name);
            responseBody.Password.Should().Be(user1.Password);
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

            await client.PostAsJsonAsync("register", user1);
            await client.PostAsJsonAsync("register", user2);

            var instance = new TokenGenerator();
            var token = instance.Generate(user1.Email!, true);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using HttpResponseMessage response = await client.DeleteAsync("users/2");

            var getUserResponse = await client.GetAsync("users/2");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            getUserResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Theory]
        [MemberData(nameof(TestGetAllUsersData))]
        public async Task TestDeleteUsersByUser(UserDTO user1, UserDTO user2)
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
