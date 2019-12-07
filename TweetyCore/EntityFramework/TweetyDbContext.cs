using Microsoft.EntityFrameworkCore;

namespace TweetyCore.EntityFramework
{
    public class TweetyDbContext : DbContext
    {
        public TweetyDbContext(DbContextOptions<TweetyDbContext> options)
            : base(options)
        { }

    }
}
