using TweetyCore.Utils;

namespace TweetyCore.Models
{
    public class DinSosCategory : IQueryCategory
    {
        public string Id { get; private set; } = CategoryConstants.Id.DinSos;
        public string Name { get; private set; } = CategoryConstants.Display.DinSos;
        public List<HasilTweet> Tweet { get; private set; } = new List<HasilTweet>();
    }
}
