using System;

namespace NetSteps.Models.Core
{
	/// <summary>
	/// Base interface for model references.
	/// </summary>
	/// <typeparam name="M">model type M</typeparam>
	public interface IModelReference<out M> : ICloneable
	{
		/// <summary>
		/// Indicates whether the reference is empty.
		/// </summary>
		bool IsEmpty { get; }
		/// <summary>
		/// Indicates whether the reference has a model.
		/// </summary>
		bool HasModel { get; }
		/// <summary>
		/// Indicates whether the reference has an identity key for the referenced model.
		/// </summary>
		bool HasIdentityKey { get; }
		/// <summary>
		/// Gets the referenced model.
		/// </summary>
		M Model { get; }
		/// <summary>
		/// For non-empty references that don't yet have a model, resolves the referenced model
		/// and returns an updated reference.
		/// </summary>
		/// <returns></returns>
		IModelReference<M> ResolveModel();
		/// <summary>
		/// For non-empty references that don't yet have a model, gets a resolver capable of
		/// resolving the referenced model. (Used by the framework).
		/// </summary>
		IModelResolver<M> Resolver { get; }
	}

	/// <summary>
	/// Interface for references to identifiable models (those having an identity key).
	/// </summary>
	/// <typeparam name="M">model type M</typeparam>
	/// <typeparam name="IK">identity key type IK</typeparam>
	public interface IModelReference<out M, out IK> : IModelReference<M>
	{
		/// <summary>
		/// Gets the referenced model's identity key.
		/// </summary>
		IK IdentityKey { get; }
	}	
}
