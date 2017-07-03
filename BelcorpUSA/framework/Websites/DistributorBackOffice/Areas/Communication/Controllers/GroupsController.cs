using DistributorBackOffice.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;

namespace DistributorBackOffice.Areas.Communication.Controllers
{
	public class GroupsController : BaseCommunicationController
	{
		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Communications-Groups", "~/", Constants.SiteType.BackOffice)]
		public virtual ActionResult Edit(int? id)
		{
			try
			{
				var group = new DistributionList();
				if (id.HasValue && id > 0)
				{
					group = DistributionList.Load(id.Value);
				}

				return View(group);
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				throw exception;
			}
		}

		[FunctionFilter("Communications-Groups", "~/", Constants.SiteType.BackOffice)]
		public virtual ActionResult Delete(int groupID)
		{
			try
			{
				if (groupID != 0)
				{
					DistributionList group = DistributionList.Load(groupID);

					group.StartEntityTracking();

					group.Active = false;

					group.Save();

					DistributionListCacheHelper.RemoveDistributionListsForAccountID(CurrentAccountId);

					return Json(new { result = true });
				}

				return Json(new { result = false, message = Translation.GetTerm("ErrorDeletingAccount", "Error deleting Account.") });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: CurrentSite.AccountID);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[FunctionFilter("Communications-Groups", "~/", Constants.SiteType.BackOffice)]
		public virtual ActionResult Save(int groupID, string groupName)
		{
			try
			{
				//Get group object.

				DistributionList group;

				if (groupID != 0)
				{
					group = DistributionList.Load(groupID);
				}
				else
				{
					group = new DistributionList();
				}

				// Check to see whether the name conflicts with an existing list
				var listWithSameName =
					DistributionList.LoadByAccountID(CurrentAccount.AccountID).FirstOrDefault(dl => dl.Active && dl.Name == groupName);

				if (listWithSameName != null)
				{
					return this.Json(
						new
						{
							result = false,
							message = Translation.GetTerm("GroupByNameAlreadyExists", "A group by that name already exists.")
						});
				}

				group.StartEntityTracking();

				group.AccountID = CurrentAccount.AccountID;
				group.Name = groupName;
				group.DistributionListTypeID = (int)Constants.DistributionListType.Groups;
				group.Active = true;

				group.Save();

				DistributionListCacheHelper.RemoveDistributionListsForAccountID(CurrentAccountId);

				return Json(new { result = true, groupID = group.DistributionListID });
			}
			catch (Exception ex)
			{
				var exception =
					EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Communications-Groups", "~/", Constants.SiteType.BackOffice)]
		public virtual ActionResult SaveGroupsForContact(Dictionary<int, bool> groupMemberships, int accountID)
		{
			try
			{
				NetSteps.Data.Entities.Account contact = NetSteps.Data.Entities.Account.LoadByAccountNumberFull(accountID.ToString());

				bool hasChanged = false;

				foreach (KeyValuePair<int, bool> item in groupMemberships)
				{
					DistributionSubscriber current = contact.DistributionSubscribers.FirstOrDefault(d => d.DistributionListID == item.Key);

					if (current == null && item.Value)
					{
						current = new DistributionSubscriber
						{
							DistributionListID = item.Key,
							AccountID = accountID,
							Active = item.Value
						};

						current.Save();

						hasChanged = true;
					}
					else if (current != null && item.Value != current.Active)
					{
						current.StartEntityTracking();

						current.Active = item.Value;

						current.Save();

						hasChanged = true;
					}
				}

				if (hasChanged)
				{
					DistributionListCacheHelper.RemoveDistributionListsForAccountID(CurrentAccount.AccountID);
				}

				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[FunctionFilter("Communications-Groups", "~/", Constants.SiteType.BackOffice)]
		public virtual ActionResult SaveContact(int groupID, int accountID)
		{
			try
			{
				var subscriber = new DistributionSubscriber();

				var group = DistributionListCacheHelper.GetDistributionListByAccountID(CurrentAccountId).FirstOrDefault(c => c.DistributionListID == groupID);
				if (group != null && group.DistributionSubscribers.Any(c => c.AccountID == accountID))
				{
					subscriber = group.DistributionSubscribers.FirstOrDefault(c => c.AccountID == accountID);
				}

				subscriber.StartEntityTracking();

				subscriber.AccountID = accountID;
				subscriber.Active = true;
				subscriber.DistributionListID = groupID;

				subscriber.Save();

				DistributionListCacheHelper.RemoveDistributionListsForAccountID(CurrentAccountId);

				return Json(new { result = true, groupID = subscriber.DistributionListID, distributionSubscriberID = subscriber.DistributionSubscriberID });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[FunctionFilter("Communications-Groups", "~/", Constants.SiteType.BackOffice)]
		public virtual ActionResult SaveContacts(int accountID, Dictionary<int, bool> newValues)
		{
			try
			{
				if (newValues != null && newValues.Any())
				{
					foreach (var item in newValues)
					{
						var listId = item.Key;
						var subscribeToList = item.Value;
						var dList = DistributionList.LoadFull(listId);
						if (dList != null)
						{
							var subscription = dList.DistributionSubscribers.FirstOrDefault(d => d.AccountID == accountID) ?? new DistributionSubscriber();

							if (subscribeToList)
							{
								subscription.StartEntityTracking();
								subscription.AccountID = accountID;
								subscription.Active = true;
								subscription.DistributionListID = dList.DistributionListID;
								subscription.DateSubscribedUTC = DateTime.Now.ApplicationNow().ToUniversalTime();
								subscription.Save();
							}
							else if (subscription.DistributionListID == listId)
							{
								subscription.StartEntityTracking();
								subscription.Active = false;
								subscription.DateCancelledUTC = DateTime.Now.ApplicationNow().ToUniversalTime();
								subscription.Save();
							}
						}
					}

					DistributionListCacheHelper.RemoveDistributionListsForAccountID(CurrentAccountId);

					return Json(new { result = true });
				}
				return Json(new { result = false });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[FunctionFilter("Communications-Groups", "~/", Constants.SiteType.BackOffice)]
		public virtual ActionResult RemoveContact(int groupID, int ID)
		{
			try
			{
				DistributionSubscriber subscriber = DistributionListCacheHelper.GetDistributionListByAccountID(CurrentAccountId).FirstOrDefault(d => d.DistributionListID == groupID).DistributionSubscribers.FirstOrDefault(d => d.DistributionSubscriberID == ID);

				subscriber.StartEntityTracking();

				subscriber.Active = false;

				subscriber.Save();

				DistributionListCacheHelper.RemoveDistributionListsForAccountID(CurrentAccountId);

				return Json(new { result = true, groupID = subscriber.DistributionListID, distributionSubscriberID = subscriber.DistributionSubscriberID });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[OutputCache(CacheProfile = "AutoCompleteData")]
		[FunctionFilter("Communications-Groups", "~/", Constants.SiteType.BackOffice)]
		public virtual ActionResult SearchContacts(string query)
		{
			try
			{
				var contacts = NetSteps.Data.Entities.Account.SlimSearchEmail(query, sponsorID: CurrentAccount.AccountID).Select(p => new
				{
					accountID = p.Key,
					id = p.Key,
					text = p.Value
				});

				return Json(contacts);
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Communications-Groups", "~/", Constants.SiteType.BackOffice)]
		public virtual ActionResult GetGroupContacts(int page, int pageSize, int groupID)
		{
			try
			{
				if (groupID > 0)
				{
					var group = DistributionListCacheHelper.GetDistributionListByAccountID(CurrentAccountId).FirstOrDefault(g => g.DistributionListID == groupID);
					if (group != null)
					{
						var contacts = group.DistributionSubscribers.Where(c => c.Active);
						if (contacts.Any())
						{
							var accounts = NetSteps.Data.Entities.Account.LoadBatchSlim(contacts.Where(c => c.AccountID.HasValue).Select(c => c.AccountID.Value));
							var builder = new StringBuilder();
							foreach (var contact in contacts)
							{
								var account = accounts.FirstOrDefault(a => a.AccountID == contact.AccountID);
								if (account != null)
								{
									builder.Append("<tr>");
									builder.AppendCell(string.Format("<a href=\"javascript:void(0);\" onclick=\"RemoveContact({0}); return false;\" class=\"groupContactRow\"><span>{1}</span></a>", contact.DistributionSubscriberID, Translation.GetTerm("Remove", "Remove")));
									builder.AppendCell(account.FirstName);
									builder.AppendCell(account.LastName);
									builder.AppendCell(account.EmailAddress);
									builder.Append("</tr>");
								}
							}

							return Json(new { result = true, totalPages = Math.Ceiling(contacts.Count() / (double)pageSize), page = builder.ToString() });
						}
					}
				}

				return Json(new { result = true, totalPages = 0, page = "<tr><td colspan=\"9\">" + Translation.GetTerm("ThereWereNoRecordsFoundThatMeetThatCriteriaPleaseTryAgain", "There were no records found that meet that criteria. Please try again.") + "</td></tr>" });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}
	}
}