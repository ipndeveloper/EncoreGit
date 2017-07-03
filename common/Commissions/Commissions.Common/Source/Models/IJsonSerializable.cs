using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Probably defined somewhere else - but I'm adding it here now for convenience.
	/// </summary>
	public interface IJsonSerializable
	{
		/// <summary>
		/// To the json.
		/// </summary>
		/// <returns></returns>
		string ToJson();
	}
}
