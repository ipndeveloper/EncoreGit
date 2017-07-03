using System;
using System.Linq;
using System.Diagnostics.Contracts;


namespace NetSteps.Encore.Core.IoC
{
	/// <summary>
	/// Various Type extensions.
	/// </summary>
	public static class TypeExtensions
	{
		/// <summary>
		/// Gets the fully qualified, human readable name for a delegate.
		/// </summary>
		/// <param name="d"></param>
		/// <returns></returns>
		public static string GetFullName(this Delegate d)
		{
			Contract.Requires<ArgumentNullException>(d != null);
			Contract.Assume(d.Target != null);
			Contract.Assume(d.Method != null);
			return String.Concat(d.Target.GetType().FullName, ".", d.Method.Name, "()");
		}
	}
}
