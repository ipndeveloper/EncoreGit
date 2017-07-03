using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using NetSteps.Silverlight.Extensions;

namespace NetSteps.Silverlight
{
    public static partial class Utilities
    {
        public static class String
        {
            public static string TruncateString(string text, int numberOfChar, bool addTrailingDots)
            {
                string newString = string.Empty;
                text = text.Trim();

                if (text.Length > numberOfChar)
                    newString = text.Substring(0, numberOfChar);
                else
                    newString = text;

                if (addTrailingDots && text.Length > numberOfChar)
                    newString += "...";

                return newString;
            }

            /// <summary>
            /// Takes a Pascal CASED string and inserts spaces:
            /// Example: "PascalCaseString" becomes "Pascal Case String" - JHE
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public static string PascalToSpaced(string name)
            {
                // ignore missing text
                if (string.IsNullOrEmpty(name))
                    return string.Empty;
                // split the words
                Regex regex = new Regex("(?<=[a-z])(?<x>[A-Z])|(?<=.)(?<x>[A-Z])(?=[a-z])");
                name = regex.Replace(name, " ${x}");
                // get rid of any underscores or dashes
                name = name.Replace("_", string.Empty);
                return name.Replace("-", string.Empty);
            }

            /// <summary>
            /// Return the camel-case representation of the specified string value.
            /// </summary>
            /// <param name="value">The value.</param>
            /// <returns></returns>
            public static string ToCamelCase(string value)
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                char[] chars = value.ToCharArray();
                int i = 0;

                while (i < chars.Length && char.IsUpper(chars[i]))
                {
                    chars[i] = char.ToLower(chars[i++], CultureInfo.InvariantCulture);
                }

                return new string(chars);
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
        }
    }
}
