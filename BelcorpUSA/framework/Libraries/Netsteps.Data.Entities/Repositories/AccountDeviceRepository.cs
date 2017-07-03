using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Extensions;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities.Repositories
{
    public partial class AccountDeviceRepository
    {
        public List<AccountDevice> GetDevicesForNewsNotifications(int newsID, int maxResults)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    int? domainEventQueueItemID = (from deqi in context.DomainEventQueueItems
                                                   join ec in context.EventContexts on deqi.EventContextID equals ec.EventContextID
                                                   where ec.NewsID == newsID
                                                   select deqi.DomainEventQueueItemID).FirstOrDefault();

                    // Get distinct devices that haven't received a notification about this news item yet
					// and if it's an android device, only if no other android on that account has received the notification
					var results = context.AccountDevices
							.Where(ad => !context.DeviceNotifications
								.Any(dn => dn.AccountDeviceID == ad.AccountDeviceID 
									&& dn.DomainEventQueueItemID == domainEventQueueItemID)
								&& ad.Active
								&& !ad.Account.AccountDevices
									.Where(ad2 => ad2.DeviceTypeID == 1)
										.Any(ad2 => ad2.DeviceNotifications
												.Any(dn2 => dn2.DomainEventQueueItemID == domainEventQueueItemID)))
							.DistinctBy(ad => ad.DeviceID)
							.Take(maxResults);

					var finalResults = new List<AccountDevice>();

					foreach (var device in results)
					{
						var account = context.Accounts.FirstOrDefault(a => a.AccountID == device.AccountID);
						var split = account.AccountNumber.Split('-');
						if (split.Length == 2)
						{
							var countryCode = split[1];
							var site = FindSiteWithCountry(countryCode);
							if (site != null)
							{
								var newsItem = News.LoadFull(newsID);
								var prodContent = newsItem.HtmlSection.ProductionContent(site, account.DefaultLanguageID);
								if (prodContent != null)
								{
									finalResults.Add(device);
								}
							}
						}
					}

					return finalResults;
                }
            });
        }

		public AccountDevice Load(string deviceid)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					var device = context.AccountDevices.FirstOrDefault(d => d.DeviceID == deviceid);

					if (device != null)
						device.StartTracking();

					return device;
				}
			});
		}

		private Site FindSiteWithCountry(string country)
		{
			var baseSites = Site.LoadBaseSites();

			Site site = null;

			//todo: cleaner, nicer
			switch (country)
			{
				case "at":
					site = baseSites.FirstOrDefault(s => s.Name.StartsWith("Austria Distributor Back Office"));
					break;
				case "au":
					site = baseSites.FirstOrDefault(s => s.Name.StartsWith("Australia BackOffice"));
					break;
				case "ca":
					site = baseSites.FirstOrDefault(s => s.Name.StartsWith("Canada BackOffice"));
					break;
				case "cz":
					site = baseSites.FirstOrDefault(s => s.Name.StartsWith("Czech Republic Distributor Back Office"));
					break;
				case "dk":
					site = baseSites.FirstOrDefault(s => s.Name.StartsWith("Denmark Distributor Back Office"));
					break;
				case "fi":
					site = baseSites.FirstOrDefault(s => s.Name.StartsWith("Finland Distributor Back Office"));
					break;
				case "fr":
					site = baseSites.FirstOrDefault(s => s.Name.StartsWith("France Distributor Back Office"));
					break;
				case "de":
					site = baseSites.FirstOrDefault(s => s.Name.StartsWith("Germany Distributor Back Office"));
					break;
				case "ie":
					site = baseSites.FirstOrDefault(s => s.Name.StartsWith("Ireland Distributor Back Office"));
					break;
				case "it":
					site = baseSites.FirstOrDefault(s => s.Name.StartsWith("Italy Distributor Back Office"));
					break;
				//case "mx":
				//    site = baseSites.FirstOrDefault(s => s.Name.StartsWith("Mexico Distributor Back Office"));
				//    break;
				case "no":
					site = baseSites.FirstOrDefault(s => s.Name.StartsWith("Norway Distributor Back Office"));
					break;
				case "pl":
					site = baseSites.FirstOrDefault(s => s.Name.StartsWith("Poland Distributor Back Office"));
					break;
				case "sk":
					site = baseSites.FirstOrDefault(s => s.Name.StartsWith("Slovakia Distributor Back Office"));
					break;
				case "se":
					site = baseSites.FirstOrDefault(s => s.Name.StartsWith("Sweden Distributor Back Office"));
					break;
				case "ch":
					site = baseSites.FirstOrDefault(s => s.Name.StartsWith("Switzerland Distributor Back Office"));
					break;
				case "uk":
					site = baseSites.FirstOrDefault(s => s.Name.StartsWith("United Kingdom Distributor Back Office"));
					break;
				case "us":
				default:
					site = baseSites.FirstOrDefault(s => s.Name.StartsWith("BackOffice Base Site"));
					break;
			}

			return site;
		}
    }
}
