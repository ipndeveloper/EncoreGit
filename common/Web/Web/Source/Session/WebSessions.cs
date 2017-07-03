using System.Collections.Concurrent;

namespace NetSteps.Web
{
    public class WebSessions
    {
        public static readonly ConcurrentDictionary<string, SessionInfo> CurrentSessions = new ConcurrentDictionary<string, SessionInfo>();
    }
}
