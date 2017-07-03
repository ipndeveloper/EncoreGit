using System.Collections.Generic;
using nsCore.Extensions;
using NetSteps.Data.Entities.Cache;
using NetSteps.Encore.Core.IoC;

namespace nsCore.Areas.Admin.Models.Shared
{
    public class OrderRulesOptionsModel
    {
        public IEnumerable<NavigationItem> NavItems
        {
            get
            {
                var orderRulesMenuOptionHandler = Create.New<IOrderRulesOptionsHandler>();
                return orderRulesMenuOptionHandler.GetNavItems();
            }
        }
    }

    public interface IOrderRulesOptionsHandler
    {
        IEnumerable<NavigationItem> GetNavItems();
    }

    [ContainerRegister(typeof(IOrderRulesOptionsHandler), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public class DefaultOrderRulesMenuOptionHandler : IOrderRulesOptionsHandler
    {
        public virtual IEnumerable<NavigationItem> GetNavItems()
        {
            var navItems = new List<NavigationItem>
            {
                 new NavigationItem()
                {
                    LinkText = this.GetTerm("BrowseRule", "Browse Rule"),
                    Url = "~/Admin/OrderRules/Index"
                },
                new NavigationItem()
                {
                    LinkText = this.GetTerm("CreateRule", "Create New Rule"),
                    Url = "~/Admin/OrderRules/Edit"
                },
                new NavigationItem()
                {
                    LinkText = this.GetTerm("OrderPreconditions", "Order Pre-conditions"),
                    Url = "~/Admin/OrderRules/OrderPreCondition"
                }
                //new NavigationItem()
                //{
                //    LinkText = this.GetTerm("OrderConditions", "Order Conditions"),
                //    Url = "~/Admin/OrderRules"
                //}
            };

            return navItems;
        }

        protected string GetTerm(string termName, string defaultValue = "")
        {
            return CachedData.Translation.GetTerm(termName, defaultValue);
        }
    }
}