using System;
using NetSteps.Common.Base;

namespace NetSteps.Web.Mvc.Controls.Models.Enrollment
{
    [Serializable]
    public class EnrollmentConfig
    {
        public short SiteTypeID { get; set; }
        public short AccountTypeID { get; set; }
        public bool Enabled { get; set; }
        public OrderedList<EnrollmentStepConfig> Steps { get; set; }
        public EnrollmentSponsorConfig Sponsor { get; set; }
        public EnrollmentBasicInfoConfig BasicInfo { get; set; }
        public EnrollmentBillingConfig Billing { get; set; }
        public EnrollmentWebsiteConfig Website { get; set; }
        public EnrollmentOrderConfig EnrollmentOrder { get; set; }
        public EnrollmentAutoshipConfig Autoship { get; set; }
        public EnrollmentSubscriptionAutoshipConfig Subscription { get; set; }
        public DisbursementProfilesConfig DisbursementProfiles { get; set; }
    }

    [Serializable]
    public class EnrollmentStepConfig
    {
        public string Name { get; set; }
        public string TermName { get; set; }
        public string Controller { get; set; }
        public bool Skippable { get; set; }
        public OrderedList<EnrollmentStepSectionConfig> Sections { get; set; }
    }

    [Serializable]
    public class EnrollmentStepSectionConfig
    {
        public string Name { get; set; }
        public string TermName { get; set; }
        public string Action { get; set; }
        public bool Skippable { get; set; }
        public bool Completed { get; set; }
    }

    [Serializable]
    public class EnrollmentSponsorConfig
    {
        public bool DenySponsorChange { get; set; }
    }

    [Serializable]
    public class EnrollmentBasicInfoConfig
    {
        public bool SetShippingAddressFromMain { get; set; }
        public bool SetBillingAddressFromMain { get; set; }
    }

    [Serializable]
    public class EnrollmentBillingConfig
    {
        public bool HideBillingAddress { get; set; }
    }

    [Serializable]
    public class EnrollmentWebsiteConfig
    {
        public int? AutoshipScheduleID { get; set; }
    }

    [Serializable]
    public class EnrollmentOrderConfig
    {
        public bool ImportShoppingOrder { get; set; }
        public bool SaveAsAutoshipOrder { get; set; }
        public decimal? MinimumCommissionableTotal { get; set; }
    }

    [Serializable]
    public class EnrollmentAutoshipConfig
    {
        public int? AutoshipScheduleID { get; set; }
        public bool ImportShoppingOrder { get; set; }
        public bool Hidden { get; set; }
        public decimal? MinimumCommissionableTotal { get; set; }
        public bool Skippable { get; set; }
    }

    [Serializable]
    public class EnrollmentSubscriptionAutoshipConfig
    {
        public int? AutoshipScheduleID { get; set; }
    }

    [Serializable]
    public class DisbursementProfilesConfig
    {
        public DisbursementProfilesConfig()
        {
            Hidden = true;
        }

        public bool Hidden { get; set; }
    }
}
