using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Tweety.Integration.Test.Factory;
using TweetyCore;
using TweetyCore.Models;
using Xunit;

namespace Tweety.Integration.Test.Api.Controllers
{
    public class ApiControllerTests : IClassFixture<ApiQueryTestApplicationFactory<Program>>
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

        public ApiControllerTests(ApiQueryTestApplicationFactory<Program> factory)
        {
            _factory = factory;
            _httpClient = _factory.CreateClient();
        }

        [Fact]
        public async Task SearchTweet_UnAuthorized()
        {
            using var body = new StringContent(JsonConvert.SerializeObject(_defaultRequest));
            var response = await _httpClient.PostAsync($"/api/v1/tweety/find", body);
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
