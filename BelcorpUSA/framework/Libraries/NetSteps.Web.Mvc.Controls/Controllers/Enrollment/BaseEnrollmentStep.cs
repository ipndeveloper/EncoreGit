using System.Web.Mvc;

using NetSteps.Data.Common.Context;
using NetSteps.Data.Common.Services;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;
using NetSteps.Enrollment.Common.Models.Config;
using NetSteps.Enrollment.Common.Models.Context;
using NetSteps.Orders.Common.Models;
using NetSteps.Commissions.Common;

namespace NetSteps.Web.Mvc.Controls.Controllers.Enrollment
{
    public class BaseEnrollmentStep : EnrollmentStep
    {
		protected IOrderService OrderService { get { return Create.New<IOrderService>(); } }
		protected readonly ICommissionsService _commissionsService = Create.New<ICommissionsService>();
        public Order InitialOrder
        {
            get
            {
                return (Order)_enrollmentContext.InitialOrder;
            }
            set
            {
                _enrollmentContext.InitialOrder = value;
            }
        }

        public Account EnrollingAccount
        {
            get
            {
                return (Account)_enrollmentContext.EnrollingAccount;
            }
            set
            {
                _enrollmentContext.EnrollingAccount = value;
            }
        }

        public AutoshipOrder AutoshipOrder
        {
            get
            {
                return (AutoshipOrder)_enrollmentContext.AutoshipOrder;
            }
            set
            {
                _enrollmentContext.AutoshipOrder = value;
            }
        }

        public AutoshipOrder SubscriptionOrder
        {
            get
            {
                return (AutoshipOrder)_enrollmentContext.SiteSubscriptionAutoshipOrder;
            }
            set
            {
                _enrollmentContext.SiteSubscriptionAutoshipOrder = value;
            }
        }

        protected override void OnActionExecuted(ActionResult result)
        {
            if (result is ViewResultBase)
            {
                LoadViewData();
            }

            base.OnActionExecuted(result);
        }

        public void OnEnrollmentComplete(Account account, IOrder initialOrder)
        {
            ((Account)account).OnEnrollmentCompleted((Order)initialOrder);
        }

        protected virtual void LoadViewData()
        {
            ViewBag.EnrollmentContext = _enrollmentContext;
            ViewBag.HasNextItem = _enrollmentContext.EnrollmentConfig.Steps.HasNextItem;
            ViewBag.HasPreviousItem = _enrollmentContext.EnrollmentConfig.Steps.HasPreviousItem;
            ViewBag.AllSteps = _enrollmentContext.EnrollmentConfig.Steps;
            ViewBag.IsSkippable = _enrollmentContext.EnrollmentConfig.Steps.CurrentItem.Skippable;
            ViewBag.StepCounter = _enrollmentContext.StepCounter;
        }

		protected virtual Order TotalOrder(Order order)
		{
			var orderContext = Create.New<IOrderContext>();
			orderContext.Order = order;
			OrderService.UpdateOrder(orderContext);

			return orderContext.Order.AsOrder();
		}

        public BaseEnrollmentStep(IEnrollmentStepConfig stepConfig, Controller controller, IEnrollmentContext enrollmentContext)
            : base(stepConfig, controller, enrollmentContext) { }

    }
}
