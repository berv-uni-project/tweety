using System.Collections.Generic;
using TweetyCore.Utils;

namespace TweetyCore.Models
{
    public class DinPemCategory : IQueryCategory
    {
        public string Id { get; private set; } = CategoryConstants.Id.DinPem;
        public string Name { get; private set; } = CategoryConstants.Display.DinPem;
        public List<HasilTweet> Tweet { get; private set; } = new List<HasilTweet>();
        public int Num { get; set; } = 0;
    }
}
