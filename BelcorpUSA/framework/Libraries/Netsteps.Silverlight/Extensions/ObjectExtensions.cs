using System;
using System.Collections.Generic;
using System.Reflection;

namespace NetSteps.Silverlight.Extensions
{
    public static class ObjectExtensions
    {
        public static List<PropertyInfo> FindClassProperties(this object _obj, List<string> propertiesToFind)
        {
            return Reflection.FindClassProperties(_obj.GetType(), propertiesToFind);
        }

        public static List<PropertyInfo> FindClassProperties(this object _obj)
        {
            return Reflection.FindClassProperties(_obj.GetType());
        }

        public static PropertyInfo FindClassProperty(this object _obj, string property)
        {
            return Reflection.FindClassProperty(_obj.GetType(), property);
        }

        #region object
        #region object.ToStringSafe()
        public static string ToStringSafe(this object _obj)
        {
            if (_obj == null)
            {
                return string.Empty;
            }

            try
            {
                return _obj.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
        #endregion object.ToStringSafe()

        //#region object.ToInt()
        //public static int ToInt(this object _obj)
        //{
        //    int retval;
        //    if (int.TryParse(_obj.ToStringSafe(), out retval))
        //    {
        //        return retval;
        //    }

        //    return 0;
        //}
        //#endregion object.ToInt()

        #region object.ToDouble()
        public static double ToDouble(this object _obj)
        {
            double retval;
            if (double.TryParse(_obj.ToStringSafe(), out retval))
            {
                return retval;
            }

            return 0;
        }

        public static double? ToDoubleNullable(this object _obj)
        {
            double retval;
            if (double.TryParse(_obj.ToStringSafe(), out retval))
            {
                return retval;
            }

            return null;
        }
        #endregion object.ToDouble()

        #region object.ToDate()
        public static DateTime ToDate(this object _obj)
        {
            DateTime retval;
            if (DateTime.TryParse(_obj.ToStringSafe(), out retval))
            {
                return retval;
            }

            return DateTime.MinValue;
        }
        #endregion object.ToDate()
        #endregion
    }
}
