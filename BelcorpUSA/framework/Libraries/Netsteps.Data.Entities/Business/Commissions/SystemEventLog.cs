// -----------------------------------------------------------------------
// <copyright file="SystemEventLog.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace NetSteps.Data.Entities.Commissions
{
    using System;

    public class SystemEventLog
    {

        #region Constructor

        public SystemEventLog()
        {
            this.EventMessage = string.Empty;
        }

        #endregion

        #region Properties

        public int SystemEventLogID { get; set; }

        public int SystemEventApplicationID { get; set; }

        public int SystemEventLogTypeID { get; set; }

        public string EventMessage { get; set; }

        public DateTime CreatedDate { get; set; }

        #endregion

        #region Methods

        public object ToJson()
        {
            return new
            {
                systemEventLogID = this.SystemEventLogID.ToString(),
                createdDate = this.CreatedDate.AddHours(1).ToString("dddd, MMMM d, yyyy - HH:mm:ss"),
                eventMessage = this.EventMessage,
                systemEventLogTypeID = this.SystemEventLogTypeID.ToString()
            };
        }

        #endregion

    }
}
