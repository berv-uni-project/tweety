using Autofac.Extras.Moq;
using System;
using TweetyCore.Test.Data;
using TweetyCore.Utils.StringMatcher;
using Xunit;

namespace TweetyCore.Test
{
    public class BooyerTest
    {
        [Theory]
        [ClassData(typeof(StringMatchingData))]
        public void StringMatchingTest(string longString, string keyword, int expectedResult)
        {
            using var mock = AutoMock.GetLoose();
            var booyer = mock.Create<Booyer>();
            var result = booyer.Solve(longString, keyword);
            Assert.Equal(expectedResult, result);
        }
    }
}
