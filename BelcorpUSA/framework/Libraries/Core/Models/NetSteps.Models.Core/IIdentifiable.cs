using System;

namespace NetSteps.Models.Core
{
	/// <summary>
	/// Base interface for models that are uniquely identifiable by an identity key.
	/// This interface is used by the framework to work with models in an the abstract.
	/// </summary>
	public interface IIdentifiable
	{
		/// <summary>
		/// Gets the identifiable's identity key type.
		/// </summary>
		/// <returns></returns>
		Type GetIdentityKeyType();
		/// <summary>
		/// Gets the identifiable's identity key.
		/// </summary>
		/// <returns></returns>
		object GetUntypedIdentityKey();
	}

	/// <summary>
	/// Interface for models that are uniquely identifiable by identity key type IK
	/// </summary>
	/// <typeparam name="IK">identity key type IK</typeparam>
	public interface IIdentifiable<out IK> : IIdentifiable
	{
		/// <summary>
		/// Gets the instance's unique identity.
		/// </summary>
		/// <returns>the instance's identity key</returns>
		IK GetIdentityKey();
	}
}
