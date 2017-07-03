using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for HostessRewardType.
	/// </summary>
	[ContractClass(typeof(Contracts.HostessRewardTypeContracts))]
	public interface IHostessRewardType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The HostessRewardTypeID for this HostessRewardType.
		/// </summary>
		int HostessRewardTypeID { get; set; }
	
		/// <summary>
		/// The Name for this HostessRewardType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this HostessRewardType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this HostessRewardType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this HostessRewardType.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IHostessRewardType))]
		internal abstract class HostessRewardTypeContracts : IHostessRewardType
		{
		    #region Primitive properties
		
			int IHostessRewardType.HostessRewardTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHostessRewardType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHostessRewardType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IHostessRewardType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IHostessRewardType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
