using NetSteps.Encore.Core.IoC;

namespace NetSteps.Models.Core
{
	/// <summary>
	/// Context used during the framework's copy
	/// logic to communicate the container and detect cycles
	/// and multiple references in the graph.
	/// </summary>
	public interface ICopyContext
	{
		/// <summary>
		/// Gets the IoC container scope associated with the context.
		/// </summary>
		IContainer Container { get; }

		/// <summary>
		/// Gets the lifespan tracking that should be used for copies made within the context.
		/// </summary>
		LifespanTracking LifespanTracking { get; }

		/// <summary>
		/// Gets or adds a copy of source model M.
		/// </summary>
		/// <typeparam name="M">model type M</typeparam>
		/// <param name="model">source model M</param>
		/// <param name="copier">model copier for model type M</param>
		/// <returns>a copy of the source model</returns>
		M GetOrAddCopy<M>(M model, IModelCopier<M> copier);
		/// <summary>
		/// Gets or adds a copy of identifiable source model M.
		/// </summary>
		/// <typeparam name="M">model type M</typeparam>
		/// <typeparam name="IK">identity key type IK</typeparam>
		/// <param name="model">source model M</param>
		/// <param name="key">identity key for the source model.</param>
		/// <param name="copier">model copier for model type M</param>
		/// <returns>a copy of the source model</returns>		
		M GetOrAddIdentifiable<M, IK>(M model, IK key, IModelCopier<M> copier);

		/// <summary>
		/// Gets or adds a model reference.
		/// </summary>
		/// <typeparam name="M">model type M</typeparam>
		/// <param name="reference">the reference</param>
		/// <returns>the copy</returns>
		IModelReference<M> GetOrAddReference<M>(IModelReference<M> reference);
		/// <summary>
		/// Gets or adds a model reference.
		/// </summary>
		/// <typeparam name="M">model type M</typeparam>
		/// <typeparam name="IK">identity key type IK</typeparam>
		/// <param name="reference">the reference</param>
		/// <returns>the copy</returns>
		IModelReference<M, IK> GetOrAddIdentifiableReference<M, IK>(IModelReference<M, IK> reference);
	}	
}
