using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Server;
using System.Data;

namespace NetSteps.Data.Entities.Business
{
    public class TaxCacheSearchData
    {
       
            public	int    TaxCacheID  {get; set;}
            public short TaxDataSourceID { get; set; }
            public int WarehouseAddressID { get; set; }
            public int TaxCategoryID { get; set; }
            public string PostalCode { get; set; }
            public string State { get; set; }
            public string StateAbbreviation { get; set; }
            public string City { get; set; }
            public string County { get; set; }
            public string Street { get; set; }
            public int CountryID { get; set; }
            public string CountyFIPS { get; set; }
            public decimal CitySalesTax { get; set; }
            public decimal CityUseTax { get; set; }
            public decimal CityLocalSales { get; set; }
            public decimal CityLocalUse { get; set; }
            public decimal CountySalesTax { get; set; }
            public decimal CountyUseTax { get; set; }
            public decimal CountyLocalSales { get; set; }
            public decimal CountyLocalUse { get; set; }
            public decimal CountrySalesTax { get; set; }
            public decimal DistrictSalesTax { get; set; }
            public decimal StateSalesTax { get; set; }
            public decimal StateUseTax { get; set; }
            public decimal CombinedSalesTax { get; set; }
            public decimal CombinedUseTax { get; set; }
            public bool CountyDefault { get; set; }
            public bool GeneralDefault { get; set; }
            public bool InCityLimits { get; set; }
            public bool ChargeTaxOnShipping { get; set; }
            public DateTime DateCreatedUTC { get; set; }
            public DateTime DateCachedUTC { get; set; }
            public DateTime EffectiveDateUTC { get; set; }
            public DateTime ExpirationDateUTC { get; set; }
            public float Latitude { get; set; }
            public float Longitude { get; set; }
            public DateTime DataVersion { get; set; }
            public decimal SpecialTax { get; set; }
            public decimal MiscTax { get; set; }
            public decimal TaxPercentage { get; set; }
            public bool Active { get; set; }

            public int iTransaccion { get; set; }


    }


    public class TaxCacheSearchDataType : List<TaxCacheSearchData>, IEnumerable<SqlDataRecord>
    {

        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            var TaxCacheType= new SqlDataRecord(
                        new SqlMetaData("TaxCacheID", SqlDbType.Int),	
                        new SqlMetaData("TaxDataSourceID", SqlDbType.SmallInt),	
                        new SqlMetaData("WarehouseAddressID", SqlDbType.Int),	
                        new SqlMetaData("TaxCategoryID", SqlDbType.Int),	
                        new SqlMetaData("PostalCode", SqlDbType.NVarChar ,100),	
                        new SqlMetaData("State", SqlDbType.NVarChar,500),	
                        new SqlMetaData("StateAbbreviation", SqlDbType.NChar,6),	
                        new SqlMetaData("City", SqlDbType.NVarChar,500),	
                        new SqlMetaData("County", SqlDbType.NVarChar,500),	
                        new SqlMetaData("Street", SqlDbType.VarChar,200),	
                        new SqlMetaData("CountryID", SqlDbType.Int),	
                        new SqlMetaData("CountyFIPS	", SqlDbType.NVarChar,10),
                        new SqlMetaData("CitySalesTax", SqlDbType.Decimal),	
                        new SqlMetaData("CityUseTax", SqlDbType.Decimal),	
                        new SqlMetaData("CityLocalSales", SqlDbType.Decimal),	
                        new SqlMetaData("CityLocalUse", SqlDbType.Decimal),	
                        new SqlMetaData("CountySalesTax", SqlDbType.Decimal),	
                        new SqlMetaData("CountyUseTax", SqlDbType.Decimal),	
                        new SqlMetaData("CountyLocalSales", SqlDbType.Decimal),	
                        new SqlMetaData("CountyLocalUse", SqlDbType.Decimal),	
                        new SqlMetaData("CountrySalesTax", SqlDbType.Decimal),	
                        new SqlMetaData("DistrictSalesTax", SqlDbType.Decimal),	
                        new SqlMetaData("StateSalesTax", SqlDbType.Decimal),	
                        new SqlMetaData("StateUseTax", SqlDbType.Decimal),	
                        new SqlMetaData("CombinedSalesTax", SqlDbType.Decimal),	
                        new SqlMetaData("CombinedUseTax", SqlDbType.Decimal),	
                        new SqlMetaData("CountyDefault	", SqlDbType.Bit),
                        new SqlMetaData("GeneralDefault	", SqlDbType.Bit),
                        new SqlMetaData("InCityLimits", SqlDbType.Bit),	
                        new SqlMetaData("ChargeTaxOnShipping", SqlDbType.Bit),	
                        new SqlMetaData("DateCreatedUTC", SqlDbType.DateTime),	
                        new SqlMetaData("DateCachedUTC	", SqlDbType.DateTime),
                        new SqlMetaData("EffectiveDateUTC", SqlDbType.DateTime),	
                        new SqlMetaData("ExpirationDateUTC", SqlDbType.DateTime),	
                        //new SqlMetaData("Latitude", SqlDbType.Float),	
                        //new SqlMetaData("Longitude", SqlDbType.Float),	
                        new SqlMetaData("DataVersion", SqlDbType.DateTime),	
                        new SqlMetaData("SpecialTax", SqlDbType.Decimal),	
                        new SqlMetaData("MiscTax", SqlDbType.Decimal),	
                        new SqlMetaData("TaxPercentage", SqlDbType.Decimal),	
                        new SqlMetaData("Active	", SqlDbType.Bit),
                        new SqlMetaData("iTransaccion", SqlDbType.Int)
                );

            foreach (TaxCacheSearchData enTax in this)
                {

                  

                    TaxCacheType.SetInt32(0, enTax.TaxCacheID);
                    TaxCacheType.SetInt16(1, enTax.TaxDataSourceID);
                    TaxCacheType.SetInt32(2, enTax.WarehouseAddressID);
                    TaxCacheType.SetInt32(3, enTax.TaxCategoryID);
                    TaxCacheType.SetString(4, enTax.PostalCode);
                    TaxCacheType.SetString(5, enTax.State);

                    TaxCacheType.SetString(6, enTax.StateAbbreviation);
                    TaxCacheType.SetString(7, enTax.City);
                    TaxCacheType.SetString(8, enTax.County);
                    TaxCacheType.SetString(9, enTax.Street);
                    TaxCacheType.SetInt32(10, enTax.CountryID);
                    TaxCacheType.SetString(11, enTax.CountyFIPS);


                    TaxCacheType.SetDecimal(12, enTax.CitySalesTax);
                    TaxCacheType.SetDecimal(13, enTax.CityUseTax);
                    TaxCacheType.SetDecimal(14, enTax.CityLocalSales);
                    TaxCacheType.SetDecimal(15, enTax.CityLocalUse);
                    TaxCacheType.SetDecimal(16, enTax.CountySalesTax);
                    TaxCacheType.SetDecimal(17, enTax.CountyUseTax);
                    TaxCacheType.SetDecimal(18, enTax.CountyLocalSales);
                    TaxCacheType.SetDecimal(19, enTax.CountyLocalUse);
                    TaxCacheType.SetDecimal(20, enTax.CountrySalesTax);
                    TaxCacheType.SetDecimal(21, enTax.DistrictSalesTax);
                    TaxCacheType.SetDecimal(22, enTax.StateSalesTax);
                    TaxCacheType.SetDecimal(23, enTax.StateUseTax);
                    TaxCacheType.SetDecimal(24, enTax.CombinedSalesTax);
                    TaxCacheType.SetDecimal(25, enTax.CombinedUseTax);
                    TaxCacheType.SetBoolean(26, enTax.CountyDefault);
                    TaxCacheType.SetBoolean(27, enTax.GeneralDefault);
                    TaxCacheType.SetBoolean(28, enTax.InCityLimits);
                    TaxCacheType.SetBoolean(29, enTax.ChargeTaxOnShipping);
                    TaxCacheType.SetDateTime(30, enTax.DateCreatedUTC);
                    TaxCacheType.SetDateTime(31, enTax.DateCachedUTC);
                    TaxCacheType.SetDateTime(32, enTax.EffectiveDateUTC);
                    TaxCacheType.SetDateTime(33, enTax.ExpirationDateUTC);
                    //TaxCacheType.SetFloat(34, Latitude);
                    //TaxCacheType.SetFloat(35, Latitude);

                    TaxCacheType.SetDateTime(34, enTax.DataVersion);
                    TaxCacheType.SetDecimal(35, enTax.SpecialTax);
                    TaxCacheType.SetDecimal(36, enTax.MiscTax);
                    TaxCacheType.SetDecimal(37, enTax.TaxPercentage);
                    TaxCacheType.SetBoolean(38, enTax.Active);
                    TaxCacheType.SetInt32(39, enTax.iTransaccion);
                    yield return TaxCacheType;
                }

        }
    }

}
