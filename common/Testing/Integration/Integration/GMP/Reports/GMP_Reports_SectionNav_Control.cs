using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_SectionNav_Control : Control<UnorderedList>
    {
        public GMP_Reports_Accounts_Page Accounts(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("Accounts", AttributeName.ID.InnerText)).CustomClick(timeout);
            return Util.GetPage<GMP_Reports_Accounts_Page>(timeout, pageRequired);
        }

        public GMP_Reports_Audit_Page AuditReports(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("Audit Reports", AttributeName.ID.InnerText)).CustomClick(timeout);
            return Util.GetPage<GMP_Reports_Audit_Page>(timeout, pageRequired);
        }

        public GMP_Reports_CommissionsPrep_Page CommissionsPrep(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("Commissions Prep", AttributeName.ID.InnerText)).CustomClick(timeout);
            return Util.GetPage<GMP_Reports_CommissionsPrep_Page>(timeout, pageRequired);
        }

        public GMP_Reports_Executive_Page Executive(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("Executive", AttributeName.ID.InnerText)).CustomClick(timeout);
            return Util.GetPage<GMP_Reports_Executive_Page>(timeout, pageRequired);
        }

        public GMP_Reports_FieldFacing_Page FieldFacing(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("Field Facing", AttributeName.ID.InnerText)).CustomClick(timeout);
            return Util.GetPage<GMP_Reports_FieldFacing_Page>(timeout, pageRequired);
        }

        public GMP_Reports_Finance_Page Finance(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("Finance", AttributeName.ID.InnerText)).CustomClick(timeout);
            return Util.GetPage<GMP_Reports_Finance_Page>(timeout, pageRequired);
        }

        public GMP_Reports_Inventory_Page Inventory(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("Inventory", AttributeName.ID.InnerText)).CustomClick(timeout);
            return Util.GetPage<GMP_Reports_Inventory_Page>(timeout, pageRequired);
        }

        public GMP_Reports_Sales_Page Sales(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("Sales", AttributeName.ID.InnerText)).CustomClick(timeout);
            return Util.GetPage<GMP_Reports_Sales_Page>(timeout, pageRequired);
        }
    }
}
