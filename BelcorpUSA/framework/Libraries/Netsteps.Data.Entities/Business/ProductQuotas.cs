using System;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Repositories;
using System.Collections.Generic;

namespace NetSteps.Data.Entities.Business
{
    /// <summary>
    /// Quotas of Product, Business Logic
    /// </summary>
    public class ProductQuotas
    {

        #region Instance
        private static ProductQuotas _instance;

        public static ProductQuotas Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ProductQuotas();
                return _instance;
            }
        }

        #endregion

        public static PaginatedList<ProductQuotaSearchData> Search(FilterPaginatedListParameters<ProductQuotaSearchData> parameters, int Active, int LanguageID, string SKUorName)
        {
            try
            {
                return ProductQuotasRepository.Search(parameters, Active, LanguageID, SKUorName);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static void Delete(List<int> items)
        {
            try
            {
                ProductQuotasRepository.Delete(items);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static void ChangeQuotaStatus(List<int> items, bool Active)
        {
            try
            {
                ProductQuotasRepository.ChangeQuotaStatus(items, Active);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static int Save(ProductQuotaSearchData restriction,
                                bool hasAccountTypes,
                                string[] accountTypeIDs,

                                bool hasTitleTypes,
                                string[] paidAsTitleIDs,
                                string[] recognizedTitleIDs,

                                bool hasAccount,
                                string[] accountIDs
            )
        {
            int restrictionID = 0;
            try
            {
                if (hasAccountTypes || hasTitleTypes || hasAccount)
                {
                    restrictionID = ProductQuotasRepository.Save(restriction, paidAsTitleIDs, recognizedTitleIDs, accountTypeIDs, accountIDs);
                    return restrictionID;
                }
                restriction.RestricionType = restriction.RestricionType;
                restrictionID = ProductQuotasRepository.Save(restriction);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return restrictionID;
        }
        public static List<QuotaTypes> SearchQuotaTypes()
        {
            try
            {
                return ProductQuotasRepository.SearchQuotaTypes();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public ProductQuotaEntity LoadFullQuotaByID(int restrictionID, int LanguageID)
        {
            return ProductQuotasRepository.LoadFullQuotaByID(restrictionID, LanguageID);
        }

        public bool UpdateRestrictionState(int restrictionID,
                                           bool state,
                                           bool hasAccount,
                                           string[] accountIDs)
        {
            return ProductQuotasRepository.UpdateRestrictionState(restrictionID, state, hasAccount, accountIDs);
        }

    }
}
