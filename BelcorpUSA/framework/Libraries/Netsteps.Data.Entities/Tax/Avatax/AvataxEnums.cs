
namespace NetSteps.Data.Entities.AvataxAPI
{
    public class Constants
    {
        public const string AVATAX_CONFIGSECTION = "AvataxConfig";
        public const string AVATAX_SHIPPINGITEMCODE = "SHIPPING";
        //public const string AVATAX_SHIPPINGTAXCODE = "FR020100";
        public const string AVATAX_SHIPPINGTAXCODE = "ShippingTaxCode";
        // Older Shipping Tax Code FR020200

        public const string AVATAX_HANDLINGITEMCODE = "HANDLING";
        public const string AVATAX_HANDLINGTAXCODE = "HandlingTaxCode";
        public const string AVATAX_RESTOCKINGITEMCODE = "RestockingFeeSKU";

        public const string AVATAX_ACCOUNTPROPERTY_TYPE_NAME = "ExemptReason";

        public const string AVATAX_URL = "Url";
        public const string AVATAX_VIAURL = "ViaUrl";
        public const string AVATAX_USERNAME = "UserName";
        public const string AVATAX_PASSWORD = "Password";
        public const string AVATAX_ACCOUNT = "Account";
        public const string AVATAX_LICENSE = "License";
        public const string AVATAX_COMPANYCODE = "CompanyCode";
        public const string AVATAX_CLIENTPROFILE = "ClientProfile";

        public const string AVATAX_ORIGINADDRESSLINE1 = "OriginAddressLine1";
        public const string AVATAX_ORIGINADDRESSLINE2 = "OriginAddressLine2";
        public const string AVATAX_ORIGINADDRESSLINE3 = "OriginAddressLine3";
        public const string AVATAX_ORIGINADDRESSCITY = "OriginAddressCity";
        public const string AVATAX_ORIGINADDRESSREGION = "OriginAddressRegion";
        public const string AVATAX_ORIGINADDRESSPOSTALCODE = "OriginAddressPostalCode";
        public const string AVATAX_ORIGINADDRESSCOUNTRY = "OriginAddressCountry";
        public const string AVATAX_CLASSNAME = "AvataxCalculator";

        public const string RESTOCKINGFEESKU = "RestockingFeeSKU";

        /// <summary>
        /// TaxOverrideType
        /// </summary>
        public enum TaxOverrideType
        {
            None = 0,
            TaxAmount = 1,
            Exemption = 2,
            TaxDate = 3,
        }
        /// <summary>
        /// Enum for columns used to represent Line data
        /// </summary>
        public enum LineColumns
        {
            No = 0,
            ItemCode,
            Qty,
            Amount,
            Discounted,
            Discount,
            ExemptionNo,
            Reference1,
            Reference2,
            RevAcct,
            TaxCode,
            CustomerUsageType,
            Description,
            IsTaxOverriden,
            TaxOverride,
            TaxOverrideType,
            Reason,
            TaxAmount,
            TaxDate
        }
        /// <summary>
        /// Enum for GetTaxRequest columns
        /// </summary>
        public enum TaxRequestColumns
        {
            DocDate = 0,
            Discount,
            ExemptionNo,
            CustomerUsageType,
            SalespersonCode,
            PurchaseOrderNo,
            LocationCode,
            TaxOverrideType,
            TaxAmount,
            TaxDate,
            Reason,
            CurrencyCode
        }

        public enum DetailLevel
        {
            Summary = 0,
            Document = 1,
            Line = 2,
            Tax = 3,
            Diagnostic = 4,
        }

        public enum CancelCode
        {
            Unspecified = 0,
            PostFailed = 1,
            DocDeleted = 2,
            DocVoided = 3,
            AdjustmentCancelled = 4,
        }

        public enum DocumentType
        {
            Any = -1,
            SalesOrder = 0,
            SalesInvoice = 1,
            PurchaseOrder = 2,
            PurchaseInvoice = 3,
            ReturnOrder = 4,
            ReturnInvoice = 5,
        }
    }
}
