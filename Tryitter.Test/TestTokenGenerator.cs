using Tryitter.Models;
using Tryitter.Services;

namespace Tryitter.Test;

public class TestTokenGenerator
{
    /// <summary>
    /// "Teste para TokenGenerator em que token não é nulo e possui 3 partes"
    /// </summary>
    [Fact]
    public void TestTokenGeneratorSuccess()
    {
        var user = new User
        {
            Email = "Mayara@mail.com",
            Password = "may123450",
        };
        var instance = new TokenGenerator();
        var token = instance.Generate(user.Email, user.Admin);

        token.Should().NotBeNullOrEmpty();
        token.Split(".").Should().HaveCount(3);
    }
}
