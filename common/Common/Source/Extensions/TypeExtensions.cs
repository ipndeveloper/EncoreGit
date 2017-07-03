using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NetSteps.Encore.Core;
using NetSteps.Common.Base;
using NetSteps.Common.Expressions;
using System.ComponentModel.DataAnnotations;
using System.Collections.Concurrent;

namespace NetSteps.Common.Extensions
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Type Extensions
	/// Created: 05-19-2010
	/// </summary>
	public static class TypeExtensions
	{
		public static bool IsNumeric(this Type type)
		{
			type = GetNonNullableType(type);

			if (type != null && !type.IsEnum)
			{
				switch (Type.GetTypeCode(type))
				{
					case TypeCode.Char:
					case TypeCode.SByte:
					case TypeCode.Byte:
					case TypeCode.Int16:
					case TypeCode.UInt16:
					case TypeCode.Int32:
					case TypeCode.UInt32:
					case TypeCode.Int64:
					case TypeCode.UInt64:
					case TypeCode.Single:
					case TypeCode.Double:
					case TypeCode.Decimal:
						return true;
				}
			}

			return false;
		}

		public static Type GetNonNullableType(this Type type)
		{
			if (IsNullableType(type))
				return Nullable.GetUnderlyingType(type);

			return type;
		}

		public static bool IsNullableType(this Type type)
		{
			if (type == null)
				return false;

			return type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>));
		}

		public static List<PropertyInfo> GetPropertiesCached(this Type type)
		{
			return Reflection.Reflection.FindClassProperties(type);
		}

		public static List<PropertyInfo> GetPropertiesByOrderIfDeclared(this Type type)
		{
			return (from p in type.GetProperties()
							let o =
									p.IsDefined(typeof(DisplayAttribute), false)
											? ((DisplayAttribute)p.GetCustomAttributes(typeof(DisplayAttribute), false).First()).GetOrder() ?? 0
											: 0
							orderby o
							select p).ToList();
		}

		public static IEnumerable<PropertyInfo> GetPropertiesByAttribute<TAttr>(this Type type)
		{
			return Reflection.Reflection.FindClassPropertiesWithAttributeType(type, typeof(TAttr));
		}

		public static IEnumerable<PropertyInfo> GetMetadataPropertiesByAttribute<TAttr>(this Type type)
		{
			var metadataClassType = type.GetMetadataClassType();
			return metadataClassType != null
					? metadataClassType.GetPropertiesByAttribute<TAttr>()
					: Enumerable.Empty<PropertyInfo>();
		}

		public static PropertyInfo GetPropertyCached(this Type type, string property)
		{
			return Reflection.Reflection.FindClassProperty(type, property);
		}

		//public static bool PropertyExists(this Type type, string property)
		//{
		//    var prop = Reflection.Reflection.FindClassProperty(type, property);
		//    return !(prop == null);
		//}

		public static bool PropertyExists(this Type t, string property, Action<Type, PropertyInfo> actionOnEachProperty = null)
		{
			return Reflection.Reflection.PropertyExists(t, property, actionOnEachProperty);
		}

		public static List<MethodInfo> GetMethodsCached(this Type type)
		{
			return Reflection.Reflection.FindClassMethods(type);
		}

		public static IEnumerable<MethodInfo> GetMethodsByAttribute(this Type type, Type attributeType)
		{
			return Reflection.Reflection.FindClassMethodsWithAttributeType(type, attributeType);
		}

		public static IEnumerable<MethodInfo> GetMethodsByAttribute<T, TAttr>() where TAttr : Attribute
		{
			return Reflection.Reflection.FindClassMethodsWithAttributeType(typeof(T), typeof(TAttr));
		}

		public static IEnumerable<MethodInfo> GetMethodsByNameCached(this Type type, string methodName)
		{
			return Reflection.Reflection.FindClassMethodsByName(type, methodName);
		}

		public static MethodInfo GetMethodCached(this Type type, string method)
		{
			return Reflection.Reflection.FindClassMethod(type, method);
		}

		public static ConstructorInfo GetConstructorCached(this Type type)
		{
			return Reflection.Reflection.FindConstructor(type, null);
		}

		public static ConstructorInfo GetConstructorCached(this Type type, Type[] args)
		{
			return Reflection.Reflection.FindConstructor(type, args);
		}

		/// <summary>
		/// Creates a compiled lambda expression for the constructor and calls it to create a new object - DES
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="type"></param>
		/// <returns></returns>
		public static T New<T>(this Type type)
		{
			return ExpressionHelper.GetActivator<T>(type.GetConstructorCached())();
		}

		/// <summary>
		/// Creates a compiled lambda expression for the constructor and calls it to create a new object - DES
		/// </summary>
		/// <param name="type"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public static T New<T>(this Type type, IEnumerable<object> args)
		{
			return ExpressionHelper.GetActivator<T>(type.GetConstructorCached(args.Select(a => a.GetType()).ToArray()))(args);
		}

		/// <summary>
		/// Creates a compiled lambda expression for the constructor and calls it to create a new object - DES
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="type"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public static T New<T>(this Type type, params object[] args)
		{
			return ExpressionHelper.GetActivator<T>(type.GetConstructorCached(args.Select(a => a.GetType()).ToArray()))(args);
		}

		/// <summary>
		/// Creates a compiled lambda expression for the constructor and calls it to create a new object - DES
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static object New(this Type type)
		{
			return ExpressionHelper.GetActivator(type.GetConstructorCached())();
		}

		/// <summary>
		/// Creates a compiled lambda expression for the constructor and calls it to create a new object - DES
		/// </summary>
		/// <param name="type"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public static object New(this Type type, IEnumerable<object> args)
		{
			return ExpressionHelper.GetActivator(type.GetConstructorCached(args.Select(a => a.GetType()).ToArray()))(args);
		}

		/// <summary>
		/// Creates a compiled lambda expression for the constructor and calls it to create a new object - DES
		/// </summary>
		/// <param name="type"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public static object New(this Type type, params object[] args)
		{
			return ExpressionHelper.GetActivator(type.GetConstructorCached(args.Select(a => a.GetType()).ToArray()))(args);
		}

		public delegate object CtorDelegate();
		private static ConcurrentDictionary<object, CtorDelegate> _ctors = new ConcurrentDictionary<object, CtorDelegate>();
		
		/// <summary>
		/// Emits some IL to create a new object using a parameterless constructor - DES
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static object NewFast(this Type type)
		{
			CtorDelegate dlg = _ctors.GetOrAdd(type.GetKeyForType(),
				unused =>
				{
					System.Reflection.Emit.DynamicMethod dm = new System.Reflection.Emit.DynamicMethod(type.Name + "Ctor", type, new Type[] { });

					System.Reflection.Emit.ILGenerator ilgen = dm.GetILGenerator();
					ilgen.Emit(System.Reflection.Emit.OpCodes.Newobj, type.GetConstructorCached(new Type[] { }));
					ilgen.Emit(System.Reflection.Emit.OpCodes.Ret);

					return (CtorDelegate)dm.CreateDelegate(typeof(CtorDelegate));
				}
			);
			return dlg();
		}

		/// <summary>
		/// Returns the metadata class (if any) that is associated with this type.
		/// </summary>
		public static Type GetMetadataClassType(this Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}

			return type
					.GetCustomAttributes(typeof(MetadataTypeAttribute), true)
					.Cast<MetadataTypeAttribute>()
					.Select(x => x.MetadataClassType)
					.FirstOrDefault();
		}
	}
}