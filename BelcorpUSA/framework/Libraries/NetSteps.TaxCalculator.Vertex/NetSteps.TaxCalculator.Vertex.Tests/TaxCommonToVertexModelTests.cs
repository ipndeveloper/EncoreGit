using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.TaxCalculator.Vertex.CalculateTaxService60;
using NetSteps.TaxCalculator.Vertex.Converters;

namespace NetSteps.TaxCalculator.Vertex.Tests
{
	[TestClass]
	public class TaxCommonToVertexModelTests
	{
		[TestMethod]
		public void BuildOrderItemTaxesFromResponse_ShouldNotThrowNullRefWhenLineItemUnitPriceIsNull()
		{
			var item = CreateVertexResponseLineItem();
			item.UnitPrice = null;
			var lineItems = new List<IVertexResponseLineItem> { item };

			var results = TaxCommonToVertexModel.BuildOrderItemTaxesFromResponse(lineItems);

			foreach (var result in results)
			{
				result.Value.ToList();
			}
		}

		#region Helpers
		private IVertexResponseLineItem CreateVertexResponseLineItem()
		{
			var item = new LineItemQSOType();

			var taxes = new TaxesType[1];
			taxes[0] = new TaxesType
			{
				EffectiveRate = .2m,
				taxCode = "x",
				Jurisdiction = new TaxesTypeJurisdiction 
				{
					jurisdictionLevel = JurisdictionLevelCodeType.STATE,
					Value = "STATE"
				}
			};
			item.Taxes = taxes;
			item.UnitPrice = new AmountType { Value = 10 };

			return item;
		}
		#endregion
	}
}
