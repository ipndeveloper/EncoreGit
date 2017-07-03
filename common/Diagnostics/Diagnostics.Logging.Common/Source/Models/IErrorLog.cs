using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Diagnostics.Logging.Common.Models
{
	/// <summary>
	/// Common interface for ErrorLog.
	/// </summary>
	[ContractClass(typeof(Contracts.ErrorLogContracts))]
	public interface IErrorLog
	{
	    #region Primitive properties
	
		/// <summary>
		/// The ErrorLogID for this ErrorLog.
		/// </summary>
		int ErrorLogID { get; set; }
	
		/// <summary>
		/// The SessionID for this ErrorLog.
		/// </summary>
		string SessionID { get; set; }
	
		/// <summary>
		/// The ApplicationID for this ErrorLog.
		/// </summary>
		Nullable<short> ApplicationID { get; set; }
	
		/// <summary>
		/// The AccountID for this ErrorLog.
		/// </summary>
		Nullable<int> AccountID { get; set; }
	
		/// <summary>
		/// The UserID for this ErrorLog.
		/// </summary>
		Nullable<int> UserID { get; set; }
	
		/// <summary>
		/// The OrderID for this ErrorLog.
		/// </summary>
		Nullable<int> OrderID { get; set; }
	
		/// <summary>
		/// The LogDateUTC for this ErrorLog.
		/// </summary>
		System.DateTime LogDateUTC { get; set; }
	
		/// <summary>
		/// The MachineName for this ErrorLog.
		/// </summary>
		string MachineName { get; set; }
	
		/// <summary>
		/// The ExceptionTypeName for this ErrorLog.
		/// </summary>
		string ExceptionTypeName { get; set; }
	
		/// <summary>
		/// The Source for this ErrorLog.
		/// </summary>
		string Source { get; set; }
	
		/// <summary>
		/// The Message for this ErrorLog.
		/// </summary>
		string Message { get; set; }
	
		/// <summary>
		/// The PublicMessage for this ErrorLog.
		/// </summary>
		string PublicMessage { get; set; }
	
		/// <summary>
		/// The Form for this ErrorLog.
		/// </summary>
		string Form { get; set; }
	
		/// <summary>
		/// The QueryString for this ErrorLog.
		/// </summary>
		string QueryString { get; set; }
	
		/// <summary>
		/// The TargetSite for this ErrorLog.
		/// </summary>
		string TargetSite { get; set; }
	
		/// <summary>
		/// The StackTrace for this ErrorLog.
		/// </summary>
		string StackTrace { get; set; }
	
		/// <summary>
		/// The Referrer for this ErrorLog.
		/// </summary>
		string Referrer { get; set; }
	
		/// <summary>
		/// The BrowserInfo for this ErrorLog.
		/// </summary>
		string BrowserInfo { get; set; }
	
		/// <summary>
		/// The UserHostAddress for this ErrorLog.
		/// </summary>
		string UserHostAddress { get; set; }
	
		/// <summary>
		/// The IsUnhandledExeption for this ErrorLog.
		/// </summary>
		Nullable<bool> IsUnhandledExeption { get; set; }
	
		/// <summary>
		/// The InternalMessage for this ErrorLog.
		/// </summary>
		string InternalMessage { get; set; }
	
		/// <summary>
		/// The ApplicationPoolName for this ErrorLog.
		/// </summary>
		string ApplicationPoolName { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IErrorLog))]
		internal abstract class ErrorLogContracts : IErrorLog
		{
		    #region Primitive properties
		
			int IErrorLog.ErrorLogID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IErrorLog.SessionID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<short> IErrorLog.ApplicationID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IErrorLog.AccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IErrorLog.UserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IErrorLog.OrderID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime IErrorLog.LogDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IErrorLog.MachineName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IErrorLog.ExceptionTypeName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IErrorLog.Source
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IErrorLog.Message
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IErrorLog.PublicMessage
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IErrorLog.Form
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IErrorLog.QueryString
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IErrorLog.TargetSite
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IErrorLog.StackTrace
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IErrorLog.Referrer
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IErrorLog.BrowserInfo
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IErrorLog.UserHostAddress
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<bool> IErrorLog.IsUnhandledExeption
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IErrorLog.InternalMessage
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IErrorLog.ApplicationPoolName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
