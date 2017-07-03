using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Models.Core
{
	/// <summary>
	/// Interface indicating that a Model may be part of a composition.
	/// Used by the framework to indicate that an instance is part of
	/// a composition and to communicate the parents to the child.
	/// </summary>
	public interface IComposable
	{
		/// <summary>
		/// Gets the instance's composition parent.
		/// </summary>
		/// <returns></returns>
		ICompositionParent GetCompositionParent();
		/// <summary>
		/// Used by the framework to indicate that an instance is part
		/// of a composition.
		/// </summary>
		/// <param name="parent">the instance's parent</param>
		void SetCompositionParent(ICompositionParent parent);
	}
}
