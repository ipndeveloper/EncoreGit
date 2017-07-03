using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using NetSteps.Common.Configuration;
using NetSteps.Common.EldResolver;
using NetSteps.Common.Serialization;

namespace NetSteps.Common.Extensions
{
    /// <summary>
    /// Author: John Egbert
    /// Description: String Extensions
    /// Created: 11-01-2008
    /// </summary>
    public static class StringExtensions
    {
        #region Validation Methods
        public static bool IsValidInt(this string integer)
        {
            int unused;
            return int.TryParse(integer, out unused);
        }

        public static bool IsValidLong(this string integer)
        {
            long unused;
            return long.TryParse(integer, out unused);
        }

        public static bool IsValidShort(this string integer)
        {
            short unused;
            return short.TryParse(integer, out unused);
        }

        public static bool IsValidDateTime(this string dateTime)
        {
            DateTime date;
            return DateTime.TryParse(dateTime, out date);
        }

        public static bool IsValidEmail(this string email)
        {
            return Regex.IsMatch(email, RegularExpressions.Email);
        }

        public static bool IsValidTime(this string time)
        {
            DateTime newDateTime = DateTime.Now.ApplicationNow();
            string dateTime = string.Format("{0} {1}", newDateTime.ToShortDateString(), time);
            return DateTime.TryParse(dateTime, out newDateTime);
        }

        public static bool IsNullOrEmpty(this string value)
        {
            string val = (value == null) ? string.Empty : value.Trim();
            return string.IsNullOrEmpty(val);
        }

        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
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
            return input != null && input.Length == 0;
        }

        /// <summary>
        /// true, if the string contains only digits or float-point.
        /// Spaces are not considered.
        /// </summary>
        /// <param name="s">input string</param>
        /// <param name="floatpoint">true, if float-point is considered</param>
        /// <returns>true, if the string contains only digits or float-point</returns>
        public static bool IsNumberOnly(this string s, bool floatpoint)
        {
            if (s == null) return false;
            s = s.Trim();
            if (s.Length == 0)
                return false;
            return Regex.IsMatch(s, @"^[\d" + (floatpoint ? @"\.\," : "") + "]*$");
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

            if (input.Length > 0)
                return String.Compare(input, input.ToUpper(), false) == 0;

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
            return input.Replace(input.Substring(0, 1), string.Empty).Length == 0;
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
            return input.Replace(" ", string.Empty).Length == 0;
        }

        public static bool IsRegexMatch(this string value, string regularExpression)
        {
            return Regex.IsMatch(value, regularExpression);
        }
        #endregion

        #region Conversion Methods
        public static int ToInt(this string value)
        {
            return value.IsValidInt() ? Convert.ToInt32(value) : 0;
        }

        public static int? ToIntNullable(this string value)
        {
            value = value.ToCleanString();
            return value.IsNullOrEmpty() ? (int?)null : Convert.ToInt32(value);
        }

        public static short ToShort(this string value)
        {
            return value.IsValidInt() ? Convert.ToInt16(value) : (short)0;
        }

        public static long ToLong(this string value)
        {
            return value.IsValidInt() ? Convert.ToInt64(value) : 0;
        }

        public static bool ToBool(this string value)
        {
            string boolValue = value.ToCleanString().ToLower();
            if (boolValue == "1" || boolValue == "true" || boolValue == "t" || boolValue == "y" || boolValue == "yes")
                return true;
            else
                return false;
        }

        public static bool? ToBoolNullable(this string value)
        {
            string boolValue = string.Empty;
            if (value.IsNullOrEmpty())
                return null;
            else
                boolValue = value.ToCleanString().ToLower();

            if (boolValue == "1" || boolValue == "true" || boolValue == "t" || boolValue == "yes" || boolValue == "y")
                return true;
            else
                return false;
        }

        public static List<string> ToStringList(this string value)
        {
            return value.Split(',').ToList();
        }

        public static List<string> ToStringList(this string value, char separatingCharacter)
        {
            return value.Split(separatingCharacter).ToList();
        }

        /// <summary>
        /// Example: List{string} filterOptions = Utilities.String.GetStringList("All, A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z", Char.Parse(","));
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

            string[] array = commaSeparatedValues.Split(separatingCharacter);

            for (int i = 0; i < array.Length; i++)
            {
                string objectStringValue = array[i].Trim();
                if (objectType.IsEnum)
                    result = (T)Enum.Parse(objectType, objectStringValue, true);
                else
                {
                    TypeConverter converter = TypeDescriptor.GetConverter(objectType);
                    if (converter.CanConvertFrom(typeof(string)))
                        result = (T)converter.ConvertFrom(objectStringValue);
                }
                //if (obj is Boolean)
                //{
                //    string boolValue = objectStringValue.ToLower();
                //    if (boolValue.ToBool())
                //        result = (T)Convert.ChangeType(true, objectType, null);
                //    else
                //        result = (T)Convert.ChangeType(false, objectType, null);
                //}
                //else
                //    result = (T)Convert.ChangeType(objectStringValue, objectType, null);

                returnValue.Add(result);
            }

            return returnValue;
        }

        public static string ToCleanString(this string value)
        {
            return (value == null) ? string.Empty : value.Trim();
        }

        public static string ToCleanStringNullable(this string value)
        {
            value = (value == null) ? string.Empty : value.Trim();
            return (string.IsNullOrEmpty(value)) ? null : value;
        }

        public static decimal ToDecimal(this string value)
        {
            value = value.ToCleanString();
            return value.IsNullOrEmpty() ? 0 : Convert.ToDecimal(value);
        }

        public static decimal? ToDecimalNullable(this string value)
        {
            value = value.ToCleanString();
            return value.IsNullOrEmpty() ? (decimal?)null : Convert.ToDecimal(value);
        }

        public static T ToEnum<T>(this string value, T defaultValue) where T : struct
        {
            Type type = typeof(T);
            if (!type.IsEnum)
                throw new ArgumentException("T", "Type must be an enum");
            try
            {
                T result = default(T);

                if (Enum.TryParse<T>(value, true, out result))
                    return result;
                else
                    return defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        public static DateTime? ToDateTimeNullable(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            else
            {
                DateTime parsedDateTime = new DateTime();
                return DateTime.TryParse(value, out parsedDateTime) ? parsedDateTime : (DateTime?)null;
            }
        }

        public static Int64 ToBigInt(this string value)
        {
            return value.IsValidLong() ? Convert.ToInt64(value) : 0;
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

        public static string RemoveFileExtension(this string value)
        {
            if (value != null)
                return value = value.Remove(value.LastIndexOf('.'), value.Length - value.LastIndexOf('.'));
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

        public static string TrimEnd(this string text, string trimString)
        {
            return text.TrimEnd(trimString.ToCharArray());
        }

        /// <summary>
        /// Replaces http:// with https://
        /// TODO: This method should not take a bool condition. - Lundy
        /// </summary>
        /// <param name="value"></param>
        /// <param name="forceSSL"> </param>
        /// <returns></returns>
        public static string ConvertToSecureUrl(this string value, bool forceSSL)
        {
            if (value.Contains("http://") && forceSSL)
                return value.Replace("http://", "https://");
            return value;
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
            catch { return string.Empty; }
        }
        public static string ProperCaseIfUpper(this string value)
        {
            return (value != value.ToUpper()) ? value : value.ProperCase();
        }

        /// <summary>
        /// Takes a Pascal CASED string and inserts spaces:
        /// Example: "PascalCaseString" becomes "Pascal Case String" - JHE
        /// </summary>
        /// <param name="name"> </param>
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

        public static string ToPascalCase(this string text)
        {
            if (text.ToCleanString().IsNullOrEmpty())
                return string.Empty;
            var list = text.ToCleanString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (list.Count() > 1)
                return list.ToString(i => i[0].ToString().ToUpper() + i.ToLower().Substring(1) + " ").Trim();
            else
                return text.ToCleanString();
        }

        public static string ToTitleCase(this string text)
        {
            if (text.IsNullOrEmpty())
                return text;

            string returnString = string.Empty;
            string[] arrayOfText = text.Trim().Split(' ');

            foreach (string item in arrayOfText)
            {
				if (string.IsNullOrEmpty(item))
				{
					continue;
				}
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

        /// <summary>
        /// Converts a string to title case.
        /// </summary>
        /// <param name="text"> </param>
        /// <returns>A string.</returns>
        public static string TitleCase(this string text)
        {
            return TitleCase(text, true);
        }

        /// <summary>
        /// Converts a string to title case.
        /// </summary>
        /// <param name="text"> </param>
        /// <param name="ignoreShortWords">If true, 
        /// does not capitalize words like
        /// "a", "is", "the", etc.</param>
        /// <returns>A string.</returns>
        public static string TitleCase(this string text, bool ignoreShortWords)
        {
            List<string> ignoreWords = null;
            if (ignoreShortWords)
            {
                // Add more ignore words?
                ignoreWords = new List<string>();
                ignoreWords.Add("a");
                ignoreWords.Add("is");
                ignoreWords.Add("was");
                ignoreWords.Add("the");
            }

            string[] tokens = text.Split(' ');
            StringBuilder sb = new StringBuilder(text.Length);
            foreach (string s in tokens)
            {
                if (ignoreShortWords == true
                    && s != tokens[0]
                    && ignoreWords.Contains(s.ToLower()))
                {
                    sb.Append(s + " ");
                }
                else
                {
                    sb.Append(s[0].ToString().ToUpper());
                    sb.Append(s.Substring(1).ToLower());
                    sb.Append(" ");
                }
            }

            return sb.ToString().Trim();
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

        public static bool ContainsIgnoreCase(this string original, string value)
        {
            if (original == null && value == null)
                return true;
            if (original == null || value == null)
                return false;
            return original.IndexOf(value, StringComparison.OrdinalIgnoreCase) != -1;
        }

        public static bool EqualsIgnoreCase(this string original, string value)
        {
            if (original == null && value == null)
                return true;
            if (original == null || value == null)
                return false;
            return original.Equals(value, StringComparison.OrdinalIgnoreCase);
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
                string phoneDigits = Regex.Replace(phone, @"\D", "");
                if (phoneDigits.Length == 10)
                {
                    switch (format)
                    {
                        case PhoneFormats.OnlyNumbers:
                            return phoneDigits;
                        case PhoneFormats.Dashes:
                            return phoneDigits.Substring(0, 3) + '-' + phoneDigits.Substring(3, 3) + '-' + phoneDigits.Substring(6);
                        case PhoneFormats.ParenthesisAndDash:
                            return '(' + phoneDigits.Substring(0, 3) + ") " + phoneDigits.Substring(3, 3) + '-' + phoneDigits.Substring(6);
                    }
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// http://prettycode.org/ - JHE
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static string XmlSanitizedString(this string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return xml;
            }

            var buffer = new StringBuilder(xml.Length);

            foreach (char c in xml)
            {
                if (IsLegalXmlChar(c))
                {
                    buffer.Append(c);
                }
            }

            return buffer.ToString();
        }

        public static string MaskString(this string value, int numberOfUnmaskedChars)
        {
            string maskedString = string.Empty;

            if (!value.IsNullOrEmpty())
            {
                for (int i = 0; i < value.Length; i++)
                {
                    if ((i + numberOfUnmaskedChars) == value.Length)
                    {
                        maskedString += value.Substring(value.Length - numberOfUnmaskedChars, numberOfUnmaskedChars);
                        return maskedString;
                    }
                    else
                        maskedString += "*";
                }
            }
            return maskedString;
        }

        /// <summary>
        /// Trims leading and trailing character provided by parameter. - JHE
        /// </summary>
        /// <param name="value"></param>
        /// <param name="characters"> </param>
        /// <returns></returns>
        public static string Trim(this string value, params string[] characters)
        {
            if (characters == null || characters.Length == 0 || string.IsNullOrEmpty(value))
                return value;
            var chars = characters.Where(c => !string.IsNullOrEmpty(c)).Select(c => c[0]);
            return value.Trim().TrimStart(chars.ToArray()).TrimEnd(chars.ToArray());
        }

        public static string GetFileExtention(this string filePath)
        {
            string fileExtention = string.Empty;
            if (filePath.IndexOf('.') >= 0)
                fileExtention = filePath.Substring(filePath.LastIndexOf('.') + 1);

            return fileExtention;
        }

        #region HTML Extensions
        public static string SpaceToNbsp(this string text)
        {
            return text.Replace(" ", "&nbsp;");
        }
        public static string NbspToSpace(this string text)
        {
            return text.Replace("&nbsp;", " ");
        }

        public static string StripNewLine(this string text)
        {
            return text.Replace("\r\n", string.Empty).Replace("\n", string.Empty).Replace("\r", string.Empty);
        }
        public static string NewLineToBR(this string text)
        {
            return text.Replace("\r\n", "<br />").Replace("\n", "<br />");
        }

        /// <summary>
        /// Removes all HTML tags from the passed string
        /// </summary>
        /// <returns>A string.</returns>
        public static string StripTags(this string text)
        {
            if (text == null)
                return string.Empty;

            Regex stripTags = new Regex("<(.|\n)+?>");
            return stripTags.Replace(text, string.Empty).Trim();
        }
        #endregion

        public static bool StartsWithAny(this string text, string[] values)
        {
            return values.Any(v => text.StartsWith(v));
        }

        public static string ResolveUrl(this string url)
        {
            if (url == null)
                return null;

            if (url.IndexOf("://") != -1)
                return url;

            if (url.StartsWith("~"))
            {
                if (System.Web.HttpContext.Current != null)
                {
                    return System.Web.HttpContext.Current.Request.ApplicationPath + (System.Web.HttpContext.Current.Request.ApplicationPath.EndsWith("/") ? "" : "/") + url.Substring(2);
                }
                else
                    throw new ArgumentException("Invalid URL: Relative URL not allowed.");
            }
            return url;
        }

        /// <summary>
        /// Used to check against weak passwords. 
        /// For input: 'secret' it returns '123456' - JHE
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string GetIncrementalNumberStringEquivalent(this string text)
        {
            string returnValue = string.Empty;
            for (int i = 1; i <= text.Length; i++)
                returnValue += i.ToString();
            return returnValue;
        }

        public static List<int> AllIndexesOf(this string input, string value)
        {
            int index = input.IndexOf(value);
            List<int> indexes = new List<int>();
            while (index > -1)
            {
                indexes.Add(index);
                index = input.IndexOf(value, index + 1);
            }
            return indexes;
        }

        public static List<int> AllIndexesOf(this string input, char value)
        {
            return input.AllIndexesOf(value.ToString());
        }

        public static XElement ToXElement(this string text)
        {
            try
            {
                return XElement.Parse(text);
            }
            catch
            {
                return null;
            }
        }

        public static string AppendBackSlash(this string value)
        {
            return value.EndsWith("\\") ? value : value + "\\";
        }

        public static string AppendForwardSlash(this string value)
        {
            return value.EndsWith("/") ? value : value + "/";
        }

        public static string Truncate(this string value, int length, bool appendTrailingDots = false)
        {
            value = value.ToCleanString();
            if (value.Length > length)
            {
                if (appendTrailingDots)
                    return value.Substring(0, length - 1) + "...";
                else
                    return value.Substring(0, length - 1);
            }
            else
                return value;
        }

        public static Common.Constants.FileType GetFileType(this string path)
        {
            string extension = System.IO.Path.GetExtension(path).ToLower();

            switch (extension)
            {
                case ".pdf":
                    return Constants.FileType.PDF;
                case ".abm":
                case ".afx":
                case ".ai":
                case ".art":
                case ".arw":
                case ".bmp":
                case ".cdr":
                case ".cgm":
                case ".cpt":
                case ".cr2":
                case ".cvx":
                case ".dcm":
                case ".dib":
                case ".dng":
                case ".drw":
                case ".dt2":
                case ".emf":
                case ".emz":
                case ".eps":
                case ".fxg":
                case ".gif":
                case ".hdp":
                case ".ipx":
                case ".itc2":
                case ".jpeg":
                case ".jpg":
                case ".jpx":
                case ".max":
                case ".mng":
                case ".png":
                case ".ppm":
                case ".ps":
                case ".psp":
                case ".srf":
                case ".svg":
                case ".svgz":
                case ".tga":
                case ".thm":
                case ".tif":
                case ".vsd":
                case ".wb1":
                case ".wbc":
                case ".wbd":
                case ".xar":
                case ".xcf":
                    return Constants.FileType.Image;
                case ".aa":
                case ".aa3":
                case ".aac":
                case ".acm":
                case ".afc":
                case ".aif":
                case ".at3":
                case ".caf":
                case ".cpr":
                case ".dcf":
                case ".dmsa":
                case ".dmse":
                case ".dss":
                case ".emp":
                case ".emx":
                case ".flac":
                case ".iff":
                case ".m4a":
                case ".m4b":
                case ".m4r":
                case ".mp3":
                case ".mpa":
                case ".nra":
                case ".ogg":
                case ".omf":
                case ".pcast":
                case ".ptf":
                case ".ra":
                case ".ram":
                case ".rns":
                case ".sib":
                case ".snd":
                case ".vpm":
                case ".wav":
                case ".wma":
                    return Constants.FileType.Audio;
                case ".3g2":
                case ".3gp":
                case ".asf":
                case ".asx":
                case ".avi":
                case ".bdm":
                case ".bsf":
                case ".cpi":
                case ".divx":
                case ".dmsm":
                case ".dream":
                case ".dvdmedia":
                case ".dvr-ms":
                case ".f4v":
                case ".fbr":
                case ".hdmov":
                case ".imovieproj":
                case ".m4v":
                case ".mkv":
                case ".mod":
                case ".moi":
                case ".mov":
                case ".mp4":
                case ".mpeg":
                case ".mpg":
                case ".mts":
                case ".mxf":
                case ".ogm":
                case ".psh":
                case ".rcproject":
                case ".rm":
                case ".rmvb":
                case ".scm":
                case ".smil":
                case ".srt":
                case ".stx":
                case ".tix":
                case ".trp":
                case ".ts":
                case ".vob":
                case ".vro":
                case ".wmv":
                case ".wtv":
                case ".xvid":
                case ".yuv":
                    return Constants.FileType.Video;
                case ".flv":
                case ".swf":
                    return Constants.FileType.Flash;
                case ".doc":
                case ".docx":
                case ".odt":
                case ".rtf":
                case ".txt":
                case ".wpd":
                case ".wps":
                    return Constants.FileType.Word;
                case ".csv":
                case ".ods":
                case ".tsv":
                case ".xls":
                case ".xlsx":
                    return Constants.FileType.Excel;
                case ".odp":
                case ".ppt":
                case ".pptx":
                    return Constants.FileType.Powerpoint;
                case ".7z":
                case ".cab":
                case ".deb":
                case ".gz":
                case ".pkg":
                case ".rar":
                case ".rpm":
                case ".sea":
                case ".sfx":
                case ".sit":
                case ".sitx":
                case ".tar":
                case ".tar.gz":
                case ".tgz":
                case ".war":
                case ".zip":
                case ".zipx":
                    return Constants.FileType.Archive;
                default:
                    return Constants.FileType.Unknown;
            }
        }

        public static string EnsurePrepend(this string value, string prependValue)
        {
            value = value.ToCleanString();
            if (!value.StartsWith(prependValue))
                return prependValue + value;
            else
                return value;
        }

        public static string AddFileUploadPathToken(this string url)
        {
            if (string.IsNullOrEmpty(url))
                return url;
            return url.Replace(ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.FileUploadAbsolutePath).AppendBackSlash(), "<!--filepath-->");
        }

        public static string ReplaceFileUploadPathToken(this string url)
        {
            if (string.IsNullOrEmpty(url))
                return url;
            return Regex.Replace(url.Replace("<!--filepath-->", ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.FileUploadAbsoluteWebPath).AppendForwardSlash()).Replace("\\", "/"), "([^:])//", "$1/");
        }

        public static string ReplaceXMLFileUploadPathToken(this string xmlContent)
        {
            if (xmlContent.IsNullOrWhiteSpace())
                return string.Empty;

            return xmlContent.ToXElement().Descendant("Src").Value.ReplaceFileUploadPathToken();
        }

        public static string RemoveFileUploadToken(this string url)
        {
            var retVal = Regex.Replace(url.Replace("\\", "/").Replace("<!--filepath-->", "/FileUploads/"), "([^:])//", "$1/");
            return retVal;
        }

        public static string WebUploadPathToAbsoluteUploadPath(this string url)
        {
            if (string.IsNullOrEmpty(url))
                return url;
            return url.Replace(ConfigurationManager.FileUploadWebPath, ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.FileUploadAbsolutePath).AppendBackSlash()).Replace("/", "\\");
        }

        public static string AbsoluteUploadPathToWebUploadPath(this string url)
        {
            if (string.IsNullOrEmpty(url))
                return url;
            return url.Replace(ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.FileUploadAbsolutePath).AppendBackSlash(), ConfigurationManager.FileUploadWebPath).Replace("\\", "/");
        }

        public static string AbsoluteUploadPathToAbsoluteWebUploadPath(this string url)
        {
            if (string.IsNullOrEmpty(url))
                return url;
            return url.Replace(ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.FileUploadAbsolutePath).AppendBackSlash(), ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.FileUploadAbsoluteWebPath).AppendForwardSlash()).Replace("\\", "/");
        }

        public static string AddAbsoluteUploadPath(this string filename, params string[] folders)
        {
            return ConfigurationManager.GetAbsoluteUploadPath(folders) + filename;
        }

        public static string RemoveAbsoluteUploadPath(this string filePath, params string[] folders)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return filePath;

            return filePath.Replace(ConfigurationManager.GetAbsoluteUploadPath(folders), string.Empty);
        }

        public static string AddWebUploadPath(this string filename, params string[] folders)
        {
            return ConfigurationManager.GetWebUploadPath(folders) + filename;
        }

        public static string RemoveWebUploadPath(this string url, params string[] folders)
        {
            if (string.IsNullOrWhiteSpace(url))
                return url;

            return url.Replace(ConfigurationManager.GetWebUploadPath(folders), string.Empty);
        }


        public static string RemoveNonNumericCharacters(this string text)
        {
            if (text.IsNullOrEmpty())
                return string.Empty;

            return Regex.Replace(text.ToCleanString(), @"\D", string.Empty);
        }

        /// <summary>
        /// Deserialize the json text to the generic type
        /// </summary>
        public static T FromJson<T>(this string jsonText, IEnumerable<Type> knownTypes = null)
        {
            return JsonSerializationHelper.Deserialize<T>(jsonText, knownTypes);
        }

        public static string ZipCode(this string zip)
        {
            if (zip.Length <= 5)
                return zip;
            else
                return string.Format("{0}-{1}", zip.Substring(0, 5), zip.Substring(5));
        }

        /// <summary>
        /// Retrieves a substring without throwing an exception if the original string is too short.
        /// </summary>
        public static string SubstringSafe(this string original, int startIndex, int length)
        {
            if (string.IsNullOrEmpty(original)
                || startIndex >= original.Length)
            {
                return string.Empty;
            }

            if ((startIndex + length) > original.Length)
            {
                return original.Substring(startIndex);
            }

            return original.Substring(startIndex, length);
        }

        public static string GetURLScheme(this string url)
        {
            var match = Regex.Match(url, RegularExpressions.URL);
            return match.Groups["scheme"].Success ? match.Groups["scheme"].Value : "";
        }

        public static string GetURLAuthority(this string url)
        {
            var match = Regex.Match(url, RegularExpressions.URL);
            return match.Groups["authority"].Success ? match.Groups["authority"].Value : "";
        }

        public static string GetURLDomain(this string url)
        {
            var authority = url.GetURLAuthority();
            List<int> allPeriods = authority.AllIndexesOf(".");
            if (allPeriods.Count > 1)
            {
                return authority.Substring(allPeriods[allPeriods.Count - 2] + 1);
            }
            return authority;
        }

        public static string GetURLSubdomain(this string url)
        {
            var authority = url.GetURLAuthority();
            List<int> allPeriods = authority.AllIndexesOf(".");
            if (allPeriods.Count > 1)
            {
                return authority.Substring(0, allPeriods[allPeriods.Count - 2]);
            }
            return "";
        }

        public static string GetURLPath(this string url)
        {
            var match = Regex.Match(url, RegularExpressions.URL);
            return match.Groups["path"].Success ? match.Groups["path"].Value : "";
        }

        public static string GetURLQueryString(this string url)
        {
            var match = Regex.Match(url, RegularExpressions.URL);
            return match.Groups["query"].Success ? match.Groups["query"].Value : "";
        }

        public static string GetURLFragment(this string url)
        {
            var match = Regex.Match(url, RegularExpressions.URL);
            return match.Groups["fragment"].Success ? match.Groups["fragment"].Value : "";
        }

        public static string ToCellString(this string value, string cssClass = "", string style = "", int? columnSpan = null)
        {
            return string.Format("<td{0}{1}{2}>{3}</td>",
                !string.IsNullOrEmpty(cssClass) ? " class=\"" + cssClass + "\"" : cssClass,
                !string.IsNullOrEmpty(style) ? " style=\"" + style + "\"" : style,
                (columnSpan != null) ? " colspan=\"" + columnSpan.ToString() + "\"" : string.Empty,
                value);
        }

        /// <summary>
        /// Gets the effective site url from the given url.
        /// </summary>
        /// <param name="url">
        /// The url from HttpContext.Request.
        /// </param>
        /// <returns>
        /// Returns the effective site url.
        /// </returns>
        public static string GetSiteUrl(this string url)
        {
            var strippedUri = new Uri(url.EldDecode());
            return string.Format("{0}://{1}/", strippedUri.Scheme, strippedUri.Authority);
        }
		
        public static string ToJavaScriptStringEncode(this string value)
        {
            return System.Web.HttpUtility.JavaScriptStringEncode(value);
        }

        /// <summary>
        /// Whether a given character is allowed by XML 1.0.
        /// </summary>
        private static bool IsLegalXmlChar(int character)
        {
            return
            (
                 character == 0x9 /* == '\t' == 9   */        ||
                 character == 0xA /* == '\n' == 10  */        ||
                 character == 0xD /* == '\r' == 13  */        ||
                (character >= 0x20 && character <= 0xD7FF) ||
                (character >= 0xE000 && character <= 0xFFFD) ||
                (character >= 0x10000 && character <= 0x10FFFF)
            );
        }

    }
}
