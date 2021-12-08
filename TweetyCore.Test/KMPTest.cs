using Autofac.Extras.Moq;
using TweetyCore.Test.Data;
using TweetyCore.Utils.StringMatcher;
using Xunit;

namespace TweetyCore.Test
{
    public class KMPTest
    {
        [Theory]
        [ClassData(typeof(StringMatchingData))]
        public void StringMatchingTest(string longString, string keyword, int expectedResult)
        {
            using var mock = AutoMock.GetLoose();
            var booyer = mock.Create<KMP>();
            var result = booyer.Solve(longString, keyword);
            Assert.Equal(expectedResult, result);
        }
    }
}
