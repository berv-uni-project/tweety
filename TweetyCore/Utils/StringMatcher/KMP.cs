using Microsoft.Extensions.Logging;

namespace TweetyCore.Utils.StringMatcher
{
    public class KMP : IKMP
    {
        private string _input; //string for searching
        private string _keyword; //the keyword to find
        private int[] _borderFunction;
        private readonly ILogger<KMP> _logger;

        public KMP(ILogger<KMP> logger)
        {
            _logger = logger;
        }

        private void InitBorderFunction()
        {
            //border function algorithm
            _logger.LogInformation("Setup Border Function");
            int size = _keyword.Length;
            _borderFunction = new int[size + 1];

            int j = 0;
            int i = 1;
            while (i < size)
            {
                if (_keyword[j] == _keyword[i])
                {
                    _borderFunction[i + 1] = j + 1;
                    i++;
                    j++;
                }
                else if (j > 0)
                {
                    j = _borderFunction[j];
                }
                else
                {
                    _borderFunction[i + 1] = 0;
                    i++;
                }
            }

        }

        public int Solve(string input, string keyword)
        {
            if (input.Length == 0)
            {
                _logger.LogWarning("No Input");
                return -2;
            }
            else if (keyword.Length == 0)
            {
                _logger.LogWarning("No Keyword");
                return -3;
            }
            else
            {
                _input = input.ToLower();
                _keyword = keyword.ToLower();
                //init borderfunction
                InitBorderFunction();

                //progress to solving
                int n = _input.Length;
                int m = _keyword.Length;

                int i = 0;
                int j = 0;

                while (i < n)
                {
                    if (_keyword[j] == _input[i])
                    {
                        if (j == m - 1)
                        {
                            return i - m + 1;
                        }
                        i++;
                        j++;
                    }
                    else if (j > 0)
                    {
                        j = _borderFunction[j];
                    }
                    else
                    {
                        i++;
                    }
                }
                return -1;
            }
        }
    }
}
