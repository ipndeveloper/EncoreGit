using System;
using System.Text;
using System.Text.RegularExpressions;

namespace TestMasterHelpProvider.DataFaker
{
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
			string result = String.Empty;

			if (length > 0)
			{
				result = Alpha(1);
			}

			if (length > 1)
			{
				result = String.Concat(result, SelectFrom((length - 1), "abcdefghijkmnopqrstuvwxyz0123456789"));
			}

			return result;
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
