using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NetSteps.Encore.Core;
using NetSteps.Models.Core.SPI;
using System.ComponentModel;

namespace NetSteps.Models.Core
{
	/// <summary>
	/// Base, abstract model implementation.
	/// </summary>
	/// <typeparam name="M">model type M</typeparam>
	[Serializable]
	public abstract partial class Model<M> : IModelSPI<M>, IEquatable<M>
	{
		static readonly int CHashCodeSeed = typeof(Model<M>).GetKeyForType().GetHashCode();
		
		Status<ModelStates> _state;
		//ICompositionParent _parent;

		/// <summary>
		/// Creates a new immutable instance.
		/// </summary>
		protected Model() : this(ModelStates.Immutable) { }

		/// <summary>
		/// Creates a new instance with the starting state given.
		/// </summary>
		/// <param name="startingState">the instance's starting state</param>
		protected Model(ModelStates startingState)
		{
			_state = new Status<ModelStates>(startingState);
		}

		///// <summary>
		///// Gets the instance's composition parents.
		///// </summary>
		///// <returns></returns>
		//public ICompositionParent GetCompositionParent()
		//{
		//  return _parent;
		//}

		//public void SetCompositionParent(ICompositionParent parent)
		//{
		//  Contract.Assert(parent != null);
		//  CheckMutation();

		//  _parent = parent;
		//}

		/// <summary>
		/// Gets the model's state.
		/// </summary>
		/// <returns></returns>
		public ModelStates GetModelState() { return _state.CurrentState; }

		/// <summary>
		/// Indicates whether state has changed via mutation.
		/// </summary>
		/// <returns></returns>
		public bool HasMutations() { return _state.CurrentState.HasFlag(ModelStates.Dirty); }

		/// <summary>
		/// Gets the mutation record for a single property.
		/// </summary>
		/// <param name="property">the property</param>
		/// <returns></returns>
		public MutationKinds GetMutationForProperty(string property)
		{
			Contract.Assert(property != null);

			return PerformGetMutationForProperty(property);
		}

		/// <summary>
		/// Gets all mutation records related to the current instance.
		/// </summary>
		/// <returns>mutation records describing mutations since
		/// the last clean copy.</returns>
		public abstract IEnumerable<Mutation> GetMutations();

		/// <summary>
		/// Marks an instance as clean; clearing all mutation records.
		/// </summary>
		/// <param name="context">a marking context</param>
		public void MarkClean(IMarkingContext context)
		{
			Contract.Assert(context != null);
			if (!_state.CurrentState.HasFlag(ModelStates.Writing)) throw new ImmutableModelException();

			if (context.TryAddMark(this))
			{
				ExcludeModelStates(ModelStates.Dirty | ModelStates.DirtyComposition, false, true, context);
			}
		}

		/// <summary>
		/// Marks an instance as mutated.
		/// </summary>
		/// <param name="context">a marking context</param>
		/// <param name="kinds">the kind of mutation</param>
		public void MarkMutated(IMarkingContext context, MutationKinds kinds)
		{
			Contract.Assert(context != null);
			if (!_state.CurrentState.HasFlag(ModelStates.Writing)) throw new ImmutableModelException();

			if (context.TryAddMark(this))
			{
				var states = ModelStates.Immutable;
				if (kinds.HasFlag(MutationKinds.Direct))
					states = ModelStates.Dirty;
				if (kinds.HasFlag(MutationKinds.ByComposition))
					states |= ModelStates.DirtyComposition;
				IncludeModelStates(states, false, false, context);
			}
		}

		/// <summary>
		/// Gets the mutation record for a single property.
		/// </summary>
		/// <param name="property">the property</param>
		/// <returns></returns>
		protected abstract MutationKinds PerformGetMutationForProperty(string property);

		/// <summary>
		/// Checks whether the model is in a state that allows mutation.
		/// </summary>
		/// <exception cref="ImmutableModelException">thrown if the model cannot be mutated.</exception>
		protected void CheckMutation()
		{
			if (!_state.CurrentState.HasFlag(ModelStates.Writing))
				throw new ImmutableModelException();
		}
		/// <summary>
		/// Modifies the model's state by including the states given.
		/// </summary>
		/// <param name="states">the states to include</param>
		/// <param name="cascade">indicates whether the state should cascade to referents</param>
		/// <param name="context">a marking context</param>			
		public void IncludeModelStates(ModelStates states, bool cascade, IMarkingContext context)
		{
			Contract.Assert(context != null);
			if (!_state.CurrentState.HasFlag(ModelStates.Writing)
				&& !states.HasFlag(ModelStates.Writing)) throw new ImmutableModelException();

			IncludeModelStates(states, true, cascade, context);
		}
		/// <summary>
		/// Modifies the model's state by including the states given.
		/// </summary>
		/// <param name="states">the states to include</param>
		/// <param name="mark">indicates whether marking context should be used</param>
		/// <param name="cascade">indicates whether the state should cascade to referents</param>
		/// <param name="context">a marking context</param>					
		protected void IncludeModelStates(ModelStates states, bool mark, bool cascade, IMarkingContext context)
		{
			Contract.Assert(context != null);
			if (mark == false || context.TryAddMark(this))
			{
				_state.ChangeState(_state.CurrentState | states);
				if (cascade)
				{
					PerformCascadeIncludeModelStates(states, context);
				}
			}
		}

		/// <summary>
		/// Modifies the model's state by excluding the states given.
		/// </summary>
		/// <param name="states">the states to excluded</param>
		/// <param name="cascade">indicates whether the state should cascade to referents</param>
		/// <param name="context">a marking context</param>
		public void ExcludeModelStates(ModelStates states, bool cascade, IMarkingContext context)
		{
			Contract.Assert(context != null);
			if (!states.HasFlag(ModelStates.Writing)) throw new ImmutableModelException();

			ExcludeModelStates(states, true, cascade, context);
		}
		/// <summary>
		/// Modifies the model's state by excluding the states given.
		/// </summary>
		/// <param name="states">the states to include</param>
		/// <param name="mark">indicates whether marking context should be used</param>
		/// <param name="cascade">indicates whether the state should cascade to referents</param>
		/// <param name="context">a marking context</param>							
		protected void ExcludeModelStates(ModelStates states, bool mark, bool cascade, IMarkingContext context)
		{
			Contract.Assert(context != null);

			if (mark == false || context.TryAddMark(this))
			{
				_state.ChangeState(_state.CurrentState & ~states);
				if (cascade)
				{
					PerformCascadeExcludeModelStates(states, context);
				}
			}
		}

		/// <summary>
		/// Modifies the model's state to the states given.
		/// </summary>
		/// <param name="states">the states</param>
		/// <param name="cascade">indicates whether the state should cascade to referents</param>
		/// <param name="context">a marking context</param>			
		public void ChangeModelStates(ModelStates states, bool cascade, IMarkingContext context)
		{
			Contract.Assert(context != null);
			if (!_state.CurrentState.HasFlag(ModelStates.Writing)
					&& !states.HasFlag(ModelStates.Writing)) throw new ImmutableModelException();

			if (context.TryAddMark(this))
			{
				_state.ChangeState(states);
				if (cascade)
				{
					PerformCascadeChangeModelStates(states, context);
				}
			}
		}

		/// <summary>
		/// Called by the framework when subclasses should cascade model state to referenced models.
		/// </summary>
		/// <param name="states">model states to include</param>
		/// <param name="context">marking context</param>
		protected virtual void PerformCascadeIncludeModelStates(ModelStates states, IMarkingContext context)
		{ // Unless a Model is composed of other Models it won't need to cascade anything.
		}

		/// <summary>
		/// Called by the framework when subclasses should cascade model state to referenced models.
		/// </summary>
		/// <param name="states">model states to exclude</param>
		/// <param name="context">marking context</param>
		protected virtual void PerformCascadeExcludeModelStates(ModelStates states, IMarkingContext context)
		{ // Unless a Model is composed of other Models it won't need to cascade anything.
		}

		/// <summary>
		/// Called by the framework when subclasses should cascade model state to referenced models.
		/// </summary>
		/// <param name="states">model states</param>
		/// <param name="context">marking context</param>
		protected virtual void PerformCascadeChangeModelStates(ModelStates states, IMarkingContext context)
		{ // Unless a Model is composed of other Models it won't need to cascade anything.
		}

		/// <summary>
		/// Produces a copy of the current instance.
		/// </summary>
		/// <param name="context">a copy context</param>
		/// <returns>a copy of the current instance</returns>
		public abstract M Copy(ICopyContext context);

		/// <summary>
		/// Determines if the model is equal to another.
		/// </summary>
		/// <param name="other">the other model</param>
		/// <returns><em>true</em> if equal; otherwise <em>false</em></returns>
		public virtual bool Equals(M other)
		{
			return other is Model<M>
				&& ModelEquals(other as Model<M>);
		}

		/// <summary>
		/// Overridden by subclasses to determine if the model is equal to another.
		/// </summary>
		/// <param name="other">the other model</param>
		/// <returns><em>true</em> if equal; otherwise <em>false</em></returns>
		protected bool ModelEquals(Model<M> other)
		{
			return other != null
				&& _state.Equals(other._state);
				//&& _parent == other._parent;
		}

		/// <summary>
		/// Determines if the model is equal to another object.
		/// </summary>
		/// <param name="obj">the other object</param>
		/// <returns><em>true</em> if equal; otherwise <em>false</em></returns>
		public override bool Equals(object obj)
		{
			return obj is Model<M>
				&& ModelEquals((Model<M>)obj);
		}

		/// <summary>
		/// Gets the model's hashcode.
		/// </summary>
		/// <returns>a hashcode</returns>
		public override int GetHashCode()
		{
			int prime = Constants.RandomPrime;
			int result = CHashCodeSeed * prime;
			result ^= (int)_state.CurrentState * prime;
			//if (_parent != null)
			//{
			//  result ^= _parent.GetHashCode() * prime;
			//}
			return result;
		}

		/// <summary>
		/// Mutates the current instance; resulting in a new instance
		/// reflecting the mutation.
		/// </summary>
		/// <typeparam name="MD">mutation descriptor type M</typeparam>
		/// <param name="context">a mutation context</param>
		/// <param name="mutationDescriptor">mutation descriptor</param>
		/// <returns>a new instance reflecting the mutation</returns>
		public M Mutate<MD>(IMutationContext context, MD mutationDescriptor)
		{
			var copy = this.Copy(context);
			(copy as Model<M>).IncludeModelStates(ModelStates.Writing, true, new MarkingContext());
			PerformMutate(context, copy, mutationDescriptor);
			(copy as Model<M>).ExcludeModelStates(ModelStates.Writing, true, new MarkingContext());
			return copy;
		}

		/// <summary>
		/// Mutates the current instance; resulting in a new instance
		/// reflecting the mutation.
		/// </summary>
		/// <typeparam name="MD">mutation descriptor type M</typeparam>
		/// <param name="context">a mutation context</param>
		/// <param name="copy">a copy of the current model</param>
		/// <param name="mutationDescriptor">mutation descriptor</param>
		/// <returns>a new instance reflecting the mutation</returns>
		protected abstract void PerformMutate<MD>(IMutationContext context, M copy, MD mutationDescriptor);

		/// <summary>
		/// Gets the identity key for a referent.
		/// </summary>
		/// <typeparam name="IK">identity key type IK</typeparam>
		/// <param name="referentName">referent's name (according to model type M)</param>
		/// <returns>The identity key of the referent. If the referent's identity key is not known the result will be equal to default(IK).</returns>
		/// <exception cref="UndefinedReferentException">thrown if <paramref name="referentName"/> is undefined. A referent is the target of
		/// a reference to another model and the referent names are identical to the property name defined on model type M refering to the
		/// referenced model.</exception>
		public IK GetReferentID<IK>(string referentName)
		{
			Contract.Assert(referentName != null);
			Contract.Assert(referentName.Length > 0);
			return PerformGetReferentID<IK>(referentName);
		}

		/// <summary>
		/// Overriden by subclasses that have referents to get the identity key for a referent.
		/// </summary>
		/// <typeparam name="IK">identity key type IK</typeparam>
		/// <param name="referentName">referent's name (according to model type M)</param>
		/// <returns>The identity key of the referent. If the referent's identity key is not known the result will be equal to default(IK).</returns>
		/// <exception cref="UndefinedReferentException">thrown if <paramref name="referentName"/> is undefined. A referent is the target of
		/// a reference to another model and the referent names are identical to the property name defined on model type M refering to the
		/// referenced model.</exception>
		protected virtual IK PerformGetReferentID<IK>(string referentName)
		{
			throw new UndefinedReferentException(String.Format("Referent not found: ", referentName));
		}

		/// <summary>
		/// Gets the reference to a referent.
		/// </summary>
		/// <typeparam name="R">referent type R</typeparam>
		/// <param name="referentName">referent's name (according to model type M)</param>
		/// <returns>a reference to the referent</returns>
		/// <exception cref="UndefinedReferentException">thrown if <paramref name="referentName"/> is undefined. A referent is the target of
		/// a reference to another model and the referent names are identical to the property name defined on model type M refering to the
		/// referenced model.</exception>
		public IModelReference<R> GetReference<R>(string referentName)
		{
			Contract.Assert(referentName != null);
			Contract.Assert(referentName.Length > 0);
			return PerformGetReference<R>(referentName);
		}

		/// <summary>
		/// Overriden by subclasses that have referents to get the reference to a referent.
		/// </summary>
		/// <typeparam name="R">referent type R</typeparam>
		/// <param name="referentName">referent's name (according to model type M)</param>
		/// <returns>a reference to the referent</returns>
		/// <exception cref="UndefinedReferentException">thrown if <paramref name="referentName"/> is undefined. A referent is the target of
		/// a reference to another model and the referent names are identical to the property name defined on model type M refering to the
		/// referenced model.</exception>
		protected virtual IModelReference<R> PerformGetReference<R>(string referentName)
		{
			throw new UndefinedReferentException(String.Format("Referent not found: ", referentName));
		}

		/// <summary>
		/// Gets a referent.
		/// </summary>
		/// <typeparam name="R">referent type R</typeparam>
		/// <param name="referentName">referent's name (according to model type M)</param>
		/// <returns>the referent</returns>
		/// <exception cref="UndefinedReferentException">thrown if <paramref name="referentName"/> is undefined. A referent is the target of
		/// a reference to another model and the referent names are identical to the property name defined on model type M refering to the
		/// referenced model.</exception>
		public R GetReferent<R>(string referentName)
		{
			Contract.Assert(referentName != null);
			Contract.Assert(referentName.Length > 0);
			return PerformGetReferent<R>(referentName);
		}

		/// <summary>
		/// Overriden by subclasses that have referents to get a referent.
		/// </summary>
		/// <typeparam name="R">referent type R</typeparam>
		/// <param name="referentName">referent's name (according to model type M)</param>
		/// <returns>the referent</returns>
		/// <exception cref="UndefinedReferentException">thrown if <paramref name="referentName"/> is undefined. A referent is the target of
		/// a reference to another model and the referent names are identical to the property name defined on model type M refering to the
		/// referenced model.</exception>
		protected virtual R PerformGetReferent<R>(string referentName)
		{
			throw new UndefinedReferentException(String.Format("Referent not found: ", referentName));
		}

		/// <summary>
		/// Cascades a mutation to a referent.
		/// </summary>
		/// <typeparam name="R">referent type R</typeparam>
		/// <typeparam name="MD">mutation type MU</typeparam>
		/// <param name="referentName">referent's name (according to model type M)</param>
		/// <param name="context">a mutation context</param>
		/// <param name="mutationDescriptor">the mutation descriptor</param>
		/// <returns>the mutated referent</returns>
		/// <exception cref="UndefinedReferentException">thrown if <paramref name="referentName"/> is undefined. A referent is the target of
		/// a reference to another model and the referent names are identical to the property name defined on model type M refering to the
		/// referenced model.</exception>
		public R CascadeReferentMutation<R, MD>(string referentName, IMutationContext context, MD mutationDescriptor)
		{
			Contract.Assert(referentName != null);
			Contract.Assert(referentName.Length > 0);
			Contract.Assert(context != null);

			R referent = PerformGetReferent<R>(referentName);
			if (EqualityComparer<R>.Default.Equals(default(R), referent))
			{
				referent = context.Container.New<R>(context.LifespanTracking);
			}

			IModelSPI<R> spi = referent as IModelSPI<R>;
			if (spi != null)
			{
				return spi.Mutate(context, mutationDescriptor);
			}
			else if (referent is IModel)
			{
				return Model.Mutate(referent, mutationDescriptor);
			}
			return referent;
		}	
	}

}
