using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;

namespace ApiClientModule.UnitTests
{
    public class Tests
    {
        private ServiceProvider _serviceProvider;
        [SetUp]
        public void Setup()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<ITokenProvider, TokenProvider>();
            services.AddSingleton<IApiClientService, ApiClientService>();
            //services.AddTransient<AuthorizationDelegatingHandler>();

            RetryHandlerSettings retryHandlerSettings = new RetryHandlerSettings();
            retryHandlerSettings.DelayMsBetweenRetries = 1000;
            retryHandlerSettings.MaxRetries = 1;
            retryHandlerSettings.RetryCondition = async (response) =>
            {
                return await Task.FromResult(true);
            };

            services.AddTransient(sp =>
            {
                //var innerHandler = new HttpClientHandler();
                return new RetryHandler(sp.GetRequiredService<ITokenProvider>(), retryHandlerSettings);
            });


            services.AddHttpClient("api", config => { config.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/"); })
                //.AddHttpMessageHandler<AuthorizationDelegatingHandler>()
                .AddHttpMessageHandler<RetryHandler>();  // ConfigurePrimaryHttpMessageHandler

            _serviceProvider = services.BuildServiceProvider();

        }

        [Test]
        public async Task GetEndpoint()
        {
            var apiClientService = _serviceProvider.GetRequiredService<IApiClientService>();
            apiClientService.SetClientId("api");
            var resp = await apiClientService.GetAsync<GetResponse1>("todos/1");
            Assert.That(resp, Is.Not.Null);
        }

        //[Test]
        //public async Task GetWrongEndpoint()
        //{
        //    var apiClientService = _serviceProvider.GetRequiredService<IApiClientService>();
        //    apiClientService.SetClientId("api");
        //    var resp = await apiClientService.GetAsync<GetResponse1>("todosz/1");
            
        //}

        [Test]
        public async Task GetEndpointWithUrlParameters()
        {
            var apiClientService = _serviceProvider.GetRequiredService<IApiClientService>();
            apiClientService.SetClientId("api");
            var resp = await apiClientService.GetAsync<GetRequest, List<GetResponse2>>("comments", new GetRequest());
            Assert.That(resp.Count, Is.GreaterThan(1));
        }

        [Test]
        public async Task PostEndpoint()
        {
            var apiClientService = _serviceProvider.GetRequiredService<IApiClientService>();
            apiClientService.SetClientId("api");
            var resp = await apiClientService.PostAsync<PostRequest, PostResponse>("posts", new PostRequest() { Title = "foo", Body = "bar", UserId = 1} );
            Assert.That(resp.Id, Is.GreaterThan(0));
            Assert.That(resp.Title, Is.EqualTo("foo"));
            Assert.That(resp.Body, Is.EqualTo("bar"));
            Assert.That(resp.UserId, Is.EqualTo(1));
        }

        [Test]
        public async Task PostEndpointWithoutResponse()
        {
            var apiClientService = _serviceProvider.GetRequiredService<IApiClientService>();
            apiClientService.SetClientId("api");
            await apiClientService.PostAsync<PostResponse>("posts");
            Assert.Pass("Test passed");
        }

        [TearDown]
        public void TearDown()
        {
            _serviceProvider.Dispose();
        }
    }

    public class GetRequest
    {
        public int PostId { get; set; } = 1;
    }

    public class GetResponse1
    {
        public int UserId {  get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Completed { get; set; }
    }

    public class GetResponse2
    {
        public int PostId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Body { get; set; }
    }

    public class PostRequest
    {
        public int UserId { get; set; }
        public string Body { get; set; }
        public string Title { get; set; }
        public int Id { get; set; }
    }

    public class PostResponse : PostRequest
    {
       
    }
}
