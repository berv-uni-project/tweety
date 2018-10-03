using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Tweetinvi;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Models;
using Tweety.Models;
using Utils.StringMatching;

namespace TweetyCore.Utils
{
    public class TwitterConnect
    {
        private TweetResult tweetResults;
        private QueryCategory dinasKesehatan;
        private QueryCategory dinasBinamarga;
        private QueryCategory dinasPemuda;
        private QueryCategory dinasPendidikan;
        private QueryCategory dinasSosial;
        private QueryCategory other;
        private bool[] categorized;

        public TwitterConnect()
        {
            tweetResults = new TweetResult
            {
                Query = new List<QueryCategory>()
            };
            dinasKesehatan = new QueryCategory
            {
                Id = "dinas_kesehatan",
                Name = "Dinas Kesehatan",
                Num = 0,
                Tweet = new List<HasilTweet>()
            };
            dinasBinamarga = new QueryCategory
            {
                Id = "dinas_binamarga",
                Name = "Dinas Binamarga",
                Num = 0,
                Tweet = new List<HasilTweet>()
            };
            dinasPemuda = new QueryCategory
            {
                Id = "dinas_pemuda",
                Name = "Dinas Pemuda",
                Num = 0,
                Tweet = new List<HasilTweet>()
            };
            dinasPendidikan = new QueryCategory
            {
                Id = "dinas_pendidikan",
                Name = "Dinas Pendidikan",
                Num = 0,
                Tweet = new List<HasilTweet>()
            };
            dinasSosial = new QueryCategory
            {
                Id = "dinas_sosial",
                Name = "Dinas Sosial",
                Num = 0,
                Tweet = new List<HasilTweet>()
            };
            other = new QueryCategory
            {
                Id = "no_category",
                Name = "No Category",
                Num = 0,
                Tweet = new List<HasilTweet>()
            };
            tweetResults.Query.Add(dinasKesehatan);
            tweetResults.Query.Add(dinasBinamarga);
            tweetResults.Query.Add(dinasPemuda);
            tweetResults.Query.Add(dinasPendidikan);
            tweetResults.Query.Add(dinasSosial);
            tweetResults.Query.Add(other);
        }

        private void Connect()
        {
            string customer_key = Environment.GetEnvironmentVariable("CUSTOMER_KEY");
            string customer_secret = Environment.GetEnvironmentVariable("CUSTOMER_SECRET");
            string token = Environment.GetEnvironmentVariable("TOKEN");
            string token_secret = Environment.GetEnvironmentVariable("TOKEN_SECRET");
            // When a new thread is created, the default credentials will be the Application Credentials
            Auth.ApplicationCredentials = new TwitterCredentials(customer_key, customer_secret, token, token_secret);
        }

        public Tuple<int, TweetResult> ProcessTag(Tags tags)
        {
            int sumOfTweets = ParseTag(tags);
            return Tuple.Create(sumOfTweets, tweetResults);
        }

        private int ParseTag(Tags tag)
        {
            int sumOfTweet = 0;
            Connect();
            var searchParameter = Search.CreateTweetSearchParameter(tag.Name);
            searchParameter.MaximumNumberOfResults = 100;
            var tweets = Search.SearchTweets(searchParameter);
            if (tweets != null)
            {
                sumOfTweet = tweets.Count();
                categorized = new bool[sumOfTweet];
                for (int i = 0; i < sumOfTweet; i++)
                {
                    categorized[i] = false;
                }

                foreach (QueryCategory category in tweetResults.Query)
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
                    if (keywords != "")
                    {
                        GetQuery(category, tweets, keywords, tag.isKMP);
                    }
                }
                for (int j = 0; j < sumOfTweet; j++)
                {
                    if (!categorized[j])
                    {
                        HasilTweet hasilTemp = new HasilTweet
                        {
                            TweetContent = tweets.ElementAt(j),
                            Result = tweets.ElementAt(j).Text
                        };
                        tweetResults.Query.Find(query => query.Id == "no_category").Tweet.Add(hasilTemp);
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
                    indexFound = -1;
                    if (isKMP)
                    {
                        indexFound = KMP.Solve(tweet.Text, keyWord);
                    }
                    else
                    {
                        indexFound = Booyer.Solve(tweet.Text, keyWord);
                    }
                    if (indexFound != -1)
                    {
                        cats++;
                        categorized[i] = true;
                        newText = Regex.Replace(newText, keyWord, @"<b>$&</b>", RegexOptions.IgnoreCase);
                    }
                }
                if (cats > 0)
                {
                    HasilTweet hasil = new HasilTweet
                    {
                        TweetContent = tweet,
                        Result = newText
                    };
                    tweetResults.Query.Find(q => q.Id == category.Id).Tweet.Add(hasil);
                }
                i++;
            }
        }
    }
}

