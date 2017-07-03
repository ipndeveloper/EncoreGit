using System.Text;
using System.Text.RegularExpressions;

namespace NetSteps.Common.DataFaker
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Methods to generate 'Fake' data for use in testing.
    /// Created: 03-18-2009
    /// </summary>
    public static class StringFaker
    {
        public static string Numeric(int length)
        {
            return SelectFrom(length, "0123456789");
        }

        public static string Numeric(int length, int decimals)
        {
            return SelectFrom(length - decimals, "0123456789") + "." + SelectFrom(decimals, "0123456789");
        }

        public static string Alpha(int length)
        {
            return SelectFrom(length, "abcdefghijkmnopqrstuvwxyz");
        }

        public static string AlphaNumeric(int length)
        {
            return SelectFrom(length, "0123456789abcdefghijkmnopqrstuvwxyz");
        }

        public static string SelectFrom(int numElements, string characters)
        {
            var returned = new StringBuilder();
            var length = characters.Length;
            while (numElements > 0)
            {
                returned.Append(characters[Random.Next(length)]);
                numElements--;
            }

            return returned.ToString();
        }

        public static string Randomize(string pattern)
        {
            return Regex.Replace(pattern, "[#\\?]",
                m => (m.ToString() == "#" ? Numeric(1) : Alpha(1))
                );
        }
    }
}
