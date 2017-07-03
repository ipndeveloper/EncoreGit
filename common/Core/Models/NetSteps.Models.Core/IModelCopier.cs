using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Models.Core
{
	/// <summary>
	/// Makes copies of model type M
	/// </summary>
	/// <typeparam name="M">model type M</typeparam>
	public interface IModelCopier<in M>
	{
		/// <summary>
		/// Produces a copy of model type M.
		/// </summary>
		/// <param name="context">a copy context</param>
		/// <param name="source">the model being copied</param>
		/// <returns>a copy of the source model</returns>
		object MakeCopy(ICopyContext context, M source);
	}
}
