using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BelcorpUSA.Edi.Common.Orders
{
	public abstract class EdiX12List<T> : List<T>
	{
		/// <summary>
		/// ISA09 - Interchange Date
		/// </summary> 
		public DateTime InterchangeDateUTC { get; set; }

		/// <summary>
		/// ISA13 - Interchange Control Number - Uniquely identifies an interchange for tracking by trading partners and VANS, and can be used to detect duplicate, missing, or out of sequence transmissions. May take any numeric value, but is usually incremented by one for each interchange sent to the same trading partner.
		/// </summary>
		public int InterchangeControlNumber { get; set; }

		/// <summary>
		/// ISA05 - Qualifier to designate the system/method of code structure used to designate the sender or receiver ID element being qualified
		/// <example>ZZ - Mutually Defined</example>
		/// </summary>
		public string SenderIdQualifier { get; set; }

		/// <summary>
		/// ISA06 - Interchange Sender ID - Identifies the sender.
		/// </summary>
		public string SenderId { get; set; }

		/// <summary>
		/// ISA05 - Qualifier to designate the system/method of code structure used to designate the sender or receiver ID element being qualified
		/// <example>ZZ - Mutually Defined</example>
		/// </summary>
		public string RecieverIdQualifier { get; set; }

		/// <summary>
		/// ISA08 - Interchange Receiver ID - Identifies the receiver
		/// </summary>
		public string RecieverId { get; set; }

		/// <summary>
		/// File name that this record was read from or stored to.
		/// </summary>
		public virtual string FileName { get; set; }
	}
}
