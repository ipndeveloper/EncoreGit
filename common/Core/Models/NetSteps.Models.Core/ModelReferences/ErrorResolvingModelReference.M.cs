using System;
using System.Diagnostics.Contracts;
using NetSteps.Encore.Core;

namespace NetSteps.Models.Core.ModelReferences
{
	/// <summary>
	/// Reference indicating an error occurred when resolving a reference. Any
	/// reads against this reference rethrows the exception.
	/// </summary>
	/// <typeparam name="M"></typeparam>
	public sealed class ErrorResolvingModelReference<M> : IModelReference<M>
	{
		static readonly int CHashCodeSeed = typeof(ErrorResolvingModelReference<M>).GetKeyForType().GetHashCode();

		Exception _e;

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="e">an exception thrown during resolve</param>
		public ErrorResolvingModelReference(Exception e)
		{
			Contract.Requires<ArgumentNullException>(e != null);

			_e = e;
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
		public M Model { get { throw _e; } }

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
			return this;
		}

		/// <summary>
		/// For non-empty references that don't yet have a model, gets a resolver capable of
		/// resolving the referenced model. (Used by the framework).
		/// </summary>
		public IModelResolver<M> Resolver { get { return null; } }

		/// <summary>
		/// Compares the reference to another.
		/// </summary>
		/// <param name="other">the other</param>
		/// <returns><em>true</em> if equal; otherwise <em>false</em></returns>
		public bool Equals(ErrorResolvingModelReference<M> other)
		{
			return other != null
				&& _e == other._e;
		}

		/// <summary>
		/// Compares the reference to another object.
		/// </summary>
		/// <param name="obj">the other object</param>
		/// <returns><em>true</em> if equal; otherwise <em>false</em></returns>
		public override bool Equals(object obj)
		{
			return obj is ErrorResolvingModelReference<M>
				&& Equals((ErrorResolvingModelReference<M>)obj);
		}

		/// <summary>
		/// Gets a hashcode.
		/// </summary>
		/// <returns>the hashcode</returns>
		public override int GetHashCode()
		{
			int prime = Constants.RandomPrime;
			int result = CHashCodeSeed * prime;
			result ^= _e.GetHashCode() * prime;
			return result;
		}
	}

}
