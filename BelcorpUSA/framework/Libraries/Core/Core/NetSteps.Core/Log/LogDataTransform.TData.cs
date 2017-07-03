using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Reflection;

namespace NetSteps.Encore.Core.Log
{
	/// <summary>
	/// Static class used by logging to transform runtime types into textual data.
	/// </summary>
	/// <typeparam name="TData">data type TData</typeparam>
	public static class LogDataTransform<TData>
	{
		/// <summary>
		/// Transforms data into textual form suitable for the log.
		/// </summary>
		/// <param name="data">the data</param>
		/// <returns>a string representation</returns>
		public static string Transform(TData data)
		{
			// TODO: generate an inspector class with the ability to sanitize the data being logged.
			if (Object.Equals(default(TData), data))
			{
				return String.Concat("null data of type: ", typeof(TData).GetReadableSimpleName());
			}
			else
			{
				return data.ToJson();
			}
		}
	}
}
