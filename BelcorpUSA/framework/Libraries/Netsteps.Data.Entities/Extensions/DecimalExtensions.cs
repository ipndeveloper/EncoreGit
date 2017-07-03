using System.Globalization;
using System.Threading;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Cache;
using System.Configuration;
using NetSteps.Common.Globalization;
using System;
using System.Text.RegularExpressions;

namespace NetSteps.Data.Entities.Extensions
{
    public static class DecimalExtensions
    {
        public static string ToString(this decimal? value, int currencyID)
        {
            CultureInfo culture = currencyID == 0 ? Thread.CurrentThread.CurrentCulture : SmallCollectionCache.Instance.Currencies.GetById(currencyID).Culture;
            return value.ToDecimal().ToString("C", culture);
        }

        public static string ToString(this decimal? value, CultureInfo currencyID)
        {
            //CultureInfo culture = currencyID == 0 ? Thread.CurrentThread.CurrentCulture : SmallCollectionCache.Instance.Currencies.GetById(currencyID).Culture;
            return value.ToDecimal().ToString("C", currencyID);
        }
        public static string ToString(this decimal value, CultureInfo currencyID)
        {
            //CultureInfo culture = currencyID == 0 ? Thread.CurrentThread.CurrentCulture : SmallCollectionCache.Instance.Currencies.GetById(currencyID).Culture;
            return value.ToString("C", currencyID);
        }

        public static string ToString(this decimal value, int currencyID)
        {
            CultureInfo culture = currencyID == 0 ? Thread.CurrentThread.CurrentCulture : SmallCollectionCache.Instance.Currencies.GetById(currencyID) != null ? SmallCollectionCache.Instance.Currencies.GetById(currencyID).Culture : Currency.Load(currencyID).Culture;
            return value.ToString("C", culture);
        }

        public static string ToString(this decimal? value, Currency currency)
        {
            CultureInfo culture = currency == null ? Thread.CurrentThread.CurrentCulture : currency.Culture;
            return value.ToDecimal().ToString("C", culture);
        }

        public static string ToString(this decimal value, Currency currency)
        {
            CultureInfo culture = currency == null ? Thread.CurrentThread.CurrentCulture : currency.Culture;
            return value.ToString("C", culture);
        }

        public static string TwoDecimalPlaces(this decimal value)
        {
            return string.Format("{0:0.##}", value);
        }

        public static decimal FormatGlobalizationDecimal(this decimal input)
        {
            var KeyDecimals = ConfigurationManager.AppSettings["CultureDecimal"];

            /*CS.05MAY2016.Inicio*/
            var culture = CultureInfoCache.GetCultureInfo("En");
            string digitsOnly = @"([A-Za-z$])";
            var output = Convert.ToDecimal(Regex.Replace(input.ToString(), digitsOnly, "", RegexOptions.Compiled));
            return output;
            /*CS.05MAY2016.Fin*/

            //if (KeyDecimals == "ES")
            //{
            //    var culture = CultureInfoCache.GetCultureInfo("En");
            //    var output = Convert.ToDecimal(input, culture);
            //    return output;
            //}
            //else return input;
        }

        public static decimal FormatGlobalizationDecimal(this string input)
        {
            //var KeyDecimals = ConfigurationManager.AppSettings["CultureDecimal"];
            //var culture = CultureInfoCache.GetCultureInfo("En");

            /*CS.05MAY2016.Inicio*/
                string digitsOnly = @"([A-Za-z$])";
                var output = Convert.ToDecimal(Regex.Replace(input.ToString(), digitsOnly, "", RegexOptions.Compiled));
                return output;
            /*CS.05MAY2016.Fin*/
            //////var KeyDecimals = ConfigurationManager.AppSettings["CultureDecimal"];
            //////var culture = CultureInfoCache.GetCultureInfo("En");
            ////////if (KeyDecimals == "ES")
            ////////{
            //////var output = Convert.ToDecimal(input, culture);
            //////return output;
            //}
            //else return Convert.ToDecimal(input, culture);
        }

        public static string ToStringDecimalGlobalization(this decimal? input, int currencyID)
        {
            var KeyDecimals = ConfigurationManager.AppSettings["CultureDecimal"];
            if (KeyDecimals == "ES")
            {
                var culture = CultureInfoCache.GetCultureInfo("En");
                var output = Convert.ToDecimal(input, culture);
                
                //CultureInfo culture = currencyID == 0 ? Thread.CurrentThread.CurrentCulture : SmallCollectionCache.Instance.Currencies.GetById(currencyID).Culture;
                return output.ToString("C", culture);
            }
            else return input.ToString();
        }

        public static string ToStringDecimalGlobalization(this decimal input, int currencyID)
        {
            var KeyDecimals = ConfigurationManager.AppSettings["CultureDecimal"];
            if (KeyDecimals == "ES")
            {
                var culture = CultureInfoCache.GetCultureInfo("En");
                var output = Convert.ToDecimal(input, culture);

                //CultureInfo culture = currencyID == 0 ? Thread.CurrentThread.CurrentCulture : SmallCollectionCache.Instance.Currencies.GetById(currencyID).Culture;
                return output.ToString("C", culture);
            }
            else return input.ToString();
        }
    }
}
