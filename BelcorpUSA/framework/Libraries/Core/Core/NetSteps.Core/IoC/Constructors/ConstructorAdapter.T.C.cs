
namespace NetSteps.Encore.Core.IoC.Constructors
{
	/// <summary>
	/// Adapter for constructors defined on concrete type C of type T
	/// </summary>
	/// <typeparam name="T">type T</typeparam>
	/// <typeparam name="C">concrete type C</typeparam>
	public abstract partial class ConstructorAdapter<T, C> : ConstructorAdapter<T>
		where C: T
	{
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		public ConstructorAdapter()
		{
		}
	}
}
