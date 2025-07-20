namespace ApiClientModule
{
    public interface ITokenProvider
    {
        Task<Token> GetTokenAsync(); // TODO gestire anche altre forme di authorization

        Task CheckValidityAsync(Token token)
        {
            return Task.CompletedTask;
        }
    }
}