using NetSteps.Common.Base;
using NetSteps.Data.Entities.Repositories;
using System;
using NetSteps.Data.Entities.Exceptions;
using System.Collections.Generic;
using NetSteps.Data.Entities.EntityModels;
namespace NetSteps.Data.Entities.Business
{
    public class SupportMotives
    { 
        //public SupportMotives()
        //    : base("Core", "SupportMotives", "SupportMotiveID")
        //{
        //}

        /// <summary>
        /// Search Periods with filter & pagination parameters
        /// </summary>
        /// <param name="searchParams"></param>
        /// <returns></returns>
        public static PaginatedList<SupportMotiveSearchData> Search(SupportMotiveSearchParameters searchParams)
        {
            try
            {
                return SupportMotiveRepository.Search(searchParams);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static Dictionary<string, string> SearchAllSupportMotiveLevel( out Dictionary<string, Dictionary<string, string>> dcAdiocionales)
        {
            return SupportMotiveRepository.SearchAllSupportMotiveLevel(out dcAdiocionales);
        }

        /// <summary>
        /// Search Periods with filter & pagination parameters
        /// </summary>
        /// <param name="searchParams"></param>
        /// <returns></returns>
        public static List<SupportMotiveSearchData> Search()
        {
            try
            {
                return SupportMotiveRepository.Search();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        /// <summary>
        /// Search Periods with filter & pagination parameters
        /// </summary>
        /// <param name="searchParams"></param>
        /// <returns></returns>
        public static List<SupportMotiveSearchData> SearchByMotive(string name)
        {
            try
            {
                return SupportMotiveRepository.SearchByMotive(name);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        /// <summary>
        /// Search Periods with filter & pagination parameters
        /// </summary>
        /// <param name="searchParams"></param>
        /// <returns></returns>
        public static List<MarketSearchData> GetMarketsbySupportMotive(string SupportMotiveID)
        {
            try
            {
                return SupportMotiveRepository.GetMarketsbySupportMotive(SupportMotiveID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static List<int> GetMotiveLevelBySupportMotive(int SupportMotiveID)
        {
            try
            {
                return SupportMotiveDataAccess.GetMotiveLevelBySupportMotive(SupportMotiveID);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static string ConcatenarSupportLevel(int SupportLevelID)
        {
            try
            {
                return SupportMotiveDataAccess.ConcatenarSupportLevel(SupportLevelID);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static Dictionary<int, string> GetSuportLevelConcats(System.Data.DataTable dtSupportLevel)
        {
            try
            {
                return SupportMotiveDataAccess.GetSuportLevelConcats(dtSupportLevel);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        
        public static int Save(SupportMotiveSearchData motive, string[] SupportLevelIDs, string[] MarketIDs, out byte  resultadoInsertarSupportLevelMotive, out byte  resultadoEliminarSupportLevelMotive)
        {
            try
            {

                return SupportMotiveRepository.Save(motive, SupportLevelIDs, MarketIDs, out resultadoInsertarSupportLevelMotive, out resultadoEliminarSupportLevelMotive);

            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static void SaveMotiveProperty(SupportMotivePropertyTypeSearchData property)
        {
            try
            {

                SupportMotiveRepository.SaveMotiveProperty(property);

            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static List<SupportMotivePropertyDinamic> GetSupportMotivePropertyDinamic()
        {
            try
            {
                return SupportMotiveRepository.GetSupportMotivePropertyDinamic();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }


        public static PaginatedList<SupportMotivePropertyTypeSearchData> SearchMotiveProperty(SupportMotiveSearchParameters searchParams)
        {
            try
            {
                return SupportMotiveRepository.SearchMotiveProperty(searchParams);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static List<SupportMotivePropertyTypeSearchData> GetPropertyTypesBySupportMotive(int SupportMotiveID)
        {
            return SupportMotiveRepository.GetPropertyTypesBySupportMotive(SupportMotiveID);
        }

        public static void SaveMotiveTask(SupportMotiveTaskSearchData task)
        {
            try
            {

                SupportMotiveRepository.SaveMotiveTask(task);

            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static PaginatedList<SupportMotiveTaskSearchData> SearchMotiveTask(SupportMotiveSearchParameters searchParams)
        {
            try
            {
                return SupportMotiveRepository.SearchMotiveTask(searchParams);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static void DeleteTaskItems(string[] MarketIDs)
        {
            try
            {

                SupportMotiveRepository.DeleteSupportTaskIDs(MarketIDs);
                
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static void DeletePropertyItems(string[] PropertyIDs)
        {
            try
            {

                SupportMotiveRepository.DeleteSupportMotivePropertyIDs(PropertyIDs);

            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static PaginatedList<SupportMotivePropertyValuesSearchData> SearchPropertyValue(SupportMotiveSearchParameters searchParams)
        {
            try
            {
                return SupportMotiveRepository.SearchPropertyValues(searchParams);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static void DeletePropertyValueItems(string[] ValueIDs)
        {
            try
            {
                SupportMotiveRepository.DeletePropertyValueIDs(ValueIDs);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static void SavePropertyValue(SupportMotivePropertyValuesSearchData task)
        {
            try
            {
                SupportMotiveRepository.SavePropertyValue(task);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static PaginatedList<SupportMotiveSearchData> SearchFilter(SupportMotiveSearchParameters supportMotiveSearchParameters)
        {
            return SupportMotiveRepository.SearchFilter(supportMotiveSearchParameters);
        }
    }
}