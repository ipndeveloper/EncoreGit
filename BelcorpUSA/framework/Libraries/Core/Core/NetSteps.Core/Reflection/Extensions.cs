﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Runtime.CompilerServices;


namespace NetSteps.Encore.Core.Reflection
{
	/// <summary>
	/// Extension methods for System.Type.
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Gets a readable full name. Since this method uses reflection it should be used
		/// rarely. It was created to supply simpler type names when constructing error messages.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>A readable name such as My.Namespace.MyType&lt;string, int></returns>
		public static string GetReadableFullName(this Type type)
		{
			Contract.Requires<ArgumentNullException>(type != null);

			string result;
			Type tt = (type.IsArray) ? type.GetElementType() : type;
			string simpleName = tt.Name;

			Contract.Assume(simpleName != null);
			Contract.Assert(simpleName.Length >= 0);

			if (simpleName.Contains('`'))
			{
				simpleName = simpleName.Substring(0, simpleName.IndexOf("`", StringComparison.InvariantCulture));
				var args = tt.GetGenericArguments();
				for (int i = 0; i < args.Length; i++)
				{
					if (i == 0)
						simpleName = String.Concat(simpleName, '<', args[i].GetReadableSimpleName());
					else
						simpleName = String.Concat(simpleName, ',', args[i].GetReadableSimpleName());
				}
				simpleName = String.Concat(simpleName, '>');
			}
			if (tt.IsNested)
			{
				result = String.Concat(tt.DeclaringType.GetReadableFullName(), "+", simpleName);
			}
			else
			{
				result = String.Concat(tt.Namespace, ".", simpleName);
			}
			return result;
		}

		/// <summary>
		/// Gets a readable simple name for a type.
		/// </summary>
		/// <param name="type">the type</param>
		/// <returns>A readable name such as MyType&lt;string, int></returns>
		public static string GetReadableSimpleName(this Type type)
		{
			Contract.Requires<ArgumentNullException>(type != null);

			Type tt = (type.IsArray) ? type.GetElementType() : type;
			string simpleName = tt.Name;
			if (simpleName.Contains('`'))
			{
				simpleName = simpleName.Substring(0, simpleName.IndexOf("`", StringComparison.InvariantCulture));
				var args = tt.GetGenericArguments();
				for (int i = 0; i < args.Length; i++)
				{
					if (i == 0)
						simpleName = String.Concat(simpleName, '<', args[i].GetReadableSimpleName());
					else
						simpleName = String.Concat(simpleName, ',', args[i].GetReadableSimpleName());
				}
				simpleName = String.Concat(simpleName, '>');
			}
			return simpleName;
		}

		/// <summary>
		/// Determines if a type is a number.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static bool IsNumber(this Type type)
		{
			switch (Type.GetTypeCode(type))
			{
				case TypeCode.Byte:
				case TypeCode.SByte:
				case TypeCode.Int16:
				case TypeCode.Int32:
				case TypeCode.Int64:
				case TypeCode.UInt16:
				case TypeCode.UInt32:
				case TypeCode.UInt64:
				case TypeCode.Single:
				case TypeCode.Double:
				case TypeCode.Decimal:
					return true;
				default:
					return false;
			}
		}
		
		/// <summary>
		/// Determines if the privided type is an anonymous type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		/// <remarks>
		/// Adapted from answers found here:
		/// http://stackoverflow.com/questions/1650681/determining-whether-a-type-is-an-anonymous-type
		/// </remarks>
		public static bool IsAnonymousType(this Type type)
		{
			Contract.Requires<ArgumentNullException>(type != null);
			var name = type.Name;
			if (name.Length < 3)
			{
				return false;
			}
			return name[0] == '<'
					&& name[1] == '>'
					&& name.IndexOf("AnonymousType", StringComparison.Ordinal) > 0
					&& type.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Count() > 0;
		}

		/// <summary>
		/// Gets the type hierarchy in declaration (depth-first, inheritance) order.
		/// </summary>
		/// <param name="type">the type</param>
		/// <returns>type hierarchy in declaration (depth-first, inheritance) order</returns>
		public static IEnumerable<Type> GetTypeHierarchyInDeclarationOrder(this Type type)
		{
			Contract.Requires<ArgumentNullException>(type != null);

			if (type.BaseType == null) return type.GetInterfaces().Reverse().Concat(Enumerable.Repeat(type, 1));

			return GetTypeHierarchyInDeclarationOrder(type.BaseType)
				.Union(type.GetInterfaces().Reverse())
				.Concat(Enumerable.Repeat(type, 1));
		}

		/// <summary>
		/// Using reflection, gets the type's properties that are both read and write.
		/// </summary>
		/// <param name="type">the type</param>
		/// <returns>the properties</returns>
		public static IEnumerable<PropertyInfo> GetReadWriteProperties(this Type type)
		{
			Contract.Requires<ArgumentNullException>(type != null);
			return from p in type.GetProperties()
						 where p.CanRead && p.CanWrite
						 select p;
		}

		/// <summary>
		/// Using reflection, gets the type's properties that can be read.
		/// </summary>
		/// <param name="type">the type</param>
		/// <returns>the properties</returns>
		public static IEnumerable<PropertyInfo> GetReadableProperties(this Type type)
		{
			Contract.Requires<ArgumentNullException>(type != null);
			return from p in type.GetProperties()
						 where p.CanRead
						 select p;
		}

		/// <summary>
		/// Using reflection, gets the type's properties that can be read.
		/// </summary>
		/// <param name="type">the type</param>
		/// <param name="binding">binding flags</param>
		/// <returns>the properties</returns>
		public static IEnumerable<PropertyInfo> GetReadableProperties(this Type type, BindingFlags binding)
		{
			Contract.Requires<ArgumentNullException>(type != null);
			return from p in type.GetProperties(binding)
						 where p.CanRead
						 select p;
		}

		/// <summary>
		/// Using reflection, gets a readable property by name.
		/// </summary>
		/// <param name="propertyName">the property name</param>
		/// <param name="type">the type</param>
		/// <returns>a property or null</returns>
		public static PropertyInfo GetReadableProperty(this Type type, string propertyName)
		{
			Contract.Requires<ArgumentNullException>(type != null);
			Contract.Requires<ArgumentNullException>(propertyName != null);

			PropertyInfo p = type.GetProperty(propertyName);
			return (p != null && p.CanRead) ? p : null;
		}

		/// <summary>
		/// Using reflection, gets readable properties for the type and all of its base classes, interfaces.
		/// </summary>
		/// <param name="type">the type</param>
		/// <param name="binding">binding flags</param>
		/// <returns>writable properties</returns>
		public static IEnumerable<PropertyInfo> GetReadablePropertiesFromHierarchy(this Type type, BindingFlags binding)
		{
			Contract.Requires<ArgumentNullException>(type != null);

			List<PropertyInfo> results = new List<PropertyInfo>();
			foreach (var t in type.GetTypeHierarchyInDeclarationOrder())
			{
				results.AddRange(from p in t.GetProperties(binding)
												 where p.CanRead && p.GetGetMethod() != null
												 select p);
			}
			return results;
		}

		/// <summary>
		/// Using reflection, gets a readable property by name and assignability.
		/// </summary>
		/// <param name="type">the type</param>
		/// <param name="propertyName">the property name</param>
		/// <param name="binding">binding flags</param>
		/// <param name="assignableFromType">a type used to test assignability</param>
		/// <returns>a property or null</returns>
		public static PropertyInfo GetReadablePropertyWithAssignmentCompatablity(this Type type, string propertyName, BindingFlags binding, Type assignableFromType)
		{
			Contract.Requires<ArgumentNullException>(type != null);
			Contract.Requires<ArgumentNullException>(propertyName != null);
			Contract.Requires<ArgumentNullException>(assignableFromType != null);

			return (from p in GetReadableProperties(type, binding)
							where p.Name == propertyName
								&& p.PropertyType.IsAssignableFrom(assignableFromType)
							select p).FirstOrDefault();
		}

		/// <summary>
		/// Using reflection, gets a writable property by name and assignability.
		/// </summary>
		/// <param name="type">the type</param>
		/// <param name="propertyName">the property name</param>
		/// <param name="binding">binding flags</param>
		/// <param name="assignableFromType">a type used to test assignability</param>
		/// <returns>a property or null</returns>
		public static PropertyInfo GetWritablePropertyWithAssignmentCompatablity(this Type type, string propertyName, BindingFlags binding, Type assignableFromType)
		{
			Contract.Requires<ArgumentNullException>(type != null);
			Contract.Requires<ArgumentNullException>(propertyName != null);
			Contract.Requires<ArgumentNullException>(assignableFromType != null);

			return (from p in GetWritableProperties(type, binding)
							where p.Name == propertyName
								&& p.PropertyType.IsAssignableFrom(assignableFromType)
							select p).FirstOrDefault();
		}

		/// <summary>
		/// Using reflection, gets a writable property by name and assignability.
		/// </summary>
		/// <param name="type">the type</param>
		/// <param name="propertyName">the property name</param>
		/// <param name="binding">binding flags</param>
		/// <param name="assignableFromType">a type used to test assignability</param>
		/// <returns>a property or null</returns>
		public static PropertyInfo GetWritablePropertyWithAssignmentCompatablityFromHierarchy(this Type type, string propertyName, BindingFlags binding, Type assignableFromType)
		{
			Contract.Requires<ArgumentNullException>(type != null);
			Contract.Requires<ArgumentNullException>(propertyName != null);
			Contract.Requires<ArgumentNullException>(assignableFromType != null);

			return (from p in GetWritablePropertiesFromHierarchy(type, binding)
							where p.Name == propertyName
								&& p.PropertyType.IsAssignableFrom(assignableFromType)
							select p).FirstOrDefault();
		}

		/// <summary>
		/// Using reflection, gets writable properties.
		/// </summary>
		/// <param name="type">the type</param>
		/// <returns>writable properties</returns>
		public static IEnumerable<PropertyInfo> GetWritableProperties(this Type type)
		{
			Contract.Requires<ArgumentNullException>(type != null);

			return from p in type.GetProperties()
						 where p.CanWrite && p.GetSetMethod() != null
						 select p;
		}

		/// <summary>
		/// Using reflection, gets writable properties.
		/// </summary>
		/// <param name="type">the type</param>
		/// <param name="binding">binding flags</param>
		/// <returns>writable properties</returns>
		public static IEnumerable<PropertyInfo> GetWritableProperties(this Type type, BindingFlags binding)
		{
			Contract.Requires<ArgumentNullException>(type != null);

			return from p in type.GetProperties(binding)
						 where p.CanWrite && p.GetSetMethod() != null
						 select p;
		}

		/// <summary>
		/// Using reflection, gets writable properties for the type and all of its base classes, interfaces.
		/// </summary>
		/// <param name="type">the type</param>
		/// <param name="binding">binding flags</param>
		/// <returns>writable properties</returns>
		public static IEnumerable<PropertyInfo> GetWritablePropertiesFromHierarchy(this Type type, BindingFlags binding)
		{
			Contract.Requires<ArgumentNullException>(type != null);


			List<PropertyInfo> results = new List<PropertyInfo>();
			foreach (var t in type.GetTypeHierarchyInDeclarationOrder())
			{
				results.AddRange(from p in t.GetProperties(binding)
												 where p.CanWrite && p.GetSetMethod() != null
												 select p);
			}
			return results;
		}

		/// <summary>
		/// Using reflection, gets a writable property by name.
		/// </summary>
		/// <param name="type">the type</param>
		/// <param name="propertyName">the property name</param>
		/// <returns>a property or null</returns>
		public static PropertyInfo GetWriteableProperty(this Type type, string propertyName)
		{
			Contract.Requires<ArgumentNullException>(type != null);
			Contract.Requires<ArgumentNullException>(propertyName != null);

			PropertyInfo p = type.GetProperty(propertyName);
			return (p != null && p.CanWrite && p.GetSetMethod() != null) ? p : null;
		}

		/// <summary>
		/// Using reflection, gets a generic method from the target type.
		/// </summary>
		/// <param name="type">the target type</param>
		/// <param name="name">the property name</param>
		/// <param name="binding">binding flags</param>
		/// <param name="parameterCount">number of parameters on the target method</param>
		/// <param name="genericArgumentCount">number of generic arguments on the target method</param>
		/// <returns>a property or null</returns>
		public static MethodInfo GetGenericMethod(this Type type, string name, BindingFlags binding
			, int parameterCount, int genericArgumentCount)
		{
			Contract.Requires<ArgumentNullException>(type != null);
			Contract.Requires<ArgumentNullException>(name != null);
			Contract.Requires<ArgumentNullException>(name.Length > 0);
			Contract.Requires<ArgumentNullException>(genericArgumentCount > 0);
			return (from m in type.GetMethods(binding)
							where String.Equals(name, m.Name, StringComparison.Ordinal)
							 && m.IsGenericMethodDefinition
							 && m.GetGenericArguments().Count() == genericArgumentCount
							 && m.GetParameters().Count() == parameterCount
							select m).SingleOrDefault();
		}

		/// <summary>
		/// Using reflection, gets a generic method from the target type.
		/// </summary>
		/// <param name="type">the target type</param>
		/// <param name="name">the property name</param>
		/// <param name="parameterCount">number of parameters on the target method</param>
		/// <param name="genericArgumentCount">number of generic arguments on the target method</param>
		/// <returns>a property or null</returns>
		public static MethodInfo GetGenericMethod(this Type type, string name, int parameterCount, int genericArgumentCount)
		{
			Contract.Requires<ArgumentNullException>(type != null);
			Contract.Requires<ArgumentNullException>(name != null);
			Contract.Requires<ArgumentNullException>(name.Length > 0);
			Contract.Requires<ArgumentNullException>(genericArgumentCount > 0);
			var r = (from m in type.GetMethods()
							 where String.Equals(name, m.Name, StringComparison.Ordinal)
								&& m.IsGenericMethodDefinition
								&& m.GetGenericArguments().Count() == genericArgumentCount
								&& m.GetParameters().Count() == parameterCount
							 select m);
			return r.SingleOrDefault();
		}

		/// <summary>
		/// Determins if the target type is an implementation of the given generic definition.
		/// </summary>
		/// <param name="type">the target type</param>
		/// <param name="generic">the generic definition</param>
		/// <returns>true if the target is an implementation of the generic definition</returns>
		public static bool IsTypeofGenericTypeDefinition(this Type type, Type generic)
		{
			Contract.Requires<ArgumentNullException>(type != null);
			Contract.Requires<ArgumentNullException>(generic != null);


			if (generic.IsInterface)
			{
				foreach (var intf in type.GetInterfaces().Where(i => i.IsGenericType))
				{
					if (intf.GetGenericTypeDefinition() == generic)
						return true;
				}
				return false;
			}

			var t = type;
			while (t != null)
			{
				var gtd = t.GetGenericTypeDefinition();
				if (gtd == generic) return true;

				// Check base types...
				var b = gtd.BaseType;
				while (b != null)
				{
					if (b == generic) return true;
					b = b.BaseType;
				}

				t = t.BaseType;
			}
			return false;
		}

		/// <summary>
		/// If the type is IEnumerable&lt;>, gets the element type (typeof(T)).
		/// </summary>
		/// <param name="type">the type</param>
		/// <returns>type T of the IEnumerable&lt;T> if the given type is enumerable; otherwise <em>null</em>.</returns>
		public static Type FindEnumerableElementType(this Type type)
		{
			if (type == null || type == typeof(string))
				return null;
			if (type.IsArray)
				return typeof(IEnumerable<>).MakeGenericType(type.GetElementType());
			if (type.IsGenericType)
			{
				foreach (var arg in type.GetGenericArguments())
				{
					var ienum = typeof(IEnumerable<>).MakeGenericType(arg);
					if (ienum.IsAssignableFrom(type))
					{
						return ienum;
					}
				}
			}
			var ifaces = type.GetInterfaces();
			if (ifaces != null && ifaces.Length > 0)
			{
				foreach (var iface in ifaces)
				{
					var ienum = FindEnumerableElementType(iface);
					if (ienum != null)
						return ienum;
				}
			}
			if (type.BaseType != null && type.BaseType != typeof(object))
			{
				return FindEnumerableElementType(type.BaseType);
			}
			return null;
		}

		/// <summary>
		/// Given a type, finds the type's element type.
		/// </summary>
		/// <param name="type">the type</param>
		/// <returns>the type's element type</returns>
		public static Type FindElementType(this Type type)
		{
			var ienum = FindEnumerableElementType(type);
			if (ienum == null)
				return type;
			return ienum.GetGenericArguments()[0];
		}

		/// <summary>
		/// Determines if the arrays are equal or if the items in two different arrays
		/// are equal.
		/// </summary>
		/// <typeparam name="T">Item type T</typeparam>
		/// <param name="lhs">Left-hand comparand</param>
		/// <param name="rhs">Right-hand comparand</param>
		/// <returns><b>true</b> if the arrays are equal or if the items in the arrays are equal.</returns>
		internal static bool EqualsOrItemsEqual<T>(this T[] lhs, T[] rhs)
		{
			bool result = Object.Equals(lhs, rhs);
			if (result == false && lhs != null && rhs != null
				&& lhs.LongLength == rhs.LongLength)
			{
				if (lhs.Length == 0)
				{ // two empty arrays are equal.
					result = true;
				}
				else
				{
					var comparer = EqualityComparer<T>.Default;
					for (int i = 0; i < lhs.LongLength; i++)
					{
						result = comparer.Equals(lhs[i], rhs[i]);
						if (!result) break;
					}
				}
			}
			return result;
		}

		/// <summary>
		/// Formats an exception for output into the log.
		/// </summary>
		/// <param name="ex">the exception</param>
		/// <param name="exposeStackTrace">indicates whether stack trace should be exposed in the output</param>
		/// <returns>a string representation of the exception</returns>
		internal static string FormatForLogging(this Exception ex, bool exposeStackTrace)
		{
			Contract.Requires<ArgumentNullException>(ex != null);
			var builder = new StringBuilder(400)
					.Append(ex.GetType().FullName).Append(": ").Append(ex.Message);
			var e = ex.InnerException;
			while (e != null)
			{
				builder.Append(Environment.NewLine).Append("\t InnerException >")
						.Append(e.GetType().FullName).Append(": ").Append(e.Message);

				e = e.InnerException;
			}
			if (exposeStackTrace)
			{
				builder.Append(Environment.NewLine).Append("\t StackTrace >>")
					.Append(ex.StackTrace);
			}
			return builder.ToString();
		}
	}
}