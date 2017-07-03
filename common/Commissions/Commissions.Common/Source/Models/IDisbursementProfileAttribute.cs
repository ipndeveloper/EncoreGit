using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for DisbursementProfileAttribute.
	/// </summary>
	[ContractClass(typeof(Contracts.DisbursementProfileAttributeContracts))]
	public interface IDisbursementProfileAttribute
	{
	    #region Primitive properties
	
		/// <summary>
		/// The DisbursementProfileID for this DisbursementProfileAttribute.
		/// </summary>
		int DisbursementProfileID { get; set; }
	
		/// <summary>
		/// The DisbursementAttributeID for this DisbursementProfileAttribute.
		/// </summary>
		int DisbursementAttributeID { get; set; }
	
		/// <summary>
		/// The Value for this DisbursementProfileAttribute.
		/// </summary>
		string Value { get; set; }
	
		/// <summary>
		/// The UserID for this DisbursementProfileAttribute.
		/// </summary>
		Nullable<int> UserID { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The DisbursementAttribute for this DisbursementProfileAttribute.
		/// </summary>
	    IDisbursementAttribute DisbursementAttribute { get; set; }
	
		/// <summary>
		/// The DisbursementProfile for this DisbursementProfileAttribute.
		/// </summary>
	    IDisbursementProfile DisbursementProfile { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IDisbursementProfileAttribute))]
		internal abstract class DisbursementProfileAttributeContracts : IDisbursementProfileAttribute
		{
		    #region Primitive properties
		
			int IDisbursementProfileAttribute.DisbursementProfileID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IDisbursementProfileAttribute.DisbursementAttributeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IDisbursementProfileAttribute.Value
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IDisbursementProfileAttribute.UserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IDisbursementAttribute IDisbursementProfileAttribute.DisbursementAttribute
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IDisbursementProfile IDisbursementProfileAttribute.DisbursementProfile
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
