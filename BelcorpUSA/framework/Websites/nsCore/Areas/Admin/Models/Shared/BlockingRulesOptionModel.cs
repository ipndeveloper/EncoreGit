using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using nsCore.Extensions;
using NetSteps.Encore.Core.IoC;
using NetSteps.Data.Entities.Cache;

namespace nsCore.Areas.Admin.Models.Shared
{
    /* CGI(AHAA) - BR-BLK-001 - Bloqueo Manual - Inicio */
    public class BlockingRulesOptionModel
    {
        public IEnumerable<NavigationItem> NavItems
        {
            get
            {
                var orderRulesMenuOptionHandler = Create.New<IBlockingRulesOptionHandler>();
                return orderRulesMenuOptionHandler.GetNavItems();
            }
        }

    }

    public interface IBlockingRulesOptionHandler
    {
        IEnumerable<NavigationItem> GetNavItems();
    }

    [ContainerRegister(typeof(IBlockingRulesOptionHandler), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public class DefaultBlockingRulesMenuOptionHandler : IBlockingRulesOptionHandler
    {
        public virtual IEnumerable<NavigationItem> GetNavItems()
        {
            var navItems = new List<NavigationItem>
            {
                new NavigationItem()
                {
                    LinkText = this.GetTerm("BlockingType", "Blocking Type"),
                    Url = "~/Admin/BlockingType"
                },
                new NavigationItem()
                {
                 LinkText = this.GetTerm("BlockingSubType ","Blocking Sub Type"),
                    Url = "~/Admin/BlockingSubType"
                }
            };

            return navItems;
        }

        protected string GetTerm(string termName, string defaultValue = "")
        {
            return CachedData.Translation.GetTerm(termName, defaultValue);
        }
    }
    /* CGI(AHAA) - BR-BLK-001 - Bloqueo Manual - Fin */
}