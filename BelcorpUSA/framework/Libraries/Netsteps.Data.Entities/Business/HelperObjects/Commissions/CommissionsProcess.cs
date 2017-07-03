using System;

namespace NetSteps.Data.Entities.Commissions
{
    /// <summary>
    /// The commissions process object
    /// </summary>
    public class CommissionsProcess
    {
        public Int32 ProcessID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? LastCompleted { get; set; }
        public int StatusID { get; set; }

        private CommissionsProcessStatus status;
        public CommissionsProcessStatus Status
        {
            get
            {
                switch (StatusID)
                {
                    case 1:
                        status = CommissionsProcessStatus.Pending;
                        break;
                    case 2:
                        status = CommissionsProcessStatus.Skipped;
                        break;
                    case 3:
                        status = CommissionsProcessStatus.InProgress;
                        break;
                    case 4:
                        status = CommissionsProcessStatus.Complete;
                        break;
                    default:
                        status = CommissionsProcessStatus.Error;
                        break;
                }

                return status;
            }
            set
            {
                StatusID = (int)value;
                status = value;
            }
        }
        public bool Manual { get; set; }
        public bool ScheduleToRun { get; set; }
    }
}
