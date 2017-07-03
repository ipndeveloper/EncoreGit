using NetSteps.Encore.Core.IoC;

namespace NetSteps.Encore.Core
{
	/// <summary>
	/// Copies properties of the source object to the target object.
	/// </summary>
	/// <typeparam name="S">source type S</typeparam>
	/// <typeparam name="T">target type T</typeparam>
	[CopierAutoImplement]
	public interface ICopier<S, T>
	{
		/// <summary>
		/// Copies properties from a source object to a target object.
		/// </summary>
		/// <param name="target">the target object</param>
		/// <param name="source">the source object</param>
		/// <param name="kind">kind of copy (loose or strict)</param>
		/// <param name="container">a container scope</param>
		void CopyTo(T target, S source, CopyKind kind, IContainer container);
	}
}
