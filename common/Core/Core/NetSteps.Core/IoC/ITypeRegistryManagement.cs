namespace NetSteps.Encore.Core.IoC
{
	/// <summary>
	/// Interface for managing type registries.
	/// </summary>
	public interface ITypeRegistryManagement : ITypeRegistry
	{
		/// <summary>
		/// Makes a copy of a type registry for the target container.
		/// </summary>
		/// <param name="container">a target container</param>
		/// <returns>a copy of the type registry</returns>
		ITypeRegistry MakeCopyForContainer(IContainer container);
	}
}
