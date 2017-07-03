using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace NetSteps.Models.Core.ModelReferences
{
	/// <summary>
	/// Immutable reference to an immutable model.
	/// </summary>
	/// <typeparam name="M">model type M</typeparam>
	/// <typeparam name="IK">identity key type IK</typeparam>
	public sealed class ImmutableIdentifiableModelReference<M, IK> : IModelReference<M, IK>
	{
		M _model;

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="model">the model</param>
		public ImmutableIdentifiableModelReference(M model)
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
		public bool HasIdentityKey { get { return true; } }

		/// <summary>
		/// Gets the referenced model.
		/// </summary>
		public M Model { get { return _model; } }

		/// <summary>
		/// Gets the referenced model's identity key.
		/// </summary>
		public IK IdentityKey
		{
			get
			{
				return ((IIdentifiable<IK>)_model).GetIdentityKey();
			}
		}

		/// <summary>
		/// Clones the instance.
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
			return new ImmutableIdentifiableModelReference<M, IK>(_model);
		}

		/// <summary>
		/// For non-empty references that don't yet have a model, gets a resolver capable of
		/// resolving the referenced model. (Used by the framework).
		/// </summary>
		public IModelResolver<M> Resolver { get { return null; } }
	}

}
