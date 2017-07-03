using NetSteps.Common.Base;
using NetSteps.Encore.Core.IoC;
using System;

namespace NetSteps.Enrollment.Common.Models.Config
{
	public interface IEnrollmentConfig
	{
		int SiteTypeID { get; set; }
		short AccountTypeID { get; set; }
		bool Enabled { get; set; }
		OrderedList<IEnrollmentStepConfig> Steps { get; set; }
		IEnrollmentSponsorConfig Sponsor { get; set; }
		IEnrollmentBasicInfoConfig BasicInfo { get; set; }
		IEnrollmentBillingConfig Billing { get; set; }
		IEnrollmentWebsiteConfig Website { get; set; }
		IEnrollmentOrderConfig EnrollmentOrder { get; set; }
		IEnrollmentAutoshipConfig Autoship { get; set; }
		IEnrollmentSubscriptionAutoshipConfig Subscription { get; set; }
		IDisbursementProfilesConfig DisbursementProfiles { get; set; }
	}

	[Serializable]
	[ContainerRegister(typeof(IEnrollmentConfig), RegistrationBehaviors.Default)]
	public class EnrollmentConfig : IEnrollmentConfig
	{
		public int SiteTypeID { get; set; }
		public short AccountTypeID { get; set; }
		public bool Enabled { get; set; }
		public OrderedList<IEnrollmentStepConfig> Steps { get; set; }
		public IEnrollmentSponsorConfig Sponsor { get; set; }
		public IEnrollmentBasicInfoConfig BasicInfo { get; set; }
		public IEnrollmentBillingConfig Billing { get; set; }
		public IEnrollmentWebsiteConfig Website { get; set; }
		public IEnrollmentOrderConfig EnrollmentOrder { get; set; }
		public IEnrollmentAutoshipConfig Autoship { get; set; }
		public IEnrollmentSubscriptionAutoshipConfig Subscription { get; set; }
		public IDisbursementProfilesConfig DisbursementProfiles { get; set; }
	}
}
