using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Validation.Common;

namespace Miche.Orders.Query.Order
{
    public class IDRangeOrderQuery : BaseOrderQuery
    {
        public IDRangeOrderQuery(int startID, int endID)
        {
            StartId = startID;
            EndId = endID;
        }

        public int StartId { get; private set; }
        public int EndId { get; private set; }

        public override IQueryable GetRecords()
        {
            var queries = GetQueries();
            IQueryable result = null;
            foreach (var query in queries)
            {
                result =
                    (
                    from o in query
                    where o.OrderID >= StartId && o.OrderID <= EndId
                    select o
                );
            }
            return result;
        }

        public override string GetWhereClauseString(string orderAlias)
        {
            return String.Format("{0}.OrderID >= {1} AND {0}.OrderID <= {2}", orderAlias, StartId, EndId);
        }
    }
}
