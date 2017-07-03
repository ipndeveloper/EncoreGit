using System;
using System.Linq;
using NetSteps.Addresses.Common.Models;
using NetSteps.Common.Exceptions;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Taxes.Common.Models
{
	public static class Extensions
	{
		/// <summary>
		/// Transforms a calculation fault into a business exception.
		/// </summary>
		/// <param name="fault"></param>
		/// <returns></returns>
		public static Exception ToBusinessException(this ITaxCalculationFault fault)
		{
			if (fault == null)
			{
				return new NetStepsBusinessException("Did not receive tax calculation from external calculator.");
			}

			if (String.IsNullOrWhiteSpace(fault.Code))
			{
				return new NetStepsBusinessException(fault.Message);
			}

			if (String.IsNullOrWhiteSpace(fault.Message))
			{
				return new NetStepsBusinessException(String.Concat("Tax calculation resulted in an error code: ", fault.Code));
			}

			return new NetStepsBusinessException(String.Concat(fault.Message, Environment.NewLine, "Code: ", fault.Code));
		}

		/// <summary>
		/// Gets an item's calculated tax by jurisdiction level.
		/// </summary>
		/// <param name="item">the order item</param>
		/// <param name="applicability">the tax' applicability</param>
		/// <param name="levels">the jurisdiction levels of interest</param>
		/// <returns>the applicable tax</returns>
		public static decimal GetCalculatedTaxByLevel(this ITaxOrderItem item,
			params JurisdictionLevel[] levels)
		{
			return item.Quantity * item.Taxes
				.Where(it => levels.Contains(it.Jurisdiction.Level))
				.Sum(it => it.CalculatedTax);
		}

		/// <summary>
		/// Gets an item's effective rate by jurisdiction level.
		/// </summary>
		/// <param name="item">the order item</param>
		/// <param name="applicability">the tax' applicability</param>
		/// <param name="levels">the jurisdiction levels of interest</param>
		/// <returns>the applicable tax rate</returns>
		public static decimal GetEffectiveRateByLevel(this ITaxOrderItem item,
			params JurisdictionLevel[] levels)
		{
			return item.Taxes
				.Where(it => levels.Contains(it.Jurisdiction.Level))
				.Sum(it => it.EffectiveRate);
		}

		/// <summary>
		/// Gets an item's calculated tax by applicability.
		/// </summary>
		/// <param name="item">the order item</param>
		/// <param name="applicability">the tax' applicability</param>
		/// <returns></returns>
		public static decimal GetCalculatedTax(this ITaxOrderItem item)
		{
			return item.Quantity * item.Taxes.Sum(it => it.CalculatedTax);
		}

		/// <summary>
		/// Converts an address to a tax address for use with tax calculators,
		/// or returns null if the source <see cref="IAddress"/> is null.
		/// </summary>
		/// <param name="address">The <see cref="IAddress"/> to convert.</param>
		/// <returns>The converted <see cref="ITaxAddress"/> or null.</returns>
		public static ITaxAddress ToTaxAddress(this IAddress address)
		{
			if (address == null) return null;

			var addr = Create.New<ITaxAddress>();
			addr.StreetAddress1 = address.Address1;
			addr.StreetAddress2 = address.Address2;
			addr.City = TaxAddressProperty.FromCity(address.City);
			addr.MainDivision = TaxAddressProperty.FromState(address.State);
			addr.SubDivision = TaxAddressProperty.FromCounty(address.County);
			addr.PostalCode = TaxAddressProperty.FromPostalCode(address.PostalCode);
			addr.Country = TaxAddressProperty.FromCountry(address.Country, address.CountryCode);
			return addr;
		}

		/// <summary>
		/// Gets an address property's code if present, otherwise gets it's name. 
		/// If neither is present returns null.
		/// </summary>
		/// <param name="prop"></param>
		/// <returns></returns>
		public static string CodeOrNameOrDefault(this ITaxAddressProperty prop)
		{
			var result = default(String);
			if (prop == null)
			{
				return result;
			}

			if (!String.IsNullOrWhiteSpace(prop.Code))
			{
				result = prop.Code;
			}
			else if (!String.IsNullOrWhiteSpace(prop.Name))
			{
				result = prop.Name;
			}

			return result;
		}

		/// <summary>
		/// Gets an address property's name if present, otherwise gets it's code. 
		/// If neither is present returns null.
		/// </summary>
		public static string NameOrCodeOrDefault(this ITaxAddressProperty prop)
		{
			var result = default(String);
			if (prop == null)
			{
				return result;
			}

			if (!String.IsNullOrWhiteSpace(prop.Name))
			{
				result = prop.Name;
			}
			else if (!String.IsNullOrWhiteSpace(prop.Code))
			{
				result = prop.Code;
			}

			return result;
		}

		/// <summary>
		/// Calculates the total taxable amount, ignoring fees and discounts
		/// </summary>
		public static decimal GetTaxableAmount(this ITaxOrderItem item)
		{
			return item.Quantity * item.UnitPrice;
		}
	}
}
