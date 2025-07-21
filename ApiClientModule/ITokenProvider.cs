namespace ApiClientModule
{
    public interface ITokenProvider
    {
        Task<Token> GetTokenAndCheckValidityAsync(HttpRequestMessage httpRequest); // TODO gestire anche altre forme di authorization

    }
}