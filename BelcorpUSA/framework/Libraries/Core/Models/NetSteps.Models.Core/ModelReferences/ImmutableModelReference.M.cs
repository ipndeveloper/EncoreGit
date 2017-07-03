using System.Collections.Generic;
using NetSteps.Encore.Core;

namespace NetSteps.Models.Core.ModelReferences
{
	/// <summary>
	/// Immutable reference to an immutable model.
	/// </summary>
	/// <typeparam name="M">model type M</typeparam>
	public sealed class ImmutableModelReference<M> : IModelReference<M>
	{
		static readonly int CHashCodeSeed = typeof(ImmutableModelReference<M>).GetKeyForType().GetHashCode();

		M _model;

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="model">the referenced model</param>
		public ImmutableModelReference(M model)
		{
			_model = model;
		}

		/// <summary>
		/// Indicates whether the reference is empty.
		/// </summary>
		public bool IsEmpty { get { return false; } }

		/// <summary>
		/// Indicates whether the reference has a model.
		/// </summary>
		public bool HasModel { get { return true; } }

		/// <summary>
		/// Indicates whether the reference has an identity key for the referenced model.
		/// </summary>
		public bool HasIdentityKey { get { return false; } }

		/// <summary>
		/// Gets the referenced model.
		/// </summary>
		public M Model { get { return _model; } }

		/// <summary>
		/// Gets a clone of the reference.
		/// </summary>
		/// <returns>a clone</returns>
		public object Clone()
		{
			return base.MemberwiseClone();
		}

		/// <summary>
		/// For non-empty references that don't yet have a model, resolves the referenced model
		/// and returns an updated reference.
		/// </summary>
		/// <returns></returns>
		public IModelReference<M> ResolveModel()
		{
			return new ImmutableModelReference<M>(_model);
		}

		/// <summary>
		/// For non-empty references that don't yet have a model, gets a resolver capable of
		/// resolving the referenced model. (Used by the framework).
		/// </summary>
		public IModelResolver<M> Resolver { get { return null; } }

		/// <summary>
		/// Determines if the reference is equal to another.
		/// </summary>
		/// <param name="other">the other reference</param>
		/// <returns><em>true</em> if equal; otherwise <em>false</em></returns>
		public bool Equals(ImmutableModelReference<M> other)
		{
			return other != null
				&& EqualityComparer<M>.Default.Equals(_model, other._model);
		}

		/// <summary>
		/// Determines if the reference is equal to another object.
		/// </summary>
		/// <param name="obj">the other object</param>
		/// <returns><em>true</em> if equal; otherwise <em>false</em></returns>
		public override bool Equals(object obj)
		{
			return obj is ImmutableModelReference<M>
				&& Equals((ImmutableModelReference<M>)obj);
		}

		/// <summary>
		/// Get's the reference's hashcode.
		/// </summary>
		/// <returns>a hashcode</returns>
		public override int GetHashCode()
		{
			int prime = Constants.RandomPrime;
			int result = CHashCodeSeed * prime;
			result ^= _model.GetHashCode() * prime;
			return result;
		}
	}

}
