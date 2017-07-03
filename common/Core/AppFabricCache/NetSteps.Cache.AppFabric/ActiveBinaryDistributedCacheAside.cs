using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Core.Cache;
using NetSteps.Encore.Core.Log;
using System.Diagnostics.Contracts;
using System.Diagnostics;

namespace NetSteps.Cache.AppFabric
{
	public sealed class ActiveBinaryDistributedCacheAside<K, R> : BinaryDistributedCacheAside<K, R>
		where R : class//, IEquatable<R>
	{
		#region Fields

		static readonly ILogSink __log = typeof(ActiveBinaryDistributedCacheAside<K, R>).GetLogSink();

		#endregion

		#region Properties

		private ICacheItemResolver<K, R> Resolver { get; set; }

		protected override ILogSink Log { get { return __log; } }

		#endregion

		#region Constructors

		public ActiveBinaryDistributedCacheAside(string cacheName, ICacheItemResolver<K, R> resolver)
			: this(cacheName, resolver, null)
		{ }

		public ActiveBinaryDistributedCacheAside(string cacheName, ICacheItemResolver<K, R> resolver, MruCacheOptions options)
			: base(cacheName, options)
		{
			Contract.Requires<ArgumentNullException>(resolver != null);

			Resolver = resolver;
		}

		#endregion

		#region Methods

		protected override ResolutionKind TryResolveMissingItem(K key, out R value)
		{
			return Resolver.TryResolve(key, out value);
		}

		protected override ResolutionManyKind TryResolveManyMissingItems(IEnumerable<K> keys, out IEnumerable<KeyValuePair<K, R>> values)
		{
			if (Resolver is ICacheManyItemResolver<K, R>)
				return ((ICacheManyItemResolver<K, R>)Resolver).TryResolveMany(keys, out values);
			else
			{
				if (Log.IsLogging(SourceLevels.Warning))
					Log.Warning("TryResolveManyMissingItems called on Cache named '{0}' when no ICacheManyItemResolver provided.  Reverting to 1 to 1 resolution, this may impact performance, consider providing ICacheManyItemResolver instead.", Name);

				var result = true;
				var resolved = new List<KeyValuePair<K, R>>(keys.Count());
				
				foreach (var key in keys)
				{
					R item;
					if (TryResolveMissingItem(key, out item).HasFlag(ResolutionKind.Resolved))
						resolved.Add(new KeyValuePair<K, R>(key, item));
					else
						result = false;
				}
				
				values = resolved;

				if (result)
					return ResolutionManyKind.Created;
				else if (values.Any())
					return ResolutionManyKind.PartiallyResolved;
				else
					return ResolutionManyKind.None;
			}
		}

		#endregion
	}
}
