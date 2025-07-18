using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ApiClientModule
{
    public class AuthorizationDelegatingHandler : DelegatingHandler
    {
        private readonly ITokenProvider _tokenProvider;

        public AuthorizationDelegatingHandler(ITokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_tokenProvider == null)
                throw new Exception("You need to implement the interface ITokenProvider");

            var token = await _tokenProvider.GetTokenAsync();

            if (!string.IsNullOrEmpty(token.AccessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
