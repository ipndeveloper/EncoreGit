using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Models.Core
{
	/// <summary>
	/// Interfaces for collaborations between Models that have a parent-child 
	/// relationship. This interface is implemented when a Model is a composition
	/// parent.
	/// </summary>
	public interface ICompositionParent
	{
		/// <summary>
		/// Marks the parent as being dirty by virtue of a change
		/// in a child's state.
		/// </summary>
		void MarkDirtyByComposition();
	}
}
