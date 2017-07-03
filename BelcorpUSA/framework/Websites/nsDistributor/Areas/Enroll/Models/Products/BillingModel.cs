using System;
using System.Collections.Generic;
using System.Web.Mvc;
using NetSteps.Addresses.Common.Models;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Generated;
using NetSteps.Enrollment.Common.Models.Context;
using NetSteps.Web.Mvc.Extensions;
using nsDistributor.Areas.Enroll.Models.Shared;

namespace nsDistributor.Areas.Enroll.Models.Products
{
	public class BillingModel : SectionModel
	{
		#region Values
		protected int BillingAddressSourceTypeIDTypeID;
		public virtual int BillingAddressSourceTypeID
		{
			get
			{
				return this.BillingAddressSourceTypeIDTypeID;
			}
			set
			{
				// These are the only allowed types
				this.BillingAddressSourceTypeIDTypeID = (value == (int)ConstantsGenerated.AddressType.Main ||
														 value == (int)ConstantsGenerated.AddressType.Shipping)
					? value
					: (int)ConstantsGenerated.AddressType.Billing;
			}
		}
		#endregion

		#region Resources
		public virtual MvcHtmlString MainAddressHtml { get; set; }
		public virtual MvcHtmlString ShippingAddressHtml { get; set; }
		public virtual bool HideBillingAddress { get; set; }
		#endregion

		#region Models
		public virtual BasicAddressModel BillingAddress { get; set; }
		public virtual PaymentMethodModel PaymentMethod { get; set; }
		#endregion

		#region Infrastructure
		public BillingModel()
		{
			this.BillingAddressSourceTypeIDTypeID = (int)ConstantsGenerated.AddressType.Main;
			this.BillingAddress = new BasicAddressModel();
			this.PaymentMethod = new PaymentMethodModel();
		}

		public virtual BillingModel LoadValues(
			int billingAddressSourceTypeID,
			int countryID,
			int languageID,
			IAddress billingAddress,
			IPayment paymentMethod)
		{
			Constants.AddressType addressType;
			if (!Enum.TryParse(billingAddressSourceTypeID.ToString(), out addressType))
			{
				throw new Exception("Address Type doesn't exist");
			}

			this.BillingAddressSourceTypeID = billingAddressSourceTypeID;

			this.BillingAddress.LoadValues(
				countryID,
				billingAddress,
				this.BillingAddressSourceTypeID == (int)ConstantsGenerated.AddressType.Billing
			);

			this.PaymentMethod.LoadValues(
				paymentMethod,
				languageID
			);

			return this;
		}

		public virtual BillingModel LoadResources(
			IAddress mainAddress,
			IAddress shippingAddress,
			IEnumerable<PaymentType> paymentTypes,
			bool hideBillingAddress)
		{
			this.HideBillingAddress = hideBillingAddress;

			this.MainAddressHtml = mainAddress
				.ToDisplay(IAddressExtensions.AddressDisplayTypes.Web)
				.ToMvcHtmlString();

			this.ShippingAddressHtml = shippingAddress
				.ToDisplay(IAddressExtensions.AddressDisplayTypes.Web)
				.ToMvcHtmlString();

			this.PaymentMethod.LoadResources(
				paymentTypes
			);

			return this;
		}

		public virtual BillingModel ApplyTo(IEnrollmentContext enrollmentContext)
		{
			enrollmentContext.BillingAddressSourceTypeID = this.BillingAddressSourceTypeID;

			return this;
		}

		public virtual BillingModel ApplyTo(
			Address address,
			Address mainAddress,
			Address shippingAddress)
		{
			if (this.BillingAddressSourceTypeID == (int)ConstantsGenerated.AddressType.Main)
			{
				new BasicAddressModel()
					.LoadValues(
						mainAddress.CountryID,
						mainAddress)
					.ApplyTo(address);
			}
			else if (this.BillingAddressSourceTypeID == (int)ConstantsGenerated.AddressType.Shipping)
			{
				new BasicAddressModel()
					.LoadValues(
						shippingAddress.CountryID,
						shippingAddress)
					.ApplyTo(address);
			}
			else
			{
				this.BillingAddress.ApplyTo(address);
			}

			return this;
		}

		public virtual BillingModel ApplyTo(
			IPayment paymentMethod)
		{
			this.PaymentMethod.ApplyTo(paymentMethod);

			return this;
		}
		#endregion
	}
}