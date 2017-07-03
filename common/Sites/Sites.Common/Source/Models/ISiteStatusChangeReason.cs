using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Sites.Common.Models
{
	/// <summary>
	/// Common interface for SiteStatusChangeReason.
	/// </summary>
	[ContractClass(typeof(Contracts.SiteStatusChangeReasonContracts))]
	public interface ISiteStatusChangeReason
	{
	    #region Primitive properties
	
		/// <summary>
		/// The SiteStatusChangeReasonID for this SiteStatusChangeReason.
		/// </summary>
		short SiteStatusChangeReasonID { get; set; }
	
		/// <summary>
		/// The Name for this SiteStatusChangeReason.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The Description for this SiteStatusChangeReason.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Editable for this SiteStatusChangeReason.
		/// </summary>
		bool Editable { get; set; }
	
		/// <summary>
		/// The Active for this SiteStatusChangeReason.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(ISiteStatusChangeReason))]
		internal abstract class SiteStatusChangeReasonContracts : ISiteStatusChangeReason
		{
		    #region Primitive properties
		
			short ISiteStatusChangeReason.SiteStatusChangeReasonID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ISiteStatusChangeReason.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ISiteStatusChangeReason.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ISiteStatusChangeReason.Editable
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ISiteStatusChangeReason.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
