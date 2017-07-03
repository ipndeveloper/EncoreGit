using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Models.Core
{
	/// <summary>
	/// Interface for classes that can resolve references to models of type M
	/// </summary>
	/// <typeparam name="M">model type M</typeparam>
	public interface IModelResolver<out M>
	{
		/// <summary>
		/// Resolves a referenced model instance.
		/// </summary>
		/// <returns>the referenced model</returns>
		M Resolve();
	}	
}
