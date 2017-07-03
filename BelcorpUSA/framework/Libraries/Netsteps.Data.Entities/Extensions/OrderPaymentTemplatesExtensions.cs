using System.Linq;
using NetSteps.Common.DataFaker;
using NetSteps.Common.Extensions;
using NetSteps.Encore.Core.IoC;
using System;

using NetSteps.Data.Entities.Generated;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business;
using NetSteps.Common.Base;

namespace NetSteps.Data.Entities
{
    /// <summary>
    /// Author: Karina  Torres 
    /// Description: Collection Management Extensions
    /// Created: 31-07-2015
    /// </summary>
    public  class OrderPaymentTemplatesExtensions
    {
        /// <summary>
        /// Create by KTC
        /// </summary>
        /// <returns>nothing</returns>
        public static PaginatedList<OrderPaymentTemplatesSearchData> GetOrderPaymentTemplates(OrderPaymentTemplatesSearchParameters param)
        {
            List<OrderPaymentTemplatesSearchData> result = DataAccess.ExecWithStoreProcedureLists<OrderPaymentTemplatesSearchData>("Core", "upsGetOrderPaymentTemplates").ToList();
            IQueryable<OrderPaymentTemplatesSearchData> matchingItems = result.AsQueryable<OrderPaymentTemplatesSearchData>();
            var resultTotalCount = matchingItems.Count();
            //matchingItems = matchingItems.ApplyPagination(param);
            var OrderPaymentTemplatesResult = matchingItems.ToPaginatedList<OrderPaymentTemplatesSearchData>(param, resultTotalCount);
            return OrderPaymentTemplatesResult;
        }

        public static void Insert(OrderPaymentTemplatesSearchParameters param)
        {
            int statusID = DataAccess.ExecWithStoreProcedureSave("Core", "upsInsertOrderPaymentTemplate",
                new SqlParameter("Description", SqlDbType.VarChar) { Value = param.Description },
                new SqlParameter("Days", SqlDbType.VarChar) { Value = param.Days },
                new SqlParameter("MinimalAmount", SqlDbType.VarChar) { Value = param.MinimalAmount }

             );
        }

        public static void Update(OrderPaymentTemplatesSearchParameters param)
        {
            int statusID = DataAccess.ExecWithStoreProcedureSave("Core", "upsUpdateOrderPaymentTemplate",
                new SqlParameter("OrderPaymentTemplateId", SqlDbType.VarChar) { Value = param.OrderPaymentTemplateId },
                new SqlParameter("Description", SqlDbType.VarChar) { Value = param.Description },
                new SqlParameter("Days", SqlDbType.VarChar) { Value = param.Days },
                new SqlParameter("MinimalAmount", SqlDbType.VarChar) { Value = param.MinimalAmount }

             );
        }

        public static void Delete(int orderPaymentTemplateId)
        {
            int statusID = DataAccess.ExecWithStoreProcedureSave("Core", "upsDeleteOrderPaymentTemplate",
                new SqlParameter("OrderPaymentTemplateId", SqlDbType.VarChar) { Value = orderPaymentTemplateId  }        
             );
        }
    }
}
