using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NetSteps.Encore.Core;
using NetSteps.Encore.Core.IoC;
using NetSteps.Models.Core.ModelReferences;

namespace NetSteps.Models.Core
{	
	/// <summary>
	/// Context object used during the framework's copy
	/// logic to communicate the container and detect cycles
	/// and multiple references in the graph.
	/// </summary>
	public class CopyContext : ICopyContext
	{
		ConcurrentDictionary<object, ModelRecord> _copies = new ConcurrentDictionary<object, ModelRecord>();

		/// <summary>
		/// Constructs new instance using the container given.
		/// </summary>
		/// <param name="container">an IoC container</param>
		public CopyContext(IContainer container)
			: this(container, LifespanTracking.Default)
		{
			Contract.Requires<ArgumentNullException>(container != null);
		}
		/// <summary>
		/// Constructs new instance using the container given.
		/// </summary>
		/// <param name="container">an IoC container</param>
		/// <param name="tracking">lifespan tracking for copies made within the context.</param>
		public CopyContext(IContainer container, LifespanTracking tracking)
		{
			Contract.Requires<ArgumentNullException>(container != null);
			this.Container = container;
			this.LifespanTracking = tracking;
		}

		/// <summary>
		/// Gets the IoC container scope associated with the context.
		/// </summary>
		public IContainer Container { get; private set; }

		/// <summary>
		/// Gets the lifespan tracking that should be used for copies made within the context.
		/// </summary>
		public LifespanTracking LifespanTracking { get; private set; }

		/// <summary>
		/// Gets or adds a copy of source model M.
		/// </summary>
		/// <typeparam name="M">model type M</typeparam>
		/// <param name="model">source model M</param>
		/// <param name="copier">model copier for model type M</param>
		/// <returns>a copy of the source model</returns>
		public M GetOrAddCopy<M>(M model, IModelCopier<M> copier)
		{
			Contract.Assert(copier != null);
			ModelRecord record = _copies.GetOrAdd(model, unused =>
			{
				return new ModelRecord<M>((M)copier.MakeCopy(this, model));
			});
			return (M)record.UntypedModel;
		}

		/// <summary>
		/// Gets or adds a copy of identifiable source model M.
		/// </summary>
		/// <typeparam name="M">model type M</typeparam>
		/// <typeparam name="IK">identity key type IK</typeparam>
		/// <param name="model">source model M</param>
		/// <param name="key">identity key for the source model.</param>
		/// <param name="copier">model copier for model type M</param>
		/// <returns>a copy of the source model</returns>		
		public M GetOrAddIdentifiable<M, IK>(M model, IK key, IModelCopier<M> copier)
		{
			var identifiable = model as IIdentifiable<IK>;

			var mkey = new ModelAndKey<M, IK>(identifiable.GetIdentityKey());
			ModelRecord record;
			if (!_copies.TryGetValue(model, out record))
			{
				ModelRecord added = null;
				record = _copies.GetOrAdd(mkey, unused =>
				{
					added = new ModelRecord<M, IK>((M)copier.MakeCopy(this, model));
					return added;
				});
				// by ref as well as key...
				if (added != null)
				{
					_copies.TryAdd(model, added);
				}
			}
			return (M)record.UntypedModel;
		}		

		/// <summary>
		/// Gets or adds a model reference.
		/// </summary>
		/// <typeparam name="M">model type M</typeparam>
		/// <param name="reference">the reference</param>
		/// <returns>the copy</returns>
		public IModelReference<M> GetOrAddReference<M>(IModelReference<M> reference)
		{
			if (reference.IsEmpty) return reference;

			if (reference.HasIdentityKey)
			{
				return WarnAndConvertToIdentifiableReference(reference);
			}
			else if (reference.HasModel)
			{
				M model = reference.Model.Copy(this);
				ModelRecord record;
				_copies.TryGetValue(model, out record);
				return (IModelReference<M>)record.UntypedReference;
			}
			else
			{
				ModelRecord record = _copies.GetOrAdd(reference, unused =>
				{
					return new UnresolvedModelRecord<M>(reference.Resolver);
				});
				return (IModelReference<M>)record.UntypedReference;
			}
		}

		/// <summary>
		/// Gets or adds a model reference.
		/// </summary>
		/// <typeparam name="M">model type M</typeparam>
		/// <typeparam name="IK">identity key type IK</typeparam>
		/// <param name="reference">the reference</param>
		/// <returns>the copy</returns>
		public IModelReference<M, IK> GetOrAddIdentifiableReference<M, IK>(IModelReference<M, IK> reference)
		{
			if (reference.IsEmpty) return reference;

			if (reference.HasModel)
			{
				M model = reference.Model;
				var identifiable = model as IIdentifiable<IK>;
				var mkey = new ModelAndKey<M, IK>(identifiable.GetIdentityKey());
				ModelRecord record;
				if (!_copies.TryGetValue(mkey, out record))
				{
					model = reference.Model.Copy(this);
					_copies.TryGetValue(mkey, out record);
				}
				return (IModelReference<M, IK>)record.UntypedReference;
			}
			else
			{
				ModelRecord record = _copies.GetOrAdd(reference, unused =>
				{
					return new UnresolvedModelRecord<M, IK>((IIdentifiableModelResolver<M, IK>)reference.Resolver);
				});
				return (IModelReference<M, IK>)record.UntypedReference;
			}
		}

		private IModelReference<M> WarnAndConvertToIdentifiableReference<M>(IModelReference<M> reference)
		{
			throw new NotImplementedException();
		}

		class ModelAndKey<M, IK> : IEquatable<ModelAndKey<M, IK>>
		{
			static readonly IEqualityComparer<IK> KeyComparer = EqualityComparer<IK>.Default;
			static readonly int CHashCodeSeed = typeof(M).GetKeyForType().GetHashCode() * Constants.RandomPrime;
			static readonly string CModelName = typeof(M).GetKeyForType();

			IK _key;

			public ModelAndKey(IK key)
			{
				_key = key;
			}
			public string ModelName { get { return CModelName; } }
			public IK Key { get { return _key; } }

			public bool Equals(ModelAndKey<M, IK> other)
			{
				return other != null
					&& KeyComparer.Equals(other._key, _key);
			}

			public override bool Equals(object obj)
			{
				return obj is ModelAndKey<M, IK>
					&& Equals((ModelAndKey<M, IK>)obj);
			}

			public override int GetHashCode()
			{
				int prime = Constants.RandomPrime;
				int result = CHashCodeSeed * prime;
				result ^= _key.GetHashCode() * prime;
				return result;
			}
		}

		abstract class ModelRecord
		{
			public abstract object UntypedModel { get; }
			public virtual object UntypedKey { get { return null; } }
			public virtual object UntypedReference { get { return null; } }
		}

		class ModelRecord<M> : ModelRecord
		{
			public ModelRecord(M model)
				: this(model, new ImmutableModelReference<M>(model))
			{
			}
			public ModelRecord(M model, IModelReference<M> reference)
			{
				this.Model = model;
				this.Reference = reference;
			}
			public M Model { get; private set; }
			public IModelReference<M> Reference { get; private set; }

			public override object UntypedModel { get { return Model; } }
			public override object UntypedReference { get { return Reference; } }
		}

		class UnresolvedModelRecord<M> : ModelRecord
		{
			IModelResolver<M> _resolver;
			public UnresolvedModelRecord(IModelResolver<M> resolver)
			{
				_resolver = resolver;
				Reference = new UnresolvedModelReference<M>(resolver);
			}
			public M Model { get { return _resolver.Resolve(); } }
			public IModelReference<M> Reference { get; private set; }

			public override object UntypedModel { get { return Model; } }
			public override object UntypedReference { get { return Reference; } }
		}

		class ModelRecord<M, IK> : ModelRecord
		{
			public ModelRecord(M model)
				: this(model, new ImmutableIdentifiableModelReference<M, IK>(model))
			{
			}
			public ModelRecord(M model, IModelReference<M> reference)
			{
				this.Model = model;
				this.Reference = reference;
			}
			public M Model { get; private set; }
			public IModelReference<M> Reference { get; private set; }

			public override object UntypedModel { get { return Model; } }
			public override object UntypedReference { get { return Reference; } }
			public override object UntypedKey { get { return ((IIdentifiable<IK>)Model).GetIdentityKey(); } }
		}

		class UnresolvedModelRecord<M, IK> : ModelRecord
		{
			IIdentifiableModelResolver<M, IK> _resolver;
			public UnresolvedModelRecord(IIdentifiableModelResolver<M, IK> resolver)
			{
				_resolver = resolver;
				Reference = new UnresolvedIdentifiableModelReference<M, IK>(resolver);
			}
			public M Model { get { return _resolver.Resolve(); } }
			public IModelReference<M> Reference { get; private set; }
			public IK Key { get { return _resolver.IdentityKey; } }

			public override object UntypedModel { get { return Model; } }
			public override object UntypedReference { get { return Reference; } }
			public override object UntypedKey { get { return Key; } }
		}
	}

}
