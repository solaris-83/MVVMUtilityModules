

namespace ApiClientModule
{
    public class Token
    {
        public required string AccessToken { get; set; }
        public DateTime ExpiresAt { get; set; } = DateTime.MaxValue;
        public int ExpiresIn { get; set; }
    }

    //public class TokenProvider : ITokenProvider
    //{
    //    public Func<Task<Token>> FetchTokenAsync { get; set; }
    //    private string _accessToken;
    //    private DateTime _expiresAt;
    //    private readonly SemaphoreSlim _lock = new(1, 1);

    //    public async Task<string> GetTokenAsync()
    //    {
    //        if (string.IsNullOrEmpty(_accessToken) || DateTime.UtcNow >= _expiresAt)
    //        {
    //            await _lock.WaitAsync();
    //            try
    //            {
    //                if (string.IsNullOrEmpty(_accessToken) || DateTime.UtcNow >= _expiresAt)
    //                {
    //                    var newToken = await FetchTokenAsync(); // e.g. use HttpClient
    //                    _accessToken = newToken.AccessToken;
    //                    _expiresAt = DateTime.UtcNow.AddSeconds(newToken.ExpiresIn - 60); // leave a buffer
    //                }
    //            }
    //            finally
    //            {
    //                _lock.Release();
    //            }
    //        }

    //        return _accessToken;
    //    }
    //}


}
