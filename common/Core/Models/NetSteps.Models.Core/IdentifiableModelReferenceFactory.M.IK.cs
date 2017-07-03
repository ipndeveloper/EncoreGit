using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Models.Core.ModelReferences;

namespace NetSteps.Models.Core
{
	/// <summary>
	/// Base, abstract implementation of the IdentifiableModelReferenceFactory interface.
	/// </summary>
	/// <typeparam name="M">model type M</typeparam>
	/// <typeparam name="IK">identity key type IK</typeparam>
	public abstract class IdentifiableModelReferenceFactory<M, IK> : ModelReferenceFactory<M>, IIdentifiableModelReferenceFactory<M, IK>
	{
		/// <summary>
		/// Creates a new instance
		/// </summary>
		protected IdentifiableModelReferenceFactory()
			: base(true)
		{
		}
		/// <summary>
		/// Gets the identity key's type.
		/// </summary>
		public Type IdentityKeyType { get { return typeof(IK); } }

		/// <summary>
		/// Makes a new reference from a referent.
		/// </summary>
		/// <param name="model">the model/referent</param>
		/// <returns>a reference to the model</returns>
		public override IModelReference<M> MakeFromReferent(M model)
		{
			return new ImmutableIdentifiableModelReference<M, IK>(model);
		}

		/// <summary>
		/// Makes a reference from a model's identity key.
		/// </summary>
		/// <param name="id">an identity key</param>
		/// <returns>a reference to an model's identity key</returns>
		public IModelReference<M, IK> MakeFromReferentID(IK id)
		{
			return new UnresolvedIdentifiableModelReference<M, IK>(new RepositoryBackedModelResolver<M, IK>(id));
		}
	}
}
