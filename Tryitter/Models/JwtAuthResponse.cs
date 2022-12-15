namespace Tryitter
{
    [Serializable]
    public class JwtAuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
