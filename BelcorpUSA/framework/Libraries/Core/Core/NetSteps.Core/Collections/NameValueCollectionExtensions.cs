using System;
using System.Collections.Specialized;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace NetSteps.Encore.Core
{
	/// <summary>
	/// Contains extensions for NameValueCollection
	/// </summary>
	public static class NameValueCollectionExtensions
	{
		/// <summary>
		/// Transforms the value part of a name-value pair to type T if it
		/// is present in the collection.
		/// </summary>
		/// <typeparam name="T">result type T</typeparam>
		/// <param name="nvc">the collection</param>
		/// <param name="name">the value's name</param>
		/// <returns>a result type T if the name-value pair is present; otherwise default(T)</returns>
		public static T FirstValueOrDefault<T>(this NameValueCollection nvc, string name)
		{
			Contract.Requires(nvc != null);
			Contract.Requires(name != null);
			Contract.Requires(name.Length > 0);

			var values = nvc.GetValues(name);
			if (values.Length > 0)
			{
				return (T)Convert.ChangeType(values[0], typeof(T));
			}
			return default(T);
		}
		/// <summary>
		/// Transforms the value part of a name-value pair to type T if it
		/// is present in the collection.
		/// </summary>
		/// <typeparam name="T">result type T</typeparam>
		/// <param name="nvc">the collection</param>
		/// <param name="name">the value's name</param>
		/// <param name="transform">optional function used to transform the value</param>
		/// <returns>a result type T if the name-value pair is present; otherwise default(T)</returns>
		public static T FirstValueOrDefault<T>(this NameValueCollection nvc, string name, Func<string, T> transform)
		{
			Contract.Requires(nvc != null);
			Contract.Requires(name != null);
			Contract.Requires(name.Length > 0);

			var values = nvc.GetValues(name);
			if (transform != null && values.Length > 0)
			{
				return transform(values[0]);
			}
			return default(T);
		}

	}
}
