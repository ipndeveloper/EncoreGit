using System;
using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities
{
    public class Routes
    {
        #region Routes
        public static List<RoutesData> Search()
        {
            return RouteDA.Search();
        }

        public static PaginatedList<RoutesData> Search(RouterParametros searchParams)
        {
            try
            {
                return RoutesRepository.Search(searchParams);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        //llenr lokoup
        public static System.Collections.Generic.Dictionary<int, string> GetRoutesSearch(string text)
        {
            Dictionary<int, string> results;
            try
            {
                results = RoutesRepository.SearchRoutesByText(text);

                return results;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }

        }
        /// <summary>
        /// Developed By KLC
        /// Save Route
        /// </summary>
        /// <param name="routeID"></param>
        /// <param name="nameRoute"></param>
        public static int spInsertRoute(int? routeID, string nameRoute)
        {
            try
            {
               return RoutesRepository.spInsertRoute(routeID, nameRoute);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        /// <summary>
        /// Developed By KLC - CSTI
        /// Delete Zonas
        /// </summary>
        /// <param name="routeID"></param>
        /// <returns></returns>
        //public static int spDeleteRouteScopes(int routeID)
        public static int spDeleteRouteScopes(ZonesData Zone)
        {
            try
            {
                //return RoutesRepository.spDeleteRouteScopes(routeID);
                return RoutesRepository.spDeleteRouteScopes(Zone);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        /// <summary>
        /// Developed By KLC - CSTI
        /// Save Zonas
        /// </summary>
        /// <param name="routeID"></param>
        /// <returns></returns>
        public static int InsertRoutesZones(ZonesData Zone)
        {
            try
            {
                return RoutesRepository.InsertRoutesZones(Zone);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        /// <summary>
        /// Developed By KLC
        /// Search City
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static List<StateProvincesData> SearchCitys(string state)
        {
            try
            {
                return RoutesRepository.SearchCitys(state);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        #endregion
        #region Zones
        public static List<ZonesData> SearchZones()
        {
            return RoutesRepository.SearchZones();
        }

        public static PaginatedList<ZonesData> SearchZones(RouterParametros searchParams)
        {
            try
            {
                return RoutesRepository.SearchZones(searchParams);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static List<StateProvincesData> SearchStates()
        {
            return RoutesRepository.SearchStates();
        }

        #endregion
    }
}
