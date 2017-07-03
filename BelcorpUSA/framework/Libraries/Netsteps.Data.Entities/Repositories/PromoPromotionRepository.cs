namespace NetSteps.Data.Entities.Repositories
{
    using NetSteps.Data.Entities.Repositories.Interfaces;
    using System.Collections.Generic;
    using NetSteps.Data.Entities.Dto;
    using System.Linq;
    using System;
    using System.Data.SqlClient;
    using System.Data;
    using NetSteps.Common.Exceptions;

    /// <summary>
    /// This class implements the IPromoPromotionRepository interface.
    /// </summary>
    public class PromoPromotionRepository : IPromoPromotionRepository
    {
        #region HasACombinationOfItems

        public void InsOrCondition(int promotionID)
        {
            DataAccess.ExecWithStoreProcedureSave("Core", "uspInsOrCondition", new SqlParameter("PromotionID", SqlDbType.Int) { Value = promotionID });
        }

        public bool ExistsOrCondition(int promotionID)
        {
            bool result = DataAccess.ExecWithStoreProcedureBool("Core", "uspExistsOrCondition", new SqlParameter("PromotionID", SqlDbType.Int) { Value = promotionID });
            return result;
        }

        #endregion

        #region HasADefinedQVTotal

        public void InsAndConditionQVTotal(int promotionID, decimal QvMin, decimal QvMax)
        {
            DataAccess.ExecWithStoreProcedureSave("Core", "uspInsAndConditionQVTotalRanges", new SqlParameter("PromotionID", SqlDbType.Int) { Value = promotionID },
                                                                                             new SqlParameter("QvMin", SqlDbType.Money) { Value = QvMin },
                                                                                             new SqlParameter("QvMax", SqlDbType.Money) { Value = QvMax });
        }

        public Dictionary<bool,Dictionary<decimal,decimal>> ExistsAndConditionQVTotal(int promotionID)
        {
            Dictionary<bool, Dictionary<decimal, decimal>> result = new Dictionary<bool, Dictionary<decimal, decimal>>();
            IDataReader dr = DataAccess.ExecuteReader(DataAccess.GetCommand("uspExistsAndConditionQVTotalRanges", new Dictionary<string, object>() { { "PromotionID", promotionID } }, ConnectionStrings.BelcorpCore));
            Dictionary<decimal, decimal> qvRange = new Dictionary<decimal, decimal>();
            while (dr.Read())
            {
                qvRange.Add(Convert.ToDecimal(dr["QvMin"]), Convert.ToDecimal(dr["QvMax"]));
                result.Add(Convert.ToBoolean(dr["RetVal"]),qvRange);
                break;
            }
            dr.Close();
            return result;
        }

        #endregion

        public List<PromoPromotionDto> ListPromotions(string Description)
        {
            IDbCommand dbCommand = null;
            try
            {
                var collection = new List<PromoPromotionDto>();
                //dbCommand = DataAccess.SetCommand("uspLoadReportsByCategory", GetReportConnectionString());
                //IDataReader reader = DataAccess.ExecuteReader(dbCommand);
                //Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@OrderNumber", Description } };
                SqlDataReader reader = DataAccess.GetDataReader("uspPromotionTypeConfigurationEspecial", null, "Core");
                while (reader.Read())
                {
                    PromoPromotionDto entidad = new PromoPromotionDto();
                    entidad.PromotionID = Convert.ToInt32(reader["PromotionID"]);
                    if (reader["StartDate"] != DBNull.Value) entidad.StartDate = Convert.ToDateTime(reader["StartDate"]);
                    if (reader["EndDate"] != DBNull.Value) entidad.EndDate = Convert.ToDateTime(reader["EndDate"]);
                    entidad.Description = Convert.ToString(reader["Description"]);
                    entidad.Status = Convert.ToString(reader["Status"]);
                    if (reader["PromotionStatusTypeID"] != DBNull.Value) entidad.PromotionStatusTypeID = Convert.ToInt32(reader["PromotionStatusTypeID"]);
                    if (reader["SuccessorPromotionID"] != DBNull.Value) entidad.SuccessorPromotionID = Convert.ToInt32(reader["SuccessorPromotionID"]);
                    entidad.PromotionKind = DataAccess.GetString("PromotionKind", reader);
                    collection.Add(entidad);
                }
                return collection;
            }
            catch (Exception ex)
            {
                throw new LoadDataException("Unable to load Reports", ex);
            }
            finally
            {
                DataAccess.Close(dbCommand);
            }
        }

        public List<PromoPromotionDto> ListPromotionsByPromotionTypeConfigurationPerPromotions()
        {
            using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCore))
            {
                var PromotionTypeConfigurationID = (DbContext.PromoPromotionTypeConfigurations.Where(r => r.Active == true).Select(r => r.PromotionTypeConfigurationID)).FirstOrDefault();

                var data = (from promotions in DbContext.PromoPromotions
                            join promoStatus in DbContext.PromoPromotionStatusTypes
                             on promotions.PromotionStatusTypeID equals promoStatus.PromotionStatusType
                            join PerPromotions in DbContext.PromoPromotionTypeConfigurationPerPromotions
                            on promotions.PromotionID equals PerPromotions.PromotionID
                            where PerPromotions.PromotionTypeConfigurationID == (int)PromotionTypeConfigurationID
                            select new PromoPromotionDto()
                            {
                                PromotionTypeConfigurationPerPromotionID = PerPromotions.PromotionTypeConfigurationPerPromotionID,
                                PromotionID = promotions.PromotionID,
                                Description = promotions.Description,
                                StartDate = promotions.StartDate,
                                EndDate = promotions.EndDate,
                                Status = promoStatus.TermName
                            });

                if (data == null)
                    return new List<PromoPromotionDto>();
                else
                    return data.ToList();
            }
        }

        public void InsertPromotionRewardEffectApplyOrderItemPropertyValues(int promotionID, int productPriceTypeID)
        {
            using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCore))
            {
                var data = (from promotion in DbContext.PromoPromotions
                            join promotionReward in DbContext.PromoPromotionRewards on promotion.PromotionID equals promotionReward.PromotionID
                            join pre in DbContext.PromoPromotionRewardEffects on promotionReward.PromotionRewardID equals pre.PromotionRewardID
                            where promotion.PromotionID == promotionID
                            select pre);

                foreach (var item in data)
                {
                    DbContext.PromoPromotionRewardEffectApplyOrderItemPropertyValues.Add(new EntityModels.PromoPromotionRewardEffectApplyOrderItemPropertyValueTable()
                    {
                        PromotionRewardEffectID = item.PromotionRewardEffectID,
                        ProductPriceTypeID = productPriceTypeID
                    });
                }

                DbContext.SaveChanges();
            }
        }

        public void UpdatePromotionRewardEffectApplyOrderItemPropertyValues(int promotionID, int productPriceTypeID)
        {
            using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCore))
            {
                var data = (from promotion in DbContext.PromoPromotions
                            join promotionReward in DbContext.PromoPromotionRewards on promotion.PromotionID equals promotionReward.PromotionID
                            join pre in DbContext.PromoPromotionRewardEffects on promotionReward.PromotionRewardID equals pre.PromotionRewardEffectID
                            join preaoipv in DbContext.PromoPromotionRewardEffectApplyOrderItemPropertyValues on pre.PromotionRewardEffectID equals preaoipv.PromotionRewardEffectID
                            where promotion.PromotionID == promotionID
                            select preaoipv);

                foreach (var item in data)
                {
                    item.ProductPriceTypeID = productPriceTypeID;
                }

                DbContext.SaveChanges();

                if (data.Count() == 0)
                    InsertPromotionRewardEffectApplyOrderItemPropertyValues(promotionID, productPriceTypeID);
            }
        }

        public void UpdatePromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmounts(int promotionQualificationId, bool cumulative)
        {
            using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCore))
            {
                var data = (from pqcpttrca in DbContext.PromoPromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmounts
                            where pqcpttrca.PromotionQualificationID == promotionQualificationId
                            select pqcpttrca);

                foreach (var item in data)
                {
                    item.Cumulative = cumulative;
                }

                DbContext.SaveChanges();
            }
        }

        public PromoPromotionDto GetByID(int promotionId)
        {
            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCore))
            {
                var result = (from p in context.PromoPromotions
                              join pq in context.PromoPromotionQualifications on p.PromotionID equals pq.PromotionID
                              join pqcpttrca in context.PromoPromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmounts on pq.PromotionQualificationID equals pqcpttrca.PromotionQualificationID
                              join pqcpttr in context.PromoPromotionQualificationCustomerPriceTypeTotalRanges on pq.PromotionQualificationID equals pqcpttr.PromotionQualificationID
                              where p.PromotionID == promotionId
                              select new PromoPromotionDto()
                              {
                                  PromotionID = p.PromotionID,
                                  Description = p.Description,
                                  StartDate = p.StartDate,
                                  EndDate = p.EndDate,
                                  PromotionKind = p.PromotionKind,
                                  PromotionStatusTypeID = p.PromotionStatusTypeID,
                                  SuccessorPromotionID = p.SuccessorPromotionID,
                                  Cumulative = pqcpttrca.Cumulative,
                                  ConditionProductPriceTypeId = pqcpttr.ProductPriceTypeID
                              }).FirstOrDefault();

                var rewardProductPriceTypeId = (from pr in context.PromoPromotionRewards
                                                join pre in context.PromoPromotionRewardEffects on pr.PromotionRewardID equals pre.PromotionRewardID
                                                join preapipv in context.PromoPromotionRewardEffectApplyOrderItemPropertyValues on pre.PromotionRewardEffectID equals preapipv.PromotionRewardEffectID
                                                where pr.PromotionID == promotionId
                                                select preapipv.ProductPriceTypeID).FirstOrDefault();
                if (result == null)
                    return null;

                result.RewardProductPriceTypeId = rewardProductPriceTypeId;
                return result;
            }
        }

        //public List<PromoPromotionDto> ListPromotionsByPromotionTypeConfigurationEspecial()
        //{
        //    using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCore))
        //    {
        //        var data = (from promotions in DbContext.PromoPromotions
        //                    join promoStatus in DbContext.PromoPromotionStatusTypes on promotions.PromotionStatusTypeID equals promoStatus.PromotionStatusType
        //                    join qualifications in DbContext.PromoPromotionQualifications on promotions.PromotionID equals qualifications.PromotionID
        //                    join promotionReward in DbContext.PromoPromotionRewards on promotions.PromotionID equals promotionReward.PromotionID
        //                    where promotions.PromotionKind == "Default Cart Rewards"
        //                    && (qualifications.PromotionPropertyKey == "Customer PriceType Range" | qualifications.PromotionPropertyKey == "Customer Subtotal Range")
        //                    && promotionReward.PromotionPropertyKey == "Reduced Subtotal"
        //                    select new PromoPromotionDto()
        //                    {
        //                        //PromotionTypeConfigurationPerPromotionID = 0,//PerPromotions.PromotionTypeConfigurationPerPromotionID,
        //                        PromotionID = promotions.PromotionID,
        //                        Description = promotions.Description,
        //                        StartDate = promotions.StartDate,
        //                        EndDate = promotions.EndDate,
        //                        Status = promoStatus.TermName
        //                    });

        //        if (data == null)
        //            return new List<PromoPromotionDto>();
        //        else
        //            return data.ToList();
        //    }
        //}


    }
}
