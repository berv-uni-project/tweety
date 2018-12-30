using System;

namespace Utils.StringMatching
{
	public static class KMP
	{
		private static string input; //string for searching
		private static string keyword; //the keyword to find
		private static int[] borderFunction;

		private static void InitBorderFunction()
		{
			//border function algorithm
			int size = keyword.Length;
			borderFunction = new int[size + 1];

			int j = 0;
			int i = 1;
			while (i < size) {
				if (keyword[j] == keyword[i]) {
					borderFunction[i + 1] = j + 1;
					i++;
					j++;
				} else if (j > 0) {
					j = borderFunction[j];
				} else {
					borderFunction[i + 1] = 0;
					i++;
				}
			}

		}

		public static int Solve(string _input, string _keyword)
		{
			if (_input.Length == 0)
			{
				// Console.WriteLine ("Please set your input");
				return -2;
			} else if (_keyword.Length == 0)
			{
				// Console.WriteLine ("Please set your keyword");
				return -3;
			} else
			{
				input = _input.ToLower();
				keyword = _keyword.ToLower();
				//init borderfunction
				InitBorderFunction();

				//progress to solving
				int n = input.Length;
				int m = keyword.Length;

				int i = 0;
				int j = 0;

				while (i < n)
				{
					if (keyword[j] == input[i])
					{
						if (j == m - 1)
						{
							return i - m + 1;
						}
						i++;
						j++;
					} else if (j > 0)
					{
						j = borderFunction[j];
					} else
					{
						i++;
					}
				}
				return -1;
			}
		}
	}

	public static class Booyer
	{
		private static string input;
		private static string keyword;
		private static int[] last;
		private const int maxEmoticon = 128;

		private static void InitLast(string _keyword)
		{
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

		public static int Solve(string _input, string _keyword)
		{
			input = _input.ToLower();
			keyword = _keyword.ToLower();
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
					if (j == 0) {
						return i;
					} else {
						i--;
						j--;
					}
				} else
				{
					int lo;
					if (!((int) input[i] > 128 || (int) input[i] < 0))
					{
						lo = last[input[i]];
					} else
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
