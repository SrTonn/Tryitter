using Tryitter.Models;
using Tryitter.Validations;

namespace Tryitter.Test;

public class TestUserValitation
{
    [Fact]
    public void TestUserValidationSuccess()
    {
        UserValidation.IsValidUser(new User()).Should().BeTrue();
    }

    [Theory(DisplayName = "Teste para TokenGenerator em que token não é nulo e possui 3 partes")]
    [InlineData("Mayara@mail.com")]
    [InlineData("Mayara@mail.co")]
    [InlineData("Mayara@aloha.gg")]
    public void TestTokenGeneratorSuccess(string email)
    {
        UserValidation.IsValidEmail(email).Should().BeTrue();
    }
}
