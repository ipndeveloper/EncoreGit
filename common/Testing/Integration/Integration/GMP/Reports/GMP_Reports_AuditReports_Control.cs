using WatiN.Core;


namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_AuditReports_Control : GMP_Reports_Reports_Control
    {
        public GMP_Reports_AccountRecordAudit_Page ClickAccountRecordAudit(int? timeout = null)
        {
            return OpenReport<GMP_Reports_AccountRecordAudit_Page>("Account\\+Record\\+Audit", timeout);
        }

        public GMP_Reports_OrdersAudit_Page ClickOrdersAudit(int? timeout = null)
        {
            return OpenReport<GMP_Reports_OrdersAudit_Page>("Orders\\+Audit", timeout);
        }

        public GMP_Reports_OrderQueueAudit_Page ClickOrderQueueAudit(int? timeout = null)
        {
            return OpenReport<GMP_Reports_OrderQueueAudit_Page>("Order\\+Queue\\+Audit", timeout);
        }
    }
}
