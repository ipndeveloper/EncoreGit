using System;

namespace NetSteps.Data.Entities.Tax
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Helper class for working with tax data on OrderItem entities.
	/// Created: 04-15-2010
	/// </summary>
	[Serializable]
	public class OrderItemTaxes
	{
		#region Properties
		public decimal ShippingTaxRate { get; set; }

		public decimal TaxPercent { get; set; }

		public decimal TaxAmountOrderItem { get; set; }

		public decimal TaxAmountShipping { get; set; }

		public decimal TaxAmountTotal { get; set; }

		public decimal TaxPercentCity { get; set; }

		public decimal TaxAmountCity { get; set; }

		public decimal TaxPercentCityLocal { get; set; }

		public decimal TaxAmountCityLocal { get; set; }

		public decimal TaxPercentCounty { get; set; }

		public decimal TaxAmountCounty { get; set; }

		public decimal TaxPercentCountyLocal { get; set; }

		public decimal TaxAmountCountyLocal { get; set; }

		public decimal TaxPercentState { get; set; }

		public decimal TaxAmountState { get; set; }

		public decimal TaxPercentDistrict { get; set; }

		public decimal TaxAmountDistrict { get; set; }

		public decimal TaxPercentCountry { get; set; }

		public decimal TaxAmountCountry { get; set; }

		public decimal TaxableTotal { get; set; }
		#endregion

		#region Methods
		public void ResetTotals()
		{
			this.TaxableTotal = 0;
			this.TaxAmountTotal = 0;
			this.TaxAmountCity = 0;
			this.TaxAmountCityLocal = 0;
			this.TaxAmountCounty = 0;
			this.TaxAmountCountyLocal = 0;
			this.TaxAmountState = 0;
			this.TaxAmountDistrict = 0;
			this.TaxAmountShipping = 0;
			this.TaxAmountCountry = 0;
		}

		public void CopyTaxAmountsTo(OrderItemTaxes destination)
		{
			destination.TaxableTotal += this.TaxableTotal;
			destination.TaxAmountTotal += this.TaxAmountTotal;
			destination.TaxAmountCity += this.TaxAmountCity;
			destination.TaxAmountCityLocal += this.TaxAmountCityLocal;
			destination.TaxAmountCounty += this.TaxAmountCounty;
			destination.TaxAmountCountyLocal += this.TaxAmountCountyLocal;
			destination.TaxAmountState += this.TaxAmountState;
			destination.TaxAmountDistrict += this.TaxAmountDistrict;
			destination.TaxAmountShipping += this.TaxAmountShipping;
		}
		#endregion
	}
}
