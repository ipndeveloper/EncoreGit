using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace TestMasterHelpProvider.Extensions
{
	public static class StringExtensions
	{
		#region Validation Methods

		public static bool IsValidInt(this string integer)
		{
			if (integer != null)
				integer = integer.Trim();

			if (string.IsNullOrEmpty(integer))
				return false;
			else
			{
				Regex numericPattern = new Regex(@"^[-+]?\d*$");
				return numericPattern.IsMatch(integer);
			}
		}

		public static bool IsValidDateTime(this string dateTime)
		{
			DateTime date;
			if (DateTime.TryParse(dateTime, out date))
				return true;

			return false;
		}

		public static bool IsValidEmail(this string email)
		{
			bool returnValue = true;

			returnValue = !email.IsNullOrEmpty();

			if (returnValue)
			{
				Regex rX = new System.Text.RegularExpressions.Regex(RegularExpressions.Email);
				Match m = rX.Match(email);

				returnValue = m.Success;
			}

			if (returnValue)
			{
				returnValue = CountStringOccurrences(email, "@") == 1;
			}

			return returnValue;
		}

		public static bool IsValidTime(this string time)
		{
			DateTime newDateTime = DateTime.Now;
			string dateTime = string.Format("{0} {1}", newDateTime.ToShortDateString(), time);
			if (!DateTime.TryParse(dateTime, out newDateTime))
				return false;
			else
				return true;
		}

		public static bool IsNullOrEmpty(this string value)
		{
			string val = (value == null) ? string.Empty : value.Trim();
			return string.IsNullOrEmpty(val);
		}

		/// <summary>
		/// Determines whether the specified input is empty.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <returns>
		/// 	<c>true</c> if the specified input is empty; otherwise, <c>false</c>.
		/// </returns>
		/// <exception cref="T:System.ArgumentNullException"/>
		public static bool IsEmpty(this string input)
		{
			if (input == null) throw new ArgumentNullException();

			bool res = false;

			if (input.Length == 0)
			{
				res = true;
			}
			return res;
		}

		/// <summary>
		/// true, if the string contains only digits or float-point.
		/// Spaces are not considred.
		/// </summary>
		/// <param name="s">input string</param>
		/// <param name="floatpoint">true, if float-point is considered</param>
		/// <returns>true, if the string contains only digits or float-point</returns>
		public static bool IsNumberOnly(this string s, bool floatpoint)
		{
			s = s.Trim();
			if (s.Length == 0)
				return false;
			foreach (char c in s)
			{
				if (!char.IsDigit(c))
				{
					if (floatpoint && (c == '.' || c == ','))
						continue;
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Determines whether entire string is UPPER case. 
		/// </summary>
		/// <param name="input">The input string.</param>
		/// <returns>
		/// 	<c>true</c> if [is case upper] [the specified input]; otherwise, <c>false</c>.
		/// </returns>
		/// <exception cref="T:System.ArgumentNullException"/>
		public static bool IsCaseUpper(this string input)
		{
			if (input == null) throw new ArgumentNullException();

			if (input.IsEmpty())
			{
			}
			else
			{
				return String.Compare(input, input.ToUpper(), false) == 0;
			}
			return false;
		}

		/// <summary>
		/// Determines whether the string consists of just one char.
		/// </summary>
		/// <param name="input">The input string.</param>
		/// <returns>
		/// 	<c>true</c> if [is repeated char] [the specified input]; otherwise, <c>false</c>.
		/// </returns>
		/// <exception cref="T:System.ArgumentNullException"/>
		public static bool IsRepeatedChar(this string input)
		{
			if (input == null) throw new ArgumentNullException();

			//??? what about a string with only 1 char???
			if (input.IsEmpty()) return false;
			return input.Replace(input.Substring(0, 1), String.Empty).Length == 0;
		}

		/// <summary>
		/// Determines whether the string is pure whitespace.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <returns>
		/// 	<c>true</c> if the specified input is whitespace; otherwise, <c>false</c>.
		/// </returns>
		/// <exception cref="T:System.ArgumentNullException"/>
		public static bool IsWhitespace(this string input)
		{
			if (input == null) throw new ArgumentNullException();

			if (input.IsEmpty()) return false;
			return input.Replace(" ", String.Empty).Length == 0;
		}

		public static bool IsRegExMatch(this string value, string regularExpression)
		{
			return new Regex(regularExpression).IsMatch(value);
		}
		#endregion

		#region Conversion Methods
		public static int ToInt(this string value)
		{
			int result = 0;
			if (value.IsValidInt())
				int.TryParse(value, out result);
			return result;
		}

		public static long ToLong(this string value)
		{
			if (value.IsValidInt())
				return Convert.ToInt64(value);
			else
				return 0;
		}

		public static bool ToBool(this string value)
		{
			string boolValue = (value == null) ? string.Empty : value.ToString().ToLower();
			if (boolValue == "1" || boolValue == "true" || boolValue == "t" || boolValue == "y" || boolValue == "yes")
				return true;
			else
				return false;
		}

		public static List<string> ToStringList(this string value)
		{
			return GetFromStringList<string>(value, ',');
		}

		public static List<string> ToStringList(this string value, char separatingCharacter)
		{
			return GetFromStringList<string>(value, separatingCharacter);
		}

		/// <summary>
		/// Example: List<string> filterOptions = Utilities.String.GetStringList("All, A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z", Char.Parse(","));
		/// Works for strings, integers, bool, and enum lists right now - JHE
		/// </summary>
		/// <param name="commaSeparatedValues"></param>
		/// <param name="separatingCharacter"></param>
		/// <returns></returns>
		/// Test this - JHE
		public static List<T> GetFromStringList<T>(string commaSeparatedValues, char separatingCharacter)
		{
			Type objectType = typeof(T);
			T result = default(T);

			List<T> returnValue = new List<T>();
			char[] sep = { separatingCharacter };
			Array array = commaSeparatedValues.Split(sep);

			for (int i = 0; i < array.Length; i++)
			{
				string objectStringValue = array.GetValue(i).ToString().Trim();
				object obj = (T)result;
				if (obj is Enum)
					result = (T)Enum.Parse(objectType, objectStringValue, true);
				if (obj is Boolean)
				{
					string boolValue = objectStringValue.ToLower();
					if (boolValue.ToBool())
						result = (T)Convert.ChangeType(true, objectType, null);
					else
						result = (T)Convert.ChangeType(false, objectType, null);
				}
				else
					result = (T)Convert.ChangeType(objectStringValue, objectType, null);

				returnValue.Add(result);
			}

			return returnValue;
		}

		public static string ToCleanString(this string value)
		{
			return (value == null) ? string.Empty : value.Trim();
		}
		#endregion

		#region Remove/Replace Methods
		public static string ReplaceIgnoreCase(this string value, string oldValue, string newValue)
		{
			return Regex.Replace(value, oldValue, newValue, RegexOptions.IgnoreCase | RegexOptions.Multiline);
		}

		public static string RemoveIgnoreCase(this string value, string oldValue)
		{
			return Regex.Replace(value, oldValue, string.Empty, RegexOptions.IgnoreCase | RegexOptions.Multiline);
		}

		public static string RemoveSpaces(this string value)
		{
			if (value != null)
				return value.Replace(" ", string.Empty);
			else
				return value;
		}

		/// <summary>
		/// Remove accent from strings 
		/// </summary>
		/// <example>
		///  input:  "Příliš žluťoučký kůň úpěl ďábelské ódy."
		///  result: "Prilis zlutoucky kun upel dabelske ody."
		/// </example>
		/// <param name="s"></param>
		/// <remarks>founded at http://stackoverflow.com/questions/249087/
		/// how-do-i-remove-diacritics-accents-from-a-string-in-net</remarks>
		/// <returns>string without accents</returns>
		public static string RemoveDiacritics(this string s)
		{
			if (s.IsNullOrEmpty())
				return string.Empty;

			string stFormD = s.Normalize(NormalizationForm.FormD);
			StringBuilder sb = new StringBuilder();

			for (int ich = 0; ich < stFormD.Length; ich++)
			{
				UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
				if (uc != UnicodeCategory.NonSpacingMark)
				{
					sb.Append(stFormD[ich]);
				}
			}
			return (sb.ToString().Normalize(NormalizationForm.FormC));
		}

		/// <summary>
		/// Removes all extra white space, including leading and trailing whitespace.
		/// </summary>
		/// <param name="input">The input string.</param>
		/// <exception cref="T:System.ArgumentNullException"/>
		public static string RemoveExtraWhiteSpace(this string input)
		{
			if (input == null) throw new ArgumentNullException();

			// trim leading spaces
			input = input.Trim();
			return ReplaceWithMultipleSweeps(input, "  ", " ");
		}

		/// <summary>
		/// Replaces the all instances of stringFind with replaceWith. 
		/// Does multiple sweeps until there are no more matches, unlike String.Replace() which only does 1 sweep.
		/// </summary>
		/// <param name="input">The input string.</param>
		/// <param name="stringToFind">The string to match.</param>
		/// <param name="replaceWith">The string to replace with.</param>
		/// <exception cref="T:System.ArgumentNullException"/>
		public static string ReplaceWithMultipleSweeps(this string input, string stringToFind, string replaceWith)
		{
			if (input == null) throw new ArgumentNullException();
			if (stringToFind == null) throw new ArgumentNullException();
			if (replaceWith == null) throw new ArgumentNullException();

			while (input.Contains(stringToFind))
			{
				input = input.Replace(stringToFind, replaceWith);
			}
			return input;
		}
		#endregion

		#region Casing Methods

		public static string ProperCase(this string text)
		{
			try
			{
				text = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(text.ToLower());
				text = text.Replace(" And ", " and ");
				text = text.Replace(" The ", " the ");
				text = text.Replace(" On ", " on ");
				text = text.Replace(" In ", " in ");
				text = text.Replace(" Llc", " LLC");
				text = text.Replace(" XX", " XX");
				text = text.Replace(" XXX", " XXX");

				return text;
			}
			catch 
			{ 
				return String.Empty; 
			}
		}

		public static string ProperCaseIfUpper(this string value)
		{
			return (value != value.ToUpper()) ? value : value.ProperCase();
		}

		/// <summary>
		/// Takes a Pascal CASED string and inserts spaces:
		/// Example: "PascalCaseString" becomes "Pascal Case String" - JHE
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string PascalToSpaced(this string name)
		{
			if (string.IsNullOrEmpty(name))
				return string.Empty;

			Regex regex = new Regex("(?<=[a-z])(?<x>[A-Z])|(?<=.)(?<x>[A-Z])(?=[a-z])");
			name = regex.Replace(name, " ${x}");
			// get rid of any underscores or dashes
			name = name.Replace("_", string.Empty);
			return name.Replace("-", string.Empty);
		}

		public static string ToTitleCase(this string text)
		{
			if (text.IsNullOrEmpty())
				return string.Empty;

			string returnString = string.Empty;
			string[] arrayOfText = text.Split(' ');

			foreach (string item in arrayOfText)
			{
				string s = item.ToLower();
				string d = item[0].ToString().ToUpper();

				for (int i = 1; i < s.Length; i++)
					if (s[i - 1] == ' ')
						d += s[i].ToString().ToUpper();
					else
						d += s[i].ToString();

				returnString += " " + d;
			}

			return returnString.Trim();
		}

		#endregion

		public static string GetMd5Sum(this string str)
		{
			Encoder enc = System.Text.Encoding.Unicode.GetEncoder();

			byte[] unicodeText = new byte[str.Length * 2];
			enc.GetBytes(str.ToCharArray(), 0, str.Length, unicodeText, 0, true);

			MD5 md5 = new MD5CryptoServiceProvider();
			byte[] result = md5.ComputeHash(unicodeText);

			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < result.Length; i++)
				sb.Append(result[i].ToString("X2"));

			return sb.ToString();
		}

		public static bool ContainsIgnoreCase(this string value, string stringValue)
		{
			if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(stringValue))
				return false;
			return value.ToUpper().Contains(stringValue.ToUpper());
		}

		/// <summary>
		/// Reverse the string
		/// from http://en.wikipedia.org/wiki/Extension_method
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string Reverse(this string input)
		{
			char[] chars = input.ToCharArray();
			Array.Reverse(chars);
			return new String(chars);
		}

		public static string ToFullMask(this string value)
		{
			string returnValue = string.Empty;
			for (int i = 0; i < value.Length; i++)
			{
				returnValue += "*";
			}

			return returnValue;
		}

		public static bool IsMatch(this string input, string pattern)
		{
			return new Regex(pattern).IsMatch(input);
		}

		public enum PhoneFormats
		{
			OnlyNumbers /*1231231234*/,
			Dashes /*123-123-1234*/,
			ParenthesisAndDash/*(123) 123-1234*/
		}

		public static string PhoneFormat(this string phone, PhoneFormats format)
		{
			if (!string.IsNullOrEmpty(phone))
			{
				StringBuilder allDigits = new StringBuilder();
				foreach (char c in phone)
				{
					if (Char.IsDigit(c))
						allDigits.Append(c);
				}
				string phoneDigits = allDigits.ToString();
				if (phoneDigits.Length == 10)
				{
					switch (format)
					{
						case PhoneFormats.OnlyNumbers:
							return allDigits.ToString();
						case PhoneFormats.Dashes:
							return phoneDigits.Substring(0, 3) + '-' + phoneDigits.Substring(3, 3) + '-' + phoneDigits.Substring(6);
						case PhoneFormats.ParenthesisAndDash:
							return '(' + phoneDigits.Substring(0, 3) + ") " + phoneDigits.Substring(3, 3) + '-' + phoneDigits.Substring(6);
					}
				}
			}
			return String.Empty;
		}

		/// <summary>
		/// Count occurrences of strings.
		/// </summary>
		public static int CountStringOccurrences(string text, string pattern)
		{
			// Loop through all instances of the string 'text'.
			int count = 0;
			int i = 0;
			while ((i = text.IndexOf(pattern, i)) != -1)
			{
				i += pattern.Length;
				count++;
			}
			return count;
		}

		/// <summary>
		/// Capitalizes a string.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="cultureInfo"></param>
		/// <returns></returns>
		public static string Capitalize(this string source, CultureInfo cultureInfo)
		{
			string result = source;

			if (!String.IsNullOrEmpty(source))
			{
				char[] chars = source.ToCharArray();
				string firstLetter = String.Format("{0}", chars[0]).ToUpper(cultureInfo);

				if (result.Length > 1)
				{
					result = String.Concat(firstLetter, source.Substring(1));
				}
				else
				{
					result = firstLetter;
				}
			}

			return result;
		}

		/// <summary>
		/// Capitalizes a string.
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static string Capitalize(this string source)
		{
			string result = source;

			if (!String.IsNullOrEmpty(source))
			{
				char[] chars = source.ToCharArray();
				string firstLetter = String.Format("{0}", chars[0]).ToUpper();

				if (result.Length > 1)
				{
					result = String.Concat(firstLetter, source.Substring(1));
				}
				else
				{
					result = firstLetter;
				}
			}

			return result;
		}

		/// <summary>
		/// Capitalizes a string.
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static string CapitalizeInvariant(this string source)
		{
			string result = source;

			if (!String.IsNullOrEmpty(source))
			{
				char[] chars = source.ToCharArray();
				string firstLetter = String.Format("{0}", chars[0]).ToUpperInvariant();

				if (result.Length > 1)
				{
					result = String.Concat(firstLetter, source.Substring(1));
				}
				else
				{
					result = firstLetter;
				}
			}

			return result;
		}

		/// <summary>
		/// Ensures that a string has proper case.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="cultureInfo"></param>
		/// <returns></returns>
		public static string CapitalizeAndProperCase(this string source, CultureInfo cultureInfo)
		{
			string result = source;
			string[] words = result.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

			for (int wordIndex = 0; wordIndex < words.Length; wordIndex++)
			{
				words[wordIndex] = Capitalize(words[wordIndex].ToLower(), cultureInfo);
			}

			result = String.Join(" ", words);

			return result;
		}

		/// <summary>
		/// Ensures that a string has proper case.
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static string CapitalizeAndProperCase(this string source)
		{
			string result = source;
			string[] words = result.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

			for (int wordIndex = 0; wordIndex < words.Length; wordIndex++)
			{
				words[wordIndex] = Capitalize(words[wordIndex].ToLower());
			}

			result = String.Join(" ", words);

			return result;
		}

		/// <summary>
		/// Ensures that a string has proper case.
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static string CapitalizeAndProperCaseInvariant(this string source)
		{
			string result = source;
			string[] words = result.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

			for (int wordIndex = 0; wordIndex < words.Length; wordIndex++)
			{
				words[wordIndex] = CapitalizeInvariant(words[wordIndex].ToLower());
			}

			result = String.Join(" ", words);

			return result;
		}
	}
}
