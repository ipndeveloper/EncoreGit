namespace NetSteps.Data.Entities.Repositories
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using NetSteps.Data.Entities.Dto;
    using NetSteps.Data.Entities.Repositories.Interfaces;
    using NetSteps.Data.Entities.Utility;
    using NetSteps.Data.Entities.EntityModels;

    /// <summary>
    /// Implementacion de la interface Inteface
    /// </summary>
    public partial class BonusValueRepository : IBonusValueRepository
    {
        public void Insert(BonusValueDto dto)
        {
            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCommission))
            {
                context.BonusValues.Add(new BonusValueTable()
                {
                    BonusValueID = dto.BonusValueID,
                    AccountID = dto.AccountID,
                    BonusAmount = dto.BonusAmount,
                    BonusTypeID = dto.BonusTypeID,
                    CorpBonusAmount = dto.CorpBonusAmount,
                    CorpCurrencyTypeID = dto.CorpCurrencyTypeID,
                    CountryID = dto.CountryID,
                    CurrencyTypeID = dto.CurrencyTypeID,
                    DateModified = dto.DateModified,
                    PeriodID = dto.PeriodID
                });

                context.SaveChanges();
            }
        }
    }
}