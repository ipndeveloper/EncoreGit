namespace NetSteps.Data.Entities.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NetSteps.Data.Entities.Repositories.Interfaces;

    /// <summary>
    /// Implementation of the IBonusTypeRepository Interface
    /// </summary>
    public class BonusTypeRepository : IBonusTypeRepository
    {
        /// <summary>
        /// Get all Bonus Types joined with plans
        /// </summary>
        /// <returns></returns>
        public List<Dto.BonusTypeDto> GetAllByCommission()
        {
            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCommission))
            {
                var data = (from r in context.BonusTypes
                            join p in context.Plans on r.PlanId equals p.PlanId
                            select new Dto.BonusTypeDto()
                            {
                                BonusTypeId = r.BonusTypeId,
                                Name = r.Name,
                                BonusCode = r.BonusCode,
                                ClientCode = r.ClientCode,
                                ClientName = r.ClientName,
                                EarningsTypeId = r.EarningsTypeId,
                                Editable = r.Editable,
                                IsCommission = r.IsCommission,
                                PlanId = r.PlanId,
                                TermName = r.TermName,
                                Enabled = r.Enabled,
                                PlanName = p.Name
                            }).ToList();

                if (data == null)
                    throw new Exception("Bonus Type not found");

                return data;
            }
        }

        /// <summary>
        /// Gets all
        /// </summary>
        /// <returns>List of Bonus Types</returns>
        public List<Dto.BonusTypeDto> GetAll()
        {
            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCommission))
            {
                var data = (from r in context.BonusTypes
                            select new Dto.BonusTypeDto()
                            {
                                BonusTypeId = r.BonusTypeId,
                                Name = r.Name,
                                BonusCode = r.BonusCode,
                                ClientCode = r.ClientCode,
                                ClientName = r.ClientName,
                                EarningsTypeId = r.EarningsTypeId,
                                Editable = r.Editable,
                                IsCommission = r.IsCommission,
                                PlanId = r.PlanId,
                                TermName = r.TermName,
                                Enabled = r.Enabled
                            }).ToList();

                if (data == null)
                    throw new Exception("Bonus Type not found");

                return data;
            }
        }
    }
}