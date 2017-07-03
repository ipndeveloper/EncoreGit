using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for AccountDevice.
	/// </summary>
	[ContractClass(typeof(Contracts.AccountDeviceContracts))]
	public interface IAccountDevice
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AccountDeviceID for this AccountDevice.
		/// </summary>
		int AccountDeviceID { get; set; }
	
		/// <summary>
		/// The AccountID for this AccountDevice.
		/// </summary>
		int AccountID { get; set; }
	
		/// <summary>
		/// The DeviceTypeID for this AccountDevice.
		/// </summary>
		short DeviceTypeID { get; set; }
	
		/// <summary>
		/// The DeviceID for this AccountDevice.
		/// </summary>
		string DeviceID { get; set; }
	
		/// <summary>
		/// The Active for this AccountDevice.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The DeviceType for this AccountDevice.
		/// </summary>
	    IDeviceType DeviceType { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The DeviceNotifications for this AccountDevice.
		/// </summary>
		IEnumerable<IDeviceNotification> DeviceNotifications { get; }
	
		/// <summary>
		/// Adds an <see cref="IDeviceNotification"/> to the DeviceNotifications collection.
		/// </summary>
		/// <param name="item">The <see cref="IDeviceNotification"/> to add.</param>
		void AddDeviceNotification(IDeviceNotification item);
	
		/// <summary>
		/// Removes an <see cref="IDeviceNotification"/> from the DeviceNotifications collection.
		/// </summary>
		/// <param name="item">The <see cref="IDeviceNotification"/> to remove.</param>
		void RemoveDeviceNotification(IDeviceNotification item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAccountDevice))]
		internal abstract class AccountDeviceContracts : IAccountDevice
		{
		    #region Primitive properties
		
			int IAccountDevice.AccountDeviceID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAccountDevice.AccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IAccountDevice.DeviceTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountDevice.DeviceID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IAccountDevice.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IDeviceType IAccountDevice.DeviceType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IDeviceNotification> IAccountDevice.DeviceNotifications
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAccountDevice.AddDeviceNotification(IDeviceNotification item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAccountDevice.RemoveDeviceNotification(IDeviceNotification item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
