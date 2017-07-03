using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.Base
{
	static class IDataReaderExtensions
	{	public static T GetNullable<T>(this IDataRecord record, int index)
		{
			var value = record.GetValue(index);
			return value != DBNull.Value ? (T)Convert.ChangeType(value, typeof(T)) : default(T);
		}

        //R GetValueOrDefault<R>(DbDataReader reader, string column)
        //{
        //    var code = Type.GetTypeCode(typeof (R));
        //    switch (code)
        //    {
        //        case TypeCode.Empty:
        //            break;
        //        case TypeCode.Object:
        //            if (typeof (R).IsArray)
        //            {

        //            }
        //            if (typeof (R) == typeof (Guid))
        //            {

        //            }
        //            if (typeof (R).IsGenericType && typeof (R).GetGenericTypeDefinition() == typeof (Nullable<>))
        //            {
        //                var realType = typeof (R).GetGenericArguments()[0];
        //            }
        //            break;
        //        case TypeCode.DBNull:
        //            break;
        //        case TypeCode.Boolean:
        //            break;
        //        case TypeCode.Char:
        //            break;
        //        case TypeCode.SByte:
        //            break;
        //        case TypeCode.Byte:
        //            break;
        //        case TypeCode.Int16:
        //            return (R) (object) reader.GetInt16(column);
        //            break;
        //        case TypeCode.UInt16:
        //            break;
        //        case TypeCode.Int32:
        //            break;
        //        case TypeCode.UInt32:
        //            break;
        //        case TypeCode.Int64:
        //            break;
        //        case TypeCode.UInt64:
        //            break;
        //        case TypeCode.Single:
        //            break;
        //        case TypeCode.Double:
        //            break;
        //        case TypeCode.Decimal:
        //            break;
        //        case TypeCode.DateTime:
        //            break;
        //        case TypeCode.String:
        //            break;
        //        default:
        //            throw new ArgumentOutOfRangeException();
        //    }
        //}

	}
}
