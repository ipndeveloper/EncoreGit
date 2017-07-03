using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Admin
{
    public class GMP_Admin_AutoshipScheduleEdit_Page : GMP_Admin_Base_Page
    {
        TextField _schedule;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _schedule = Document.GetElement<TextField>(new Param("name"));
        }

         public override bool IsPageRendered()
        {
            return Document.GetElement<SelectList>(new Param("orderTypeId")).Exists;
        }

        public string Schedule
        {
            get { return _schedule.CustomGetText(); }
            set { _schedule.CustomSetTextHelper(value); }
        }

        public GMP_Admin_AutoshipSchedules_Page Cancel(int? timeout = null, bool pageRequired = true)
        {
            timeout = Document.GetElement<Link>(new Param("Button", AttributeName.ID.ClassName)).CustomClick(timeout);
            return Util.GetPage<GMP_Admin_AutoshipSchedules_Page>(timeout, pageRequired);
        }
    }
}
