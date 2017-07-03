using System.Linq;
using NetSteps.Common.Globalization;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;
namespace NetSteps.Data.Entities.Extensions
{
    public static class TaxCacheExtensions
    {
        /// <summary>
        /// Copies all of the non-null tax values from the TaxCacheOverride object to the TaxCache object.  
        /// </summary>
        /// <param name="tco">Override to copy values from</param>
        /// <param name="tc">TaxCache to apply values to</param>
        public static void CopyNonNullTaxValues(this TaxCacheOverride tco, TaxCache tc)
        {
            var propsToCheck = typeof(TaxCacheOverride).GetProperties().Where(p => p.PropertyType == typeof(decimal?) || p.PropertyType == typeof(bool?));
            foreach (var prop in propsToCheck)
            {
                if (prop.PropertyType == typeof(bool?))
                {
                    bool? value = prop.GetValue(tco, null) as bool?;
                    if (value.HasValue)
                    {
                        typeof(TaxCache).GetProperty(prop.Name).SetValue(tc, value.Value, null);
                    }
                }
                else if (prop.PropertyType == typeof(decimal?))
                {
                    decimal? value = prop.GetValue(tco, null) as decimal?;
                    if (value.HasValue)
                    {
                        typeof(TaxCache).GetProperty(prop.Name).SetValue(tc, value.Value, null);
                    }
                }
            }
        }

        public static List<PostalCodeData> PostalLookUpByAccountID(int countryId, string zip, int accountID, int addressID = 0)
        {
            List<PostalCodeData> Result = new List<PostalCodeData>();
            Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@PostalCode", zip }, 
                                                                                       { "@CountryID", countryId },
                                                                                       { "@AccountID", accountID } ,
                                                                                       { "@AddressID", addressID } 
            };
            //SqlDataReader reader = DataAccess.GetDataReader("GetPostalLookUp", parameters, "Core");
            SqlDataReader reader = DataAccess.GetDataReader("uspGetPostaCodeByCountry", parameters, "Core");

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Result.Add(new PostalCodeData()
                    {
                        City = Convert.ToString(reader["City"]),
                        County = Convert.ToString(reader["County"]),
                        StateID = Convert.ToInt32(reader["StateID"]),    
                        StateAbbreviation = Convert.ToString(reader["StateAbbreviation"]),
                        Country = Convert.ToString(reader["Country"]),
                        Street = Convert.ToString(reader["Street"]),
                        EditaCounty = Convert.ToBoolean(reader["EditaCounty"]),
                        EditaStreet = Convert.ToBoolean(reader["EditaStreet"]),
                    });
                }
            }
            return Result;
        }
    }
}
