using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Tweety.Models;
using Tweetinvi;
using System.IO;
using Tweetinvi.Core.Authentication;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Interfaces;
using Utils.StringMatching;

namespace Tweety.Controllers
{
    public class TweetyFormController : Controller
    {
        private int jumlahtweetygdidapat;
        [HttpGet]
        public ActionResult SearchForm()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SearchForm(Tags tag)
        {
            if (ModelState.IsValid)
            {
                TweetResult Ax = new TweetResult();
                Ax.Query = new List<QueryCategory>();
                QueryCategory Ay = new QueryCategory();
                Ay.Id = tag.DinasKesehatan;
                Ay.Name = "Dinas Kesehatan";
                Ay.Num = 0;
                Ay.Tweet = new List<HasilTweet>();
                Ax.Query.Add(Ay);
                Ay = new QueryCategory();
                Ay.Id = tag.DinasBinamarga;
                Ay.Name = "Dinas Binamarga";
                Ay.Num = 0;
                Ay.Tweet = new List<HasilTweet>();
                Ax.Query.Add(Ay);
                Ay = new QueryCategory();
                Ay.Id = tag.DinasPemuda;
                Ay.Name = "Dinas Pemuda";
                Ay.Num = 0;
                Ay.Tweet = new List<HasilTweet>();
                Ax.Query.Add(Ay);
                Ay = new QueryCategory();
                Ay.Id = tag.DinasPendidikan;
                Ay.Name = "Dinas Pendidikan";
                Ay.Num = 0;
                Ay.Tweet = new List<HasilTweet>();
                Ax.Query.Add(Ay);
                Ay = new QueryCategory();
                Ay.Id = tag.DinasSosial;
                Ay.Name = "Dinas Sosial";
                Ay.Num = 0;
                Ay.Tweet = new List<HasilTweet>();
                Ax.Query.Add(Ay);
                ParseTag(tag, Ax);
                if (jumlahtweetygdidapat > 0)
                {
                    TempData["TweetAct"] = Ax;
                    return RedirectToAction("ShowResult", "Result");
                }
                else
                {
                    return RedirectToAction("NoResult", "Result");
                }
            }
            return View();
        }

        public ActionResult About()
        {
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
            jumlahtweetygdidapat = 0;

            try
            {
                QueryCategory NoCategory = new QueryCategory();
                NoCategory.Name = "No Category";
                var tweets = Search.SearchTweets(searchParameter);
                if (tweets != null)
                {
                    jumlahtweetygdidapat = tweets.Count();
                    tweets.ForEach(t => InsertT(t, Tweet, ref idx));
                    bool[] NoCate = new bool[jumlahtweetygdidapat];
                    for(int g = 0; g< jumlahtweetygdidapat; g++)
                    {
                        NoCate[g] = false;
                    }
                    foreach(QueryCategory value in Tweetx.Query)
                    {
                        GetQuery(value.Id, Tweet, value, ref NoCate, tag);
                    }
                    for (int g = 0; g < jumlahtweetygdidapat; g++) {
                        if (!NoCate[g])
                        {
                            HasilTweet HasilTemp = new HasilTweet();
                            HasilTemp.TweetContent = Tweet[g];
                            HasilTemp.result = Tweet[g].Text;
                            NoCategory.Tweet.Add(HasilTemp);
                        }
                    }
                }
                Tweetx.Query.Add(NoCategory);
            }

            catch (FileNotFoundException ex)
            {
                Console.WriteLine("File Not Found.");
            }
        }

        void InsertT(ITweet T, ITweet[] Tx, ref int i)
        {
            if (i < jumlahtweetygdidapat)
            {
                Tx[i] = T;
                i++;
            }
        }

        private void GetQuery(string Query, ITweet[] Tweet, QueryCategory A, ref bool[] N, Tags tag)
        {
            int k;
            int l;
            int y;
            y = 0;
            for (int j = 0; j < jumlahtweetygdidapat; j++)
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
                    string thequery = "";
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
                        thequery = querySearch;
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
                        Ax.result = Ax.TweetContent.Text;
                        foreach (int pos in Ax.StartMark) {
                            if (pos != -1)
                            {
                                int i = Ax.StartMark.IndexOf(pos);
                                string n = Ax.result.Substring(pos, Ax.QueryLength.ElementAt(i));
                                Ax.result = Ax.result.Replace(n, "<strong>" + n + "</strong>");
                            }
                        }
                    }
                }
            }
        } 

    }
}
