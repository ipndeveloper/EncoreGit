using System.Collections.Generic;

namespace NetSteps.Data.Common.Entities
{
    public interface IOrder
    {
        int OrderID { get; set; }
        /// <summary>
        /// Holds the PreorderID Value
        /// </summary>
        int PreorderID { get; set; }
        int WareHouseID { get; set; }

        short OrderStatusID { get; set; }
        decimal? ShippingTotal { get; set; }
        IList<IOrderCustomer> OrderCustomers { get; }
        IList<IOrderAdjustment> OrderAdjustments { get; }
        void AddOrderAdjustment(IOrderAdjustment adjustment);

        void Save();

        IOrderItem AddItem(int productId, int quantity);
        IOrderItem AddItem(IOrderCustomer customer, int productId, int quantity, string parentGuid = null, int? dynamicKitGroupId = null, short? orderItemParentTypeID = null, bool disableDuplicateChecking = true);

        bool CalculationsDirty { get; set; }

        void CalculateTotals();

        decimal? GrandTotal { get; set; }

        short OrderTypeID { get; set; }

        int GetShippingMarketID();

        decimal? Subtotal { get; set; }
        int CurrencyID { get; set; }

        bool RemoveItem(int orderItemId);
        bool RemoveItem(IOrderItem orderItem);

        // This is used to retrieve the existing order adjustments.  This is put here SIMPLY to get this working
        // until the Order refactor.
        //TODO: move to the refactored order
        IEnumerable<int> GetExistingOrderAdjustmentIDsForAccount(int accountID);

        int? ParentOrderID { get; set; }

        IList<IAutoshipOrder> AutoshipOrders { get; }

        bool ClearAdjustments();

		decimal GetDefaultShippingTotal();

		ICollection<IOrder> ChildOrders { get; }

		int ConsultantID { get; set; }
    }
}
