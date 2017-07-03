using System.Collections.Generic;

namespace NetSteps.Models.Core
{
	/// <summary>
	/// Interface for working with model references.
	/// </summary>
	/// <typeparam name="M">model type M</typeparam>
	public interface IModelReferenceFactory<M> : IEqualityComparer<M>
	{
		/// <summary>
		/// Indicates whether the model has an identity key.
		/// </summary>
		bool HasIdentityKey { get; }
		/// <summary>
		/// Makes a new reference from a referent.
		/// </summary>
		/// <param name="model">the model/referent</param>
		/// <returns>a reference to the model</returns>
		IModelReference<M> MakeFromReferent(M model);
		/// <summary>
		/// Marks a reference as clean.
		/// </summary>
		/// <param name="reference">the reference</param>
		/// <param name="context">a marking context</param>
		/// <returns>a reference to the clean model</returns>
		IModelReference<M> MarkClean(IModelReference<M> reference, IMarkingContext context);
		/// <summary>
		/// Determines if a reference is equal to a model.
		/// </summary>
		/// <param name="lhs">the reference</param>
		/// <param name="rhs">the model</param>
		/// <returns><em>true</em> if the target of the reference is equal to the model; otherwise <em>false</em></returns>
		bool Equals(IModelReference<M> lhs, M rhs);
		/// <summary>
		/// Cascades marking of model state to the target of a reference.
		/// </summary>
		/// <param name="reference">the reference</param>
		/// <param name="states">states to be excluded</param>
		/// <param name="context">a marking context</param>
		void CascadeIncludeModelStates(IModelReference<M> reference, ModelStates states, IMarkingContext context);
	}	
}
