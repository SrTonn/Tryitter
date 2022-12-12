using FluentAssertions;
using Tryitter.Services;
using Tryitter.DTOs;

namespace Tryitter.Test;

public class TestTokenGenerator
{
    [Theory(DisplayName = "Teste para TokenGenerator em que token não é nulo e possui 3 partes")]
    [InlineData("Mayara@mail.com", "may123450")]
    public void TestTokenGeneratorSuccess(string email, string password)
    {
        var user = new AuthDTO
        {
            Email = email,
            Password = password,
        };
        var instance = new TokenGenerator();
        var token = instance.Generate(user);

        token.Should().NotBeNullOrEmpty();
        token.Split(".").Should().HaveCount(3);
    }
}
