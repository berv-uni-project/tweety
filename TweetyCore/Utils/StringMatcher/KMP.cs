using Microsoft.Extensions.Logging;

namespace TweetyCore.Utils.StringMatcher
{
    public class KMP : IKMP
    {
        private readonly ILogger<KMP> _logger;

        public KMP(ILogger<KMP> logger)
        {
            _logger = logger;
        }

        private int[] InitBorderFunction(string keyword)
        {
            //border function algorithm
            _logger.LogInformation("Setup Border Function");
            int size = keyword.Length;
            var borderFunction = new int[size + 1];

            int j = 0;
            int i = 1;
            while (i < size)
            {
                if (keyword[j] == keyword[i])
                {
                    borderFunction[i + 1] = j + 1;
                    i++;
                    j++;
                }
                else if (j > 0)
                {
                    j = borderFunction[j];
                }
                else
                {
                    borderFunction[i + 1] = 0;
                    i++;
                }
            }

            return borderFunction;
        }

        public int Solve(string input, string keyword)
        {
            if (string.IsNullOrWhiteSpace(input) || string.IsNullOrWhiteSpace(keyword))
            {
                return -1;
            }
            var inputLowered = input.ToLower();
            var keywordLowered = keyword.ToLower();
            //init borderfunction
            var borderFunction = InitBorderFunction(keyword);

            //progress to solving
            int n = inputLowered.Length;
            int m = keywordLowered.Length;

            int i = 0;
            int j = 0;

            while (i < n)
            {
                if (keywordLowered[j] == inputLowered[i])
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
                    j = borderFunction[j];
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
