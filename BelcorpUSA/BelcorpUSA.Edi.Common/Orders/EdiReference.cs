using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BelcorpUSA.Edi.Common.Orders
{
	/// <summary>
	/// REF - Reference Identification
	/// </summary>
	public class EdiReference
	{
		public string IdentificationQualifier { get; set; }

		public string Identification { get; set; }
	}
}
