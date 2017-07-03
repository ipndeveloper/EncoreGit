using NetSteps.Data.Entities;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web.Mvc.Models;

namespace nsDistributor.Areas.Enroll.Models.Shared
{
    public interface IOrderEntryModel : IDynamicViewModel
    {
        Order Order { get; set; }
    }

    [ContainerRegister(typeof(IOrderEntryModel), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Default)]
    public class OrderEntryModel : DynamicViewModel, IOrderEntryModel
    {
        public Order Order { get; set; }
    }
}
