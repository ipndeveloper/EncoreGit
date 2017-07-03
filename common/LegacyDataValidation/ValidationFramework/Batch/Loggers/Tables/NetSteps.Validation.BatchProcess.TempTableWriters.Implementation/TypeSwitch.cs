using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Validation.BatchProcess.TempTableWriters.Implementation
{
    internal class TypeSwitch
    {
        internal TypeSwitch(Type type)
        {
            TargetType = type;
            SqlType = null;
        }

        public string SqlType { get; private set; }

        public Type TargetType { get; private set; }

        internal string Default(string sqlType)
        {
            if (SqlType == null)
            {
                SqlType = sqlType;
            }
            return SqlType;
        }

        internal bool IsOfType<T>(Type testType)
        {
            if (testType == null)
            {
                return false;
            }
            return typeof(T).IsAssignableFrom(testType);
        }

        internal void SetSqlType(string sqlType)
        {
            SqlType = sqlType;
        }
    }

    internal static class TypeSwitchExtensions
    {
        internal static TypeSwitch Case<T1>(this TypeSwitch typeSwitch, string sqlType)
        {
            return Case<T1>(typeSwitch, (x) => { return sqlType; });
        }

        internal static TypeSwitch Case<T>(this TypeSwitch typeSwitch, Func<Type, string> typeMethod)
        {
            if (typeSwitch.TargetType.IsOfType<T>())
            {
                typeSwitch.SetSqlType(typeMethod(typeSwitch.TargetType));
            }
            return typeSwitch;
        }

        internal static TypeSwitch Case<T1, T2>(this TypeSwitch typeSwitch, string sqlType)
        {
            return Case<T1, T2>(typeSwitch, (x) => { return sqlType; });
        }

        internal static TypeSwitch Case<T1, T2>(this TypeSwitch typeSwitch, Func<Type, string> typeMethod)
        {
            if (
                typeSwitch.TargetType.IsOfType<T1>() ||
                typeSwitch.TargetType.IsOfType<T2>()
               )
            {
                typeSwitch.SetSqlType(typeMethod(typeSwitch.TargetType));
            }
            return typeSwitch;
        }

        internal static TypeSwitch Case<T1, T2, T3>(this TypeSwitch typeSwitch, string sqlType)
        {
            return Case<T1, T2, T3>(typeSwitch, (x) => { return sqlType; });
        }

        internal static TypeSwitch Case<T1, T2, T3>(this TypeSwitch typeSwitch, Func<Type, string> typeMethod)
        {
            if (
                typeSwitch.TargetType.IsOfType<T1>() ||
                typeSwitch.TargetType.IsOfType<T2>() ||
                typeSwitch.TargetType.IsOfType<T3>()
               )
            {
                typeSwitch.SetSqlType(typeMethod(typeSwitch.TargetType));
            }
            return typeSwitch;
        }

        internal static TypeSwitch Case<T1, T2, T3, T4>(this TypeSwitch typeSwitch, string sqlType)
        {
            return Case<T1, T2, T3, T4>(typeSwitch, (x) => { return sqlType; });
        }

        internal static TypeSwitch Case<T1, T2, T3, T4>(this TypeSwitch typeSwitch, Func<Type, string> typeMethod)
        {
            if (
                typeSwitch.TargetType.IsOfType<T1>() ||
                typeSwitch.TargetType.IsOfType<T2>() ||
                typeSwitch.TargetType.IsOfType<T3>() ||
                typeSwitch.TargetType.IsOfType<T4>()
               )
            {
                typeSwitch.SetSqlType(typeMethod(typeSwitch.TargetType));
            }
            return typeSwitch;
        }


        internal static TypeSwitch Case<T1, T2, T3, T4, T5>(this TypeSwitch typeSwitch, string sqlType)
        {
            return Case<T1, T2, T3, T4, T5>(typeSwitch, (x) => { return sqlType; });
        }

        internal static TypeSwitch Case<T1, T2, T3, T4, T5>(this TypeSwitch typeSwitch, Func<Type, string> typeMethod)
        {
            if (
                typeSwitch.TargetType.IsOfType<T1>() ||
                typeSwitch.TargetType.IsOfType<T2>() ||
                typeSwitch.TargetType.IsOfType<T3>() ||
                typeSwitch.TargetType.IsOfType<T4>() ||
                typeSwitch.TargetType.IsOfType<T5>()
               )
            {
                typeSwitch.SetSqlType(typeMethod(typeSwitch.TargetType));
            }
            return typeSwitch;
        }
        internal static bool IsOfType<T>(this Type testType)
        {
            if (testType == null)
            {
                return false;
            }
            return typeof(T).IsAssignableFrom(testType);
        }
    }
}
