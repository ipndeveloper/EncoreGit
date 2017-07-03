using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using NetSteps.Encore.Core;
using NetSteps.Encore.Core.IoC;
using NetSteps.TaxCalculator.Vertex.CalculateTaxService60;
using NetSteps.TaxCalculator.Vertex.Config;
using NetSteps.Taxes.Common.Models;

namespace NetSteps.TaxCalculator.Vertex.Converters
{
	public static class TaxCommonToVertexModel
	{
		static string __shippingSku;
		public static string ShippingSku
		{
			get
			{
				return Util.NonBlockingLazyInitializeVolatile(ref __shippingSku, () => VertexTaxCalculatorConfigSection.Current.ShippingSku);
			}
		}

		static string __orderShippingSku;
		public static string OrderShippingSku
		{
			get
			{
				return Util.NonBlockingLazyInitializeVolatile(ref __orderShippingSku, () => string.IsNullOrWhiteSpace(VertexTaxCalculatorConfigSection.Current.OrderShippingSku) ? ShippingSku : VertexTaxCalculatorConfigSection.Current.OrderShippingSku);
			}
		}

		public static IVertexRequestType BuildRequest(string docNumber, string countryCode, ITaxCustomer customer, IEnumerable<IVertexRequestLineItem> lineItems, VertexCallType vertexCallType)
		{
			//var originPostalCode = customer.OriginAddress == default(ITaxAddress) ? null : customer.OriginAddress.PostalCode.CodeOrNameOrDefault();
			var shippingAddress = customer.ShippingAddress;
			var shippingStreetAddress1 = shippingAddress.StreetAddress1;
			var shippingStreetAddress2 = shippingAddress.StreetAddress2;
			var shippingCity = shippingAddress.City.CodeOrNameOrDefault();
			var shippingCountry = shippingAddress.Country.CodeOrNameOrDefault();
			var shippingPostalCode = shippingAddress.PostalCode.CodeOrNameOrDefault();
			var shippingMainDivision = shippingAddress.MainDivision.CodeOrNameOrDefault();
			var shippingSubDivision = shippingAddress.SubDivision.CodeOrNameOrDefault();

			IVertexRequestType request;
			switch (vertexCallType)
			{
				case VertexCallType.Invoice:
					request = new InvoiceRequestType();
					break;
				case VertexCallType.Quotation:
				default:
					request = new QuotationRequestType();
					break;
			}
		
			request.transactionType = SaleTransactionType.SALE;
			request.returnAssistedParametersIndicator = true;
			request.documentNumber = docNumber;
			request.documentDate = DateTime.Now;
			request.Seller = new SellerType
			{
				Company = countryCode
				//PhysicalOrigin = new LocationType
				//{
				//    PostalCode = originPostalCode
				//}
			};
			request.Customer = new CustomerType
			{
				CustomerCode = new CustomerCodeType
				{
					classCode = "BuyerClassCode"
				},
				isTaxExempt = customer.IsTaxExempt,
				Destination = new LocationType
				{
					StreetAddress1 = shippingStreetAddress1,
					StreetAddress2 = shippingStreetAddress2,
					City = shippingCity,
					Country = shippingCountry,
					PostalCode = shippingPostalCode,
					MainDivision = shippingMainDivision,
					SubDivision = shippingSubDivision
				}
			};
			request.LineItems = lineItems;

			return request;
		}

		public static IEnumerable<IVertexRequestLineItem> BuildLineItems(ITaxCustomer customer, VertexCallType vertexCallType)
		{
			foreach (var taxItem in customer.Items)
			{
				string productCode;
				switch (taxItem.Applicability)
				{
					case TaxApplicabilityKind.Shipping:
						productCode = ShippingSku;
						break;
					case TaxApplicabilityKind.OrderShipping:
						productCode = OrderShippingSku;
						break;
					case TaxApplicabilityKind.Price:
					default:
						productCode = taxItem.ProductCode;
						break;
				}

				IVertexRequestLineItem lineItem;
				switch (vertexCallType)
				{
					case VertexCallType.Invoice:
						lineItem = new LineItemISIType();
						break;
					case VertexCallType.Quotation:
					default:
						lineItem = new LineItemQSIType();
						break;
				}

				lineItem.lineItemId = taxItem.ItemID;
				lineItem.Quantity = new MeasureType { Value = taxItem.Quantity };
				lineItem.UnitPrice = new AmountType { Value = taxItem.UnitPrice };
				lineItem.Product = new Product { Value = productCode };
				lineItem.vendorSKU = productCode;
				lineItem.Freight = new AmountType { Value = taxItem.HandlingFee }; // + taxItem.ShippingFee }; shipping fees are added to the shipping line item from ordercustomer.shippingtotal (ShippingCalculator) and should not be included here
				if (taxItem.Applicability == TaxApplicabilityKind.Price && !taxItem.ChargeTax)
				{
					lineItem.TaxOverride = new TaxOverride { overrideType = TaxOverrideCodeType.NONTAXABLE };
				}

				yield return lineItem;
			}
		}

		public static IEnumerable<KeyValuePair<string, IEnumerable<ITaxOrderItemTaxes>>> BuildOrderItemTaxesFromResponse(IEnumerable<IVertexResponseLineItem> lineItems)
		{
			foreach (var lineItem in lineItems)
			{
				var taxes = lineItem.Taxes.Select(taxJuristiction =>
					Create.Mutation(Create.New<ITaxOrderItemTaxes>(),
						dtoLineItemTaxes =>
						{
							dtoLineItemTaxes.TaxableAmount = lineItem.UnitPrice == null ? 0m : lineItem.UnitPrice.Value;
							dtoLineItemTaxes.Jurisdiction = Create.Mutation(Create.New<IJurisdiction>(),
								dtoTaxJuristictions =>
								{
									var jurisdiction = Create.New<IEnumConverter<JurisdictionLevelCodeType, JurisdictionLevel>>();
									dtoTaxJuristictions.Level = jurisdiction.Convert(taxJuristiction.Jurisdiction.jurisdictionLevel);
									dtoTaxJuristictions.Name = taxJuristiction.Jurisdiction.Value;
								});

							dtoLineItemTaxes.TaxRule = taxJuristiction.taxCode;
							dtoLineItemTaxes.EffectiveRate = taxJuristiction.EffectiveRate;
							dtoLineItemTaxes.CalculatedTax = dtoLineItemTaxes.TaxableAmount * dtoLineItemTaxes.EffectiveRate;
						}));

				yield return new KeyValuePair<string, IEnumerable<ITaxOrderItemTaxes>>(lineItem.lineItemId, taxes);
			}
		}
	}
}

///
/// Partials to allow more code reuse in the proxy.
/// If these are breaking the service reference was probably updated and these will need to be fixed (or the service classes are no longer partials).
///
namespace NetSteps.TaxCalculator.Vertex.CalculateTaxService60
{
	public partial class QuotationRequestType : IVertexRequestType
	{
		[XmlIgnore]
		public IEnumerable<IVertexRequestLineItem> LineItems
		{
			get { return this.LineItem; }
			set { this.LineItem = value.Cast<LineItemQSIType>().ToArray(); }
		}
	}
	public partial class InvoiceRequestType : IVertexRequestType
	{
		[XmlIgnore]
		public IEnumerable<IVertexRequestLineItem> LineItems
		{
			get { return this.LineItem; }
			set { this.LineItem = value.Cast<LineItemISIType>().ToArray(); }
		}
	}
	public partial class QuotationResponseType : IVertexResponseType
	{
		[XmlIgnore]
		public IEnumerable<IVertexResponseLineItem> LineItems
		{
			get { return this.LineItem; }
			set { this.LineItem = value.Cast<LineItemQSOType>().ToArray(); }
		}
	}
	public partial class InvoiceResponseType : IVertexResponseType
	{
		[XmlIgnore]
		public IEnumerable<IVertexResponseLineItem> LineItems
		{
			get { return this.LineItem; }
			set { this.LineItem = value.Cast<LineItemISOType>().ToArray(); }
		}
	}
	public partial class LineItemQSIType : IVertexRequestLineItem { }
	public partial class LineItemISIType : IVertexRequestLineItem { }
	public partial class LineItemQSOType : IVertexResponseLineItem { }
	public partial class LineItemISOType : IVertexResponseLineItem { }
	public interface IVertexRequestType
	{
		IEnumerable<IVertexRequestLineItem> LineItems { get; set; }
		CustomerType Customer { get; set; }
		SellerType Seller { get; set; }
		DateTime documentDate { get; set; }
		string documentNumber { get; set; }
		bool returnAssistedParametersIndicator { get; set; }
		SaleTransactionType transactionType { get; set; }
	}
	public interface IVertexResponseType
	{
		IEnumerable<IVertexResponseLineItem> LineItems { get; set; }
	}
	public interface IVertexRequestLineItem
	{
		string lineItemNumber { get; set; }
		string lineItemId { get; set; }
		MeasureType Quantity { get; set; }
		AmountType UnitPrice { get; set; }
		Product Product { get; set; }
		AmountType Freight { get; set; }
		string vendorSKU { get; set; }
		TaxOverride TaxOverride { get; set; }
	}
	public interface IVertexResponseLineItem
	{
		string lineItemId { get; set; }
		AmountType UnitPrice { get; set; }
		MeasureType Quantity { get; set; }
		Product Product { get; set; }
		TaxesType[] Taxes { get; set; }
		string vendorSKU { get; set; }
		TaxOverride TaxOverride { get; set; }
	}
}
