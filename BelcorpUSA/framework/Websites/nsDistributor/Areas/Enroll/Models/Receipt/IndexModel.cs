using NetSteps.Auth.UI.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Enrollment.Common.Models.Context;
using nsDistributor.Areas.Enroll.Models.Shared;

namespace nsDistributor.Areas.Enroll.Models.Receipt
{
    public class IndexModel
    {
        public virtual SummaryModel Summary { get; set; }

		public virtual bool DisplayUserName
		{
			get
			{
				var authUIService = Create.New<IAuthenticationUIService>();
				return authUIService.GetConfiguration().ShowUsernameFormFields;
			}
		}

        public virtual string OrderNumber
        {
            get
            {
                return Summary.InitialOrder != null
                    ? Summary.InitialOrder.OrderNumber
                    : Summary.AutoshipOrder != null && Summary.AutoshipOrder.Order != null
                        ? Summary.AutoshipOrder.Order.OrderNumber
                        : null;
            }
        }

        public IndexModel()
        {
            this.Summary = new SummaryModel();
        }

        public virtual IndexModel LoadResources(
            bool showSponsorEditLink,
            bool showSponsor,
            IEnrollmentContext enrollmentContext,
            string pwsUrl,
            bool showInitialOrder,
            bool showAutoshipOrder,
            bool showSubscriptionOrder)
        {
            this.Summary.LoadResources(
                showSponsorEditLink,
                false,
                showSponsor,
                enrollmentContext,
                pwsUrl,
                showInitialOrder: showInitialOrder,
                showAutoshipOrder: showAutoshipOrder,
                showSubscriptionAutoshipOrder: showSubscriptionOrder
            );

            return this;
        }
    }
}