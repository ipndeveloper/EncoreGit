using NetSteps.Models.Core.ModelReferences;

namespace NetSteps.Models.Core
{
	/// <summary>
	/// Contains model reference utilities used by the framework.
	/// </summary>
	/// <typeparam name="M">model type M</typeparam>
	public sealed class ModelReference<M>
	{
		/// <summary>
		/// An empty model reference.
		/// </summary>
		public static IModelReference<M> Empty = new EmptyModelReference<M>();
	}

	/// <summary>
	/// Contains model reference utilities used by the framework.
	/// </summary>
	/// <typeparam name="M">model type M</typeparam>
	/// <typeparam name="IK">identity key type IK</typeparam>
	public sealed class ModelReference<M, IK>
	{
		/// <summary>
		/// An empty model reference.
		/// </summary>
		public static IModelReference<M, IK> Empty = new EmptyModelReference<M, IK>();
	}
}
