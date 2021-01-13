using Tweety.Models;

namespace TweetyCore.Models
{
    public class TweetResponse
    {
        public int Count { get; set; }
        public TweetResult Data { get; set; }
    }
}
