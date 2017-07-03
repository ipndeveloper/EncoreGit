using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetSteps.Common.Configuration;
using NetSteps.Common.Globalization;

namespace DistributorBackOffice.Areas.Orders.Helpers
{
    public static class GeneralHelper
    {
        public static decimal FormatGlobalizationDecimal(this decimal input)
        {
            var KeyDecimals = ConfigurationManager.AppSettings["CultureDecimal"];
            if (KeyDecimals == "ES")
            {
                var culture = CultureInfoCache.GetCultureInfo("En");
                var output = Convert.ToDecimal(input, culture);
                return output;
            }
            else return input;
        }
        
    }
}
