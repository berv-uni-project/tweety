using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tweety.Models
{
    public class QueryCategory
    {
        public string id;
        public string name;
        public List<HasilTweet> Tweet;
        public int num;

        public QueryCategory()
        {
            id = "";
            name = "";
            Tweet = new List<HasilTweet>();
            num = 0;
        }

    }
}