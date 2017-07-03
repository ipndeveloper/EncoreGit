using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core;

namespace NetSteps.Models.Core
{
	/// <summary>
	/// Resolver for models that are backed by a repository.
	/// </summary>
	/// <typeparam name="M">model type M</typeparam>
	/// <typeparam name="IK">identity key type IK</typeparam>
	public class RepositoryBackedModelResolver<M, IK> : IIdentifiableModelResolver<M, IK>
	{
		IK _key;
		Lazy<M> _model;

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="key">the identity key</param>
		public RepositoryBackedModelResolver(IK key)
		{
			_key = key;
			_model = new Lazy<M>(() => ResolveAgainstRepository(), LazyThreadSafetyMode.ExecutionAndPublication);
		}

		/// <summary>
		/// Gets the identity key this resolver can resolve.
		/// </summary>
		public IK IdentityKey { get { return _key; } }

		/// <summary>
		/// Resolves a referenced model instance.
		/// </summary>
		/// <returns>the referenced model</returns>
		public M Resolve()
		{
			return _model.Value;
		}

		M ResolveAgainstRepository()
		{
			using (var create = Create.SharedOrNewContainer())
			{
				var repo = create.New<IRepository<M, IK>>();
				return repo.Read(create, IdentityKey);
			}
		}
	}
}
