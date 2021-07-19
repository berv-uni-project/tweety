using Autofac.Extras.Moq;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Client.Tools;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using TweetyCore.Models;
using TweetyCore.Utils;
using TweetyCore.Utils.Twitter;
using Xunit;

namespace TweetyCore.Test
{
    public class TwitterConnectTest
    {
        [Fact]
        public async Task ProcessTagTest()
        {

            var searchTerm = "Ridwan Kamil";
            var requests = new Tags()
            {
                Name = searchTerm,
                IsKMP = true,
                DinasBinamarga = "Apa",
                DinasKesehatan = "emil",
                DinasPemuda = "kang",
                DinasPendidikan = "ridwan",
                DinasSosial = "kamil"
            };
            
            using var mock = AutoMock.GetLoose();
            var twitterClientMock = mock.Mock<ITwitterClient>();
            var twitterClientFactoryMock = mock.Mock<ITwitterClientFactories>();
            var mockResult1 = mock.Mock<ITweet>();
            mockResult1.Setup(x => x.Text).Returns("Apa Kabar Ridwan Kamil");
            var mockResult2 = mock.Mock<ITweet>();
            mockResult2.Setup(x => x.Text).Returns("Gimana nih kang Emil");
            var mockResult3 = mock.Mock<ITweet>();
            mockResult3.Setup(x => x.Text).Returns("Kang Emil, gimana nih jabar");
            var searchParam = new SearchTweetsParameters(searchTerm);
            var results = new ITweet[]
            {
                mockResult1.Object,
                mockResult2.Object,
                mockResult3.Object
            };
            twitterClientMock.Setup(client => client.Search.SearchTweetsAsync(It.Is<SearchTweetsParameters>(param => param.Query == searchTerm))).Returns(Task.FromResult(results));
            var twitterClient = mock.Create<TwitterConnect>();
            var result = await twitterClient.ProcessTag(requests);
            twitterClientMock.Verify(x => x.Search.SearchTweetsAsync(It.Is<SearchTweetsParameters>(param => param.Query == searchTerm)), Times.Exactly(1));

            var totalTweet = 3;
            Assert.Equal(totalTweet, result.Count);

            var tweetDinkes = result.Data.Query.Find(x => x.Id == CategoryConstants.Id.DinKes).Tweet.Select(x => x.Result);
            Assert.Contains("Kang <b>Emil</b>, gimana nih jabar", tweetDinkes);
        }
    }
}
