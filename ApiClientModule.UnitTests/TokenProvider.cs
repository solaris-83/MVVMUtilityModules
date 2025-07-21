
namespace ApiClientModule.UnitTests
{
    internal class TokenProvider : ITokenProvider
    {

        public Task<Token> GetTokenAndCheckValidityAsync(HttpRequestMessage httpRequest)
        {
            var t = new Token() { AccessToken = "ACD-123", ExpiresIn = 100, ExpiresAt = DateTime.Now.AddMinutes(5), TokenType = TokenTypeEnum.Bearer, Result = new Result() { IsSuccess = true } };
            return Task.FromResult(t);
        }
    }
}