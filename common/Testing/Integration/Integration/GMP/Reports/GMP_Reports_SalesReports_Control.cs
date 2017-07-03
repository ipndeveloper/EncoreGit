using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_SalesReports_Control : GMP_Reports_Reports_Control
    {

        public GMP_Reports_SalesByAccountType_Page ClickSalesByAccountType(int? timeout = null)
        {
            return OpenReport<GMP_Reports_SalesByAccountType_Page>("Sales\\+by\\+Account\\+Type", timeout);
        }

        public GMP_Reports_SalesByOrderStatus_Page ClickSalesByOrderStatus(int? timeout = null)
        {
            return OpenReport<GMP_Reports_SalesByOrderStatus_Page>("Sales\\+by\\+Order\\+Status", timeout);
        }

        public GMP_Reports_SalesByOrderType_Page ClickSalesByOrderType(int? timeout = null)
        {
            return OpenReport<GMP_Reports_SalesByOrderType_Page>("Sales\\+by\\+Order\\+Types", timeout);
        }

        public GMP_Reports_SalesByRep_Page ClickSalesByRep(int? timeout = null)
        {
            return OpenReport<GMP_Reports_SalesByRep_Page>("Sales\\+by\\+Rep", timeout);
        }

        public GMP_Reports_SalesBySKU_Page ClickSalesBySKU(int? timeout = null)
        {
            return OpenReport<GMP_Reports_SalesBySKU_Page>("Sales\\+by\\+SKU", timeout);
        }

        public GMP_Reports_SalesByState_Page ClickSalesByState(int? timeout = null)
        {
            return OpenReport<GMP_Reports_SalesByState_Page>("Sales\\+by\\+State", timeout);
        }

        public GMP_Reports_SalesVelocity_Page ClickSalesVelocity(int? timeout = null)
        {
            return OpenReport<GMP_Reports_SalesVelocity_Page>("Sales\\+Velocity", timeout);
        }
    }
}
