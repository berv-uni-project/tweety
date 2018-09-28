using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Tweetinvi;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Models;
using Tweety.Models;
using TweetyCore.Models;
using Utils.StringMatching;

namespace TweetyCore.Controllers
{
    public class HomeController : Controller
    {
        private int sumOfTweet;

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Tags tag)
        {
            if (ModelState.IsValid)
            {
                TweetResult tweetResults = new TweetResult
                {
                    Query = new List<QueryCategory>()
                };
                QueryCategory dinasKesehatan = new QueryCategory
                {
                    Id = tag.DinasKesehatan,
                    Name = "Dinas Kesehatan",
                    Num = 0,
                    Tweet = new List<HasilTweet>()
                };
                QueryCategory dinasBinamarga = new QueryCategory
                {
                    Id = tag.DinasBinamarga,
                    Name = "Dinas Binamarga",
                    Num = 0,
                    Tweet = new List<HasilTweet>()
                };
                QueryCategory dinasPemuda = new QueryCategory
                {
                    Id = tag.DinasPemuda,
                    Name = "Dinas Pemuda",
                    Num = 0,
                    Tweet = new List<HasilTweet>()
                };
                QueryCategory dinasPendidikan = new QueryCategory
                {
                    Id = tag.DinasPendidikan,
                    Name = "Dinas Pendidikan",
                    Num = 0,
                    Tweet = new List<HasilTweet>()
                };
                QueryCategory dinasSosial = new QueryCategory
                {
                    Id = tag.DinasSosial,
                    Name = "Dinas Sosial",
                    Num = 0,
                    Tweet = new List<HasilTweet>()
                };

                tweetResults.Query.Add(dinasKesehatan);
                tweetResults.Query.Add(dinasBinamarga);
                tweetResults.Query.Add(dinasPemuda);
                tweetResults.Query.Add(dinasPendidikan);
                tweetResults.Query.Add(dinasSosial);

                ParseTag(tag, tweetResults);

                if (sumOfTweet > 0)
                {
                    return View("ShowResult", tweetResults);
                }
                else
                {
                    return View("NoResult");
                }
            }
            return View();
        }

        private void ParseTag(Tags tag, TweetResult Tweetx)
        {
            ITweet[] Tweet = new ITweet[100];

            int[] DKes = new int[100];
            int idx = 0;
            string customer_key = Environment.GetEnvironmentVariable("CUSTOMER_KEY");
            string customer_secret = Environment.GetEnvironmentVariable("CUSTOMER_SECRET");
            string token = Environment.GetEnvironmentVariable("TOKEN");
            string token_secret = Environment.GetEnvironmentVariable("TOKEN_SECRET");
            // When a new thread is created, the default credentials will be the Application Credentials
            Auth.ApplicationCredentials = new TwitterCredentials(customer_key, customer_secret, token, token_secret);
            var searchParameter = Search.CreateTweetSearchParameter(tag.Name);
            searchParameter.MaximumNumberOfResults = 100;
            sumOfTweet = 0;

            QueryCategory NoCategory = new QueryCategory
            {
                Id="no_category",
                Name = "No Category"
            };
            var tweets = Search.SearchTweets(searchParameter);
            if (tweets != null)
            {
                sumOfTweet = tweets.Count();
                tweets.ForEach(t => InsertT(t, Tweet, ref idx));
                bool[] NoCate = new bool[sumOfTweet];
                for (int g = 0; g < sumOfTweet; g++)
                {
                    NoCate[g] = false;
                }
                foreach (QueryCategory value in Tweetx.Query)
                {
                    GetQuery(value.Id, Tweet, value, ref NoCate, tag);
                }
                for (int g = 0; g < sumOfTweet; g++)
                {
                    if (!NoCate[g])
                    {
                        HasilTweet HasilTemp = new HasilTweet
                        {
                            TweetContent = Tweet[g],
                            Result = Tweet[g].Text
                        };
                        NoCategory.Tweet.Add(HasilTemp);
                    }
                }
            }
            Tweetx.Query.Add(NoCategory);
        }


        private void InsertT(ITweet T, ITweet[] Tx, ref int i)
        {
            if (i < sumOfTweet)
            {
                Tx[i] = T;
                i++;
            }
        }

        private void GetQuery(string Query, ITweet[] Tweet, QueryCategory A, ref bool[] N, Tags tag)
        {
            int k;
            int l;
            for (int j = 0; j < sumOfTweet; j++)
            {
                string queryX = Query;
                string querySearch;
                bool queryBool = false;
                k = 0;
                l = 0;
                int X = -1;
                List<int> q = new List<int>();
                List<int> ql = new List<int>();
                if (queryX != null)
                {
                    string theQuery = "";
                    while (k < queryX.Length)
                    {
                        bool qfound = false;
                        while (queryX[k] == ' ' && k < queryX.Length)
                        {
                            k++;
                        }
                        int x = k;
                        while (!qfound && k < queryX.Length)
                        {
                            if (queryX[k] == ',')
                            {
                                qfound = true;
                            }
                            k++;
                        }
                        if (qfound)
                        {
                            querySearch = queryX.Substring(x, k - x - 1);
                        }
                        else
                        {
                            querySearch = queryX.Substring(x, k - x);
                        }
                        if (tag.isKMP)
                        {
                            X = KMP.Solve(Tweet[j].Text, querySearch);
                        }
                        else
                        {
                            X = Booyer.Solve(Tweet[j].Text, querySearch);
                        }
                        theQuery = querySearch;
                        q.Add(X);
                        ql.Add(querySearch.Length);
                        if (X != -1)
                        {
                            queryBool = true;
                        }
                        l++;
                    }
                    if (queryBool)
                    {
                        N[j] = true;
                        HasilTweet Ax = new HasilTweet();
                        foreach (int value in q)
                        {
                            Ax.StartMark.Add(value);
                        }
                        foreach (int value in ql)
                        {
                            Ax.QueryLength.Add(value);
                        }
                        Ax.TweetContent = Tweet[j];
                        A.Tweet.Add(Ax);
                        A.Num++;
                        Ax.Result = Ax.TweetContent.Text;
                        foreach (int pos in Ax.StartMark)
                        {
                            if (pos > 0)
                            {
                                int i = Ax.StartMark.IndexOf(pos);
                                string n = Ax.Result.Substring(pos, Ax.QueryLength.ElementAt(i));
                                Ax.Result = Ax.Result.Replace(n, "<strong>" + n + "</strong>");
                            }
                        }
                    }
                }
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

