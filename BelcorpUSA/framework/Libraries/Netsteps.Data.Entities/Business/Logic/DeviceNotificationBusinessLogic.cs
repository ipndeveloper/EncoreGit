using System.Linq;
using NetSteps.Common.Configuration;
using NetSteps.Data.Entities.DeviceNotifications;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Interfaces;

namespace NetSteps.Data.Entities.Business.Logic
{
    public partial class DeviceNotificationBusinessLogic
    {
        public virtual DeviceNotification GetDeviceNotificationForDomainEvent(AccountDevice device, DomainEventQueueItem domainEvent)
        {
            var deviceNotification = new DeviceNotification()
                                         {
                                            AccountDeviceID = device.AccountDeviceID,
                                            QueueItemStatusID = (short)Constants.QueueItemStatus.Queued,
                                            DomainEventQueueItemID = domainEvent.DomainEventQueueItemID
                                         };
            
            switch (domainEvent.DomainEventTypeID)
            {
                case (short)Constants.DomainEventType.BreakingNews:
					var messageText = string.Empty;
					var newsItem = News.LoadFull(domainEvent.EventContext.NewsID.Value);
					if (newsItem != null)
					{
						var account = device.Account;
						if (account != null)
						{
							var accountNumSplit = account.AccountNumber.Split('-');
							if (accountNumSplit.Length == 2)
							{
								var countryCode = accountNumSplit[1];
								var site = FindSiteWithCountry(countryCode);
								if (site != null)
								{
									var prodContent = newsItem.HtmlSection.ProductionContent(site, account.DefaultLanguageID);
									if (prodContent != null)
									{
										messageText = prodContent.GetTitle();
									}
								}
							}
						}
					}
					deviceNotification.Body = string.Format("{2}={0}%23news-{1}", ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.MobileBackOfficeUrl), domainEvent.EventContext.NewsID, messageText);
                    break;
                default:
                    break;
            }

            return deviceNotification;
        }

        public IDeviceNotificationSender GetDeviceNotificationSender()
        {
            return new DeviceNotificationSender();
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
