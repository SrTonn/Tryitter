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

    [Theory(DisplayName = "Teste formato do email válido")]
    [InlineData("Mayara@mail.com")]
    [InlineData("Mayara@mail.co")]
    [InlineData("Mayara@aloha.gg")]
    public void TestTokenGeneratorSuccess(string email)
    {
        UserValidation.IsValidEmail(email).Should().BeTrue();
    }   
    
    [Theory(DisplayName = "Teste formato do email inválido")]
    [InlineData("Mayaramail.com")]
    [InlineData("Mayara@mail")]
    [InlineData("@aloha.gg")]
    public void TestTokenGeneratorFail(string email)
    {
        UserValidation.IsValidEmail(email).Should().BeFalse();
    }
}
