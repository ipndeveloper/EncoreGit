using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Common.Models
{
	/// <summary>
	/// Common interface for ApplicationRunningInstance.
	/// </summary>
	[ContractClass(typeof(Contracts.ApplicationRunningInstanceContracts))]
	public interface IApplicationRunningInstance
	{
	    #region Primitive properties
	
		/// <summary>
		/// The ApplicationRunningInstanceID for this ApplicationRunningInstance.
		/// </summary>
		int ApplicationRunningInstanceID { get; set; }
	
		/// <summary>
		/// The ApplicationID for this ApplicationRunningInstance.
		/// </summary>
		short ApplicationID { get; set; }
	
		/// <summary>
		/// The MachineName for this ApplicationRunningInstance.
		/// </summary>
		string MachineName { get; set; }
	
		/// <summary>
		/// The IpAddress for this ApplicationRunningInstance.
		/// </summary>
		string IpAddress { get; set; }
	
		/// <summary>
		/// The StartDateUTC for this ApplicationRunningInstance.
		/// </summary>
		System.DateTime StartDateUTC { get; set; }
	
		/// <summary>
		/// The LastPingDateUTC for this ApplicationRunningInstance.
		/// </summary>
		System.DateTime LastPingDateUTC { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Application for this ApplicationRunningInstance.
		/// </summary>
	    IApplication Application { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IApplicationRunningInstance))]
		internal abstract class ApplicationRunningInstanceContracts : IApplicationRunningInstance
		{
		    #region Primitive properties
		
			int IApplicationRunningInstance.ApplicationRunningInstanceID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IApplicationRunningInstance.ApplicationID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IApplicationRunningInstance.MachineName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IApplicationRunningInstance.IpAddress
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime IApplicationRunningInstance.StartDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime IApplicationRunningInstance.LastPingDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IApplication IApplicationRunningInstance.Application
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
