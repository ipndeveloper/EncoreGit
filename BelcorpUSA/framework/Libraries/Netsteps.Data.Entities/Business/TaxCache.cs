using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Tax;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities
{
    public partial class TaxCache
    {
        #region Constructors
        public TaxCache(TaxRateInfo taxRateInfo)
        {
            this.TaxDataSourceID = Constants.TaxDataSource.Simpova.ToShortNullable();
            this.PostalCode = taxRateInfo.PostalCode;
            this.State = taxRateInfo.State;
            this.StateAbbreviation = taxRateInfo.StateAbbr;
            this.City = taxRateInfo.City;
            this.County = taxRateInfo.County;
            this.CountryID = taxRateInfo.CountryID;
            this.CitySalesTax = taxRateInfo.CitySalesTax;
            this.CityLocalSales = taxRateInfo.CityLocalSales;
            this.CountySalesTax = taxRateInfo.CountySalesTax;
            this.CountyLocalSales = taxRateInfo.CountyLocalSales;
            this.DistrictSalesTax = taxRateInfo.DistrictSalesTax;
            this.StateSalesTax = taxRateInfo.StateSalesTax;
            this.CombinedSalesTax = taxRateInfo.CombinedSalesTax;
            this.ChargeTaxOnShipping = taxRateInfo.ChargeTaxOnShipping;
            this.DateCreated = taxRateInfo.DateCreated;
            this.DateCached = taxRateInfo.DateCached;
            this.EffectiveDate = taxRateInfo.EffectiveDate;
            this.ExpirationDate = taxRateInfo.ExpirationDate;

            if (this.CountryID.HasValue && this.CountryID.Value > 0)
            {
                if (this.State.IsNullOrEmpty() && !this.StateAbbreviation.IsNullOrEmpty())
                {
                    var state = SmallCollectionCache.Instance.StateProvinces.FirstOrDefault(s => s.CountryID == this.CountryID.Value && s.StateAbbreviation == this.StateAbbreviation.ToCleanString());
                    if (state != null)
                        this.State = state.Name;
                }
                else if (!this.State.IsNullOrEmpty() && this.StateAbbreviation.IsNullOrEmpty())
                {
                    var state = SmallCollectionCache.Instance.StateProvinces.FirstOrDefault(s => s.CountryID == this.CountryID.Value && s.Name == this.State.ToCleanString());
                    if (state != null)
                        this.StateAbbreviation = state.StateAbbreviation;
                }
            }

        }
        #endregion

        #region Basic Crud
        public static int GetCount()
        {
            try
            {
                return Repository.Count();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static List<TaxCache> LoadByAddress(string postalCode)
        {
            try
            {
                return Repository.LoadByAddress(postalCode);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static List<TaxCache> LoadByAddress(int countryId, string postalCode)
        {
            try
            {
                return Repository.LoadByAddress(countryId, postalCode);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static List<TaxCache> LoadByAddress(int countryId, string stateAbbr, string county, string city, string postalCode)
        {
            try
            {
                return Repository.LoadByAddress(countryId, stateAbbr, county, city, postalCode);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static List<TaxCache> LoadByProvince(int countryId, string stateAbbr)
        {
            try
            {
                return Repository.LoadByProvince(countryId, stateAbbr);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }


        public static List<TaxCache> CheckForOverrides(IEnumerable<TaxCache> taxes)
        {
            try
            {
                return Repository.CheckForOverrides(taxes);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static List<string> SearchCity(string city)
        {
            try
            {
                return Repository.SearchCity(city);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static List<string> SearchPostalCode(string postalCode)
        {
            try
            {
                return Repository.SearchPostalCode(postalCode);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }


        public static List<PostalCodeData> Search(int countryId, string state, string stateAbbr, string county, string city, string postalCode)
        {
            try
            {
                return Repository.Search(countryId, state, stateAbbr, county, city, postalCode);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        public static List<PostalCodeData> Search(string location)
        {
            try
            {
                return Repository.Search(location);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        #endregion

        public static void CleanOutOldTaxData()
        {
            try
            {
                Repository.CleanOutOldTaxData();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static void MovementsTaxCache(TaxCacheSearchData oenMaterial)
        {
            try
            {
                TaxCacheRepository.MovementsTaxCache(oenMaterial);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }

        }

        public static List<NetSteps.Data.Entities.StateProvince> ListState(TaxCacheSearchData objTaxt)
        {
            try
            {
                return StateProvince.LoadStatesByCountry(objTaxt.CountryID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }

        }


        public static void BulkLoad(string uploadFileName, string path)
        {
            try
            {
                TaxCacheRepository.BulkLoad(uploadFileName,path);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }

        }

        //@01 20150717 BR-CC-003 G&S LIB: Se agregaron los metodos SearchCityFromState y SearchCountyFromCity
        public static List<string> SearchCityFromState(string state)
        {
            try
            {
                return Repository.SearchCityFromState(state);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static List<string> SearchCountyFromCity(string state, string city)
        {
            try
            {
                return Repository.SearchCountyFromCity(state,city);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
    }
}
