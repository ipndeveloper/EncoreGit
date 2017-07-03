
namespace NetSteps.Data.Entities.Tax.Avatax
{
	public class AvataxCalculationInfo
	{
		public double ShippingTaxRate { get; set; }

		public decimal ShippingTax { get; set; }

		public decimal ShippingTaxable { get; set; }

		public decimal TotalAmount { get; set; }

		public decimal TotalTaxableAmount { get; set; }

		public decimal TotalTax { get; set; }

		public double HandlingTaxRate { get; set; }

		public decimal HandlingTax { get; set; }

		public decimal HandlingTaxable { get; set; }
	}
}
