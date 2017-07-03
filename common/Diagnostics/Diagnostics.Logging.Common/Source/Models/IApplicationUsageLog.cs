using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Diagnostics.Logging.Common.Models
{
	/// <summary>
	/// Common interface for ApplicationUsageLog.
	/// </summary>
	[ContractClass(typeof(Contracts.ApplicationUsageLogContracts))]
	public interface IApplicationUsageLog
	{
	    #region Primitive properties
	
		/// <summary>
		/// The ApplicationUsageLogID for this ApplicationUsageLog.
		/// </summary>
		int ApplicationUsageLogID { get; set; }
	
		/// <summary>
		/// The ApplicationID for this ApplicationUsageLog.
		/// </summary>
		short ApplicationID { get; set; }
	
		/// <summary>
		/// The UserID for this ApplicationUsageLog.
		/// </summary>
		Nullable<int> UserID { get; set; }
	
		/// <summary>
		/// The UsageDateUTC for this ApplicationUsageLog.
		/// </summary>
		System.DateTime UsageDateUTC { get; set; }
	
		/// <summary>
		/// The AssemblyName for this ApplicationUsageLog.
		/// </summary>
		string AssemblyName { get; set; }
	
		/// <summary>
		/// The MachineName for this ApplicationUsageLog.
		/// </summary>
		string MachineName { get; set; }
	
		/// <summary>
		/// The ClassName for this ApplicationUsageLog.
		/// </summary>
		string ClassName { get; set; }
	
		/// <summary>
		/// The MethodName for this ApplicationUsageLog.
		/// </summary>
		string MethodName { get; set; }
	
		/// <summary>
		/// The MillisecondDuration for this ApplicationUsageLog.
		/// </summary>
		Nullable<double> MillisecondDuration { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IApplicationUsageLog))]
		internal abstract class ApplicationUsageLogContracts : IApplicationUsageLog
		{
		    #region Primitive properties
		
			int IApplicationUsageLog.ApplicationUsageLogID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IApplicationUsageLog.ApplicationID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IApplicationUsageLog.UserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime IApplicationUsageLog.UsageDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IApplicationUsageLog.AssemblyName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IApplicationUsageLog.MachineName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IApplicationUsageLog.ClassName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IApplicationUsageLog.MethodName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<double> IApplicationUsageLog.MillisecondDuration
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
