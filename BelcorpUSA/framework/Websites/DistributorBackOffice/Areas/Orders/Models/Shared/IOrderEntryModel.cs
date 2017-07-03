using NetSteps.Data.Entities;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web.Mvc.Models;

namespace DistributorBackOffice.Areas.Orders.Models.Shared
{
    public interface IOrderEntryModel : IDynamicViewModel
    {
        /// <summary>
        /// The order for this view, still used by older partials.
        /// Can be removed once the partials have been refactored.
        /// </summary>
        Order Order { get; set; }
    }

    [ContainerRegister(typeof(IOrderEntryModel), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Default)]
    public class OrderEntryModel : DynamicViewModel, IOrderEntryModel
    {
        public Order Order { get; set; }
    }
}