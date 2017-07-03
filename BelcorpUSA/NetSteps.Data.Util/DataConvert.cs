using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Util
{
    public static class DataConvertDA
    {
        public static Int64 ObjectToInt64(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? 0 : Convert.ToInt64(obj);
        }
        public static Int16 ObjectToInt16(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? Int16.MinValue : Convert.ToInt16(obj);
        }
        public static int ObjectToInt32(object obj)
        {
            int intStrs;
            bool intResultTryParse = int.TryParse(obj.ToString(), out intStrs);

            return ((obj == null) || (obj == DBNull.Value) || intResultTryParse == false) ? 0 : Convert.ToInt32(obj);
        }
        public static int ObjectToInt32Menos(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? -1 : Convert.ToInt32(obj);
        }
        public static int ObjectToInt32MenosVacio(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value) || (obj.ToString() == "")) ? -1 : Convert.ToInt32(obj);
        }
        public static decimal ObjectToDecimal(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? 0.00M : Convert.ToDecimal(obj);
        }
        public static decimal StrToDecimal(string obj)
        {
            return ((obj == null) || (obj == "")) ? 0.00M : Convert.ToDecimal(obj);
        }
        public static int ObjectToInt(object obj)
        {
            return ObjectToInt32(obj);
        }
        public static double ObjectToDouble(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? 0 : Convert.ToDouble(obj);
        }
        public static string ObjectToString(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? "" : Convert.ToString(obj);
        }
        public static DateTime ObjectToDateTime(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? DateTime.MinValue : Convert.ToDateTime(obj);
        }
        public static DateTime? ObjectToDateTimeNull(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? (DateTime?)null : Convert.ToDateTime(obj);
        }
      
        public static byte ObjectToByte(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? byte.MinValue : Convert.ToByte(obj);
        }
        public static int StringToInt(string str)
        {
            return ((str == null) || (str == "")) ? 0 : Convert.ToInt32(str);
        }
        public static Int64 StringToInt64(string str)
        {
            return ((str == null) || (str == "")) ? 0 : Convert.ToInt64(str);
        }
       
        public static string ObjectDecimalToStringFormatMiles(object obj)
        {
            return ObjectToDecimal(obj).ToString("#,#0.00");
        }
        public static string BoolToString(bool flag)
        {
            return flag ? "1" : "0";
        }
        public static bool StringToBool(string flag)
        {
            return flag == "1";
        }
        public static Guid ObjectToGui(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? Guid.Empty : (Guid)(obj);
        }
        public static bool ObjectToBool(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? false : (bool)obj;
        }
        public static bool? ObjectToBoolNull(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? (bool?)null : Convert.ToBoolean(obj);
        }
        public static DateTime? DBNullToDatetimeNull(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? null : (DateTime?)obj;
        }
        public static Int32? ObjectToInt32Null(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? null : new Nullable<Int32>(Convert.ToInt32(obj));
        }
        public static Int64? ObjectToInt64Null(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? null : (Int64?)(obj);
        }
        public static Decimal? ObjectToDecimalNull(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? null : (Decimal?)(obj);
        }
        public static string ObjectToStringNull(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? null : (string)obj;
        }
        public static int ObjectToInt32Custom(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? -100 : Convert.ToInt32(obj);
        }
        public static object IntToDBNull(Int32 obj)
        {
            if (obj == 0 || String.IsNullOrEmpty(obj.ToString()))
            {
                return DBNull.Value;
            }
            else
            {
                return obj;
            }

        }

        public static object decimalToDBNull(decimal obj)
        {
            if (obj == 0 || String.IsNullOrEmpty(obj.ToString()))
            {
                return DBNull.Value;
            }
            else
            {
                return obj;
            }

        }

        public static object StringToDBNull(String obj)
        {
            if (String.IsNullOrEmpty(obj.ToString()))
            {
                return DBNull.Value;
            }
            else
            {
                return obj;
            }

        }
    }

    public static class DataConvertUI
    {
        public static string IntToString(int? int1)
        {
            return ((int1 == 0) || (int1 == null)) ? "" : Convert.ToString(int1);
        }

        public static string StringToShortDate(object dt)
        {
            return ((dt == null) ) ? "" : Convert.ToDateTime(dt).ToShortDateString();
        }
    }
}
