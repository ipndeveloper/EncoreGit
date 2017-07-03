using System;
using System.Collections.Generic;
using System.Threading;
using NetSteps.Data.Entities.STOWebServices;
using NetSteps.Data.Entities.Tax.SalesTaxOfficeIntegration.Codes;

namespace NetSteps.Data.Entities.Tax.SalesTaxOfficeIntegration
{
    public class SalesTaxOfficeGeoCode : SalesTaxOfficeCalculator
    {
        private static object syncRoot = new object();
        /// <summary>
        /// Key:UID(SessionID) Value:PercentComplete
        /// </summary>
        private static Dictionary<string, int> processStatus = new Dictionary<string, int>();
        /// <summary>
        /// Key:UID(SessionID) Value:LastStatusPingDateTime
        /// </summary>
        private static Dictionary<string, DateTime> processLastPing = new Dictionary<string, DateTime>();

        /// <summary>
        /// Returns response.Geoblock
        /// </summary>
        /// <returns>response.Geoblock</returns>
        internal static string LookupGeoCode(STOWebServices.Address address)
        {
            STOServiceBindingClient service = new STOServiceBindingClient();
            GeoblockInfo response;

            try
            {
                response = service.GeoblockRequest(DateTime.Now.ToSTOFormattedString(), address);
            }
            catch (TimeoutException)
            {
                // Try again, the server might have been idle, and the app pool shut down
                Thread.Sleep(2000);
                response = service.GeoblockRequest(DateTime.Now.ToSTOFormattedString(), address);
            }
            finally
            {
                service.Close();
            }

            return response.Geoblock;
        }

    }
}
