using System;

namespace NetSteps.Encore.Core
{
	/// <summary>
	/// Copier implementation for anonymous source objects.
	/// </summary>
	/// <typeparam name="S"></typeparam>
	/// <typeparam name="T"></typeparam>
	public class AnonymousSourceCopier<S, T> : ICopier<S, T>
	{
		/// <summary>
		/// Copies source to target.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="source"></param>
		/// <param name="kind"></param>
		/// <param name="container"></param>
		public void CopyTo(T target, S source, CopyKind kind, IoC.IContainer container)
		{
			if (kind == CopyKind.Loose)
			{
				Copier<T>.LooseCopyTo(target, source, container);
			}
			else throw new NotImplementedException();
		}
	}
}
