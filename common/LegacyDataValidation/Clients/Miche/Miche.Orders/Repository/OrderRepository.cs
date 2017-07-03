using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using Miche.Orders.Converters;
using NetSteps.Validation.Common;
using NetSteps.Validation.Conversion.Core;
using NetSteps.Validation.Common.Model;
using Miche.Orders.DataModel;
using Miche.Orders.Query.Order;

namespace Miche.Orders.Repository
{
    public class OrderRepository : IRecordRepository
    {
        private readonly Func<string, IRecordConverter> _converterFactory;

        public OrderRepository(Func<string, IRecordConverter> converterFactory)
        {
            _converterFactory = converterFactory;
        }

        public IEnumerable<IRecord> RetrieveRecords(IRecordQuery query)
        {
            var context = new DataModel.Entities();

            var found = ((IQueryable<Order>)query.GetRecords()).ToList();
            var converter = _converterFactory((typeof(Order)).ToString());
            return found.Select((x) => { return converter.Convert(x, null); });
        }

        public IEnumerable<object> RetrieveRecordKeys(IRecordQuery query)
        {
            var context = new DataModel.Entities();
            var found = ((IQueryable<Order>)query.GetRecords()).Select(x => x.OrderID).ToList().Cast<object>();
            return found;
        }

        public IEnumerable<IRecord> RetrieveRecords(IEnumerable<object> recordIDs)
        {
            var query = new IDSetOrderQuery(recordIDs.Cast<int>().ToArray());
            var resultQueryable = RetrieveRecords(query);
            var resultSet = resultQueryable.ToList();
            return resultSet;
        }
    }
}