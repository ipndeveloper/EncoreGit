using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web.Mvc;
using NetSteps.Addresses.Common.Models;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Mvc.Controls.Models;
using Newtonsoft.Json;

namespace nsDistributor.Models.Checkout
{
	public class ShippingModel
	{
		#region Properties
		public ShippingAddressModel OverrideDefaultAddress { get; set; }
		public IEnumerable<ShippingAddressModel> Addresses { get; set; }
		public bool PickupPointsEnabled { get; set; }
		public IEnumerable<ShippingPickupPointModel> PickupPoints { get; set; }
		public ShippingAddressModel NewAddress { get; set; }
		public short OrderTypeId { get; set; }
        public bool ShowShipToEmail { get; set; }

		public ShippingAddressModel DefaultAddress
		{
			get
			{
				return OverrideDefaultAddress
					?? Addresses.FirstOrDefault(x => x.IsDefault)
					?? Addresses.FirstOrDefault(x => x.AddressTypeID == (int)Constants.AddressType.Main)
					?? Addresses.FirstOrDefault(x=> x.AddressTypeID == (int)Constants.AddressType.Shipping)
					?? Addresses.FirstOrDefault();
			}
		}

		public AddressModel AddressModel { get; set; }
		#endregion

		#region Constructors
		public ShippingModel()
		{
			Addresses = new List<ShippingAddressModel>();
			NewAddress = new ShippingAddressModel();
			PickupPoints = new List<ShippingPickupPointModel>();
		}
		#endregion

		#region Methods
		public IEnumerable<IAddress> GetAvailableAddresses(NetSteps.Data.Entities.Account account, Order order)
		{
			Contract.Requires<ArgumentException>(account != null, "Account must not be null");
			Contract.Requires<ArgumentException>(order != null, "Order must not be null");

			var shippingAddresses = this.GetShippingAddresses(account);
			var otherAddresses = this.GetMainAddresses(account);
			var distinctAddresses = new List<IAddress>();

			distinctAddresses.AddRangeDistinct(shippingAddresses);
			distinctAddresses.AddRangeDistinct(otherAddresses);

			return distinctAddresses;
		}

		public IEnumerable<IAddress> GetMainAddresses(NetSteps.Data.Entities.Account account)
		{
			return this.GetAddressByAddressType(account, Constants.AddressType.Main);
		}

		public IEnumerable<IAddress> GetShippingAddresses(NetSteps.Data.Entities.Account account)
		{
			return this.GetAddressByAddressType(account, Constants.AddressType.Shipping);
		}

		public IEnumerable<IAddress> GetAddressByAddressType(NetSteps.Data.Entities.Account account, Constants.AddressType addressType)
		{
			return account == null ? new List<IAddress>() : account.Addresses.Where<IAddress>(a => a.AddressTypeID == (int)addressType);
		}

		public IAddress GetDefaultShippingForParentOrder(int? parentOrderId)
		{
			return parentOrderId.HasValue ? Order.GetDefaultShippingAddress(parentOrderId.Value) : null;
		}

		public MvcHtmlString GetJSModelDataJSON()
		{
			return MvcHtmlString.Create(JsonConvert.SerializeObject(new
			{
				PickupPoints = this.PickupPoints,
				PickupPointsEnabled = this.PickupPointsEnabled
			}));
		}
		#endregion
	}
}