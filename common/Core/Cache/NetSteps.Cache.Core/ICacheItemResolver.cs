using System.Collections.Concurrent;
using System;
using System.Diagnostics.Contracts;

namespace NetSteps.Core.Cache
{
	/// <summary>
	/// Enum that indicates a resolution kind.
	/// </summary>
	[Flags]
	public enum ResolutionKind
	{
		/// <summary>
		/// None; the item was not resolved.
		/// </summary>
		None = 0,
		/// <summary>
		/// Indicates the item was resolved.
		/// </summary>
		Resolved = 1,
		/// <summary>
		/// Indicates the item was created.
		/// </summary>
		Created = Resolved | 2
	}

	/// <summary>
	/// Interface for objects that resolve items that are missing from the cache.
	/// </summary>
	/// <typeparam name="K">key type K</typeparam>
	/// <typeparam name="R">representation type R</typeparam>
	[ContractClass(typeof(CodeContracts.ContractForICacheItemResolver<,>))]
	public interface ICacheItemResolver<K, R>
	{
		/// <summary>
		/// Gets the number of items attempted.
		/// </summary>
		int AttemptCount { get; }

		/// <summary>
		/// Gets the number of items resolved.
		/// </summary>
		int ResolveCount { get; }

		/// <summary>
		/// Tries to resolve an item that is missing from the cache.
		/// </summary>
		/// <param name="key">the item's key</param>
		/// <param name="value">variable to hold the item upon success</param>
		/// <returns>the resolution kind</returns>
		/// <see cref="ResolutionKind"/>
		ResolutionKind TryResolve(K key, out R value);
	}

	namespace CodeContracts
	{
		/// <summary>
		/// Contract class for ICacheItemResolve&lt;,>
		/// </summary>
		/// <typeparam name="K">The key</typeparam>
		/// <typeparam name="R">The value representation</typeparam>
		[ContractClassFor(typeof(ICacheItemResolver<,>))]
		public abstract class ContractForICacheItemResolver<K, R> : ICacheItemResolver<K, R>
		{
			/// <summary>
			/// Gets the number of items attempted.
			/// </summary>
			public int AttemptCount
			{
				get { throw new NotImplementedException(); }
			}

			/// <summary>
			/// Gets the number of items resolved.
			/// </summary>
			public int ResolveCount
			{
				get { throw new NotImplementedException(); }
			}

			/// <summary>
			/// Tries to resolve an item that is missing from the cache.
			/// </summary>
			/// <param name="key">the item's key</param>
			/// <param name="value">variable to hold the item upon success</param>
			/// <returns>the resolution kind</returns>
			/// <see cref="ResolutionKind"/>
			public ResolutionKind TryResolve(K key, out R value)
			{
				Contract.Requires<ArgumentNullException>(key != null);

				throw new NotImplementedException();
			}
		}
	}
}
