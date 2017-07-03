namespace NetSteps.Data.Entities.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using NetSteps.Data.Entities.Dto;
    using NetSteps.Data.Entities.Repositories.Interfaces;
    using System.Data.SqlClient;
    using System.Data;

    /// <summary>
    /// Implementation of the BonusRequirement Interface
    /// </summary>
    public class BonusRequirementRepository : IBonusRequirementRepository
    {
        /// <summary>
        /// Get all bonus requirement
        /// </summary>
        /// <returns>List of bonus requirement dto</returns>
        public List<BonusRequirementDto> GetAll() {
            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCommission))
            {
                var data = (from r in context.BonusRequirements
                            select new BonusRequirementDto()
                            {
                                BonusRequirementId = r.BonusRequirementsId,
                                BonusAmount = r.BonusAmount,
                                BonusMaxAmount = r.BonusMaxAmount,
                                BonusMaxPercent = r.BonusMaxPercent,
                                BonusPercent = r.BonusPercent,
                                MaxTitleId = r.MaxTitleId,
                                MinTitleId = r.MinTitleId,
                                BonusTypeId = r.BonusTypeId,
                                CountryId = r.CountryId,
                                CurrencyTypeId = r.CurrencyTypeId,
                                EffectiveDate = r.EffectiveDate,
                                // RequirementsTreeId = r.RequirementsTreeId
                                BonusMinAmount = r.BonusMinAmount,
                                PayMonth = r.PayMonth
                            }).ToList();

                if (data == null)
                    throw new Exception("BonusRequirement not found");

                return data;
            }
        }

        /// <summary>
        /// Get Bonus Requirement by Id
        /// </summary>
        /// <param name="id">Bonus Requirement Id</param>
        /// <returns>Bonus Requirement Dto</returns>
        public BonusRequirementDto GetById(int id) {
            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCommission))
            {
                var data = (from r in context.BonusRequirements
                            where r.BonusRequirementsId == id
                            select new BonusRequirementDto()
                            {
                                BonusRequirementId = r.BonusRequirementsId,                                
                                BonusAmount = r.BonusAmount,
                                BonusMaxAmount = r.BonusMaxAmount,
                                BonusMaxPercent = r.BonusMaxPercent,
                                BonusPercent = r.BonusPercent,
                                MaxTitleId = r.MaxTitleId,
                                MinTitleId = r.MinTitleId,
                                BonusTypeId = r.BonusTypeId,
                                CountryId = r.CountryId,
                                CurrencyTypeId = r.CurrencyTypeId,
                                EffectiveDate = r.EffectiveDate,
                                //RequirementsTreeId = r.RequirementsTreeId
                                BonusMinAmount = r.BonusMinAmount,
                                PayMonth = r.PayMonth
                            }).FirstOrDefault();

                if (data == null)
                    throw new Exception("BonusRequirement not found");

                return data;
            }
        }

        /// <summary>
        /// Get all bonus requirement by filters
        /// </summary>
        /// <param name="planId">plan id</param>
        /// <param name="bonusTypeId">bonus type id</param>
        /// <returns>List of bonus requirement dto</returns>
        public List<BonusRequirementDto> GetAllByFilters(int planId, int bonusTypeId)
        {
            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCommission))
            {
                var data = from br in context.BonusRequirements 
                           join bt in context.BonusTypes on br.BonusTypeId equals bt.BonusTypeId 
                           join p in context.Plans on bt.PlanId equals p.PlanId                           
                           where p.PlanId == planId && bt.BonusTypeId == bonusTypeId
                           select new BonusRequirementDto()
                           {
                               BonusRequirementId = br.BonusRequirementsId,
                               BonusTypeId = br.BonusTypeId,
                               BonusAmount = br.BonusAmount,
                               BonusMaxAmount = br.BonusMaxAmount,
                               BonusMaxPercent = br.BonusMaxPercent,
                               MaxTitleId = br.MaxTitleId,
                               MinTitleId = br.MinTitleId,
                               BonusPercent = br.BonusPercent,
                               CountryId = br.CountryId,
                               CurrencyTypeId = br.CurrencyTypeId,
                               EffectiveDate = br.EffectiveDate,
                               //RequirementsTreeId = br.RequirementsTreeId,
                               BonusTypeName = bt.Name,
                               PlanName = p.Name,
                               BonusMinAmount = br.BonusMinAmount,
                               PayMonth = br.PayMonth
                           };

                if (data == null)
                    throw new Exception("BonusRequirement not found");

                return data.ToList();
            }
        }

        /// <summary>
        /// Get all bonus requirement by filters
        /// </summary>
        /// <param name="planId">Plan Id</param>
        /// <param name="bonusTypeId">Bonus Type Id</param>
        /// <param name="page">Page to take</param>
        /// <param name="pageSize">Rows count to take</param>
        /// <returns>List of bonus requirement dto</returns>
        public List<BonusRequirementDto> GetAllByFilters(int planId, int bonusTypeId, int page, int pageSize)
        {
            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCommission))
            {
                var data = from br in context.BonusRequirements
                           join bt in context.BonusTypes on br.BonusTypeId equals bt.BonusTypeId
                           join p in context.Plans on bt.PlanId equals p.PlanId
                           where p.PlanId == planId && (bt.BonusTypeId == bonusTypeId || bonusTypeId == 0)
                           select new BonusRequirementDto()
                           {
                               BonusRequirementId = br.BonusRequirementsId,
                               BonusTypeId = br.BonusTypeId,
                               BonusAmount = br.BonusAmount,
                               BonusMaxAmount = br.BonusMaxAmount,
                               BonusMaxPercent = br.BonusMaxPercent,
                               MaxTitleId = br.MaxTitleId,
                               MinTitleId = br.MinTitleId,
                               BonusPercent = br.BonusPercent,
                               CountryId = br.CountryId,
                               CurrencyTypeId = br.CurrencyTypeId,
                               EffectiveDate = br.EffectiveDate,
                               //RequirementsTreeId = br.RequirementsTreeId,
                               BonusTypeName = bt.Name,
                               PlanName = p.Name,
                               BonusMinAmount = br.BonusMinAmount,
                               PayMonth = br.PayMonth
                           };

                if (data == null)
                    throw new Exception("BonusRequirement not found");

                int rowsCount = 1;
                var result = PagedResult(data, page, pageSize, m=>m.BonusRequirementId, true, out rowsCount).ToList();
                foreach (var item in result)
                {
                    item.RowsCount = rowsCount;
                }
             
                return result;
            }
        }
        
        /// <summary>
        /// insert a new Bonus Requirement
        /// </summary>
        /// <param name="dto">Bonus Requirement Dto</param>
        public void Insert(BonusRequirementDto dto)
        {
            var ActiveCountry = new CountryRepository().GetCountries().Where(x => x.Active).FirstOrDefault();

            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCommission))
            {
                context.BonusRequirements.Add(new EntityModels.BonusRequirementsTable()
                {
                    BonusTypeId = dto.BonusTypeId,
                    BonusAmount = dto.BonusAmount,
                    BonusPercent = dto.BonusPercent,
                    BonusMaxAmount = dto.BonusMaxAmount,
                    BonusMaxPercent = dto.BonusMaxPercent,
                    MinTitleId = dto.MinTitleId,
                    MaxTitleId = dto.MaxTitleId,
                    BonusMinAmount = dto.BonusMinAmount,
                    PayMonth = dto.PayMonth,
                    CountryId = ActiveCountry != null ? ActiveCountry.CountryID : 1,
                    CurrencyTypeId = 1,
                    EffectiveDate = dto.EffectiveDate
                });
                
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Update Bonus Requirement
        /// </summary>
        /// <param name="dto">Bonus Requirement Dto</param>
        public void Update(BonusRequirementDto dto)
        {
            var ActiveCountry = new CountryRepository().GetCountries().Where(x => x.Active).FirstOrDefault();

            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCommission))
            {
                var data = (from r in context.BonusRequirements
                            where r.BonusRequirementsId == dto.BonusRequirementId
                            select r).FirstOrDefault();

                if (data == null)
                    throw new Exception("BonusRequirement not found");

                data.BonusTypeId = dto.BonusTypeId;
                data.BonusAmount = dto.BonusAmount;
                data.BonusPercent = dto.BonusPercent;
                data.BonusMaxAmount = dto.BonusMaxAmount;
                data.BonusMaxPercent = dto.BonusMaxPercent;
                data.MinTitleId = dto.MinTitleId;
                data.MaxTitleId = dto.MaxTitleId;
                data.BonusMinAmount = dto.BonusMinAmount;
                data.PayMonth = dto.PayMonth;
                data.CountryId = ActiveCountry != null ? ActiveCountry.CountryID : 1;
                data.CurrencyTypeId = 1;
                data.EffectiveDate = dto.EffectiveDate;

                context.SaveChanges();
            }
        }

        /// <summary>
        /// Delete a Bonus Requirement by Id
        /// </summary>
        /// <param name="id">Bonus Requirement Id</param>
        public void Delete(int id)
        {
            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCommission))
            {
                var data = (from r in context.BonusRequirements
                            where r.BonusRequirementsId == id
                            select r).FirstOrDefault();

                if (data == null)
                    throw new Exception("BonusRequirement not found");

                context.BonusRequirements.Remove(data);
                context.SaveChanges();
            }
        }
        
        /// <summary>
        /// Pages the specified query.
        /// </summary>
        /// <typeparam name="T">Generic Type Object</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="query">The Object query where paging needs to be applied.</param>
        /// <param name="pageNum">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="orderByProperty">The order by property.</param>
        /// <param name="isAscendingOrder">if set to <c>true</c> [is ascending order].</param>
        /// <param name="rowsCount">The total rows count.</param>
        /// <returns></returns>
        private static IQueryable<T> PagedResult<T, TResult>(IQueryable<T> query, int pageNum, int pageSize,
                        Expression<Func<T, TResult>> orderByProperty, bool isAscendingOrder, out int rowsCount)
        {
            if (pageSize <= 0) pageSize = 20;

            //Total result count
            rowsCount = query.Count();

            //If page number should be > 0 else set to first page
            if (rowsCount <= pageSize || pageNum <= 0) pageNum = 1;

            //Calculate nunber of rows to skip on pagesize
            int excludedRows = (pageNum - 1) * pageSize;

            query = isAscendingOrder ? query.OrderBy(orderByProperty) : query.OrderByDescending(orderByProperty);

            //Skip the required rows for the current page and take the next records of pagesize count
            return query.Skip(excludedRows).Take(pageSize);
        }

        #region BR-BO-002 BONO DE AVANCE - KLC
        /// <summary>
        /// Developed By KLC - CSTI
        /// registra el bono de avance a pagar a las consultoras que alcanzaron el titulo de empresaria por primera vez
        /// </summary>
        /// <param name="PeriodID"></param>
        public  void InsAdvanceBonus(int PeriodID)
        {
            var result = DataAccess.ExecWithStoreProcedureSave("Core", "uspBonusAdvancement2",
                new SqlParameter("PeriodID", SqlDbType.Int) { Value = PeriodID }
                );
            InsUpdateTotalCommissions(PeriodID);
        }
        /// <summary>
        /// Developed By KLC - CSTI
        /// actualize la table BonusValues con la suma de los detalles de los calculos de BonusDetails para el periodo
        /// </summary>
        /// <param name="PeriodID"></param>
        public  void InsUpdateTotalCommissions(int PeriodID)
        {
            var result = DataAccess.ExecWithStoreProcedureSave("Core", "uspBonusLoadValues",
                new SqlParameter("PeriodID", SqlDbType.Int) { Value = PeriodID }
                );
        }
        #endregion
    }
}
