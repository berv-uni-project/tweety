using Tweety.Models;
using TweetyCore.Models;

namespace TweetyCore.Utils.Twitter
{
    public interface ITwitterConnect
    {
        TweetResponse ProcessTag(Tags tags);
    }
}
