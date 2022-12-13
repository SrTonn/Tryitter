
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Tryitter.DTOs;
using Tryitter.Models;
using Tryitter.Services;

namespace Tryitter.Test;

public class TestAuthController : IClassFixture<TryitterTestContext<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    public TestAuthController(TryitterTestContext<Program> factory)
    {
        _factory = factory;
    }

    [Fact(DisplayName = "Teste registro de usuário comum")]
    public async Task TestRegisterCommomUserSuccess()
    {
        var client = _factory.CreateClient();
        var userDTO = new UserDTO
        {
            Name = "Test",
            Email = "aloha@321.com",
            Password = "password",
            Admin = false,
        };

        using HttpResponseMessage response = await client
            .PostAsJsonAsync("register", userDTO);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact(DisplayName = "Teste resposta BadRequest para email duplicado")]
    public async Task TestRegisterDuplicateEmailSuccess()
    {
        var client = _factory.CreateClient();
        var userDTO = new UserDTO
        {
            Name = "Test",
            Email = "aloha@321.com",
            Password = "password",
            Admin = false,
        };

        await client.PostAsJsonAsync("register", userDTO);

        using HttpResponseMessage response = await client
            .PostAsJsonAsync("register", userDTO);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName = "Teste registro de admin")]
    public async Task TestRegisterAdminUserSuccess()
    {
        var client = _factory.CreateClient();
        var user = new User
        {
            Name = "Admin User",
            Email = "Admin@Admin.com",
            Password = "passwordAdmin",
            Admin = true,
        };

        var instance = new TokenGenerator();
        var token = instance.Generate(user);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using HttpResponseMessage response = await client
            .PostAsJsonAsync("register", user);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact(DisplayName = "Teste 'Unauthorized' para cadastro de admin")]
    public async Task TestRegisterAdminUserFail()
    {
        var client = _factory.CreateClient();
        var newUser = new User
        {
            Name = "newAdmin",
            Email = "newAdmin@admin.com",
            Password = "passwordAdmin",
            Admin = true,
        };

        var userNotAdmin = new User
        {
            Name = "userNotAdmin",
            Email = "userNotAdmin@userNotAdmin.com",
            Password = "userNotAdmin",
            Admin = false,
        };

        var instance = new TokenGenerator();
        var token = instance.Generate(userNotAdmin);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using HttpResponseMessage response = await client
            .PostAsJsonAsync("register", newUser);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
