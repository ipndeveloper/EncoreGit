using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using NetSteps.Common.Configuration;
using NetSteps.Common.Extensions;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Interfaces;

namespace NetSteps.Data.Entities.Tax
{
    public class SimpovaTaxRateProvider : ITaxRateProvider
    {
        public List<TaxRateInfo> GetTaxInfo(string postalCode)
        {
            return GetNonCachedTaxInfo(postalCode);
            //throw new NotImplementedException();

            // TODO: Check DB for valid TaxInfo - JHE

            // TODO: If not available call GetNonCachedTaxInfo(string postalCode) - JHE

            // TODO: Save to DB - JHE

            // TODO: Return result. - JHE
        }

        public List<TaxRateInfo> GetTaxInfo(int countryId, string stateAbbr, string county, string city, string postalCode)
        {
            // TODO: Consider changing the existing (ported code) to take the additional parameters in this method 'GetTaxInfo' - JHE
            return GetNonCachedTaxInfo(postalCode);
        }

        /// <summary>
        ///   Sample Data  Snapshot on 07/08/2009 12:45 pm MST
        ///   <?xml version="1.0" ?> 
        ///   <root>
        ///       <row>
        ///           <postalcode>84115</postalcode> 
        ///           <stateprov>UT</stateprov> 
        ///           <stateprovtax>0.047</stateprovtax> 
        ///           <city>SALT LAKE CITY</city> 
        ///           <citytax>0</citytax> 
        ///           <county>SALT LAKE</county> 
        ///           <countytax>0.0135</countytax> 
        ///           <districttax>0.008</districttax> 
        ///           <combined>0.0685</combined> 
        ///       </row>
        ///       <row>
        ///           <postalcode>84115</postalcode> 
        ///           <stateprov>UT</stateprov> 
        ///           <stateprovtax>0.047</stateprovtax> 
        ///           <city>SOUTH SALT LAKE</city> 
        ///           <citytax>0</citytax> 
        ///           <county>SALT LAKE</county> 
        ///           <countytax>0.0135</countytax> 
        ///           <districttax>0.008</districttax> 
        ///           <combined>0.0685</combined> 
        ///       </row>
        ///   </root>
        /// </summary>
        /// <param name="postalCode"></param>
        /// <returns></returns>
        protected List<TaxRateInfo> GetNonCachedTaxInfo(string postalCode)
        {
            try
            {
                using (new ApplicationUsageLogger(new ExecutionContext(this)))
                {
                    string serverUrl = "http://www.simpova.com/services/tax/rtst.asp?key=5373A63EEFE6&postalcode=" + postalCode;
                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(serverUrl);
                    httpWebRequest.MaximumAutomaticRedirections = 60;
                    httpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0b;Windows NT 5.0)";
                    HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.ASCII);

                    XmlTextReader reader = new XmlTextReader(streamReader);
                    List<TaxRateInfo> results = new List<TaxRateInfo>();

                    int cnt = 0;
                    decimal dec;

                    TaxRateInfo cache = new TaxRateInfo(postalCode);

                    while (reader.Read())
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                switch (reader.Name)
                                {
                                    case "row":
                                        cnt++;
                                        cache = new TaxRateInfo(postalCode);
                                        break;
                                    case "city":
                                        cache.City = reader.ReadString();
                                        break;
                                    case "citytax":
                                        if (decimal.TryParse(reader.ReadString(), out dec))
                                            cache.CitySalesTax = dec;
                                        break;
                                    case "stateprov":
                                        cache.StateAbbr = reader.ReadString().ToCleanString();
                                        break;
                                    case "stateprovtax":
                                        if (decimal.TryParse(reader.ReadString(), out dec))
                                            cache.StateSalesTax = dec;
                                        break;
                                    case "county":
                                        cache.County = reader.ReadString();
                                        break;
                                    case "countytax":
                                        if (decimal.TryParse(reader.ReadString(), out dec))
                                            cache.CountySalesTax = dec;
                                        break;
                                    case "districttax":
                                        if (decimal.TryParse(reader.ReadString(), out dec))
                                            cache.DistrictSalesTax = dec;
                                        break;
                                    case "combined":
                                        if (decimal.TryParse(reader.ReadString(), out dec))
                                            cache.CombinedSalesTax = dec;
                                        break;
                                }
                                break;
                            case XmlNodeType.EndElement:
                                if (reader.Name == "row")
                                {
                                    int taxCachingDurationDays = ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.TaxCachingDurationDays, 30);
                                    cache.DateCreated = DateTime.Now;
                                    cache.DateCached = DateTime.Now;
                                    cache.EffectiveDate = DateTime.Now;
                                    cache.ExpirationDate = DateTime.Parse(DateTime.Now.AddDays(taxCachingDurationDays - 1).ToShortDateString() + " 11:59:59pm");
                                    cache.ChargeTaxOnShipping = GetDefaultChargeShippingTax(cache.StateAbbr);
                                    cache.TaxDataSourceID = Constants.TaxDataSource.Simpova.ToInt();
                                    cache.CountryID = Constants.Country.UnitedStates.ToInt();

                                    // Default ChargeTaxOnShipping based on data in StateProvinces table since Simpova does not return this data - JHE
                                    StateProvince state = null;
                                    if (!cache.StateAbbr.IsNullOrEmpty())
                                        state = SmallCollectionCache.Instance.StateProvinces.FirstOrDefault(s => s.StateAbbreviation == cache.StateAbbr);
                                    if (state != null && state.ChargeTaxOnShipping.HasValue)
                                        cache.ChargeTaxOnShipping = state.ChargeTaxOnShipping.Value;

                                    TaxCache taxCache = new TaxCache(cache);
                                    taxCache.Save(); // Save new response to cache table.

                                    results.Add(cache);
                                }
                                break;
                        }
                    }
                    return results;
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public bool GetDefaultChargeShippingTax(string stateAbbr)
        {
            try
            {
                switch (stateAbbr)
                {
                    // Brian Cauley - These have no shipping taxes as of 7/15/09 via ZipSales Rules.txt
                    case "AK":
                    case "AS":
                    case "CN":
                    case "DE":
                    case "FM":
                    case "IA":
                    case "ID":
                    case "LA":
                    case "MD":
                    case "MI":
                    case "MP":
                    case "MT":
                    case "NH":
                    case "OK":
                    case "OR":
                    case "PW":
                    case "UT":
                    case "WY":
                        return false;
                    default:
                        return true;
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
    }
}
