using System;
using System.Diagnostics.Contracts;
using NetSteps.Encore.Core;

namespace NetSteps.Models.Core.ModelReferences
{
	/// <summary>
	/// Reference to a model that has not yet been resolved.
	/// </summary>
	/// <typeparam name="M">model type M</typeparam>
	public sealed class UnresolvedModelReference<M> : IModelReference<M>
	{
		static readonly int CHashCodeSeed = typeof(UnresolvedModelReference<M>).GetKeyForType().GetHashCode();

		IModelResolver<M> _resolver;

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="resolver">a resolver</param>
		public UnresolvedModelReference(IModelResolver<M> resolver)
		{
			Contract.Requires<ArgumentNullException>(resolver != null);

			_resolver = resolver;
		}

		/// <summary>
		/// Indicates whether the reference is empty.
		/// </summary>
		public bool IsEmpty { get { return false; } }

		/// <summary>
		/// Indicates whether the reference has a model.
		/// </summary>
		public bool HasModel { get { return false; } }

		/// <summary>
		/// Indicates whether the reference has an identity key for the referenced model.
		/// </summary>
		public bool HasIdentityKey { get { return false; } }

		/// <summary>
		/// Gets the referenced model.
		/// </summary>
		public M Model { get { throw new NotImplementedException(); } }

		/// <summary>
		/// Clones the reference.
		/// </summary>
		/// <returns></returns>
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
			try
			{
				M resolved = _resolver.Resolve();
				return new ImmutableModelReference<M>(resolved);
			}
			catch (Exception e)
			{
				return new ErrorResolvingModelReference<M>(e);
			}
		}

		/// <summary>
		/// For non-empty references that don't yet have a model, gets a resolver capable of
		/// resolving the referenced model. (Used by the framework).
		/// </summary>
		public IModelResolver<M> Resolver { get { return _resolver; } }

		/// <summary>
		/// Determines if the reference is equal to another.
		/// </summary>
		/// <param name="other">the other reference</param>
		/// <returns><em>true</em> if equal; otherwise <em>false</em></returns>
		public bool Equals(UnresolvedModelReference<M> other)
		{
			return other != null
				&& _resolver == other._resolver;
		}

		/// <summary>
		/// Determines if the reference is equal to another object.
		/// </summary>
		/// <param name="obj">the other object</param>
		/// <returns><em>true</em> if equal; otherwise <em>false</em></returns>
		public override bool Equals(object obj)
		{
			return obj is UnresolvedModelReference<M>
				&& Equals((UnresolvedModelReference<M>)obj);
		}

		/// <summary>
		/// Get's the reference's hashcode.
		/// </summary>
		/// <returns>a hashcode</returns>
		public override int GetHashCode()
		{
			int prime = Constants.RandomPrime;
			int result = CHashCodeSeed * prime;
			result ^= _resolver.GetHashCode() * prime;
			return result;
		}
	}

}
