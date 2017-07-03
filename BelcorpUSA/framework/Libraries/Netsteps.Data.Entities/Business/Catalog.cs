using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using System.Linq;

namespace NetSteps.Data.Entities
{
	public partial class Catalog
    {
        #region Properties
        
        public string Description { get; set; }

        #endregion

        public static List<Catalog> LoadAll()
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

		public static PaginatedList<CatalogSearchData> Search(FilterPaginatedListParameters<Catalog> searchParams)
		{
			try
			{
				return Repository.Search(searchParams);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static void Copy(int copyFromCatalogID, int copyToCatalogID)
		{
			try
			{
				Repository.Copy(copyFromCatalogID, copyToCatalogID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static void BulkAddCatalogItems(int catalogID, IEnumerable<int> productIDs, DateTime? startDate, DateTime? endDate)
		{
			try
			{
				Repository.BulkAddCatalogItems(catalogID, productIDs, startDate, endDate);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static List<Catalog> LoadAllFullByMarkets(IEnumerable<int> marketIDs)
		{
			try
			{
				return Repository.LoadAllFullByMarkets(marketIDs);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static List<Catalog> LoadAllFull()
		{
			try
			{
				var list = Repository.LoadAllFull();
				foreach (var item in list)
				{
					item.StartTracking();
					item.IsLazyLoadingEnabled = true;
				}
				return list;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static List<Catalog> LoadAllFullExcept(IEnumerable<int> catalogIDs)
		{
			try
			{
				var list = Repository.LoadAllFullExcept(catalogIDs);
				ParallelOptions options = new ParallelOptions();
				Parallel.ForEach(list, options, item =>
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

        public void RemoveAccountTypeFilters(int catalogID)
        {
            try
            {
                Repository.RemoveAccountTypeFilters(catalogID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public void AddAccountTypeFilters(int catalogID, IEnumerable<short> accountTypeIDs)
        {
            try
            {
                Repository.AddAccountTypeFilters(catalogID, accountTypeIDs);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        //Developed by BAL - CSTI - AINI
        public static List<CatalogPeriod> SearchCatalogPeriods(int catalogId)
        {
            try
            {
                return PeriodsRepository.SearchCatalogPeriods(catalogId);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static void SaveCatalogsPeriods(int catalogId, string catalogPeriods)
        {
            try
            {
                var lstCatalogPeriods = catalogPeriods.Split('|');
                PeriodsRepository.SaveCatalogPeriods(catalogId, lstCatalogPeriods);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        //Developed by BAL - CSTI - AFIN

        /// <summary>
        /// Create By FHP
        /// </summary>
        /// <param name="campaignID">Codigo del Perido seleccionado</param>
        /// <returns>Dictionary con el Id y Nombre de los diversos Catálogos</returns>
        public static List<UtilSearchData.Select> GetCatalogByCampaignID(int campaignID)
        {
            try
            {
                return CatalogExtensions.GetCatalogByCampaignID(campaignID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        /// <summary>
        /// Método que retorna los Temporal Matrix Create by FHP
        /// </summary>
        /// <returns>Retorna una lista de todos los Temporal Matrix</returns>
        public static List<TemporalMatrixSearchData> GetTemporalMatrix()
        {
            try
            {
                return CatalogExtensions.GetTemporalMatrix();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        

    }
}

