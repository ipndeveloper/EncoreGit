using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Exceptions;
using System.Transactions;
using NetSteps.Data.Entities.EntityModels;

namespace NetSteps.Data.Entities.Business
{
    public partial class Title 
    {
        #region properties

        /// <summary>
        /// Gets or sets Title Id
        /// </summary>
        public int TitleId { get; set; }

        /// <summary>
        /// Gets or sets Title Code
        /// </summary>
        public string TitleCode { get; set; }

        /// <summary>
        /// Gets or sets Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Term Name
        /// </summary>
        public string TermName { get; set; }

        /// <summary>
        /// Gets or sets Sort Order
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating wheter is Active
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets Client Code
        /// </summary>
        public string ClientCode { get; set; }

        /// <summary>
        /// Gets or sets Client Name
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether has Report Visibility
        /// </summary>
        public bool ReportVisibility { get; set; }

        #endregion
        public static PaginatedList<TitleSearchData> ListTitles(FilterDateRangePaginatedListParameters<TitleSearchData> searchParameter)
        {
            try
            {
                return TitleExtensions.ListTitles(searchParameter);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);

            }
        }

        public static TitleSearchData GetTitleByID(int id)
        {
            try
            {
                TitleSearchData myTitle = TitleExtensions.GetTitleByID(id);
                myTitle.RequirementTitleCalculations = TitleExtensions.ListRequirementTitleCalculations(id);
                myTitle.RequirementLegs = TitleExtensions.ListRequirementLegs(id);

                return myTitle;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);

            }
        }

        public static Dictionary<string, string> ListTitles()
        {
            try
            {
                return TitleExtensions.ListTitles();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);

            }
        }

        public static List<RequirementTitleCalculationSearchData> ListRequirementTitleCalculations(int TitleID)
        {
            try
            {
                return TitleExtensions.ListRequirementTitleCalculations(TitleID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);

            }
        }

        public static List<RequirementLegSearchData> ListRequirementLegs(int TitleID)
        {
            try
            {
                return TitleExtensions.ListRequirementLegs(TitleID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);

            }
        }

        public static Dictionary<string, string> ListCalculationTypes(bool? ReportVisibility = null)
        {
            return TitleExtensions.ListCalculationTypes(ReportVisibility);
        }

        public static int SaveTitle(TitleSearchData pTitle)
        {
            try
            {
                int result = 0;
                using (TransactionScope scope = new TransactionScope())
                {
                    if (pTitle.TitleID == 0)
                    {
                        result = TitleExtensions.InsTitle(pTitle);
                        if (pTitle.RequirementTitleCalculations != null)
                            foreach (var ReqTitle in pTitle.RequirementTitleCalculations)
                            {
                                ReqTitle.TitleID = result;
                                TitleExtensions.InsRequirementTitleCalculation(ReqTitle);
                            }

                        if (pTitle.RequirementLegs != null)
                            foreach (var Leg in pTitle.RequirementLegs)
                            {
                                Leg.TitleID = result;
                                TitleExtensions.InsRequirementLeg(Leg);
                            }
                    }
                    else
                    {
                        TitleExtensions.UpdTitle(pTitle);
                        result = pTitle.TitleID;
                        ///*R2841 - CORRECCIÓN INTEGRACIÓN BR - INI*/
                        if (pTitle.RequirementTitleCalculations != null)
                        {
                            TitleExtensions.DelRequirementTitleCalculation(pTitle.TitleID);
                            foreach (var ReqTitle in pTitle.RequirementTitleCalculations)
                            {
                                //Se borrará el item siempre y cuando se haya seleccionado en la vista
                                if (!ReqTitle.isDelete && ReqTitle.CalculationTypeID > 0)
                                {
                                    TitleExtensions.InsRequirementTitleCalculation(ReqTitle);

                                }

                            }
                        }
                        ///*R2841 - CORRECCIÓN INTEGRACIÓN BR - FIN*/ 
                        ///
                        ///*R2841 - CORRECCIÓN INTEGRACIÓN BR - INI*/
                        if (pTitle.RequirementLegs != null)
                        {
                            TitleExtensions.DelRequirementLeg(pTitle.TitleID);
                            foreach (var Leg in pTitle.RequirementLegs)
                            {

                                //Se borrará el item siempre y cuando se haya seleccionado en la vista
                                if (!Leg.isDelete && Leg.TitleRequired > 0)
                                {
                                    TitleExtensions.InsRequirementLeg(Leg);

                                }

                            }

                        }

                        ///*R2841 - CORRECCIÓN INTEGRACIÓN BR - FIN*/


                    }
                    scope.Complete();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);

            }
        }

        public static Dictionary<string, string> ListTitlePhases()
        {
            return TitleExtensions.ListTitlePhases();
        }

        /*R2841 - HUNDRED(CGI) CORRECCIÓN INTEGRACIÓN BR - INI*/
        public static int DeleteRequirementTitle(TitleSearchData pTitle)
        {
            try
            {
                int result = 0;
                using (TransactionScope scope = new TransactionScope())
                {


                    if (pTitle.RequirementTitleCalculations != null)
                    {
                        //result = TitleExtensions.DelRequirementTitleCalculation(pTitle.TitleID);

                    }

                    //if (pTitle.RequirementLegs != null)
                    //{
                    //    TitleExtensions.DelRequirementLeg(result);
                    //    foreach (var Leg in pTitle.RequirementLegs)
                    //    {
                    //        Leg.TitleID = result;
                    //        TitleExtensions.InsRequirementLeg(Leg);
                    //    }
                    //}

                    scope.Complete();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);

            }
        }
        /*R2841 - HUNDRED(CGI) CORRECCIÓN INTEGRACIÓN BR - FIN*/

        public static IEnumerable<Titles> ListTitlesCombo()
        {
            try
            {
                return TitleExtensions.ListTitlesCombo();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }
        }

        public static IEnumerable<TitleInformation> ListTitlesByAccount(int accountID)
        {
            try
            {
                return TitleExtensions.ListTitlesByAccount(accountID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);

            }
        }

        public static IEnumerable<TitleInformationByAccount> ListGetTitlesByAccount(int accountID)
        {
            try
            {
                return TitleExtensions.ListGetTitlesByAccount(accountID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);

            }
        }
    }
}
