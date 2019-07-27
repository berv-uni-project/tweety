﻿using Microsoft.Extensions.Logging;
using System;

namespace TweetyCore.Utils.StringMatcher
{
    public class Booyer : IBooyer
    {
        private int[] _last;
        private const int _maxEmoticon = 128;
        private readonly ILogger<Booyer> _logger;

        public Booyer(ILogger<Booyer> logger)
        {
            _logger = logger;
        }

        private void InitLast(string keyword)
        {
            _logger.LogInformation("Setup Last");
            _last = new int[_maxEmoticon];

            for (int i = 0; i < _maxEmoticon; i++)
            {
                _last[i] = -1;
            }

            for (int i = 0; i < keyword.Length; i++)
            {
                _last[keyword[i]] = i;
            }
        }

        public int Solve(string input, string keyword)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(keyword))
            {
                return -1;
            }
            InitLast(keyword);
            int n = input.Length;
            int m = keyword.Length;
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
                        lo = _last[input[i]];
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
