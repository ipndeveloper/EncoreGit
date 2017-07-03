using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Addresses.Common.Models
{
	/// <summary>
	/// Common interface for PhoneType.
	/// </summary>
	[ContractClass(typeof(Contracts.PhoneTypeContracts))]
	public interface IPhoneType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The PhoneTypeID for this PhoneType.
		/// </summary>
		int PhoneTypeID { get; set; }
	
		/// <summary>
		/// The Name for this PhoneType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this PhoneType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this PhoneType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this PhoneType.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IPhoneType))]
		internal abstract class PhoneTypeContracts : IPhoneType
		{
		    #region Primitive properties
		
			int IPhoneType.PhoneTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IPhoneType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IPhoneType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IPhoneType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IPhoneType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
