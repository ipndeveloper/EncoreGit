using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NetSteps.Enrollment.Common.Models.Context;

namespace nsDistributor.Areas.Enroll.Models.AccountInfo
{
    public class ReferralInfoModel : SectionModel
    {
        #region Values
        [NSRequired]
        [NSDisplayName("ReferrerId", "Referrer Id")]
        public virtual int ReferrerId { get; set; }

        [NSDisplayName("SponsorFullName", "Sponsor Name")]
        public virtual string SponsorFullName { get; set; }

        public bool AllowSponsorChange { get; set; }
        #endregion

        #region infrastructure

        public virtual ReferralInfoModel LoadValues(
            int referrerId, bool allowSponsorChange)
        {
            this.ReferrerId = referrerId;
            this.AllowSponsorChange = allowSponsorChange;
            return this;
        }

        public virtual ReferralInfoModel LoadResources(
           int referrerId, string sponsorFullName, bool allowSponsorChange)
        {
            this.ReferrerId = referrerId;
            this.SponsorFullName = sponsorFullName;
            this.AllowSponsorChange = allowSponsorChange;
            return this;
        }

        public virtual ReferralInfoModel ApplyTo(
            IEnrollmentContext enrollmentContext)
        {
            enrollmentContext.SponsorID = this.ReferrerId;

            return this;
        }
        #endregion
    }
}