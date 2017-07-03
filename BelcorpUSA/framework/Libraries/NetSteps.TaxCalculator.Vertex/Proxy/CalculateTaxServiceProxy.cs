using System;
using System.Diagnostics.Contracts;
using System.Linq;
using NetSteps.Encore.Core;
using NetSteps.Encore.Core.IoC;
using NetSteps.TaxCalculator.Vertex.CalculateTaxService60;
using NetSteps.TaxCalculator.Vertex.Config;
using NetSteps.TaxCalculator.Vertex.Converters;
using NetSteps.Taxes.Common.Models;


namespace NetSteps.TaxCalculator.Vertex
{
	public enum VertexCallType
	{
		Quotation = 0,
		Invoice = 1
	}

	public class CalculateTaxServiceProxy
	{
		static LoginType __login;
		public static LoginType Login
		{
			get
			{
				return Util.NonBlockingLazyInitializeVolatile(ref __login, () =>
				{
					var config = VertexTaxCalculatorConfigSection.Current;
					var login = config.FillLogin(new LoginType());
					return login;
				});
			}
		}

		static VertexTaxPayerCodeCollection __taxPayerCodes;
		public static VertexTaxPayerCodeCollection TaxPayerCodes
		{
			get
			{
				return Util.NonBlockingLazyInitializeVolatile(ref __taxPayerCodes, () => VertexTaxCalculatorConfigSection.Current.TaxPayerCodeCodes);
			}
		}

		public static TResponse Invoke<TRequest, TResponse>(TRequest request)
		{
			var envelope = new VertexEnvelope();
			envelope.Login = Login;
			envelope.Item = request;
			using (var svc = new CalculateTaxWS60Client())
			{
				svc.calculateTax60(ref envelope);
			}
			return (TResponse)envelope.Item;
		}

		public static ITaxCalculationResult Quote(ITaxOrder order)
		{
			Contract.Requires<ArgumentNullException>(order != default(ITaxOrder));

			return VertexCall(order, VertexCallType.Quotation);
		}

		public static ITaxCalculationResult Invoice(ITaxOrder order)
		{
			Contract.Requires<ArgumentNullException>(order != default(ITaxOrder));

			return VertexCall(order, VertexCallType.Invoice);
		}

		private static ITaxCalculationResult VertexCall(ITaxOrder order, VertexCallType vertexCallType)
		{
			var result = Create.New<ITaxCalculationResult>();
			result.Order = order;
			if (string.IsNullOrWhiteSpace(order.CountryCode) || order.Customers == default(ITaxCustomer) || !order.Customers.SelectMany(c => c.Items).Any())
			{
				result.Status = TaxCalculationState.Skipped;
				return result;
			}
			
			foreach (var customer in order.Customers.Where(c => c.Items.Any()))
			{
				var docNumber = string.Format("{0}-{1}", order.OrderID, customer.CustomerID);
				var countryCode = TaxPayerCodes.Get(order.CountryCode).TaxPayerCode;
				var lineitems = TaxCommonToVertexModel.BuildLineItems(customer, vertexCallType);
				var request = TaxCommonToVertexModel.BuildRequest(docNumber, countryCode, customer, lineitems, vertexCallType);

				IVertexResponseType response;
				switch (vertexCallType)
				{
					case VertexCallType.Invoice:
						response = Invoke<InvoiceRequestType, InvoiceResponseType>(request as InvoiceRequestType);
						break;
					case VertexCallType.Quotation:
					default:
						response = Invoke<QuotationRequestType, QuotationResponseType>(request as QuotationRequestType);
						break;
				}

				if (response == default(IVertexResponseType))
				{
					result.Status = TaxCalculationState.Faulted;
					return result;
				}

				foreach (var taxes in TaxCommonToVertexModel.BuildOrderItemTaxesFromResponse(response.LineItems))
				{
					customer.Items.First(x => x.ItemID == taxes.Key).Taxes = taxes.Value;
				}
			}

			result.Status = TaxCalculationState.Succeeded;
			return result;
		}
	}
}
