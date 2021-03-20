using System.Collections;
using System.Collections.Generic;

namespace TweetyCore.Test.Data
{
    public class StringMatchingData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "Bervianto Leo Pratama", "bervianto", 0 };
            yield return new object[] { "Bervianto Leo Pratama", "bervi", 0 };
            yield return new object[] { "Bervianto Leo Pratama", "leo", 10 };
            yield return new object[] { "Bervianto Leo Pratama", "pratama", 14 };
            yield return new object[] { "Bervianto Leo Pratama", "via", 3 };
            yield return new object[] { "Bervianto Leo Pratama", "aseng", -1 };
            yield return new object[] { "Bervianto Leo Pratama", "saha", -1 };
            yield return new object[] { "", "", -1 };
            yield return new object[] { "Hello World!", "", -1 };
            yield return new object[] { "", "Wutt", -1 };
            yield return new object[] { "shorter", "I'm longer thooo", -1 };
            yield return new object[] { "suply", "suply...", -1 };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}