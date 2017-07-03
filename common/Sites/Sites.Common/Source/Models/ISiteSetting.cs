using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Sites.Common.Models
{
	/// <summary>
	/// Common interface for SiteSetting.
	/// </summary>
	[ContractClass(typeof(Contracts.SiteSettingContracts))]
	public interface ISiteSetting
	{
	    #region Primitive properties
	
		/// <summary>
		/// The SiteSettingID for this SiteSetting.
		/// </summary>
		int SiteSettingID { get; set; }
	
		/// <summary>
		/// The BaseSiteID for this SiteSetting.
		/// </summary>
		Nullable<int> BaseSiteID { get; set; }
	
		/// <summary>
		/// The Name for this SiteSetting.
		/// </summary>
		string Name { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(ISiteSetting))]
		internal abstract class SiteSettingContracts : ISiteSetting
		{
		    #region Primitive properties
		
			int ISiteSetting.SiteSettingID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ISiteSetting.BaseSiteID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ISiteSetting.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
