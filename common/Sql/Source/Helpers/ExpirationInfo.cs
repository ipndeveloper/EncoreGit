using System;

namespace NetSteps.Sql
{
    /// <summary>
    /// Author: John Egbert
    /// Created: 3/26/2010
    /// </summary>
    public class ExpirationInfo
    {
        public DateTime ExpirationDate { get; set; }
        public TimeSpan TimeToLive { get; set; }

        public bool IsExpired()
        {
            return ExpirationDate < DateTime.Now;
        }

        public void Reset()
        {
            ExpirationDate = DateTime.Now.Add(TimeToLive);
        }
    }
}
