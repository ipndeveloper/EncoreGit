using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for DeviceType.
	/// </summary>
	[ContractClass(typeof(Contracts.DeviceTypeContracts))]
	public interface IDeviceType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The DeviceTypeID for this DeviceType.
		/// </summary>
		short DeviceTypeID { get; set; }
	
		/// <summary>
		/// The Name for this DeviceType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this DeviceType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this DeviceType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this DeviceType.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The AccountDevices for this DeviceType.
		/// </summary>
		IEnumerable<IAccountDevice> AccountDevices { get; }
	
		/// <summary>
		/// Adds an <see cref="IAccountDevice"/> to the AccountDevices collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountDevice"/> to add.</param>
		void AddAccountDevice(IAccountDevice item);
	
		/// <summary>
		/// Removes an <see cref="IAccountDevice"/> from the AccountDevices collection.
		/// </summary>
		/// <param name="item">The <see cref="IAccountDevice"/> to remove.</param>
		void RemoveAccountDevice(IAccountDevice item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IDeviceType))]
		internal abstract class DeviceTypeContracts : IDeviceType
		{
		    #region Primitive properties
		
			short IDeviceType.DeviceTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IDeviceType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IDeviceType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IDeviceType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IDeviceType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IAccountDevice> IDeviceType.AccountDevices
			{
				get { throw new NotImplementedException(); }
			}
		
			void IDeviceType.AddAccountDevice(IAccountDevice item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IDeviceType.RemoveAccountDevice(IAccountDevice item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
