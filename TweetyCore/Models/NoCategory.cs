using System.Collections.Generic;
using TweetyCore.Utils;

namespace TweetyCore.Models
{
    public class NoCategory : IQueryCategory
    {
        public string Id { get; private set; } = CategoryConstants.Id.NoCategory;
        public string Name { get; private set; } = CategoryConstants.Display.NoCategory;
        public List<HasilTweet> Tweet { get; private set; } = new List<HasilTweet>();
        public int Num { get; set; } = 0;
    }
}
