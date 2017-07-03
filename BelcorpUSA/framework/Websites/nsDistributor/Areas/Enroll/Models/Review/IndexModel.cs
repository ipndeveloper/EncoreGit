using NetSteps.Enrollment.Common.Models.Context;
using nsDistributor.Areas.Enroll.Models.Shared;

namespace nsDistributor.Areas.Enroll.Models.Review
{
    public class IndexModel
    {
        public virtual SummaryModel Summary { get; set; }

        public IndexModel()
        {
            this.Summary = new SummaryModel();
        }

        public virtual IndexModel LoadResources(
            bool showSponsorEditLink,
            bool showSponsor,
            IEnrollmentContext enrollmentContext,
            string pwsUrl,
            bool showShippingEditLink,
            bool showInitialOrder,
            bool showAutoshipOrder,
            bool showSubscriptionAutoshipOrder,
            bool showDisbursementProfiles,
            string editInitialOrderController,
            string editInitialOrderAction,
            string editAutoshipController,
            string editAutoshipAction,
            string editDisbursementProfiles
            )
        {
            this.Summary.LoadResources(
                showSponsorEditLink,
                true,
                showSponsor,
                enrollmentContext,
                pwsUrl,
                showShippingEditLink: showShippingEditLink,
                showInitialOrder: showInitialOrder,
                showAutoshipOrder: showAutoshipOrder,
                showSubscriptionAutoshipOrder: showSubscriptionAutoshipOrder,
                showDisbursementProfiles: showDisbursementProfiles,
                editInitialOrderController: editInitialOrderController,
                editInitialOrderAction: editInitialOrderAction,
                editAutoshipController: editAutoshipController,
                editAutoshipAction: editAutoshipAction,
                editDisbursementProfiles: editDisbursementProfiles
            );

            return this;
        }
    }
}