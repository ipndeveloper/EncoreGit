using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Data.Entities.STOWebServices;

namespace NetSteps.Data.Entities.Tax.SalesTaxOfficeIntegration.Codes
{
    public enum HalfPriceItemTaxTreatment
    {
        NetSellingPrice,
        GrossBeforeCredit,
        NoTax
    }

    public enum HostessCreditItemTaxTreatment
    {
        Cost,
        SuggestedRetail,
        NoTax
    }

    public static class StateTaxTreatment
    {
        #region Private Fields
        internal static readonly Dictionary<string, HalfPriceItemTaxTreatment> HalfPriceItemTreatment
            = new Dictionary<string, HalfPriceItemTaxTreatment>
            {
                { "AL", HalfPriceItemTaxTreatment.NetSellingPrice },
		        { "AK", HalfPriceItemTaxTreatment.GrossBeforeCredit },
		        { "AZ", HalfPriceItemTaxTreatment.GrossBeforeCredit },
		        { "AR", HalfPriceItemTaxTreatment.GrossBeforeCredit },
		        { "CA", HalfPriceItemTaxTreatment.GrossBeforeCredit },
		        { "CO", HalfPriceItemTaxTreatment.NetSellingPrice },
		        { "CT", HalfPriceItemTaxTreatment.NetSellingPrice },
		        { "DE", HalfPriceItemTaxTreatment.NoTax },
		        { "DC", HalfPriceItemTaxTreatment.NetSellingPrice },
		        { "FL", HalfPriceItemTaxTreatment.NetSellingPrice },
		        { "GA", HalfPriceItemTaxTreatment.GrossBeforeCredit },
		        { "HI", HalfPriceItemTaxTreatment.NetSellingPrice },
		        { "ID", HalfPriceItemTaxTreatment.GrossBeforeCredit },
		        { "IL", HalfPriceItemTaxTreatment.NetSellingPrice },
		        { "IN", HalfPriceItemTaxTreatment.NetSellingPrice },
		        { "IA", HalfPriceItemTaxTreatment.NetSellingPrice },
		        { "KS", HalfPriceItemTaxTreatment.GrossBeforeCredit },
		        { "KY", HalfPriceItemTaxTreatment.NetSellingPrice},
		        { "LA", HalfPriceItemTaxTreatment.GrossBeforeCredit },
		        { "ME", HalfPriceItemTaxTreatment.NetSellingPrice },
		        { "MD", HalfPriceItemTaxTreatment.GrossBeforeCredit },
		        { "MA", HalfPriceItemTaxTreatment.NetSellingPrice },
		        { "MI", HalfPriceItemTaxTreatment.NetSellingPrice },
		        { "MN", HalfPriceItemTaxTreatment.GrossBeforeCredit },
		        { "MS", HalfPriceItemTaxTreatment.NetSellingPrice },
		        { "MO", HalfPriceItemTaxTreatment.NetSellingPrice },
		        { "MT", HalfPriceItemTaxTreatment.NoTax },
		        { "NE", HalfPriceItemTaxTreatment.NetSellingPrice },
		        { "NV", HalfPriceItemTaxTreatment.GrossBeforeCredit },
		        { "NH", HalfPriceItemTaxTreatment.NoTax },
		        { "NJ", HalfPriceItemTaxTreatment.NetSellingPrice },
		        { "NM", HalfPriceItemTaxTreatment.NetSellingPrice },
		        { "NY", HalfPriceItemTaxTreatment.GrossBeforeCredit },
		        { "NC", HalfPriceItemTaxTreatment.GrossBeforeCredit },
		        { "ND", HalfPriceItemTaxTreatment.NetSellingPrice },
		        { "OH", HalfPriceItemTaxTreatment.NetSellingPrice },
		        { "OK", HalfPriceItemTaxTreatment.GrossBeforeCredit },
		        { "OR", HalfPriceItemTaxTreatment.NoTax },
		        { "PA", HalfPriceItemTaxTreatment.GrossBeforeCredit },
		        { "RI", HalfPriceItemTaxTreatment.NetSellingPrice },
		        { "SC", HalfPriceItemTaxTreatment.NetSellingPrice },
		        { "SD", HalfPriceItemTaxTreatment.NetSellingPrice },
		        { "TN", HalfPriceItemTaxTreatment.GrossBeforeCredit },
		        { "TX", HalfPriceItemTaxTreatment.NetSellingPrice },
		        { "UT", HalfPriceItemTaxTreatment.NetSellingPrice },
		        { "VT", HalfPriceItemTaxTreatment.GrossBeforeCredit },
		        { "VA", HalfPriceItemTaxTreatment.NetSellingPrice },
		        { "WA", HalfPriceItemTaxTreatment.NetSellingPrice },
		        { "WV", HalfPriceItemTaxTreatment.GrossBeforeCredit },
		        { "WI", HalfPriceItemTaxTreatment.NetSellingPrice },
		        { "WY", HalfPriceItemTaxTreatment.NetSellingPrice }
            };

        internal static readonly Dictionary<string, HostessCreditItemTaxTreatment> HostessCreditItemTreatment
            = new Dictionary<string, HostessCreditItemTaxTreatment>
            {
                { "AL", HostessCreditItemTaxTreatment.Cost },
		        { "AK", HostessCreditItemTaxTreatment.NoTax },
		        { "AZ", HostessCreditItemTaxTreatment.Cost },
		        { "AR", HostessCreditItemTaxTreatment.SuggestedRetail },
		        { "CA", HostessCreditItemTaxTreatment.Cost },
		        { "CO", HostessCreditItemTaxTreatment.Cost },
		        { "CT", HostessCreditItemTaxTreatment.Cost },
		        { "DE", HostessCreditItemTaxTreatment.NoTax },
		        { "DC", HostessCreditItemTaxTreatment.Cost },
		        { "FL", HostessCreditItemTaxTreatment.Cost },
		        { "GA", HostessCreditItemTaxTreatment.Cost },
		        { "HI", HostessCreditItemTaxTreatment.Cost },
		        { "ID", HostessCreditItemTaxTreatment.SuggestedRetail },
		        { "IL", HostessCreditItemTaxTreatment.Cost },
		        { "IN", HostessCreditItemTaxTreatment.Cost },
		        { "IA", HostessCreditItemTaxTreatment.Cost },
		        { "KS", HostessCreditItemTaxTreatment.SuggestedRetail },
		        { "KY", HostessCreditItemTaxTreatment.SuggestedRetail },
		        { "LA", HostessCreditItemTaxTreatment.Cost },
		        { "ME", HostessCreditItemTaxTreatment.Cost },
		        { "MD", HostessCreditItemTaxTreatment.Cost },
		        { "MA", HostessCreditItemTaxTreatment.Cost },
		        { "MI", HostessCreditItemTaxTreatment.Cost },
		        { "MN", HostessCreditItemTaxTreatment.Cost },
		        { "MS", HostessCreditItemTaxTreatment.Cost },
		        { "MO", HostessCreditItemTaxTreatment.Cost },
		        { "MT", HostessCreditItemTaxTreatment.NoTax },
		        { "NE", HostessCreditItemTaxTreatment.Cost },
		        { "NV", HostessCreditItemTaxTreatment.Cost },
		        { "NH", HostessCreditItemTaxTreatment.NoTax },
		        { "NJ", HostessCreditItemTaxTreatment.Cost },
		        { "NM", HostessCreditItemTaxTreatment.Cost },
		        { "NY", HostessCreditItemTaxTreatment.SuggestedRetail },
		        { "NC", HostessCreditItemTaxTreatment.Cost },
		        { "ND", HostessCreditItemTaxTreatment.Cost },
		        { "OH", HostessCreditItemTaxTreatment.Cost },
		        { "OK", HostessCreditItemTaxTreatment.SuggestedRetail },
		        { "OR", HostessCreditItemTaxTreatment.NoTax },
		        { "PA", HostessCreditItemTaxTreatment.Cost },
		        { "RI", HostessCreditItemTaxTreatment.Cost },
		        { "SC", HostessCreditItemTaxTreatment.Cost },
		        { "SD", HostessCreditItemTaxTreatment.Cost },
		        { "TN", HostessCreditItemTaxTreatment.Cost },
		        { "TX", HostessCreditItemTaxTreatment.Cost },
		        { "UT", HostessCreditItemTaxTreatment.Cost },
		        { "VT", HostessCreditItemTaxTreatment.Cost },
		        { "VA", HostessCreditItemTaxTreatment.Cost },
		        { "WA", HostessCreditItemTaxTreatment.Cost },
		        { "WV", HostessCreditItemTaxTreatment.SuggestedRetail },
		        { "WI", HostessCreditItemTaxTreatment.Cost },
		        { "WY", HostessCreditItemTaxTreatment.Cost }
            };
        #endregion
    }

    public enum AddressVerificationResult
    {
        FailedAllValidations = 1,
        PassedAllValidations = 2,
        PassedPostalCodeValidation = 3,
        PassedStateValidation = 4,
        PassedStatePostalValidation = 5,
        PassedCityStatePostalValidation = 6,
        PassedCityStateValidation = 7,
        PassedCityCountyStateValidation = 8,
        PassedCountyStateValidation = 9
    }

    public static class AuthorityType
    {
        public const string City = "3";
        public const string County = "2";
        public const string Federal = "0";
        public const string Local = "4";
        public const string ReportingAgency = "5";
        public const string State = "1";
    }

    public static class CodeExtensions
    {
        public static AddressVerificationResult ToAddressVerificationResult(this int value)
        {
            return (AddressVerificationResult)Enum.Parse(typeof(AddressVerificationResult), value.ToString());
        }

        public static TransactionStatus ToTransactionStatus(this int value)
        {
            return (TransactionStatus)Enum.Parse(typeof(TransactionStatus), value.ToString());
        }

        ///--------------------------------------------------------------------
        /// <summary>
        /// This extension returns the TransactionStatus of the call
        /// response.
        /// </summary>
        ///--------------------------------------------------------------------
        public static TransactionStatus ToTransactionStatus(
            this TaxResponse taxResponse)
        {
            return GetTransactionStatus(taxResponse.TransactionStatus, taxResponse.Messages);
        }

        ///--------------------------------------------------------------------
        /// <summary>
        /// This extension returns the TransactionStatus of the call
        /// response.
        /// </summary>
        ///--------------------------------------------------------------------
        public static TransactionStatus ToTransactionStatus(
            this TransactionDetail transactionDetail)
        {
            return GetTransactionStatus(transactionDetail.transactionStatus, transactionDetail.messagesField);
        }

        ///--------------------------------------------------------------------
        /// <summary>
        /// This will do a thorough examination of the information returned
        /// from STO.  There have been cases where the error-message array
        /// indicates an error, but the TransactionStatus believes the call
        /// succeeded.
        /// </summary>
        ///--------------------------------------------------------------------
        private static TransactionStatus GetTransactionStatus(
            int transactionResult,
            Message[] errorMessages)
        {
            TransactionStatus status = transactionResult.ToTransactionStatus();

            switch (status)
            {
                case TransactionStatus.Unknown:
                case TransactionStatus.Failure:

                    if ((errorMessages != null) && (errorMessages.Length > 0))
                    {
                        // If a (6002) is encountered, then the transactionID
                        // exists.  If a (6012) is encontered, thenwe have an
                        // "in progress" transaction but not updatable.
                        //
                        if (errorMessages.Any(x => x.Code == 6002))
                        {
                            status = TransactionStatus.FailureNoTransactionId;
                        }
                        else if (errorMessages.Any(x => x.Code == 6012))
                        {
                            status = TransactionStatus.FailureInProgressUpdateable;
                        }
                        else if (errorMessages[0].Code != 0)
                        {
                            status = TransactionStatus.Failure;
                        }
                    }
                    break;

                case TransactionStatus.Success:
                    if ((errorMessages != null) && (errorMessages.Length > 0))
                    {
                        status = TransactionStatus.Failure;
                    }
                    break;
            }

            return status;
        }
    }

    public static class CustomerType
    {
        public const string Retail = "08";
    }

    internal static class DataType
    {
        public const string AuthorityTypes = "10";
        public const string BaseTypes = "01";
        public const string Countries = "23";
        public const string CustomerUsageType = "04";
        public const string DivisionsParameter1IsEntityId = "22";
        public const string Entities = "21";
        public const string ExemptionsParameter1IsEntityIdParameter2IsDivisionId = "02";
        public const string GroupItemsParameter1IsEntityIdParameter2IsProductGroupCode = "20";
        public const string LiabilityTypes = "05";
        public const string ProductGroups = "19";
        public const string Provider = "06";
        public const string SitusCanRejectDelivery = "17";
        public const string SitusDeliveryBy = "15";
        public const string SitusFreightOnBoard = "13";
        public const string SitusMailOrder = "14";
        public const string SitusPassFlag = "11";
        public const string SitusPassType = "12";
        public const string SitusShipFromPob = "18";
        public const string SitusSolicitedOutside = "16";
        public const string StatesParameter1IsCountry = "24";
        public const string TaxAuthorities = "07";
        public const string TaxCategories = "08";
        public const string TaxTypes = "03";
        public const string TransactionTypes = "09";
    }

    public static class ProductGroup
    {
        public const string GeneralMerchandise = "0000";
    }

    public static class ProviderType
    {
        public const string Retail = "70";
    }

    public enum TransactionStatus
    {
        Unknown = 0,
        Failure = 1,
        Success = 2,
        InProgress = 4,
        FailureNoTransactionId = 32,
        FailureInProgressUpdateable = 64
    }

    public static class TransactionType
    {
        /// <summary>
        /// STO Value: Sale
        /// </summary>
        public const string SalesTax = "01";
        /// <summary>
        /// STO Value: Purchase
        /// </summary>
        public const string UseTax = "02";
    }

    public static class ObjectExtensions
    {
        public static T TryParse<T>(this T enumType, string value) where T : struct
        {
            T result = enumType;
            try
            {
                result = (T)Enum.Parse(typeof(T), value);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return result;
        }

        public static string ToFormattedString(this STOWebServices.Address address)
        {
            string line1 = string.IsNullOrEmpty(address.Line1) ? string.Empty : (address.Line1 + Environment.NewLine);
            string line2 = string.IsNullOrEmpty(address.Line2) ? string.Empty : (address.Line2 + Environment.NewLine);
            string countryCode = string.IsNullOrEmpty(address.CountryCode) ? string.Empty : (Environment.NewLine + address.CountryCode);
            string formattedString = string.Format("{0}{1}{2}, {3}  {4}{5}", line1, line2, address.City, address.StateOrProvince, address.PostalCode, countryCode);
            return formattedString;
        }

        public static string ToSTOFormattedString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }
    }
}