using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using NetSteps.Addresses.Common.Models;

namespace NetSteps.Logistics.Common.Models
{
	/// <summary>
	/// Common interface for LogisticsProvider.
	/// </summary>
	[ContractClass(typeof(Contracts.LogisticsProviderContracts))]
	public interface ILogisticsProvider
	{
	    #region Primitive properties
	
		/// <summary>
		/// The LogisticsProviderID for this LogisticsProvider.
		/// </summary>
		short LogisticsProviderID { get; set; }
	
		/// <summary>
		/// The AddressID for this LogisticsProvider.
		/// </summary>
		Nullable<int> AddressID { get; set; }
	
		/// <summary>
		/// The Name for this LogisticsProvider.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The FaxNumber for this LogisticsProvider.
		/// </summary>
		string FaxNumber { get; set; }
	
		/// <summary>
		/// The EmailAddress for this LogisticsProvider.
		/// </summary>
		string EmailAddress { get; set; }
	
		/// <summary>
		/// The TermName for this LogisticsProvider.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this LogisticsProvider.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this LogisticsProvider.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Address for this LogisticsProvider.
		/// </summary>
	    IAddress Address { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(ILogisticsProvider))]
		internal abstract class LogisticsProviderContracts : ILogisticsProvider
		{
		    #region Primitive properties
		
			short ILogisticsProvider.LogisticsProviderID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ILogisticsProvider.AddressID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ILogisticsProvider.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ILogisticsProvider.FaxNumber
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ILogisticsProvider.EmailAddress
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ILogisticsProvider.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ILogisticsProvider.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ILogisticsProvider.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IAddress ILogisticsProvider.Address
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
