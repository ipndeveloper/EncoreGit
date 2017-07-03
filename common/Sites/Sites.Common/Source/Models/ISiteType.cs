using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Sites.Common.Models
{
	/// <summary>
	/// Common interface for SiteType.
	/// </summary>
	[ContractClass(typeof(Contracts.SiteTypeContracts))]
	public interface ISiteType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The SiteTypeID for this SiteType.
		/// </summary>
		short SiteTypeID { get; set; }
	
		/// <summary>
		/// The Name for this SiteType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this SiteType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this SiteType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this SiteType.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(ISiteType))]
		internal abstract class SiteTypeContracts : ISiteType
		{
		    #region Primitive properties
		
			short ISiteType.SiteTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ISiteType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ISiteType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ISiteType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ISiteType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
