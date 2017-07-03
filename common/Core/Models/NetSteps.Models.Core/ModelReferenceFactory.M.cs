using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using NetSteps.Models.Core.ModelReferences;
using NetSteps.Models.Core.SPI;
using NetSteps.Encore.Core.Reflection;

namespace NetSteps.Models.Core
{
	/// <summary>
	/// Base, abstract implementation of the IModelReferenceFactory interface.
	/// </summary>
	/// <typeparam name="M">model type M</typeparam>
	public abstract class ModelReferenceFactory<M> : IModelReferenceFactory<M>
	{
		/// <summary>
		/// Constructs a new instance (without an identity key)
		/// </summary>
		protected ModelReferenceFactory()
			: this(false)
		{
		}
		/// <summary>
		/// Constructs a new instance.
		/// </summary>
		/// <param name="hasIdentityKey">indicates whether model type M has an identity key.</param>
		protected ModelReferenceFactory(bool hasIdentityKey)
		{
			HasIdentityKey = hasIdentityKey;
		}

		/// <summary>
		/// Indicates whether model type M has an identity key.
		/// </summary>
		public bool HasIdentityKey { get; private set; }

		/// <summary>
		/// Makes a new reference from a referent.
		/// </summary>
		/// <param name="model">the model/referent</param>
		/// <returns>a reference to the model</returns>
		public virtual IModelReference<M> MakeFromReferent(M model)
		{
			return new ImmutableModelReference<M>(model);
		}

		/// <summary>
		/// Determines if a reference is equal to a model.
		/// </summary>
		/// <param name="lhs">the reference</param>
		/// <param name="rhs">the model</param>
		/// <returns><em>true</em> if the target of the reference is equal to the model; otherwise <em>false</em></returns>
		public bool Equals(IModelReference<M> lhs, M rhs)
		{
			return lhs != null
				&& !lhs.IsEmpty
				&& lhs.HasModel
				&& Equals(lhs.Model, rhs);
		}

		/// <summary>
		/// Cascades marking of model state to the target of a reference.
		/// </summary>
		/// <param name="reference">the reference</param>
		/// <param name="states">states to be excluded</param>
		/// <param name="context">a marking context</param>
		public void CascadeIncludeModelStates(IModelReference<M> reference, ModelStates states, IMarkingContext context)
		{
			Contract.Assert(reference != null);
			Contract.Assert(context != null);
			PerfromCascadeModelStates(reference, states, context);
		}

		/// <summary>
		/// Overriden by subclasses to cascade marking of model state on the target of a reference.
		/// </summary>
		/// <param name="reference">the reference</param>
		/// <param name="states">states to be excluded</param>
		/// <param name="context">a marking context</param>
		protected abstract void PerfromCascadeModelStates(IModelReference<M> reference, ModelStates states, IMarkingContext context);

		/// <summary>
		/// Marks a reference as clean.
		/// </summary>
		/// <param name="reference">the reference</param>
		/// <param name="context">a marking context</param>
		/// <returns>a reference to the clean model</returns>
		public IModelReference<M> MarkClean(IModelReference<M> reference, IMarkingContext context)
		{
			Contract.Assert(reference != null);
			Contract.Assert(context != null);
			return PerfromMarkClean(reference, context);
		}

		/// <summary>
		/// Overriden by subclasses to mark a reference as clean.
		/// </summary>
		/// <param name="reference">the reference</param>
		/// <param name="context">a marking context</param>
		/// <returns>a reference to the clean model</returns>
		protected abstract IModelReference<M> PerfromMarkClean(IModelReference<M> reference, IMarkingContext context);

		/// <summary>
		/// Evaluates two models for equality.
		/// </summary>
		/// <param name="x">model x</param>
		/// <param name="y">model y</param>
		/// <returns><em>true</em> if the models are equal; otherwise <em>false</em>.</returns>
		public bool Equals(M x, M y)
		{
			return PerformEquals(x, y);
		}

		/// <summary>
		/// Overriden by subclasses to evaluate two models for equality.
		/// </summary>
		/// <param name="x">model x</param>
		/// <param name="y">model y</param>
		/// <returns><em>true</em> if the models are equal; otherwise <em>false</em>.</returns>
		protected abstract bool PerformEquals(M x, M y);

		/// <summary>
		/// Gets a model's hashcode.
		/// </summary>
		/// <param name="model">the model</param>
		/// <returns></returns>
		public int GetHashCode(M model)
		{
			return CalculateHashCode(model);
		}

		/// <summary>
		/// Gets a model's hashcode.
		/// </summary>
		/// <param name="model">the model</param>
		/// <returns></returns>
		protected abstract int CalculateHashCode(M model);		
	}

}
