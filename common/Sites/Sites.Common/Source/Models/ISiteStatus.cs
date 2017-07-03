using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Sites.Common.Models
{
	/// <summary>
	/// Common interface for SiteStatus.
	/// </summary>
	[ContractClass(typeof(Contracts.SiteStatusContracts))]
	public interface ISiteStatus
	{
	    #region Primitive properties
	
		/// <summary>
		/// The SiteStatusID for this SiteStatus.
		/// </summary>
		short SiteStatusID { get; set; }
	
		/// <summary>
		/// The Name for this SiteStatus.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this SiteStatus.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this SiteStatus.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this SiteStatus.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(ISiteStatus))]
		internal abstract class SiteStatusContracts : ISiteStatus
		{
		    #region Primitive properties
		
			short ISiteStatus.SiteStatusID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ISiteStatus.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ISiteStatus.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ISiteStatus.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ISiteStatus.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
