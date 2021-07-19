using System.Collections.Generic;

namespace TweetyCore.Models
{
    public interface IQueryCategory
    {
        public string Id { get; }
        public string Name { get; }
        public List<HasilTweet> Tweet { get; }
    }
}