using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Diagnostics.CodeAnalysis;

namespace NetSteps.Core.Cache
{
	/// <summary>
	/// Represents a list that will refresh its contents on a given interval
	/// </summary>
	/// <typeparam name="V">The type in the list</typeparam>
	public interface ICachedList<V> : IList<V>
	{
		/// <summary>
		/// The List's name. 
		/// </summary>
		string Name { get; }

		/// <summary>
		/// The total number of refresshes performed by this list
		/// </summary>
		long TotalRefreshes { get; }

		/// <summary>
		/// Indicates if the list is currently refreshing.
		/// </summary>
		bool IsRefreshing { get; }

		/// <summary>
		/// The TimeSpan from this point in time that the List should refresh.
		/// </summary>
		TimeSpan RefreshesIn { get; }

		/// <summary>
		/// The DateTime after which the List should refresh.
		/// </summary>
		DateTime RefreshesAfter { get; }

		/// <summary>
		/// The DateTime the List last refreshed.
		/// </summary>
		DateTime LastRefreshedOn { get; }

		/// <summary>
		/// The TimeSpan interval in which the List refreshes.
		/// </summary>
		TimeSpan RefreshInterval { get; set; }
	}
}
