using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Data.Entities.Interfaces;

namespace NetSteps.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
	using NetSteps.Addresses.Common.Models;
	using NetSteps.Common;
    using NetSteps.Common.Base;
    using NetSteps.Common.Configuration;
    using NetSteps.Common.Extensions;
    using NetSteps.Common.Globalization;
    using NetSteps.Common.Interfaces;
    using NetSteps.Common.Validation.NetTiers;
    using NetSteps.Data.Entities.Business;
    using NetSteps.Data.Entities.Cache;
    using NetSteps.Data.Entities.Exceptions;
    using NetSteps.Data.Entities.Extensions;
    using NetSteps.Encore.Core.IoC;

    public partial class OrderShipment : IShippingAddress, ITempGuid, IDateLastModified
    {
		public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public string ShippingMethodName
        {
            get
            {
                var inventory = Create.New<InventoryBaseRepository>();

                if (OrderCustomer != null)
                {

                    if (!OrderCustomer.OrderItems.Any(
                            oi => inventory.GetProduct(oi.ProductID.Value).ProductBase.IsShippable))
                    {
                        return Translation.GetTerm("NotApplicable", "N/A");
                    }
                }
                else
                {
                    if (!Order.OrderCustomers.SelectMany(oc => oc.OrderItems).Any(
                            oi => inventory.GetProduct(oi.ProductID.Value).ProductBase.IsShippable))
                    {
                        return Translation.GetTerm("NotApplicable", "N/A");
                    }
                }

                if (ShippingMethodID.HasValue)
                {
                    string shippingMethodName = string.Empty; // shipment.Name should be the name of the person the package is shipping to. - JHE
                    var shippingMethod = SmallCollectionCache.Instance.ShippingMethods.GetById(ShippingMethodID.Value);
                    if (shippingMethod != null)
                        shippingMethodName = shippingMethod.Name;

                    return ShippingMethodID.HasValue ? SmallCollectionCache.Instance.ShippingMethodTranslations.GetTranslatedName(this.ShippingMethodID.ToInt(), shippingMethodName) : string.Empty;
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// This will default the AllowPOBoxShipment to what the Web.Config is using.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public bool ValidateShippingMethod(BasicResponse response)
        {
            return ValidateShippingMethod(response, ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.AllowPOBoxShipment, false));
        }

        public bool ValidateShippingMethod(BasicResponse response, bool allowPOBoxShipment)
        {
            bool allowPOBoxShippingMethod = false;
            response.Success = true;

            if (allowPOBoxShipment && this.ShippingMethod != null)
            {
                allowPOBoxShippingMethod = this.ShippingMethod.AllowPoBox;
            }
            else if (allowPOBoxShipment && this.ShippingMethodID.HasValue)
            {
                ShippingMethod method = SmallCollectionCache.Instance.ShippingMethods.GetById(this.ShippingMethodID.Value);

				allowPOBoxShippingMethod = method != null && method.ShippingMethodID > 0 ? method.AllowPoBox : false;
			}

			if (!allowPOBoxShipment || (allowPOBoxShipment && !allowPOBoxShippingMethod))
			{
				bool isPoBoxAddress = IsPoBoxAddress();

				if (isPoBoxAddress)
				{
					response.Message = Translation.GetTerm("InvalidShippingAddressTheAddressCannotBeAPOBox", CustomValidationMessages.ShippingAddressCannotBeAPOBox);
				}

				response.Success = !isPoBoxAddress;
			}

			return response.Success;
		}

		public bool IsPoBoxAddress()
		{
			return IsPoBox(this.Address1) || IsPoBox(this.Address2) || IsPoBox(this.Address3); ;
		}

		internal bool IsPoBox(string p)
		{
			if (p.IsNullOrWhiteSpace())
				return false;
			return Regex.IsMatch(p, RegularExpressions.PoBox);
		}

        public static IEnumerable<Order> GetOrderShippingDetails(IEnumerable<string> orderNumbers)
        {
            try
            {
                return BusinessLogic.GetOrderShippingDetails(orderNumbers);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static IPaginatedList<OrderShippingSearchData> Search(OrderShipmentSearchParameters searchParameters)
        {
            return BusinessLogic.Search(Repository, searchParameters);
        }

        public static IList<OrderShippingSearchData> GetOrderShippingSearchData(IEnumerable<int> orderIDs)
        {
            return Repository.GetOrderShippingSearchData(orderIDs);
        }

        internal bool IsShippable()
        {
            bool result = false;

            if (this.OrderCustomer != null)
            {
                result = this.OrderCustomer.ContainsShippableItems();
            }
            else
            {
                Order order = this.Order;

                if (order.OrderShipments == null)
                {
                    order = Order.LoadFull(order.OrderID);
                }

                List<int> directShipOCIDs = order.OrderShipments.Where(os => os.IsDirectShipment && os.OrderCustomerID.HasValue).Select(os => os.OrderCustomerID.Value).ToList();
                foreach (var orderCustomer in order.OrderCustomers.Where(oc => !directShipOCIDs.Contains(oc.OrderCustomerID)))
                {
                    if (orderCustomer.ContainsShippableItems())
                    {
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }

        public static OrderShipment LoadFullByShipmentID(int shipmentID)
        {
            return Repository.LoadFullByShipmentID(shipmentID);
        }

        public static string GetBaseTrackUrl(int shippingMethodID, string trackingNumber)
        {
            return BusinessLogic.GetBaseTrackUrl(shippingMethodID, trackingNumber);
        }

        public static short CalculateOrderShipmentStatus(OrderShipment orderShipment)
        {
            return BusinessLogic.CalculateOrderShipmentStatus(orderShipment);
        }

        #region IAddress
        int IAddress.AddressID
        {
            get
            {
                return this.SourceAddressID ?? 0;
            }
			set { }
        }
        string IAddress.FirstName
        {
            get
            {
                return this.FirstName;
            }
            set
            {
                this.FirstName = value;
            }
        }
        string IAddress.LastName
        {
            get
            {
                return this.LastName;
            }
            set
            {
                this.LastName = value;
            }
        }
        string IAddress.Name
        {
            get
            {
                return this.Name;
            }
            set
            {
                this.Name = value;
            }
        }
        string IAddress.Attention
        {
            get
            {
                return this.Attention;
            }
            set
            {
                this.Attention = value;
            }
        }
        string IAddressBasic.Address1
        {
            get
            {
                return this.Address1;
            }
            set
            {
                this.Address1 = value;
            }
        }
        string IAddressBasic.Address2
        {
            get
            {
                return this.Address2;
            }
            set
            {
                this.Address2 = value;
            }
        }
        string IAddressBasic.Address3
        {
            get
            {
                return this.Address3;
            }
            set
            {
                this.Address3 = value;
            }
        }
        string IAddressBasic.City
        {
            get
            {
                return this.City;
            }
            set
            {
                this.City = value;
            }
        }
		string IAddressBasic.County
		{
			get
			{
				return this.County;
			}
			set
			{
				this.County = value;
			}
		}
		string IAddressBasic.State
        {
            get
            {
                return this.State;
            }
            set
            {
                this.State = value;
            }
        }
        int? IAddress.StateProvinceID
        {
            get
            {
                return this.StateProvinceID;
            }
            set
            {
                this.StateProvinceID = value;
            }
        }
        string IAddressBasic.PostalCode
        {
            get
            {
                return this.PostalCode;
            }
            set
            {
                this.PostalCode = value;
            }
        }
        string IAddress.PhoneNumber
        {
            get
            {
                return this.DayPhone;
            }
            set
            {
                this.DayPhone = value;
            }
        }
        string IAddressBasic.Country
        {
            get
            {
				var country = SmallCollectionCache.Instance.Countries.GetById((this as IAddress).CountryID);
				if (country != null)
					return country.GetTerm();
				else
					return string.Empty;
			}
        }
        int IAddress.CountryID
        {
            get
            {
                return this.CountryID;
            }
            set
            {
                this.CountryID = value;
            }
        }
        short IAddress.AddressTypeID
        {
            get
            {
                return (short)Constants.AddressType.Shipping;
            }
            set
            {
                // This AddressTypeID should always be 'Shipping' for this Entity. - JHE
            }
        }
		string IAddress.CountryCode
		{
			get
			{
				return SmallCollectionCache.Instance.Countries.GetById(this.CountryID).CountryCode;
			}
		}
        bool IAddress.IsDefault
        {
            get
            {
                return false;
            }
            set
            {
            }
        }
        bool IAddress.IsWillCall
        {
            get
            {
                return this.IsWillCall;
            }
            set
            {
            }
        }
        int IAddress.ProfileID
        {
            get
            {
                return 0;
            }
            set
            {
            }
        }
        string IAddress.ProfileName
        {
            get;
            set;
        }
		string IAddress.StateProvinceAbbreviation
		{
			get
			{
				var thisAsIAddress = this as IAddress;
				if (thisAsIAddress.StateProvinceID.HasValue && thisAsIAddress.StateProvinceID > 0)
				{
					var stateProvince = SmallCollectionCache.Instance.StateProvinces.GetById(thisAsIAddress.StateProvinceID.Value);
					return stateProvince.StateAbbreviation;
				}

				return string.Empty;
			}
		}
		#endregion

        #region ITempGuid Members

        private Guid? _guid = null;
        public Guid Guid
        {
            get
            {
                if (_guid == null)
                    _guid = Guid.NewGuid();
                return _guid.Value;
            }
            internal set
            {
                _guid = value;
            }
        }

        #endregion



        string IShippingAddress.ShipToEmailAddress
        {
            get { return Email; }
            set
            {
                Email = value;
            }
        }
    }
}
