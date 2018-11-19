using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweety.Models;

namespace TweetyCore.Models
{
    public class TweetResponse
    {
        public int Count { get; set; }
        public TweetResult Data { get; set; }
    }
}
