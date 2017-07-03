using WatiN.Core;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Admin
{
    public class GMP_Admin_AutoshipSchedules_Page : GMP_Admin_Base_Page
    {
        List<string> _schedules;
        Table _scheduleTable;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _scheduleTable = Document.GetElement<Table>(new Param("DataGrid", AttributeName.ID.ClassName));
        }

         public override bool IsPageRendered()
        {
            return Document.GetElement<Link>(new Param("/Admin/AutoshipSchedules/Edit", AttributeName.ID.Href, RegexOptions.None)).Exists;
        }

         public GMP_Admin_AutoshipScheduleEdit_Page ClickAddNewSchedule(int? timeout = null, bool pageRequired = true)
        {
            timeout = Document.GetElement<Link>(new Param("Add new schedule", AttributeName.ID.InnerText)).CustomClick(timeout);
            return Util.GetPage<GMP_Admin_AutoshipScheduleEdit_Page>(timeout, pageRequired);
        }

        public List<string> Schedules
        {
            get
            {
                if (_schedules == null)
                {
                    _schedules = new List<string>();
                    foreach (Link lnk in _scheduleTable.Links)
                    {
                        _schedules.Add(lnk.CustomGetText());
                    }
                }
                return _schedules;
            }
        }

        public GMP_Admin_AutoshipScheduleEdit_Page EditSchedule(string schedule = null, int? timeout = null, bool pageRequired = true)
        {
            if (string.IsNullOrEmpty(schedule))
            {
                List<string> schedules = Schedules;
                schedule = schedules[Util.GetRandom(0, schedules.Count - 1)];
            }
            timeout = _scheduleTable.GetElement<Link>(new Param(schedule, AttributeName.ID.InnerText, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Admin_AutoshipScheduleEdit_Page>(timeout, pageRequired);
        }
    }
}
