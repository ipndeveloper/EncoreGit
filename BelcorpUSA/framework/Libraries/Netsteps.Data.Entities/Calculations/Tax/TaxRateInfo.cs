using System;

namespace NetSteps.Data.Entities.Tax
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Helper class for working with tax data.
    /// Created: 04-13-2010
    /// </summary>
    public class TaxRateInfo
    {
        public int TaxDataSourceID { get; set; }
        public int TaxCategoryID { get; set; }

        public string PostalCode { get; set; }
        public string StateAbbr { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public int CountryID { get; set; }

        public decimal CitySalesTax { get; set; }
        public decimal StateSalesTax { get; set; }
        public decimal CountySalesTax { get; set; }
        public decimal DistrictSalesTax { get; set; }
        public decimal CombinedSalesTax { get; set; }

        public decimal CityLocalSales { get; set; }
        public decimal CountyLocalSales { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateCached { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpirationDate { get; set; }

        public bool ChargeTaxOnShipping { get; set; }

        public TaxRateInfo(string postalCode)
        {
            // Set default values that may not be returned by the tax rates provider
            PostalCode = postalCode;
            ChargeTaxOnShipping = true;
            //TaxCategoryID = 1;// Product
        }
    }
}
