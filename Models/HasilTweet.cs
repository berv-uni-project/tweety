using System.Collections.Generic;
using Tweetinvi.Models;

namespace Tweety.Models
{
    public class HasilTweet
    {
        public ITweet TweetContent { set; get; }
        public List<int> StartMark { get; set; }
        public List<int> QueryLength { get; set; }
        public string Result { get; set; }

        public HasilTweet()
        {
            StartMark = new List<int>();
            QueryLength = new List<int>();
        }
    }
}