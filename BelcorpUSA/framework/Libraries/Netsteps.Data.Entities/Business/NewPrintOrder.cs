using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NetSteps.Common.Base;
using NetSteps.Core.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Data.Entities.Business;


namespace NetSteps.Data.Entities.Business
{
    public partial class NewPrintOrder
    {

        public static Dictionary<string, string> ListStatus()
        {
            Dictionary<string, string> ListStatus = new Dictionary<string, string>();
            //ListStatus.Add("All", "All");
            ListStatus.Add("1", "Active");
            ListStatus.Add("2", "Desactive");

            return ListStatus;
        }

        public static Dictionary<Int32, string> ListPeriod()
        {
            return NewPrintOrderExtensions.ListPeriod();
        }

        public static Dictionary<short, string> ListGeneralSection()
        {
            return NewPrintOrderExtensions.ListGeneralSection();
        }

        public static PaginatedList<NewPrintOrderSearchData> Search(NewPrintOrderSearchParameters parameters)
        {
            try
            {
                return NewPrintOrderExtensions.Search(parameters);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }
        }

        public static List<NewPrintOrderSearchData> SlimSearch(string query, int? pageSize = 1000)
        {
            try
            {
                return NewPrintOrderExtensions.SlimSearch(query, pageSize);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static int InsertGeneralTemplate(NewPrintOrderSearchData dataParameter)
        {
            return NewPrintOrderExtensions.InsertGeneralTemplate(dataParameter);
        }

        public static void UpdateGeneralTemplate(NewPrintOrderSearchData dataParameter)
        {
            NewPrintOrderExtensions.UpdateGeneralTemplate(dataParameter);
        }

        public static NewPrintOrderSearchData GetGeneralTemplate(int GeneralTemplateID)
        {
            return NewPrintOrderExtensions.GetGeneralTemplate(GeneralTemplateID);
        }

        public static GeneralTemplateTranslations GetGeneralTemplateTranslation(int GeneralTemplateTranslationsID)
        {
            return NewPrintOrderExtensions.GetGeneralTemplateTranslation(GeneralTemplateTranslationsID);
        }
        
        public static List<GeneralTemplateTranslations> GetGeneralTemplateTranslations(int GeneralTemplateID)
        {
            return NewPrintOrderExtensions.GetGeneralTemplateTranslations(GeneralTemplateID);
        }

        public static void DelGeneralTemplateTranslations(int GeneralTemplateTranslationsID)
        {
            NewPrintOrderExtensions.DelGeneralTemplateTranslations(GeneralTemplateTranslationsID);
        }

        public static void EditGeneralTemplateTranslations(GeneralTemplateTranslations GeneralTemplateTranslations)
        {
            NewPrintOrderExtensions.EditGeneralTemplateTranslations(GeneralTemplateTranslations);
        }

        public static void InsertGeneralTemplateTranslations(GeneralTemplateTranslations GeneralTemplateTranslations)
        {
            NewPrintOrderExtensions.InsertGeneralTemplateTranslations(GeneralTemplateTranslations);
        }
    }
}
