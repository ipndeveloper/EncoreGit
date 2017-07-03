using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Validation.Common;

namespace JewelKade.Orders.Query.Order
{
    public class DateRangeOrderQuery : BaseOrderQuery
    {
        public DateRangeOrderQuery(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        public override IQueryable GetRecords()
        {
            var queries = GetQueries();
            IQueryable result = null;
            foreach (var query in queries)
            {
                result =
                    (
                        from o in query
                        where o.CompleteDateUTC >= StartDate && o.CompleteDateUTC <= EndDate
                        select o
                );
            }
            return result;
        }

        public override string GetWhereClauseString(string orderAlias)
        {
            return String.Format("{0}.CompleteDateUTC >= '{1}' AND {0}.CompleteDateUTC <= '{2}'", orderAlias, StartDate, EndDate);
        }
    }
}
