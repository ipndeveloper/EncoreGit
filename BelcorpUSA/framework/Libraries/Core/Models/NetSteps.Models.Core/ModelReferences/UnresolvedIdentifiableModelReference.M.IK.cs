using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace NetSteps.Models.Core.ModelReferences
{
	/// <summary>
	/// Reference to a model that has not yet been resolved.
	/// </summary>
	/// <typeparam name="M">model type M</typeparam>
	/// <typeparam name="IK">identity key type IK</typeparam>
	public sealed class UnresolvedIdentifiableModelReference<M, IK> : IModelReference<M, IK>
	{
		IIdentifiableModelResolver<M, IK> _resolver;

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="resolver">a resolver</param>
		public UnresolvedIdentifiableModelReference(IIdentifiableModelResolver<M, IK> resolver)
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
		public bool HasModel { get { return true; } }

		/// <summary>
		/// Indicates whether the reference has an identity key for the referenced model.
		/// </summary>
		public bool HasIdentityKey { get { return true; } }

		/// <summary>
		/// Gets the referenced model.
		/// </summary>
		public M Model { get { throw new NotImplementedException(); } }

		/// <summary>
		/// Gets the model's identity key.
		/// </summary>
		public IK IdentityKey { get { return _resolver.IdentityKey; } }

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
				return new ImmutableIdentifiableModelReference<M, IK>(resolved);
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
	}
}
