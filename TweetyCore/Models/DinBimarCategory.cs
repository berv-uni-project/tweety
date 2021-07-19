using System.Collections.Generic;
using TweetyCore.Utils;

namespace TweetyCore.Models
{
    public class DinBimarCategory : IQueryCategory
    {
        public string Id { get; private set; } = CategoryConstants.Id.DinBimar;
        public string Name { get; private set; } = CategoryConstants.Display.DinBimar;
        public List<HasilTweet> Tweet { get; } = new List<HasilTweet>();
        public int Num { get; set; } = 0;
    }
}
