using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class MarketRepository
    {
        #region Members
        protected override Func<NetStepsEntities, IQueryable<Market>> loadAllFullQuery
        {
            get
            {
                return CompiledQuery.Compile<NetStepsEntities, IQueryable<Market>>(
                 (context) => from m in context.Markets
                                       .Include("Countries")
                                       .Include("Countries.Currencies")
                              select m);
            }
        }
        #endregion

        public List<Market> LoadAllBySiteIDAndUserID(int siteID, int corporateUserID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    // Ported from usp_market_select_all - JHE
                    var corporateUser = (from c in context.CorporateUsers
                                               .Include("User")
                                               .Include("Sites")
                                               .Include("Sites.Market")
                                         where c.CorporateUserID == corporateUserID
                                         select c).FirstOrDefault();

                    if (corporateUserID != 0 && corporateUser == null)
                        throw new Exception("Invalid corporateUserID.");

                    if ((corporateUser == null && corporateUserID == 0) || corporateUser.HasAccessToAllSites)
                    {
                        var availableMarkets = (from m in context.Markets
                                       where m.Sites.Any()
                                       select m).ToList();

                        return availableMarkets;
                    }
                    else
                    {
                        var userMarkets = corporateUser.Sites.Select(s => s.Market).Distinct().ToList();

                        return userMarkets;

                    }
                }
            });
        }
    }
}
