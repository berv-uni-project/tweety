using TweetyCore.Utils;

namespace TweetyCore.Models
{
    public class DinPenCategory : IQueryCategory
    {
        public string Id { get; private set; } = CategoryConstants.Id.DinPen;
        public string Name { get; private set; } = CategoryConstants.Display.DinPen;
        public List<HasilTweet> Tweet { get; private set; } = new List<HasilTweet>();
    }
}
