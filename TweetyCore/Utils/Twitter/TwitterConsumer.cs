using System;
using Tweetinvi;

namespace TweetyCore.Utils.Twitter
{
    public class TwitterConsumer : TwitterClient, ITwitterClient
    {
        public TwitterConsumer() : base(Environment.GetEnvironmentVariable("CUSTOMER_KEY"), Environment.GetEnvironmentVariable("CUSTOMER_SECRET"), Environment.GetEnvironmentVariable("TOKEN"), Environment.GetEnvironmentVariable("TOKEN_SECRET"))
        {

        }
    }
}
