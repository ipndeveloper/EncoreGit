using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Validation.Common;

namespace JewelKade.Orders.Query.Order
{
    public class IDSetOrderQuery : BaseOrderQuery
    {
        public IDSetOrderQuery(int[] orderIDs)
        {
            OrderIDs = orderIDs;
        }

        public int[] OrderIDs { get; private set; }

        public override IQueryable GetRecords()
        {
            var queries = GetQueries();
            IQueryable result = null;
            foreach (var query in queries)
            {
                result =
                    (
                        from o in query
                        where OrderIDs.Contains(o.OrderID)
                        select o
                    );
            }
            return result;
        }

        public override string GetWhereClauseString(string orderAlias)
        {
            return String.Format("{0}.OrderID IN ({1})", orderAlias, string.Join(",", OrderIDs));
        }
    }
}
