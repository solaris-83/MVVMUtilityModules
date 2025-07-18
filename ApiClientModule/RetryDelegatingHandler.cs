
namespace ApiClientModule
{
    public class RetryHandlerSettings
    {
        public int MaxRetries { get; set; }
        public int DelayMsBetweenRetries { get; set; }
        public Func<HttpResponseMessage, Task<bool>> RetryCondition { get; set; }
    }

    public class RetryHandler : DelegatingHandler
    {
        private readonly int _maxRetries;
        private readonly int _delayMsBetweenRetries;
        private readonly Func<HttpResponseMessage, Task<bool>> _retryCondition;

        public RetryHandler(HttpMessageHandler innerHandler, RetryHandlerSettings retryHandlerSettings) : base(innerHandler)
        {
            _maxRetries = retryHandlerSettings == null ? 0 : retryHandlerSettings.MaxRetries;
            _delayMsBetweenRetries = retryHandlerSettings == null ? 0 : retryHandlerSettings.DelayMsBetweenRetries;
            _retryCondition = retryHandlerSettings?.RetryCondition;
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
                    response = await base.SendAsync(request, cancellationToken);
                    shouldRetry = !response.IsSuccessStatusCode && _retryCondition != null && await _retryCondition(response);
                }
                catch
                {
                    shouldRetry = false;
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
