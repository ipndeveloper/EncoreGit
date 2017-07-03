using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_InventoryReports_Control : GMP_Reports_Reports_Control
    {

        public GMP_Reports_CriticalInventoryLevels_Page ClickCriticalInventoryLevels(int? timeout = null)
        {
            return OpenReport<GMP_Reports_CriticalInventoryLevels_Page>("Critical\\+Inventory\\+Levels", timeout);
        }

        public GMP_Reports_ItemReport_Page ClickItemReport(int? timeout = null)
        {
            return OpenReport<GMP_Reports_ItemReport_Page>("Item\\+Report", timeout);
        }

        public GMP_Reports_OrdersWithTrackingNumbers_Page ClickOrdersWithTrackingNumbers(int? timeout = null)
        {
            return OpenReport<GMP_Reports_OrdersWithTrackingNumbers_Page>("Orders With Tracking Numbers", timeout);
        }

        public GMP_Reports_PriceList_Page ClickPriceList(int? timeout = null)
        {
            return OpenReport<GMP_Reports_PriceList_Page>("Price\\+List", timeout);
        }

        public GMP_Reports_ProductSalesVelocity_Page ClickProductSalesVelocity(int? timeout = null)
        {
            return OpenReport<GMP_Reports_ProductSalesVelocity_Page>("Product\\+Sales\\+Velocity", timeout);
        }

        public GMP_Reports_ShipmentAging_Page ClickShipmentAging(int? timeout = null)
        {
            return OpenReport<GMP_Reports_ShipmentAging_Page>("Shipment\\+Aging", timeout);
        }
    }
}
