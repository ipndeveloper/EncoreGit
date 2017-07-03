using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace NetSteps.Core.Cache
{
	/// <summary>
	/// Enum that indicates a resolution kind.
	/// </summary>
	[Flags]
	public enum ResolutionManyKind
	{
		/// <summary>
		/// None; the items were not resolved.
		/// </summary>
		None = 0,
		/// <summary>
		/// Indicates that all of the items were resolved.
		/// </summary>
		Resolved = 1,
		/// <summary>
		/// Indicates the item was created.
		/// </summary>
		Created = 2 | Resolved,
		/// <summary>
		/// Indicates that some of the items were resolved.
		/// </summary>
		PartiallyResolved = 4 | Created
	}

	/// <summary>
	/// Interface for objects that resolve many items that are missing from the cache.
	/// </summary>
	/// <typeparam name="K">key type K</typeparam>
	/// <typeparam name="R">representation type R</typeparam>
	[ContractClass(typeof(CodeContracts.CotnractForICacheManyItemResolver<,>))]
	public interface ICacheManyItemResolver<K, R> : ICacheItemResolver<K, R>
	{
		/// <summary>
		/// Tries to resolve many items that are missing from the cache.
		/// </summary>
		/// <param name="keys">the item's key</param>
		/// <param name="values">variable to hold the item upon success</param>
		/// <returns>the resolution kind</returns>
		/// <see cref="ResolutionKind"/>
		ResolutionManyKind TryResolveMany(IEnumerable<K> keys, out IEnumerable<KeyValuePair<K, R>> values);
	}

	namespace CodeContracts
	{
		/// <summary>
		/// CodeContracts Class for ICacheManyItemResolver&lt;,>
		/// </summary>
		/// <typeparam name="K">Key type</typeparam>
		/// <typeparam name="R">representation type</typeparam>
		[ContractClassFor(typeof(ICacheManyItemResolver<,>))]
		public abstract class CotnractForICacheManyItemResolver<K, R> : ICacheManyItemResolver<K, R>
		{
			/// <summary>
			/// 
			/// </summary>
			/// <param name="keys"></param>
			/// <param name="values"></param>
			/// <returns></returns>
			public ResolutionManyKind TryResolveMany(IEnumerable<K> keys, out IEnumerable<KeyValuePair<K, R>> values)
			{
				Contract.Requires<ArgumentNullException>(keys != null);
				Contract.Requires<ArgumentOutOfRangeException>(keys.Any());

				throw new NotImplementedException();
			}

			/// <summary>
			/// 
			/// </summary>
			public int AttemptCount
			{
				get { throw new NotImplementedException(); }
			}

			/// <summary>
			/// 
			/// </summary>
			public int ResolveCount
			{
				get { throw new NotImplementedException(); }
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="key"></param>
			/// <param name="value"></param>
			/// <returns></returns>
			public ResolutionKind TryResolve(K key, out R value)
			{
				throw new NotImplementedException();
			}
		}
	}
}
