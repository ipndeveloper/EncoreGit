using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using nsCore.Extensions;
using NetSteps.Web.Mvc.Controls.Infrastructure;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Encore.Core.IoC;

namespace nsCore.Areas.Enrollment.Models.Shared
{
	public class EnrollMenuOptionsModel
	{
		public IEnumerable<NavigationItem> NavItems
		{
			get
			{
				var enrollMenuOptionHandler = Create.New<IEnrollMenuOptionHandler>();
				return enrollMenuOptionHandler.GetNavItems();
			}
		}
	}

	public interface IEnrollMenuOptionHandler
	{
		IEnumerable<NavigationItem> GetNavItems();
	}

	[ContainerRegister(typeof(IEnrollMenuOptionHandler), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
	public class DefaultEnrollMenuOptionHandler : IEnrollMenuOptionHandler
	{
		public virtual IEnumerable<NavigationItem> GetNavItems()
		{
			var navItems = new List<NavigationItem>();
			navItems.Add(new NavigationItem() { LinkText = this.GetTerm("Employee", "Employee"), Url = "~/Admin/Users/Edit" });
			if (EnrollmentConfigHandler.AccountTypeEnabled((short)Constants.AccountType.RetailCustomer))
			{
				navItems.Add(new NavigationItem() { LinkText = this.GetTerm("RetailCustomer", "Retail Customer"), Url = "~/Enrollment?acctTypeId=" + (int)Constants.AccountType.RetailCustomer });
			}

			if (EnrollmentConfigHandler.AccountTypeEnabled((short)Constants.AccountType.PreferredCustomer))
			{
				navItems.Add(new NavigationItem() { LinkText = this.GetTerm("PreferredCustomer", "Preferred Customer"), Url = "~/Enrollment?acctTypeId=" + (int)Constants.AccountType.PreferredCustomer });
			}

			if (EnrollmentConfigHandler.AccountTypeEnabled((short)Constants.AccountType.Distributor))
			{
				navItems.Add(new NavigationItem() { LinkText = this.GetTerm("Distributor"), Url = "~/Enrollment?acctTypeId=" + (int)Constants.AccountType.Distributor });
				navItems.Add(new NavigationItem() { LinkText = this.GetTerm("BusinessEntity", "Business Entity"), Url = "~/Enrollment?isEntity=true&acctTypeId=" + (int)Constants.AccountType.Distributor });
			}

			return navItems;
		}

		protected string GetTerm(string termName, string defaultValue = "")
		{
			return CachedData.Translation.GetTerm(termName, defaultValue);
		}
	}
}