
namespace NetSteps.Encore.Core.IoC.Constructors
{
	/// <summary>
	/// Adapter for constructors defined on type T
	/// </summary>
	/// <typeparam name="T">type T</typeparam>
	public abstract partial class ConstructorAdapter<T>
	{		
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		public ConstructorAdapter()
		{
		}
		/// <summary>
		/// Executes the constructor and returns the resulting instance.
		/// </summary>
		/// <param name="container">scoping container</param>
		/// <param name="name">the registered name or null</param>
		/// <param name="parameters">parameters intended for the new instance</param>
		/// <returns>a new instance</returns>
		public abstract T Execute(IContainer container, string name, params object[] parameters);
	}	
}
