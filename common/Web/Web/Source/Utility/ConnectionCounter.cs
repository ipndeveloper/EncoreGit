using System.Linq;
using System.Collections.Concurrent;

namespace NetSteps.Web.Base
{
    public class ConnectionCounterClass
    {
        public static int Count
        {
            get { return CounterList.Count; }
        }

        public static int CountAuthenticated
        {
            get
            {
                int returnValue = 0;

                foreach (ConnectionCounter item in CounterList.Values)
                {
                    if (item != null && item._IsAuthenticated)
                    {
                        returnValue++;
                    }
                }

                return returnValue;
            }
        }

        public static void Add(string SessionID, string ID)
        {
            CounterList.TryAdd(SessionID, new ConnectionCounter { _ID = ID, _SessionID = SessionID });
        }

        public static void Add(string SessionID, string ID, bool IsAuthenticated)
        {
            CounterList.AddOrUpdate(
                SessionID,
                addKey => 
                    new ConnectionCounter {_ID = ID, _SessionID = addKey, _IsAuthenticated = IsAuthenticated},
                (updateKey, oldValue) =>
                    {
                        oldValue._IsAuthenticated = IsAuthenticated;
                        return oldValue;
                    });
        }

        public static void SetAuthenticated(string SessionID, bool IsAuthenticated)
        {
            ConnectionCounter counter;

            if (CounterList.TryGetValue(SessionID, out counter))
            {
                counter._IsAuthenticated = IsAuthenticated;
            }
            else
            {
                Add(SessionID, string.Empty, IsAuthenticated);
            }
        }

        public static void Remove(string SessionID)
        {
            ConnectionCounter toRemove;
            CounterList.TryRemove(SessionID, out toRemove);
        }

        private static readonly ConcurrentDictionary<string, ConnectionCounter> CounterList = new ConcurrentDictionary<string, ConnectionCounter>();
    }

    public class ConnectionCounter
    {
        public string _ID { get; set; }
        public string _SessionID { get; set; }
        public bool _IsAuthenticated { get; set; }
    }
}
