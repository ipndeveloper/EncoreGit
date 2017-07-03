using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

using System.Runtime.CompilerServices;

namespace NetSteps.Encore.Core.Reflection.Emit
{
	/// <summary>
	/// Various extensions for help while emitting MSIL.
	/// </summary>
	public static class Extensions
	{

		/// <summary>
		/// Generates a valid type name for a generated type.
		/// </summary>
		/// <param name="type">the type upon which the generated type is based</param>
		/// <param name="suffix">a suffix for differentiation when generating more than
		/// one class based on <paramref name="type"/></param>
		/// <returns>a type name for the emitted type</returns>
		public static string FormatEmittedTypeName(this Type type, string suffix)
		{
			Contract.Requires<ArgumentNullException>(type != null);

			return String.Concat(type.Namespace
				, ".emitted"
				, MangleTypeName(type).Substring(type.Namespace.Length).Replace("+", "-")
				, suffix ?? String.Empty
				);
		}

		/// <summary>
		/// Mangles a type name so that it is usable as an emitted type's name.
		/// </summary>
		/// <param name="type">the type</param>
		/// <returns>the (possibly) mangled name</returns>
		public static string MangleTypeName(this Type type)
		{
			Contract.Requires<ArgumentNullException>(type != null);

			string result;
			var tt = (type.IsArray) ? type.GetElementType() : type;
			var simpleName = tt.Name;
			if (simpleName.Contains("`"))
			{
				simpleName = simpleName.Substring(0, simpleName.IndexOf("`"));

				var args = tt.GetGenericArguments();
				simpleName = String.Concat(simpleName, '\u2014');
				for (int i = 0; i < args.Length; i++)
				{
					if (i == 0) simpleName = String.Concat(simpleName, i, MangleTypeNameWithoutNamespace(args[i]));
					else simpleName = String.Concat(simpleName, '-', i, MangleTypeNameWithoutNamespace(args[i]));
				}
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
		/// Mangles a type name so that it is usable as an emitted type's name.
		/// </summary>
		/// <param name="type">the type</param>
		/// <returns>the (possibly) mangled name</returns>
		public static string MangleTypeNameWithoutNamespace(this Type type)
		{
			Contract.Requires<ArgumentNullException>(type != null);

			var tt = (type.IsArray) ? type.GetElementType() : type;
			var simpleName = tt.Name;
			if (simpleName.Contains("`"))
			{
				simpleName = simpleName.Substring(0, simpleName.IndexOf("`"));
				var args = tt.GetGenericArguments();
				simpleName = String.Concat(simpleName, '\u2014');
				for (int i = 0; i < args.Length; i++)
				{
					if (i == 0) simpleName = String.Concat(simpleName, i, MangleTypeNameWithoutNamespace(args[i]));
					else simpleName = String.Concat(simpleName, '-', i, MangleTypeNameWithoutNamespace(args[i]));
				}

			}
			return simpleName;
		}
	}

}
