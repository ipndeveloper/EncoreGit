using System;

namespace NetSteps.Web
{
    public class SessionInfo
    {
        public string SessionId { get; set; }
        public DateTime SessionStartTime { get; set; }
        public DateTime SessionEndTime { get; set; }
        public int UserId { get; set; }
        public string IpAddress { get; set; }
    }
}
