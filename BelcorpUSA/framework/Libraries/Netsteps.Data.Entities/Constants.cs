using System;
using System.Collections;

namespace NetSteps.Data.Entities
{
    // TODO: We should generate some of these from the DB since their are foreign
    //  keys to several of them in the DB and data tends to change per client. - JHE

    /// <summary>
    /// http://www.irritatedvowel.com/Programming/Standards.aspx
    /// Naming Conventions for enumerations: 
    ///  - Do not add "Enum" to the end of the enumeration name. 
    ///  - If the enumeration represents a set of bitwise flags, end the name with a plural otherwise keep it singular.
    /// </summary>
    [Serializable]
    public class Constants : NetSteps.Data.Entities.Generated.ConstantsGenerated
    {
        public enum AccountLanguageType
        {
            English = 1,
            Spanish = 2
        }

        public enum UI_MesasgeType
        {
            Error = 0,
            Success = 1,
            Hidden = 2
        }

        public enum SubscriptionAccessLevel
        {
            None = 1,
            Basic = 2,
            Premium = 3
        }
 
        public enum CampaignStaticAttributes
        {
            firstname,
            lastname,
            accountnumber,
            accountid,
            datesubscribed,
            distributionsubscriberid,
            campaignemailid,
            campaignid,
            emailaddress,
            campaignemaillogid
        }

        public enum ChartType
        {
            Enrollments,
            Orders
        }

        public enum CTMHAutoshipSchedule
        {
            SiteSubscriptionAnnual = 1,
            SiteSubscriptionMonthly = 2
        }

        public enum WebPermissionGroups
        {
            OrderEntryBasic,
            OrderEntryFull,
            OrderEntryAdministration,
        }

        public enum OrderSearchField
        {
            None = 0,
            ByCustomerName = 1,
            ByOrderNumber = 2,
            ByOrderStatus = 3,
            ByOrderType = 4,
            ByOrderDate = 5,
            ByDistributorAccount = 6,
            ByHostessId = 7,
            ByPartyDate = 8,
            ByStartDate = 9,
            ByCompleteDate = 10,
            ByConsultantName = 11,
            ByCustomerAccountNumber = 12,
            ByCommissionDate = 13
        }
        public enum ConsultantSearchField
        {
            None = 0,
            AccountNumber = 1,
            Name = 2,
            EnrollmentDate = 3,
            AccountType = 4,
            FullName = 5,
            EMail = 6,
            SponsorID = 7,
            SponsorName = 8,
            CityState = 9,
            UserName = 10
        }

        public enum CountryContext
        {
            None,
            Shipping,
            Billing,
            Enrollment,
            ShipTo,
            BackOffice,
            Commission
        }

        public enum BillinSchedule
        {
            None = 0,
            Annual = 1,
            Monthly = 2
        }

        public enum Market
        {
            Brazil = 56,
            UnitedStates = 1
        }
        
        public enum DataType
        {
            phone,
            address,
            state,
            postalcode,
            addressname,
            city,
            address2
        }

        public enum SystemEventApplication
        {
            DisbursementRunner = 1,
            RealtimeCommissionsRun = 2,
            PublishCommissionsToLive = 3,
            CommissionsPrepRun = 4,
            DailyCommissionRun = 5,
            WeeklyCommissionRun = 6
        }

        public class DbConnections
        {
            public const string NsSqlConnection = "NsSqlConnection";
            public const string NwSqlConnection = "NwSqlConnection";
            public const string NwOledbConnection = "NwOledbConnection";
            public const string NwOdbcConnection = "NwOdbcConnection";
        }

        public enum PageDisplayType
        {
            Default = 0,
            HiddenNavigation = 1
        }

        public const string ItemAlreadyExists = "This item is currently on the order.";
        public const string ItemMinSubTotalReqd = "This order does not meet the minimum amount.";
        public const string ItemMaxSubTotalReqd = "This order does not meet the maximum amount.";
        public const string ItemRelatedAlreadyExists = "This order currently has a related item.";
        public const string ItemEditWarning = "This action may result in the removal of promotional items due to possible subtotal changes.";
        public const string ItemUpdateZeroWarning = "Updating the item quantity to zero will remove the item.";
        public const string CustomerMinimumSubtotalWarning = "One or more orders do not have any items added. Please recheck your paperwork and add items to the customer's order.";
        public const string HostessRewardItemsWarning = "There are no Hostess Reward Items on this gathering order, please recheck your paperwork and add hostess reward items.";

        // Constants for validations
        public const decimal MinimumCustomers = 1;
        public const decimal PartyMinimumDepositPercent = 0.50m;
        public const string HTMLHelperEmailValidaionRegex = @"/^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/";

        //Return Address Type ID
        public static int GetAddressTypeId(AddressType addressTypeName)
        {
            return (int)addressTypeName;
        }

        public static AddressType GetAddressTypeName(int id)
        {
            return (AddressType)id;
        }

        // Site Subscription Types        
        public static Hashtable SiteSubscriptionTypes_CTMH
        {
            get
            {
                Hashtable ht = new Hashtable();
                ht.Add(1, "Email Only");
                ht.Add(2, "Tier I");
                ht.Add(3, "Tier II");
                return ht;
            }
        }

        public enum ReturnStatusCode
        {
            NotChanged = -1,
            NotAvailable = 0,
            Available = 1,
        }

        [Flags]
        public enum TextBoxFilterTypes
        {
            Custom = 1,
            Numbers = 2,
            UppercaseLetters = 4,
            LowercaseLetters = 8,
        }

        /// <summary>
        /// These are used by the address control. - JHE
        /// </summary>
        public enum AddressField
        {
            Attention,
            FirstName,
            LastName,
            FullName,
            Address1,
            Address2,
            Address3,
            City,
            County,
            State,
            StateId,
            Country,
            CountryId,
            PostalCode,
            Email,
            IsDefault,
            PhoneNumber
        }

        public enum RegistrationField
        {
            AccountType,
            FirstName,
            LastName,
            AccountPhoneNumber,
            AccountNumber,
            Email,
            UserName,
            Password,
            TaxExempt,
            SocialSecurityNumber,
            NickName,
            CoApplicant,
            EntityName,
            BirthDate,
            Gender
        }

        public enum BillingProfileField
        {
            BillingProfileName,
            CreditCardNumber,
            FirstName,
            LastName,
            ExpirationDate,
            BillingAddress1,
            PostalCode,
            City,
            State,
            Phone,
            IsDefault
        }

        //public enum AddressField
        //{
        //    AddressName,
        //    Attention,
        //    Address1,
        //    Address2,
        //    Address3,
        //    PostalCode,
        //    PhoneNumber,
        //    IsDefault,
        //    CountryID,
        //    City,
        //    State,
        //    County
        //}


        #region Depreciated Enums that exist in the base class ConstantsGenerated

        //public enum AddressTypeEnum
        //{
        //    NotSet,
        //    MainAddress,
        //    ShipAddress,
        //    BillingAddress
        //}


        //public enum SiteStatus
        //{
        //    UNKNOWN = 0,
        //    PENDING_ENROLLMENT = 1,
        //    PENDING_APPROVAL = 2,
        //    ACTIVE = 3,
        //    DISABLED = 4,
        //    INACTIVATED = 5,
        //    DISABLED_FOR_NON_PAYMENT = 6
        //}

        //public enum Country
        //{
        //    Unknown,
        //    USA = 1,
        //    Canada,
        //    France,
        //    Mexico,
        //    Israel
        //}

        //public enum ProductType
        //{
        //    Dermatology = 1
        //}

        //public enum ProductPriceType
        //{
        //    Retail = 1,
        //    Wholesale = 2,
        //    Upgrade = 3
        //}

        //public enum ProductTaxCategory
        //{
        //    General_Product = 1,
        //    Supplement = 2,
        //    NonTaxed = 3
        //}

        //public enum ProductFileType
        //{
        //    Image = 1
        //}

        //public enum ProductPropertyType
        //{
        //    Size = 1,
        //    Color = 2
        //}

        public enum ProductRelationType
        {
            Adjunct = 1,
            Kit = 2
        }

        //public enum HostessRewardType
        //{
        //    None = 0
        //}

        //public enum OrderItemTypes
        //{
        //    Retail = 1,
        //    Fees = 2
        //}

        //public enum AddressType
        //{
        //    None = 0,
        //    Main = 1,
        //    Shipping = 2,
        //    Billing = 3,
        //    Autoship = 4,
        //    Disbursement = 5
        //}

        //public enum PaymentType
        //{
        //    Unknown = 0,
        //    CreditCard = 1,
        //    Check = 2,
        //    Cash = 3,
        //    EFT = 4,
        //    GiftCard = 5,
        //    BankDeposit = 6,
        //    BanaMex = 7
        //}

        //public enum OrderShipmentStatusTypes
        //{
        //    Pending = 1,
        //    Cancelled = 2,
        //    Shipped = 3
        //}

        //public enum OrderPaymentStatusTypes
        //{
        //    Pending = 1,
        //    Completed = 2
        //}

        //public enum AccountTypeEnum
        //{
        //    Consultant = 1,
        //    PreferredCustomer = 2,
        //    RetailCustomer = 3,
        //    Employee = 4,
        //    AnonymousCustomer = 5,
        //    DE = 6
        //}


        //public enum OrderStatus
        //{
        //    Pending = 1,
        //    PendingError = 2,
        //    PendingCancelled = 3,
        //    Submitted = 4,
        //    Cancelled = 5,
        //    PartiallySubmitted = 6,
        //    SubmittedTemplate = 7 //Submitted Autoship Template (still can be modified after being submitted)
        //}

        /// <summary>
        /// NOTE 1-7 brought from Rodan and Fields Objects
        /// NOTE 12 - is for Natura
        /// NOTe 13 - for Natura
        /// </summary>
        //public enum OrderType
        //{
        //    Retail = 1,
        //    PC = 2,
        //    Consultant = 3,
        //    PCAutoshipTemplate = 4,
        //    ConsultantAutoshipTemplate = 5,
        //    PCAutoship = 6,
        //    ConsultantAutoship = 7,
        //    Enrollment = 8,
        //    EnrollmentAutoshipTemplate = 9,
        //    EnrollmentAutoship = 10,
        //    Override = 11,
        //    Employee = 12,
        //    Material = 13

        //}

        //public enum OrderPaymentStatus
        //{
        //    Pending = 1,
        //    Submitted = 2,
        //    Cancelled = 3
        //}

        //Order type should be defined in Client Enums because it
        //is different between clients
        //public enum OrderType
        //{
        //    Retail = 1,
        //    AutoShip = 2
        //}

        //public enum AccountType
        //{
        //    business,
        //    individual,
        //    preferredcustomer
        //}




        //public enum SiteType
        //{
        //    None = 0,
        //    Corporate = 1,
        //    BusinessManager = 2,
        //    Replicated = 3,
        //    CountryLanding = 4,
        //    SAB = 5,
        //    CRM = 6
        //}



        //public enum Gender
        //{
        //    Unknown,
        //    Male,
        //    Female
        //}


        //public enum CampaignType
        //{
        //    Mass_Emails = 1,
        //    Campaigns = 2,
        //    Event_Based_Emails = 3,
        //    Mass_Alerts = 4
        //}

        #endregion

        public enum NetStepsExceptionType
        {
            NetStepsException,
            NetStepsDataException,
            NetStepsBusinessException,
            NetStepsEntityFrameworkException,
            NetStepsPaymentGatewayException,
            NetStepsApplicationException,
            NetStepsOptimisticConcurrencyException,
            TaxesNotFoundForAddressException
        }

        // TODO: Generate Enums from the Commissions DB later - JHE
        public enum BankAccountTypeEnum
        {
            Checking = 1,
            Savings = 2
            //Other = 3
        }
            
        public enum EditableListTypes
        {
            AccountStatusChangeReason,
            ArchiveType,
            CommunicationPreference,
            ContactCategory,
            ContactMethod,
            ContactStatus,
            ContactType,
            NewsType,
            ReturnReasons,
            ReturnTypes,
            SiteStatusChangeReason,
            SupportTicketPriority,
            SupportTicketCategory,
            ReplacementReason
            //CalendarEventType,
            //CalendarCategory,
            //CalendarPriority,
            //CalendarStatus,
            //CalendarColorCoding,
            //OrderOverrideReason, // Used? (No Table for this yet)- JHE
            //TaskStatus,
            //TaskType,
            //TaskPriority,
            //TaskCategory,
            //TerminationReasons // Used? - JHE
        }

        public enum EntryOrigin
        {
            SystemCore = 1,
            SystemCommissionEngine = 2,
            ManualEntry = 3,
            CommissionPaymentProcessor = 4
        }

        public enum TitleType
        {
            NotSet = 0,
            PaidAS = 1,
            Recognition = 2,
            FQ = 3,
        }

        public enum GatewayAuthorizationStatus
        {
            NotSet = 0,
            Authorized = 2,
            Declined = 3,
            Error
        }

        public enum TreeType
        {
            Placement,
            ECWithRollArounds
        }

        public enum MailMessageGroupAddressSearchType
        {
            NotSet,
            Sent,
            Failed,
            Opened,
            Clicked,
            Totals
        }

        public enum LocalizedKindTable : int
        {
            OrderStatuses = 1,
            OrderTypes = 2,
			AccountStatuses = 3,
			AccountTypes = 4
        }
		
		public static class CacheNames
		{
			public static string SiteCache { get { return "SiteCache"; } }
			public static string UrlSiteLookupCache { get { return "UrlSiteLookupCache"; } }
			public static string SiteSettingKindsCache { get { return "SiteSettingKindsCache"; } }
			public static string SiteSettingsCache { get { return "SiteSettingsCache"; } }
		}

		public enum OrderPendingStates
		{
			Open,
			Quote,
			Completed
		}

        public enum OrderPayments
        {
            Delete = 13
        }
	}
}
	