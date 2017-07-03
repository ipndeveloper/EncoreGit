using System;
using System.Collections.Generic;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Business.Logic;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities
{
    public partial class StateProvince
    {
        #region Basic Crud
        public static List<StateProvince> LoadStatesByCountry(int countryID)
        {
            try
            {
                var list = Repository.LoadStatesByCountry(countryID);
                list.Each(item =>
                {
                    item.StartTracking();
                    item.IsLazyLoadingEnabled = true;
                });
                return list;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        #endregion

        public static Dictionary<string, string> DropDownListFromCache()
        {
            StateProvinceBusinessLogic bl = new StateProvinceBusinessLogic();

            return bl.DropDownListFromCache();
        }
    }
}
