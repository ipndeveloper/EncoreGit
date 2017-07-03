using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Repositories.Interfaces;
using System;
using System.Data.SqlClient;
using NetSteps.Data.Entities.Exceptions;
using System.Data;

namespace NetSteps.Data.Entities.Repositories
{
    public class PlanRepository: IPlanRepository
    {
        public static List<PlanSearchData> Search()
        {
            return PlanDataAccess.Search();
        }

        public static PaginatedList<PlanSearchData> Search(FilterPaginatedListParameters<PlanSearchData> searchParams)
        {
            IQueryable<PlanSearchData> matchingItems = PlanDataAccess.Search().AsQueryable<PlanSearchData>();
            var resultTotalCount = matchingItems.Count();
            matchingItems = matchingItems.ApplyPagination(searchParams);

            return matchingItems.ToPaginatedList<PlanSearchData>(searchParams, resultTotalCount);
        }

        public static void UpdateEnabledPlan(int planId, bool enabledNow)
        {
            PlanDataAccess.UpdateEnabledPlan(planId, enabledNow);
        }

        //public static void ChangeStatusShippingOrderTypes(int planId, bool enabledNow)
        //{
        //    using (EntityDBContext dbContext = new EntityDBContext(ConnectionStrings.BelcorpCore))
        //    {
        //        var entity = dbContext.SH
        //        entity.Enabled = enabledNow;
        //        //dbContext.Entry(stud).State = System.Data.Entity.EntityState.Modified;        
        //        dbContext.SaveChanges();
        //    }
        //}

        public static void ChangeStatusShippingOrderTypes(int ShippingOrderTypeID, bool enabledNow)
        {
            try
            {
                var result = 0;
                object[] parameters = { new SqlParameter("@ShippingOrderTypeID", SqlDbType.Int) { Value = ShippingOrderTypeID },
                                      new SqlParameter("@Active", SqlDbType.Bit) { Value =enabledNow }};

                using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCore))
                { result = DbContext.Database.SqlQuery<int>(GenerateQueryString("spChangeStatusShippingOrderTypes", parameters), parameters).FirstOrDefault(); }




                //Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@ShippingOrderTypeID", ShippingOrderTypeID }, 
                //                                                                            { "@Active", enabledNow }};


                //SqlCommand cmd = DataAccess.GetCommand("spChangeStatusShippingOrderTypes", parameters, "Core") as SqlCommand;
                //cmd.Connection.Open();
                //cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static void Save(PlanSearchData plan)
        {
            if (plan.PlanID == 0)
            {
                plan.PlanID = PlanDataAccess.Insert(plan);
            }
            else
            {
                PlanDataAccess.Update(plan);
            }
        }

        /// <summary>
        /// Gets all plans
        /// </summary>
        /// <returns>List of Plan Dto</returns>
        public List<Dto.PlanDto> GetAll()
        {
            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCommission))
            {
                var data = (from r in context.Plans
                            select new Dto.PlanDto()
                            {
                                PlanId = r.PlanId,
                                Name = r.Name,
                                TermName = r.TermName,
                                PlanCode = r.PlanCode,
                                DefaultPlan = r.DefaultPlan,
                                Enabled = r.Enabled
                            }).ToList();

                if (data == null)
                    throw new Exception("Plan not found");

                return data;
            }
        }

        /// <summary>
        /// Add @ as pref of parameters
        /// </summary>
        /// <param name="query">Query or store procedure</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>string format Query @parameter ...</returns>
        private static string GenerateQueryString(string query, params object[] parameters)
        {
            if (!query.Contains("@") && parameters != null)
            {
                var parameterNames = from p in parameters select ((System.Data.SqlClient.SqlParameter)p).ParameterName;
                query = string.Format("{0} {1}", query, string.Join(", ", parameterNames));
            }

            return query;
        }
    }
}
