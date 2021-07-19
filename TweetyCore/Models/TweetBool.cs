using Tweetinvi.Models;

namespace TweetyCore.Models
{
    public class TweetBool
    {
        public TweetBool(ITweet tweet)
        {
            Tweet = tweet;
        }

        public TweetBool(ITweet tweet, bool categorized)
        {
            Tweet = tweet;
            Categorized = categorized;
        }

        public ITweet Tweet { get; set; }

        public bool Categorized { get; set; }
    }
}
