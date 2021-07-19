using Microsoft.Extensions.Logging;
using System;

namespace TweetyCore.Utils.StringMatcher
{
    public class Booyer : IBooyer
    {
        private const int _maxEmoticon = 128;
        private readonly ILogger<Booyer> _logger;

        public Booyer(ILogger<Booyer> logger)
        {
            _logger = logger;
        }

        private int[] InitLast(string keyword)
        {
            _logger.LogInformation("Setup Last");
            var last = new int[_maxEmoticon];

            for (int i = 0; i < _maxEmoticon; i++)
            {
                last[i] = -1;
            }

            for (int i = 0; i < keyword.Length; i++)
            {
                last[keyword[i]] = i;
            }

            return last;
        }

        public int Solve(string input, string keyword)
        {
            if (string.IsNullOrWhiteSpace(input) || string.IsNullOrWhiteSpace(keyword))
            {
                return -1;
            }
            var _input = input.ToLower();
            var _keyword = keyword.ToLower();
            var last = InitLast(_keyword);
            int n = _input.Length;
            int m = _keyword.Length;
            int i = m - 1;

            if (i > n - 1)
            {
                return -1;
            }

            int j = m - 1;
            do
            {
                if (_keyword[j] == _input[i])
                {
                    if (j == 0)
                    {
                        return i;
                    }
                    else
                    {
                        i--;
                        j--;
                    }
                }
                else
                {
                    int lo;
                    if (!(_input[i] > 128 || _input[i] < 0))
                    {
                        lo = last[_input[i]];
                    }
                    else
                    {
                        lo = -1;
                    }
                    i = i + m - Math.Min(j, 1 + lo);
                    j = m - 1;
                }
            } while (i <= n - 1);

            return -1; //no match
        }
    }
}
