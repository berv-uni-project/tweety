using Microsoft.Extensions.Logging;
using System;

namespace TweetyCore.Utils.StringMatcher
{
    public class Booyer : IBooyer
    {
        private string _input;
        private string _keyword;
        private int[] last;
        private const int maxEmoticon = 128;
        private ILogger<Booyer> _logger;

        public Booyer(ILogger<Booyer> logger)
        {
            _logger = logger;
        }

        private void InitLast(string _keyword)
        {
            _logger.LogInformation("Setup Last");
            last = new int[maxEmoticon];

            for (int i = 0; i < maxEmoticon; i++)
            {
                last[i] = -1;
            }

            for (int i = 0; i < _keyword.Length; i++)
            {
                last[_keyword[i]] = i;
            }
        }

        public int Solve(string input, string keyword)
        {
            _input = input.ToLower();
            _keyword = keyword.ToLower();
            InitLast(keyword);
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
                if (keyword[j] == input[i])
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
                    if (!(input[i] > 128 || input[i] < 0))
                    {
                        lo = last[input[i]];
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
