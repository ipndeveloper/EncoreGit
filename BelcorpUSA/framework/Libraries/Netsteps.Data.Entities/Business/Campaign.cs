using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using System.Data.SqlClient;

namespace NetSteps.Data.Entities
{
    public partial class Campaign
    {
        #region Properties

        public DateTime? NextCampaignActionScheduledDate()
        {
            var nextScheduledAction = this.CampaignActions.OrderBy(x => x.NextRunDate).FirstOrDefault();
            return nextScheduledAction != null ? nextScheduledAction.NextRunDate : null;
        }

        #endregion

        public static List<Campaign> LoadAll()
        {
            try
            {
                return Repository.LoadAll();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static List<Campaign> LoadAllFull()
        {
            try
            {
                return Repository.LoadAllFull();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static PaginatedList<CampaignSearchData> Search(CampaignSearchParameters searchParameters)
        {
            try
            {
                return Repository.Search(searchParameters);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static List<Campaign> LoadFullAllByDomainEventTypeID(int domainEventTypeID)
        {
            try
            {
                return Repository.LoadFullAllByDomainEventTypeID(domainEventTypeID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static List<Campaign> LoadFullAllByCampaignTypeID(short campaignTypeID)
        {
            try
            {
                return Repository.LoadFullAllByCampaignTypeID(campaignTypeID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }


        #region FHP
        public static bool ProductValidate(int productID)
        {
            int isValid = CampaignActionExtensions.IsValid(productID);
            if (isValid == 0)
            {
                return false;
            }
            return true;
        }

        public static void ProductWithCatalog(int product, int catalog, string productTypeName, string productName)
        {
            int value = 0;
            if (ProductValidate(product))
            {
                value = CampaignActionExtensions.ProductWithCatalog(product, catalog, productTypeName, productName);              
            }
        }

        public static bool ProductWithMaterial(int product, int catalog)
        {
            int value = 0;
            bool result = false;
            if (ProductValidate(product))
            {
                value = CampaignActionExtensions.ProductWithMaterial(product, catalog);
                result = true;
            }
            return result;
        }

        public static bool ProductPrice(int product, int catalog)
        {
            int value = 0;
            bool result = false;
            if (ProductValidate(product))
            {
                value = CampaignActionExtensions.ProductPrice(product, catalog);
                result = true;
            }
            return result;
        } 

        #endregion

        //@01 20150724 BR-COM-002 G&S LIB: Se crea el mètodo para busqueda de Newsletters
        public static List<Campaign> BrowseCampaignsForNewsletters()
        {
            List<Campaign> result = new List<Campaign>();
            try
            {
                SqlDataReader reader = DataAccess.GetDataReader("spGetCampaigns", null, "Core");

                if (reader.HasRows)
                {
                    result = new List<Campaign>();
                    while (reader.Read())
                    {
                        result.Add(new Campaign()
                        {
                            CampaignID = Convert.ToInt32(reader["CampaignID"]),
                            Name = Convert.ToString(reader["Name"])

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;
        }
    }

}

