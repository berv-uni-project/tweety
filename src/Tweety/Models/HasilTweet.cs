using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tweetinvi.Core;
using Tweetinvi.Core.Authentication;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Streaminvi;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Json;

namespace Tweety.Models
{
    public class HasilTweet
    {
        public ITweet TweetContent { set; get; }
        public List<int> StartMark { get; set; }
        public List<int> QueryLength { get; set; }
        public string result;

        public HasilTweet()
        {
            StartMark = new List<int>();
            QueryLength = new List<int>();
        }
    }
}