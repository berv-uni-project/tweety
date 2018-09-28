using System.Collections.Generic;

namespace Tweety.Models
{
    public class QueryCategory
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<HasilTweet> Tweet { get; set; }
        public int Num { get; set; }

        public QueryCategory()
        {
            Id = "";
            Name = "";
            Tweet = new List<HasilTweet>();
            Num = 0;
        }

    }
}