using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Sites.Common.Models
{
	/// <summary>
	/// Common interface for SiteSettingValue.
	/// </summary>
	[ContractClass(typeof(Contracts.SiteSettingValueContracts))]
	public interface ISiteSettingValue
	{
	    #region Primitive properties
	
		/// <summary>
		/// The SiteSettingValueID for this SiteSettingValue.
		/// </summary>
		int SiteSettingValueID { get; set; }
	
		/// <summary>
		/// The SiteSettingID for this SiteSettingValue.
		/// </summary>
		int SiteSettingID { get; set; }
	
		/// <summary>
		/// The SiteID for this SiteSettingValue.
		/// </summary>
		int SiteID { get; set; }
	
		/// <summary>
		/// The Value for this SiteSettingValue.
		/// </summary>
		string Value { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(ISiteSettingValue))]
		internal abstract class SiteSettingValueContracts : ISiteSettingValue
		{
		    #region Primitive properties
		
			int ISiteSettingValue.SiteSettingValueID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ISiteSettingValue.SiteSettingID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ISiteSettingValue.SiteID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ISiteSettingValue.Value
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
