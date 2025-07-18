namespace ApiClientModule
{
    public interface IApiClientService
    {
        void SetClientId(string clientId);
        // GET
        Task<TResponse> GetAsync<TBody, TResponse>(string url, TBody q, CancellationToken? cancellationToken = null) where TResponse : class where TBody : class;
        Task<TResponse> GetAsync<TResponse>(string url, CancellationToken? cancellationToken = null) where TResponse : class;
        //Task GetAsync<TIn>(string url, TIn q, bool authRequired = true, bool retry = true, CancellationToken? cancellationToken = null) where TIn : class;

        // POST
        Task<TResponse> PostAsync<TBody, TResponse>(string url, TBody body, CancellationToken? cancellationToken = null) where TResponse : class where TBody : class;
        Task<TResponse> PostAsync<TResponse>(string url, CancellationToken? cancellationToken = null) where TResponse : class;
        //Task PostAsync<TBody>(string url, TBody body, CancellationToken? cancellationToken = null) where TBody : class; 
    }
}
