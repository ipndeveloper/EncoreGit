using System;

namespace NetSteps.Models.Core
{
	/// <summary>
	/// Interface for working with model references.
	/// </summary>
	/// <typeparam name="M">model type M</typeparam>
	/// <typeparam name="IK">identity key type IK</typeparam>
	public interface IIdentifiableModelReferenceFactory<M, IK> : IModelReferenceFactory<M>
	{
		/// <summary>
		/// Gets the identity key's type.
		/// </summary>
		Type IdentityKeyType { get; }
		/// <summary>
		/// Makes a reference from a model's identity key.
		/// </summary>
		/// <param name="id">an identity key</param>
		/// <returns>a reference to an model's identity key</returns>
		IModelReference<M, IK> MakeFromReferentID(IK id);
	}
}
