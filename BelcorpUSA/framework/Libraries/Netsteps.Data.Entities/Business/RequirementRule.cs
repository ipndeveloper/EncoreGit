using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Business
{
    public class RequirementRule
    {
        public static PaginatedList<RequirementRuleSearchData> ListRequirementRules(RequirementRuleSearchParameters searchParameter)
        {
            try
            {
                return RequirementRuleExtensions.ListRequirementRules(searchParameter);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);

            }
        }

        public static Dictionary<string, string> ListRuleTypes()
        {
            return RequirementRuleExtensions.ListRuleTypes();
        }

        public static Dictionary<string, string> ListPlans()
        {
            return RequirementRuleExtensions.ListPlans();
        }

        public static RequirementRuleSearchData GetRuleByID(int id)
        {
            try
            {

                return RequirementRuleExtensions.GetRuleByID(id);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);

            }
        }

        public static RuleTypeSearchData GetRuleTypeByID(int id)
        {
            try
            {
                return RequirementRuleExtensions.GetRuleTypeByID(id);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);

            }
        }

        public static int Save(RequirementRuleSearchData pDato)
        {
            try
            {
                int result = 0;
                if (pDato.RuleRequirementID == 0)
                    result = RequirementRuleExtensions.InsRule(pDato);
                else
                {
                    RequirementRuleExtensions.UpdRule(pDato);
                    result = pDato.RuleRequirementID;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);

            }
        }

        /*CS:04/03/2016*/
        public static MontoMinimoWebSiteEnroll ObtenerMontoMinimoWebSiteEnroll()
        {
            try
            {
                return RequirementRuleExtensions.ObtenerMontoMinimoWebSiteEnroll();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);

            }
        }
        /*CS:04/03/2016*/
    }
}
