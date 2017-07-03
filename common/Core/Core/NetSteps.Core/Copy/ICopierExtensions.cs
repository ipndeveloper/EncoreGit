using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NetSteps.Encore.Core.IoC;


namespace NetSteps.Encore.Core
{
	/// <summary>
	/// Extensions for ICopier&lt;S,T>
	/// </summary>
	public static class ICopierExtensions
	{
		/// <summary>
		/// Using the given copier, creates a copy of a source object.
		/// </summary>
		/// <param name="copier">the copier</param>
		/// <param name="source">the source object</param>
		public static T Copy<S, T>(this ICopier<S, T> copier, S source)
		{
			Contract.Requires<ArgumentNullException>(copier != null);
			return Copy(copier, Container.Current, source, CopyKind.Loose);
		}
		/// <summary>
		/// Using the given copier, creates a copy of a source object.
		/// </summary>
		/// <param name="copier">the copier</param>
		/// <param name="source">the source object</param>
		/// <param name="kind">kind of copy (loose or strict)</param>
		public static T Copy<S, T>(this ICopier<S, T> copier, S source, CopyKind kind)
		{
			Contract.Requires<ArgumentNullException>(copier != null);
			return Copy(copier, Container.Current, source, kind);
		}
		/// <summary>
		/// Using the given copier, creates a copy of a source object.
		/// </summary>
		/// <param name="copier">the copier</param>
		/// <param name="source">the source object</param>
		/// <param name="container">a container scope</param>
		public static T Copy<S, T>(this ICopier<S, T> copier, IContainer container, S source)
		{
			Contract.Requires<ArgumentNullException>(copier != null);
			Contract.Requires<ArgumentNullException>(container != null);
			return Copy(copier, container, source, CopyKind.Loose);
		}
		/// <summary>
		/// Using the given copier, creates a copy of a source object.
		/// </summary>
		/// <param name="copier">the copier</param>
		/// <param name="source">the source object</param>
		/// <param name="kind">kind of copy (loose or strict)</param>
		/// <param name="container">a container scope</param>
		public static T Copy<S, T>(this ICopier<S, T> copier, IContainer container, S source, CopyKind kind)
		{
			Contract.Requires<ArgumentNullException>(copier != null);
			Contract.Requires<ArgumentNullException>(container != null);
			if (EqualityComparer<S>.Default.Equals(default(S), source))
				return default(T);

			T target = container.New<T>();
			copier.CopyTo(target, source, kind, container);
			return target;
		}
	}

}
