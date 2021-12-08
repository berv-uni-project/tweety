using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Tweety.Integration.Test.Factory;
using TweetyCore;
using TweetyCore.Models;
using Xunit;

namespace Tweety.Integration.Test.Api.Controllers
{
    public class HomeControllerTests : IClassFixture<ApiQueryTestApplicationFactory<Program>>
    {
        private readonly ApiQueryTestApplicationFactory<Program> _factory;
        private readonly HttpClient _httpClient;
        private readonly Tags _defaultRequest = new()
        {
            Name = "Ridwan Kamil",
            IsKMP = true,
            DinasSosial = "ridwan",
            DinasBinamarga = "kamil",
            DinasKesehatan = "emil",
            DinasPemuda = "kang",
            DinasPendidikan = "jabar"
        };

        public HomeControllerTests(ApiQueryTestApplicationFactory<Program> factory)
        {
            _factory = factory;
            _httpClient = _factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task SearchTweet_RedirectToLogin()
        {
            var query = new QueryBuilder
            {
                { "name", _defaultRequest.Name },
                { "isKmp", _defaultRequest.IsKMP.ToString() }
            };
            using var emptyBody = new StringContent("");
            var targetUrl = $"/{query.ToQueryString()}";
            var response = await _httpClient.PostAsync(targetUrl, emptyBody);
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(System.Net.HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("login.microsoftonline.com", response.Headers.Location.Host);
            Assert.Equal("login.microsoftonline.com", response.Headers.Location.Authority);
        }


        [Fact]
        public async Task Home_Get_Allowed()
        {
            var response = await _httpClient.GetAsync($"/");
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task About_Get_Allowed()
        {
            var response = await _httpClient.GetAsync($"/About");
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task About_Post_NotAllowed()
        {
            using var content = new StringContent("");
            var response = await _httpClient.PostAsync($"/About", content);
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(System.Net.HttpStatusCode.MethodNotAllowed, response.StatusCode);
        }
    }
}
