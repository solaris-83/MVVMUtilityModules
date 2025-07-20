
using System.Net.Http.Headers;

namespace ApiClientModule
{
    public class RetryHandlerSettings
    {
        public int MaxRetries { get; set; }
        public int DelayMsBetweenRetries { get; set; }
        public Func<object, Task<bool>> RetryCondition { get; set; }
    }

    public class RetryHandler : DelegatingHandler
    {
        private readonly int _maxRetries;
        private readonly int _delayMsBetweenRetries;
        private readonly Func<object, Task<bool>> _retryCondition;
        private readonly ITokenProvider _tokenProvider;

        //public RetryDelegatingHandler(HttpMessageHandler innerHandler, RetryHandlerSettings retryHandlerSettings) : base(innerHandler)
        //{
        //    _maxRetries = retryHandlerSettings == null ? 0 : retryHandlerSettings.MaxRetries;
        //    _delayMsBetweenRetries = retryHandlerSettings == null ? 0 : retryHandlerSettings.DelayMsBetweenRetries;
        //    _retryCondition = retryHandlerSettings?.RetryCondition;
        //}

        public RetryHandler(ITokenProvider tokenProvider, RetryHandlerSettings retryHandlerSettings)
        {
            _maxRetries = retryHandlerSettings == null ? 0 : retryHandlerSettings.MaxRetries;
            _delayMsBetweenRetries = retryHandlerSettings == null ? 0 : retryHandlerSettings.DelayMsBetweenRetries;
            _retryCondition = retryHandlerSettings?.RetryCondition;
            _tokenProvider = tokenProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            int retries = 0;
            while (true)
            {
                HttpResponseMessage response = null;
                bool shouldRetry = false;

                try
                {
                    if (_tokenProvider == null)
                        throw new Exception("You need to implement the interface ITokenProvider");

                    var token = await _tokenProvider.GetTokenAsync();
                    await _tokenProvider.CheckValidityAsync(token);

                    if (!string.IsNullOrEmpty(token.AccessToken))
                    {
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
                    }

                    response = await base.SendAsync(request, cancellationToken);
                    shouldRetry = !response.IsSuccessStatusCode && _retryCondition != null && await _retryCondition(response);
                }
                catch (Exception ex)
                {
                    shouldRetry = _retryCondition != null && await _retryCondition(ex);
                    // shouldRetry = false;
                    if (!shouldRetry || retries >= _maxRetries)
                    {
                        throw;
                    }
                }

                
                if (!shouldRetry || retries >= _maxRetries)
                {
                    return response;
                }

                await Task.Delay(_delayMsBetweenRetries, cancellationToken);
                retries++;
            }
        }
    }
}
