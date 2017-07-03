// -----------------------------------------------------------------------
// <copyright file="SystemEvent.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace NetSteps.Data.Entities.Commissions
{
    using System;

    public class SystemEvent
    {

        #region Constructor

        public SystemEvent()
        {

        }

        #endregion

        #region Properties

        public DateTime CreatedDate { get; set; }

        public bool Completed { get; set; }

        public int Duration { get; set; }

        public DateTime EndTime { get; set; }

        public int PeriodID { get; set; }

        public DateTime StartTime { get; set; }

        public int SystemEventApplicationID { get; set; }

        public int SystemEventID { get; set; }
        
        #endregion

    }
}
