using Tweetinvi;
using TweetyCore.ConfigModel;

namespace TweetyCore.Utils.Twitter
{
    public class TwitterConsumer : TwitterClient, ITwitterClient
    {
        public TwitterConsumer(TwitterConfig twitterConfig) : base(twitterConfig.CustomerKey, twitterConfig.CustomerSecret, twitterConfig.UserToken, twitterConfig.UserTokenSecret)
        {

        }
    }
}
