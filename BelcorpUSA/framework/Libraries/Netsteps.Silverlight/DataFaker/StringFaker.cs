using System.Text;
using System.Text.RegularExpressions;

namespace NetSteps.Silverlight.DataFaker
{
    public static class StringFaker
    {
        public static string Numeric(int length)
        {
            return SelectFrom(length, "0123456789");
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
