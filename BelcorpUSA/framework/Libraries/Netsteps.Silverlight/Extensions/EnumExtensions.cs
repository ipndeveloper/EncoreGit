using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NetSteps.Silverlight.Extensions
{
    public class EnumNameValue
    {
        public string Name { get; set; }
        public int Value { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public static class EnumExtensions
    {
        /// <summary>
        /// Takes a Pascal CASED string and inserts spaces:
        /// Example: "PascalCaseString" becomes "Pascal Case String" - JHE
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string PascalToSpaced(this Enum value)
        {
            return Utilities.String.PascalToSpaced(value.ToString());
        }

        /// <summary>
        /// To return the integer value of the Enum as a string. - JHE
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToIntString(this Enum value)
        {
            return Convert.ToInt32(value).ToString();
        }

        /// <summary>
        /// To return the integer value of an Enum. - JHE
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt(this Enum value)
        {
            return Convert.ToInt32(value);
        }

        public static IList<T> GetValues<T>(this Enum enumeration)
        {
            return GetValues<T>();
        }
        public static IList<T> GetValues<T>()
        {
            Type enumType = typeof(T);
            if (!enumType.IsEnum)
                throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");

            IList<T> values = new List<T>();

            var fields = from field in enumType.GetFields()
                         where field.IsLiteral
                         select field;

            foreach (FieldInfo field in fields)
            {
                object value = field.GetValue(enumType);
                values.Add((T)value);
            }

            return values;
        }

        public static IList<EnumNameValue> GetValuesList<T>(this Enum enumeration)
        {
            return GetValuesList<EnumNameValue>();
        }
        public static IList<EnumNameValue> GetValuesList<T>()
        {
            Type enumType = typeof(T);
            if (!enumType.IsEnum)
                throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");

            IList<EnumNameValue> values = new List<EnumNameValue>();

            var fields = from field in enumType.GetFields()
                         where field.IsLiteral
                         select field;

            foreach (FieldInfo field in fields)
            {
                object value = field.GetValue(enumType);
                values.Add(new EnumNameValue() { Name = ((T)value).ToString().PascalToSpaced(), Value = ((T)value as Enum).ToInt() });
            }

            return values;
        }


        public static T GetRandom<T>(this Enum value, bool excludeFirst)
        {
            IList<T> values = value.GetValues<T>();

            if (excludeFirst)
                return values.GetRandom(1);
            else
                return values.GetRandom(1);
        }
    }
}
