using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media.Imaging;

namespace NetSteps.Silverlight.Extensions
{
    public static class StringExtensions
    {
        #region Validation Methods
        public static bool IsNullOrEmpty(this string value)
        {
            if (value != null)
                return string.IsNullOrEmpty(value.Trim());
            else
                return string.IsNullOrEmpty(value);
        }

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
        #endregion

        #region Conversion Methods
        public static int ToInt(this string value)
        {
            if (value.IsValidInt())
                return Convert.ToInt32(value);
            else
                return 0;
        }

        public static int? ToIntNullable(this string value)
        {
            value = value.ToCleanString();
            if (value.IsNullOrEmpty())
                return null;
            else
                return Convert.ToInt32(value);
        }

        public static bool ToBool(this string value)
        {
            string boolValue = value.ToCleanString().ToLower();
            if (boolValue == "1" || boolValue == "true" || boolValue == "t" || boolValue == "yes" || boolValue == "y")
                return true;
            else
                return false;
        }

        public static bool? ToNullableBool(this string value)
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

        public static Visibility ToVisibility(this string value)
        {
            string boolValue = value.ToString().ToLower();
            if (boolValue == "1" || boolValue == "true" || boolValue == "t" || boolValue == "yes" || boolValue == "visible")
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public static List<string> ToStringList(this string value)
        {
            return Utilities.String.GetFromStringList<string>(value, ',');
        }

        public static List<string> ToStringList(this string value, char separatingCharacter)
        {
            return Utilities.String.GetFromStringList<string>(value, separatingCharacter);
        }

        public static string ToSafeString(this string value)
        {
            return (value == null) ? string.Empty : value.Trim();
        }

        public static BitmapImage ToImageSource(this string value)
        {
            if (value != null)
            {
                return new BitmapImage(ApplicationContext.ResolveUrlWithXapAsBase(value));
            }
            else
                return null;
        }

        public static string ToCleanString(this string value)
        {
            return (value == null) ? string.Empty : value.Trim();
        }

        public static decimal ToDecimal(this string value)
        {
            value = value.ToCleanString();
            if (value.IsNullOrEmpty())
                return 0;
            else
                return Convert.ToDecimal(value);
        }

        public static decimal? ToDecimalNullable(this string value)
        {
            value = value.ToCleanString();
            if (value.IsNullOrEmpty())
                return null;
            else
                return Convert.ToDecimal(value);
        }
        #endregion

        /// <summary>
        /// Converts a string to title case.
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <returns>A string.</returns>
        public static string TitleCase(this string text)
        {
            return TitleCase(text, true);
        }
        /// <summary>
        /// Converts a string to title case.
        /// </summary>
        /// <param name="input">The string to convert.</param>
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

        public static bool ContainsIgnoreCase(this string value, string stringValue)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(stringValue))
                return false;
            return value.ToUpper().Contains(stringValue.ToUpper());
        }

        public static string ReplaceIgnoreCase(this string value, string oldValue, string newValue)
        {
            return Regex.Replace(value, Regex.Escape(oldValue), newValue, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Takes a Pascal CASED string and inserts spaces:
        /// Example: "PascalCaseString" becomes "Pascal Case String" - JHE
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string PascalToSpaced(this string value)
        {
            return Utilities.String.PascalToSpaced(value);
        }

        public static string RemoveSpaces(this string value)
        {
            if (value != null)
                return value.Replace(" ", string.Empty);
            else
                return value;
        }

        public static string Remove(this string value, string valueToRemove)
        {
            if (value != null)
                return value.Replace(valueToRemove, string.Empty);
            else
                return value;
        }

        /// <summary>
        /// Trims leading and trailing character provided by parameter. - JHE
        /// </summary>
        /// <param name="value"></param>
        /// <param name="leadin"></param>
        /// <returns></returns>
        public static string Trim(this string value, string character)
        {
            value = (value == null) ? string.Empty : value.Trim();
            if (value.Length >= 1)
            {
                if (value.Substring(0, 1) == character.ToString())
                    value = value.Substring(1, value.Length - 1);

                if (value.Length >= 1)
                    if (value.Substring(value.Length - 1) == character.ToString())
                        value = value.Substring(0, value.Length - 1);
            }
            return value.Trim();
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
        /// <param name="input">The string whose 
        /// values should be replaced.</param>
        /// <returns>A string.</returns>
        public static string StripTags(this string text)
        {
            if (text == null)
                return string.Empty;

            Regex stripTags = new Regex("<(.|\n)+?>");
            return stripTags.Replace(text, string.Empty).Trim();
        }
        #endregion


        public static string GetFileExtention(this string filePath)
        {
            string fileExtention = string.Empty;
            if (filePath.Trim().Length >= 4)
                fileExtention = filePath.ToUpper().Substring(filePath.Length - 4, 4);

            return fileExtention.Replace(".", string.Empty);
        }

    }
}
