using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Web;

namespace ApiClientModule
{
    // TODO Implementare multipart

    public class ApiClientService : IApiClientService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ApiClientService> _logger;
        private string _clientId;
        public ApiClientService(IHttpClientFactory httpClientFactory, ILogger<ApiClientService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public void SetClientId(string clientId)
        {
            _clientId = clientId;
        }

        #region GET Implementations
        public async Task<TResponse> GetAsync<TBody, TResponse>(string url, TBody q, CancellationToken? cancellationToken = null)
            where TResponse : class
            where TBody : class
        {
            var result = await _GetAsync<TBody, TResponse>(url, q, cancellationToken);
            return result;
        }

        public async Task<TResponse> GetAsync<TResponse>(string url, CancellationToken? cancellationToken = null) where TResponse : class
        {
            var result = await _GetAsync<Void, TResponse>(url, Void.Empty, cancellationToken);
            return result;
        }

        //public async Task GetAsync<TBody>(string url, TBody q, bool authRequired = true, bool retry = true, CancellationToken? cancellationToken = null) where TBody : class
        //{
        //    await _GetAsync<TBody, Void>(url, q, authRequired, retry, cancellationToken);
        //}

        #endregion

        #region POST implementations
        public async Task<TResponse> PostAsync<TBody,TResponse>(string url, TBody body, CancellationToken? cancellationToken = null)
            where TResponse : class
            where TBody : class
        {
            var result = await _PostAsync<TBody, TResponse>(url, body, cancellationToken);
            return result;
        }

        public async Task<TResponse> PostAsync<TResponse>(string url, CancellationToken? cancellationToken = null) where TResponse : class
        {
            return await _PostAsync<Void, TResponse>(url, Void.Empty, cancellationToken);
        }

        #endregion

        #region Private methods

        private async Task<TResponse> _GetAsync<TBody, TResponse>(string url, TBody query, CancellationToken? cancellationToken = null) where TBody : class where TResponse : class
        {
            var ct = cancellationToken ?? CancellationToken.None;

            try
            {
                if (typeof(TBody) != typeof(Void))
                {
                    url += "?" + ToQueryString(query);
                }
                using (var client = _clientId  == null ? _httpClientFactory.CreateClient() : _httpClientFactory.CreateClient(_clientId))
                {
                    if (typeof(TResponse) == typeof(byte[]))
                    {
                        var resultByteArray = await client.GetByteArrayAsync(url, ct);
                        return resultByteArray as TResponse;
                    }
                    else
                    {
                        var result = await client.GetAsync(url, ct);
                        if (result.IsSuccessStatusCode)
                        {
                            if (typeof(TResponse) == typeof(Void))
                            {
                                return Void.Empty as TResponse;
                            }
                            else
                            {
                                if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
                                {
                                    var nocontent = Activator.CreateInstance<TResponse>();
                                    return nocontent;
                                }

                                var response = await result.Content.ReadAsStringAsync();
#if DEBUG
                                _logger.LogDebug($" === API RESPONSE === URL: {url} #####\n{response}");
#endif
                                var returnValue = JsonSerializer.Deserialize<TResponse>(response, new JsonSerializerOptions
                                {
                                    PropertyNameCaseInsensitive = true
                                });
                                return returnValue;
                            }
                        }


                        if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        {
                            throw new ApiException($"Unauthorized {url}", result.StatusCode);
                        }
                        var error = await result.Content?.ReadAsStringAsync() ?? string.Empty;
                        throw new ApiException(error, result.StatusCode);
                    }
                }
            }
            catch (TaskCanceledException) { return default(TResponse); }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<TResponse> _PostAsync<TBody, TResponse>(string url, TBody body, CancellationToken? cancellationToken = null) where TResponse : class where TBody : class
        {
            string json = string.Empty;
            var ct = cancellationToken ?? CancellationToken.None;
            try
            {
                using (var client = _clientId == null ? _httpClientFactory.CreateClient() : _httpClientFactory.CreateClient(_clientId))
                {
                    json = JsonSerializer.Serialize(body);
                    HttpContent content = null;
                    if (typeof(TBody) != typeof(Void))
                    {
                        content = new StringContent(json, Encoding.UTF8, "application/json");
                    }
                    var result = await client.PostAsync(url, content, ct);
                    if (result.IsSuccessStatusCode)
                    {
                        if (typeof(TResponse) == typeof(Void))
                            return Void.Empty as TResponse;
                        var response = await result.Content.ReadAsStringAsync();
#if DEBUG
                        _logger.LogDebug(" === API RESPONSE === " + " URL: " + url + " Body: " + (string.IsNullOrEmpty(json) ? "" : json) + "\n" + response);
#endif
                        var returnValue = JsonSerializer.Deserialize<TResponse>(response, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                        return returnValue;
                    }
                    if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        throw new ApiException($"Unauthorized {url}", result.StatusCode);
                    }
                    var error = await result.Content?.ReadAsStringAsync() ?? string.Empty;
                    _logger.LogInformation(" === API === " + url + " => " + (string.IsNullOrEmpty(json) ? "" : json) + " " + error);
                    throw new ApiException(error, result.StatusCode);
                }
            }
            catch
            {
                throw;
            }
        }

        private string ToQueryString<T>(T obj)
        {
            var properties = from p in obj?
                                    .GetType()
                                    .GetProperties()
                             where p.GetValue(obj, null) != null
                             select $"{HttpUtility.UrlEncode(p.Name)}" +
                             $"={HttpUtility.UrlEncode(p.GetValue(obj)?.ToString())}";
            return string.Join("&", properties);
        }
        #endregion
    }

    internal class Void
    {
        private static Void empty;
        public static Void Empty => empty ?? (empty = new Void());
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class Form : Attribute
    {
        public string label { get; set; }
        public Form(string label)
        {
            this.label = label;
        }
    }
}
