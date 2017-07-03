using System;
using System.Collections.Generic;
using NetSteps.Common.Models;
using NetSteps.Orders.Common.Models;

namespace NetSteps.Events.Service.Tests.Mocks
{
	public class FakeOrder : IOrder
	{
		public void AddNote(INote item)
		{

			throw new NotImplementedException();
		}

		public void RemoveNote(INote item)
		{
			throw new NotImplementedException();
		}

		public void AddOrderAdjustment(IOrderAdjustment item)
		{
			throw new NotImplementedException();
		}

		public void Save()
		{
			throw new NotImplementedException();
		}

		public IOrderItem AddItem(int productId, int quantity)
		{
			throw new NotImplementedException();
		}

		public IOrderItem AddItem(IOrderCustomer customer, int productId, int quantity, string parentGuid = null, int? dynamicKitGroupId = new int?(), short? orderItemParentTypeID = new short?(), bool disableDuplicateChecking = true)
		{
			throw new NotImplementedException();
		}

		public void CalculateTotals()
		{
			throw new NotImplementedException();
		}

		public int GetShippingMarketID()
		{
			throw new NotImplementedException();
		}

		public bool RemoveItem(int orderItemId)
		{
			throw new NotImplementedException();
		}

		public bool RemoveItem(IOrderItem orderItem)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<int> GetExistingOrderAdjustmentIDsForAccount(int accountID)
		{
			throw new NotImplementedException();
		}

		public bool ClearAdjustments()
		{
			throw new NotImplementedException();
		}

		public decimal GetDefaultShippingTotal()
		{
			throw new NotImplementedException();
		}

		public void RemoveOrderAdjustment(IOrderAdjustment item)
		{
			throw new NotImplementedException();
		}

		public void AddOrderCustomer(IOrderCustomer item)
		{
			throw new NotImplementedException();
		}

		public void RemoveOrderCustomer(IOrderCustomer item)
		{
			throw new NotImplementedException();
		}

		public int OrderID { get; set; }
		public string OrderNumber { get; set; }
		public short OrderStatusID { get; set; }
		public short OrderTypeID { get; set; }

		public ICollection<IOrder> ChildOrders
		{
			get { throw new NotImplementedException(); }
		}

		public int ConsultantID { get; set; }
		public int? SiteID { get; set; }
		public int? ParentOrderID { get; set; }

		public IList<IAutoshipOrder> AutoshipOrders
		{
			get { throw new NotImplementedException(); }
		}

		public int CurrencyID { get; set; }
		public DateTime? CompleteDateUTC { get; set; }
		public DateTime? CommissionDateUTC { get; set; }
		public decimal? HostessRewardsEarned { get; set; }
		public decimal? HostessRewardsUsed { get; set; }
		public bool? IsTaxExempt { get; set; }
		public decimal? TaxAmountTotal { get; set; }
		public decimal? TaxAmountTotalOverride { get; set; }
		public decimal? TaxableTotal { get; set; }
		public decimal? TaxAmountOrderItems { get; set; }
		public decimal? TaxAmountShipping { get; set; }
		public decimal? TaxAmount { get; set; }
		public decimal? Subtotal { get; set; }
		public decimal? DiscountTotal { get; set; }
		public decimal? ShippingTotal { get; set; }
		public decimal? ShippingTotalOverride { get; set; }
		public decimal? GrandTotal { get; set; }
		public decimal? PaymentTotal { get; set; }
		public decimal? Balance { get; set; }
		public decimal? CommissionableTotal { get; set; }
		public int? ReturnTypeID { get; set; }
		public string StepUrl { get; set; }
		public int? ModifiedByUserID { get; set; }
		public DateTime DateCreatedUTC { get; set; }
		public int? CreatedByUserID { get; set; }
		public byte[] DataVersion { get; set; }
		public decimal? HandlingTotal { get; set; }
		public decimal? DiscountPercent { get; set; }
		public decimal? PartyShipmentTotal { get; set; }
		public decimal? PartyHandlingTotal { get; set; }
		public IEnumerable<INote> Notes { get; set; }

		public bool CalculationsDirty
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public IEnumerable<IOrderAdjustment> OrderAdjustments { get; set; }
		public IEnumerable<IOrderCustomer> OrderCustomers { get; set; }


        public void AddOrderPayment(IOrderPayment item)
        {
            throw new NotImplementedException();
        }

        public void AddOrderPaymentResult(IOrderPaymentResult item)
        {
            throw new NotImplementedException();
        }

        public void AddOrderShipment(IOrderShipment item)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IOrderPaymentResult> OrderPaymentResults
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<IOrderPayment> OrderPayments
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<IOrderShipment> OrderShipments
        {
            get { throw new NotImplementedException(); }
        }

        public void RemoveOrderPayment(IOrderPayment item)
        {
            throw new NotImplementedException();
        }

        public void RemoveOrderPaymentResult(IOrderPaymentResult item)
        {
            throw new NotImplementedException();
        }

        public void RemoveOrderShipment(IOrderShipment item)
        {
            throw new NotImplementedException();
        }
    }
}
