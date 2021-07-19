using System.Threading.Tasks;
using TweetyCore.Models;

namespace TweetyCore.Utils.Twitter
{
    public interface ITwitterConnect
    {
        Task<TweetResponse> ProcessTag(Tags tags);
    }
}
