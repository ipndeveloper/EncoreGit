using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_Sales_Page : GMP_Reports_Base_Page
    {
        protected GMP_Reports_SalesReports_Control _reports;

        public GMP_Reports_SalesReports_Control Reports
        {
            get
            {
                if (_reports == null)
                {
                    _reports = Control.CreateControl<GMP_Reports_SalesReports_Control>(_coreContent);
                }
                return _reports;
            }
        }

         public override bool IsPageRendered()
        {
            return Title.Equals("Sales");
        }
    }
}
