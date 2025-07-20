
namespace ApiClientModule.UnitTests
{
    internal class TokenProvider : ITokenProvider
    {
        public Task CheckValidityAsync(Token token)
        {
            throw new Exception("Validity not passed");
        }

        public Task<Token> GetTokenAsync()
        {
            return Task.FromResult(new Token() { AccessToken = "ACD-123", ExpiresIn = 100, ExpiresAt = DateTime.Now.AddMinutes(5) });
        }
    }
}