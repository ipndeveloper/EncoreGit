// -----------------------------------------------------------------------
// <copyright file="SystemEventSetting.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace NetSteps.Data.Entities.Commissions
{

    public class SystemEventSetting
    {

        #region Constructor

        public SystemEventSetting()
        {

        }

        #endregion

        #region Properties

        public int PrepSystemEventApplicationID { get; set; }

        public int PublishSystemEventApplicationID { get; set; }

        public int SystemEventLogErrorTypeID { get; set; }

        public int SystemEventLogNoticeTypeID { get; set; }

        #endregion

    }
}
