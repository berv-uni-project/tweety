using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Parameters;
using TweetyCore.Models;
using TweetyCore.Utils.StringMatcher;

namespace TweetyCore.Utils.Twitter
{
    public class TwitterConnect : ITwitterConnect
    {
        private readonly TweetResult _tweetResults = new()
        {
            Query = new List<IQueryCategory>()
            {
                new DinkesCategory(),
                new DinBimarCategory(),
                new DinPemCategory(),
                new DinPenCategory(),
                new DinSosCategory(),
                new NoCategory()
            }
        };
        private readonly ILogger<TwitterConnect> _logger;
        private readonly IKMP _kmp;
        private readonly IBooyer _booyer;
        private readonly ITwitterClient _twitterClient;

        public TwitterConnect(ILogger<TwitterConnect> logger,
            IKMP kmp,
            IBooyer booyer,
            ITwitterClient twitterClient
            )
        {
            _logger = logger;
            _kmp = kmp;
            _booyer = booyer;
            _twitterClient = twitterClient;
        }

        public async Task<TweetResponse> ProcessTag(Tags tags)
        {
            int sumOfTweets = await ParseTag(tags);
            _logger.LogInformation($"Sum of Tweets: {sumOfTweets}");
            return new TweetResponse()
            {
                Count = sumOfTweets,
                Data = _tweetResults
            };
        }

        #region Private Methods    
        private async Task<int> ParseTag(Tags tag)
        {
            int sumOfTweet = 0;
            var tweets = await _twitterClient.Search.SearchTweetsAsync(new SearchTweetsParameters(tag.Name)
            {
                PageSize = 500,
            });
            if (tweets != null)
            {
                sumOfTweet = tweets.Length;
                var boolTweets = tweets.Select(x => new TweetBool(x, false)).ToList();

                foreach (IQueryCategory category in _tweetResults.Query)
                {
                    string keywords = "";
                    if (category.Id == CategoryConstants.Id.DinKes)
                    {
                        keywords = tag.DinasKesehatan;
                    }
                    else if (category.Id == CategoryConstants.Id.DinBimar)
                    {
                        keywords = tag.DinasBinamarga;
                    }
                    else if (category.Id == CategoryConstants.Id.DinPen)
                    {
                        keywords = tag.DinasPendidikan;
                    }
                    else if (category.Id == CategoryConstants.Id.DinPem)
                    {
                        keywords = tag.DinasPemuda;
                    }
                    else if (category.Id == CategoryConstants.Id.DinSos)
                    {
                        keywords = tag.DinasSosial;
                    }

                    if (keywords != null && keywords != "")
                    {
                        GetQuery(ref boolTweets, category.Id, keywords, tag.IsKMP.GetValueOrDefault());
                    }
                }

                var uncategorized = boolTweets.Where(x => !x.Categorized).Select(x => new HasilTweet() { TweetContent = x.Tweet, Result = x.Tweet.Text });
                _tweetResults.Query.Find(query => query.Id == CategoryConstants.Id.NoCategory).Tweet.AddRange(uncategorized);
            }
            return sumOfTweet;
        }

        private void GetQuery(ref List<TweetBool> tweets, string categoryId, string keywords, bool isKMP)
        {
            string[] keywordsArray = keywords.Split(",");
            int i = 0;
            foreach (TweetBool tweet in tweets)
            {
                int indexFound;
                int cats = 0;
                string newText = tweet.Tweet.Text;
                foreach (string keyWord in keywordsArray)
                {
                    if (isKMP)
                    {
                        indexFound = _kmp.Solve(tweet.Tweet.Text, keyWord);
                    }
                    else
                    {
                        indexFound = _booyer.Solve(tweet.Tweet.Text, keyWord);
                    }
                    if (indexFound != -1)
                    {
                        cats++;
                        tweet.Categorized = true;
                        newText = Regex.Replace(newText, keyWord, @"<b>$&</b>", RegexOptions.IgnoreCase);
                    }
                }
                if (cats > 0)
                {
                    HasilTweet hasil = new()
                    {
                        TweetContent = tweet.Tweet,
                        Result = newText
                    };
                    _tweetResults.Query.Find(q => q.Id == categoryId).Tweet.Add(hasil);
                }
                i++;
            }
        }
        #endregion
    }
}
