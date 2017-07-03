using System;
using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities
{
    /// <summary>
    /// Period Business Logic
    /// </summary>
    public class SupportLevels
    {
        ///// <summary>
        ///// Default values to filters
        ///// </summary>
        ///// <param name="planId"></param>
        ///// <param name="startDate"></param>
        ///// <param name="endDate"></param>
        ///// <returns></returns>
        //public static SupportMotiveSearchParameters SetDefaultValuesToFilters(bool? status, string name)
        //{
        //    return new SupportMotiveSearchParameters()
        //    {
        //        Active = status.HasValue ? status : false,
        //        Name = name,
        //        SupportMotiveID = 1,
        //        PageIndex = 0,
        //        PageSize = 15,
        //        OrderByDirection = NetSteps.Common.Constants.SortDirection.Ascending
        //    };
        //}


        /// <summary>
        /// Search Periods with filter & pagination parameters
        /// </summary>
        /// <param name="searchParams"></param>
        /// <returns></returns>
        public static List<SupportLevelSearchData> Search(int? ID)
        {
            try
            {
                return SupportLevelRepository.Search(ID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        public static List<SupportLevelSearchData> TraerJeraquiaSupportLevel(int SupportLevelID)
        {
            try
            {
                return SupportLevelDataAccess.TraerJeraquiaSupportLevel(SupportLevelID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }


        public static void Save(SupportLevelSearchData level)
        {
            try
            {
                SupportLevelRepository.Save(level);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        public static void Update(SupportLevelSearchData level)
        {
            try
            {
                SupportLevelRepository.Update(level);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static string Delete(int supportLevelID)
        {
            try
            {
               return SupportLevelRepository.Delete(supportLevelID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }


        public static SupportLevelSearchData Get(int? ID)
        {
            return SupportLevelRepository.Get(ID);
        }

        public static Dictionary<string, string> GetLevels()
        {
            return SupportLevelRepository.GetLevels();
        }

        public static List<SupportLevelSearchData> GetItemsLevelMotives(string ID,string Secc)
        {
            return SupportLevelRepository.GetItemsLevelMotives(ID,Secc);
        }

        public static List<SupportLevelSearchData> ListarJerarquiaSupporLevel(int SupportLevelID)
        {
            return SupportLevelRepository.ListarJerarquiaSupporLevel(SupportLevelID);
        }
    }
}
