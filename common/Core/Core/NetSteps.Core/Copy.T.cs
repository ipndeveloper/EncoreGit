using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Encore.Core
{
	/// <summary>
	/// Utility class for making copies to target type T.
	/// </summary>
	/// <typeparam name="T">target type T</typeparam>
	public static class Copy<T>
	{
		/// <summary>
		/// Creates a copy of source.
		/// </summary>
		/// <typeparam name="S">source type S</typeparam>
		/// <param name="source">the source</param>
		/// <returns>an instance of target type T, copied from the source</returns>
		public static T From<S>(S source)
		{
			return From(source, CopyKind.Loose);
		}

		/// <summary>
		/// Creates a copy of source.
		/// </summary>
		/// <typeparam name="S">source type S</typeparam>
		/// <param name="source">the source</param>
		/// <param name="kind">kind of copy; loose or strict</param>
		/// <returns>an instance of target type T, copied from the source</returns>
		public static T From<S>(S source, CopyKind kind)
		{
			using (var create = Create.SharedOrNewContainer())
			{
				return From(create, source, CopyKind.Loose);
			}
		}

		/// <summary>
		/// Creates a copy of source.
		/// </summary>
		/// <typeparam name="S">source type S</typeparam>
		/// <param name="container">a container (scope)</param>
		/// <param name="source">the source</param>
		/// <param name="kind">kind of copy; loose or strict</param>
		/// <returns>an instance of target type T, copied from the source</returns>
		public static T From<S>(IContainer container, S source, CopyKind kind)
		{
			var copier = container.New<ICopier<S, T>>();
			return copier.Copy(container, source, kind);
		}
	}
}
