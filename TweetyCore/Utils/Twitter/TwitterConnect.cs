using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Tweety.Models;
using TweetyCore.Models;
using TweetyCore.Utils.StringMatcher;

namespace TweetyCore.Utils.Twitter
{
    public class TwitterConnect : ITwitterConnect
    {
        private readonly TweetResult _tweetResults = new()
        {
            Query = new List<QueryCategory>()
            {
                new QueryCategory
                {
                    Id = "dinas_kesehatan",
                    Name = "Dinas Kesehatan",
                    Num = 0,
                    Tweet = new List<HasilTweet>()
                }, new QueryCategory
                {
                    Id = "dinas_binamarga",
                    Name = "Dinas Binamarga",
                    Num = 0,
                    Tweet = new List<HasilTweet>()
                }, new QueryCategory
                {
                    Id = "dinas_pemuda",
                    Name = "Dinas Pemuda",
                    Num = 0,
                    Tweet = new List<HasilTweet>()
                }, new QueryCategory
                {
                    Id = "dinas_pendidikan",
                    Name = "Dinas Pendidikan",
                    Num = 0,
                    Tweet = new List<HasilTweet>()
                }, new QueryCategory
                {
                    Id = "dinas_sosial",
                    Name = "Dinas Sosial",
                    Num = 0,
                    Tweet = new List<HasilTweet>()
                }, new QueryCategory
                {
                    Id = "no_category",
                    Name = "No Category",
                    Num = 0,
                    Tweet = new List<HasilTweet>()
                }
            }
        };
        private bool[] _categorized;
        private readonly ILogger<TwitterConnect> _logger;
        private readonly IKMP _kmp;
        private readonly IBooyer _booyer;
        private readonly ITwitterClient _twitterClient;

        public TwitterConnect(ILogger<TwitterConnect> logger,
            IKMP kmp,
            IBooyer booyer
            )
        {
            _logger = logger;
            _kmp = kmp;
            _booyer = booyer;
            string customer_key = Environment.GetEnvironmentVariable("CUSTOMER_KEY");
            string customer_secret = Environment.GetEnvironmentVariable("CUSTOMER_SECRET");
            string token = Environment.GetEnvironmentVariable("TOKEN");
            string token_secret = Environment.GetEnvironmentVariable("TOKEN_SECRET");
            _twitterClient = new TwitterClient(customer_key, customer_secret, token, token_secret);
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
                PageSize = 100
            });
            if (tweets != null)
            {
                sumOfTweet = tweets.Length;
                _categorized = new bool[sumOfTweet];
                for (int i = 0; i < sumOfTweet; i++)
                {
                    _categorized[i] = false;
                }

                foreach (QueryCategory category in _tweetResults.Query)
                {
                    string keywords = "";
                    if (category.Id == "dinas_kesehatan")
                    {
                        keywords = tag.DinasKesehatan;
                    }
                    else if (category.Id == "dinas_binamarga")
                    {
                        keywords = tag.DinasBinamarga;
                    }
                    else if (category.Id == "dinas_pendidikan")
                    {
                        keywords = tag.DinasPendidikan;
                    }
                    else if (category.Id == "dinas_pemuda")
                    {
                        keywords = tag.DinasPemuda;
                    }
                    else if (category.Id == "dinas_sosial")
                    {
                        keywords = tag.DinasSosial;
                    }

                    if (keywords != null && keywords != "")
                    {
                        GetQuery(category, tweets, keywords, tag.IsKMP.GetValueOrDefault());
                    }
                }
                for (int j = 0; j < sumOfTweet; j++)
                {
                    if (!_categorized[j])
                    {
                        HasilTweet hasilTemp = new()
                        {
                            TweetContent = tweets.ElementAt(j),
                            Result = tweets.ElementAt(j).Text
                        };
                        _tweetResults.Query.Find(query => query.Id == "no_category").Tweet.Add(hasilTemp);
                    }
                }
            }
            return sumOfTweet;
        }

        private void GetQuery(QueryCategory category, IEnumerable<ITweet> tweets, string keywords, bool isKMP)
        {
            string[] keywordsArray = keywords.Split(",");
            int i = 0;
            foreach (ITweet tweet in tweets)
            {
                int indexFound;
                int cats = 0;
                string newText = tweet.Text;
                foreach (string keyWord in keywordsArray)
                {
                    if (isKMP)
                    {
                        indexFound = _kmp.Solve(tweet.Text, keyWord);
                    }
                    else
                    {
                        indexFound = _booyer.Solve(tweet.Text, keyWord);
                    }
                    if (indexFound != -1)
                    {
                        cats++;
                        _categorized[i] = true;
                        newText = Regex.Replace(newText, keyWord, @"<b>$&</b>", RegexOptions.IgnoreCase);
                        _logger.LogInformation($"Changes from {tweet.Text}, to New Text: {newText}");
                    }
                }
                if (cats > 0)
                {
                    HasilTweet hasil = new()
                    {
                        TweetContent = tweet,
                        Result = newText
                    };
                    _tweetResults.Query.Find(q => q.Id == category.Id).Tweet.Add(hasil);
                }
                i++;
            }
        }
        #endregion
    }
}
