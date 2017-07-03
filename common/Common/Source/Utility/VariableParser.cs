using System;
using System.ComponentModel;

namespace NetSteps.Common.Utility
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Helpers class to parse variable to strongly typed values.
	/// Created: 04-19-2010
	/// </summary>
	public class VariableParser
    {
        #region GetVar Overloads
        public static T GetVar<T>(object fromValue)
		{ return GetVar(fromValue, default(T), null); }
		public static T GetVar<T>(object fromValue, Func<T> instantiate)
		{ return GetVar(fromValue, default(T), null); }
		public static T GetVar<T>(object fromValue, T defaultValue)
		{ return GetVar(fromValue, defaultValue, null); }
        #endregion

        public static T GetVar<T>(object fromValue, T defaultValue, Func<T> instantiate)
		{
			Type objectType = typeof(T);
			T result = defaultValue;
			try
			{
				if (fromValue != null)
				{
                    if (result is Enum)
                        result = (T)Enum.Parse(objectType, fromValue.ToString());
                    else
                    {
                        TypeConverter tc = TypeDescriptor.GetConverter(typeof(T));
                        result = (T)tc.ConvertFrom(fromValue);

                        // This works for everything I've been able to test.
                        // If the two lines above fail to perform your conversion
                        // uncomment and modify the example code below, replacing the 
                        // two lines above with a call to Converter.ChangeType<T>(fromValue) - trotter

                        // old version - doesn't work with some types, including nullables
                        //result = (T)Convert.ChangeType(fromValue, objectType);
                    }
				}
                else
                {
                    result = instantiate != null ? instantiate() : defaultValue;
                }

				return result;
			}
			catch
			{
				// Error Probably caused by bad data (returning default data value) - JHE
                return instantiate != null ? instantiate() : defaultValue;
			}
		}
    }

    #region Example code if more advanced conversions are needed
    //public static class Converter
    //{
    //    public static T ChangeType<T>(object value)
    //    {
    //        TypeConverter tc = TypeDescriptor.GetConverter(typeof(T));
    //        return (T)tc.ConvertFrom(value);
    //    }

    //    public static void RegisterTypeConverter<T, TC>() where TC : TypeConverter
    //    {
    //        TypeDescriptor.AddAttributes(typeof(T), new TypeConverterAttribute(typeof(TC)));
    //    }

    //    static Converter()
    //    {
    //        Converter.RegisterTypeConverter<Version, VersionConverter>();
    //    }
    //}

    //class VersionConverter : TypeConverter
    //{
    //    public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
    //    {
    //        string strvalue = value as string;
    //        if (strvalue != null)
    //            return new Version(strvalue); // if valid conversion, perform custom convert

    //        return new Version(); // else return default - null may be more appropriate here for nullable types
    //    }
    //}
    #endregion
}
