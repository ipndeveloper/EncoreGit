using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Common.Interfaces
{
	/// <summary>
	/// Allows a cache to be expired.
	/// </summary>
	public interface IExpireCache
	{
		/// <summary>
		/// Expires the cache.
		/// </summary>
		void ExpireCache();
	}
}
