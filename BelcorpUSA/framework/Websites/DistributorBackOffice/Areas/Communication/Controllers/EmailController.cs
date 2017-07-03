using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DistributorBackOffice.Areas.Communication.Helpers;
using DistributorBackOffice.Areas.Communication.Models.Email;
using DistributorBackOffice.Infrastructure;
using NetSteps.Common.Configuration;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Mail;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Controls.Models;
using NetSteps.Web.Mvc.Helpers;
using Newtonsoft.Json;
using MailConstants = NetSteps.Data.Entities.Mail.Constants;
using NetSteps.Diagnostics.Utilities;

namespace DistributorBackOffice.Areas.Communication.Controllers
{
	public class EmailController : BaseCommunicationController
	{
		public enum EmailAction
		{
			BlankEmail,
			Reply,
			ReplyToAll,
			Forward,
			EditExisting,
			TestEmail
		}

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Communications-Email", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult Index()
		{
			return RedirectToRoute(new { controller = "Email", action = "Mailbox", area = "Communication", folder = NetSteps.Data.Entities.Mail.Constants.MailFolderType.Inbox });
		}

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Communications-Email", "~/Email", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult Mailbox(NetSteps.Data.Entities.Mail.Constants.MailFolderType folder = NetSteps.Data.Entities.Mail.Constants.MailFolderType.Inbox)
		{
			using (var mailboxTrace = this.TraceActivity("loading Mailbox"))
			{
				try
				{
					MailAccount mailAccount = CurrentMailAccount;
					this.TraceInformation(string.Format("got MailAccount {0}", mailAccount.MailAccountID));

					var searchParameters = GetDefaultReportParameters();
					searchParameters.MailFolderTypeID = folder.ToShort();
					searchParameters.MailAccountID = mailAccount.MailAccountID;

					var messages = MailMessage.Search(searchParameters);
					this.TraceInformation(string.Format("got {0} messages", messages.Count));

					var model = new MailboxModel();
					model.MailFolderName = folder.ToString();
					model.MailAccountID = mailAccount == null ? (int?)null : mailAccount.MailAccountID;
					model.MessageCount = messages.TotalCount;

					this.TraceInformation(string.Format("outputing view for folder: {0}, accountID: {1}, count: {2}"
						, model.MailFolderName
						, model.MailAccountID
						, model.MessageCount));

					return View(model);
				}
				catch (Exception ex)
				{
					ex.TraceException(ex);
					var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
					throw exception;
				}
			}
		}

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Communications-Bundle Email", "~/Email", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult BundleEmail()
		{
			try
			{
				var archiveIDs = TempData["BundleEmail"] == null ? new List<int>(1) : (List<int>)TempData["BundleEmail"];

				MailMessage mailMessage = new MailMessage();

				mailMessage.HTMLBody = string.Empty;
				mailMessage.Subject = string.Empty;

				mailMessage.FromAddress = CurrentAccount.EmailAddress;
				if (mailMessage.MailMessageID == 0)
					mailMessage.SaveAsDraft(CurrentMailAccount);

				MailMessage message = MailMessage.Load(mailMessage.MailMessageID);

				var excludedFiles = new List<Archive>();

				foreach (int archiveID in archiveIDs)
				{
					var archive = CurrentSite.Archives.Where(a => a.ArchiveID == archiveID).FirstOrDefault();

					if (archive == null)
						continue;

					if (!archive.IsEmailable)
					{
						excludedFiles.Add(archive);
						continue;
					}

					message.HTMLBody += string.Format("<a href=\"{0}\">{1}</a><br/>", archive.ArchivePath.ReplaceFileUploadPathToken(), archive.GetArchiveName());
				}

				if (archiveIDs.Count > 0)
				{
					//Add padding so it's easier for the client to type their message.
					message.HTMLBody = "<p>&nbsp;</p><p>" + message.HTMLBody;
					message.HTMLBody += "</p>";
				}

				ViewBag.EmailTemplates = EmailTemplate.Search(new NetSteps.Data.Entities.Business.EmailTemplateSearchParameters()
				{
					PageIndex = 0,
					PageSize = null,
					EmailTemplateTypeIDs = new List<short>() { (short)NetSteps.Data.Entities.Constants.EmailTemplateType.Standard },
					Active = true
				});

				return View("Compose", new ComposeMailMessageModel(message, excludedFiles));
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				throw exception;
			}
		}

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Communications-Downline Email", "~/Email", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult DownlineEmail(bool downlineAllEmail = false)
		{
			try
			{
				ViewBag.DownlineAllEmail = downlineAllEmail.ToJavascriptBool();

				var dwsEmailHelper = Create.New<IDwsEmailHelper>();

				List<MailMessageRecipient> recipients = new List<MailMessageRecipient>();

				List<int> accountIDs = (List<int>)TempData.Peek("EmailDownline");

				if (accountIDs != null && accountIDs.Count > 0)
				{
					int currentAccountID = CurrentAccount.AccountID;

					recipients = !downlineAllEmail
									 ? dwsEmailHelper.AddMailMessageRecipients(currentAccountID, accountIDs).ToList()
									 : dwsEmailHelper.AddDownlineEmailRecipient(accountIDs);
				}

				if (recipients.Count > 0)
				{
					ViewData["DownlineGroup"] = new List<RecipientGroup>() { new RecipientGroup() { Name = Translation.GetTerm("Downline", "Downline"), Recipients = recipients } };
				}

				ViewBag.EmailTemplates = EmailTemplate.Search(new NetSteps.Data.Entities.Business.EmailTemplateSearchParameters()
				{
					PageIndex = 0,
					PageSize = null,
					EmailTemplateTypeIDs = new List<short>() { (short)NetSteps.Data.Entities.Constants.EmailTemplateType.Standard },
					Active = true
				});

				MailAccount mailAccount = CurrentMailAccount;
				MailMessage mailMessage = new MailMessage(mailAccount);
				return View("Compose", new ComposeMailMessageModel(mailMessage, new List<Archive>()));
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				throw exception;
			}
		}


		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Communications-Downline Email", "~/Email", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult ContactsEmail()
		{
			try
			{
				List<MailMessageRecipient> recipients = new List<MailMessageRecipient>();

				List<int> accountIDs = (List<int>)TempData.Peek("EmailDownline");

				if (accountIDs != null && accountIDs.Count > 0)
				{
					int currentAccountID = CurrentAccount.AccountID;

					var contactEmailInfos = NetSteps.Data.Entities.Account.LoadBatchSlim(accountIDs);
					foreach (var info in contactEmailInfos)
					{
						var recipient = new MailMessageRecipient
											{
												Name = string.Format("{0} {1}", info.FirstName, info.LastName).Trim(),
												AccountID = info.AccountID,
												Email = info.EmailAddress
											};
						recipients.Add(recipient);
					}
				}

				if (recipients.Count > 0)
				{
					ViewData["DownlineGroup"] = new List<RecipientGroup>() { new RecipientGroup() { Name = Translation.GetTerm("Downline", "Downline"), Recipients = recipients } };
				}
                //Newsletter asociate
				ViewBag.EmailTemplates = EmailTemplate.Search(new NetSteps.Data.Entities.Business.EmailTemplateSearchParameters()
				{
					PageIndex = 0,
					PageSize = null,
					EmailTemplateTypeIDs = new List<short>() { (short)NetSteps.Data.Entities.Constants.EmailTemplateType.Standard },
					Active = true
				});

				MailAccount mailAccount = CurrentMailAccount;
				MailMessage mailMessage = new MailMessage(mailAccount);
				return View("Compose", new ComposeMailMessageModel(mailMessage, new List<Archive>()));
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				throw exception;
			}
		}

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Communications-Email", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult View(int id)
		{
			try
			{
				var message = MailMessage.Load(id);

				//set whether email is internal or external
				foreach (var v in message.To)
				{
					v.Internal = IsInternal(v.AccountID, v.Email);
				}

				MailMessage.MarkAsReadBatch(new List<int> { id });

				return View(message);
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				throw exception;
			}
		}

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Communications-Email", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult UndeleteSelected(List<int> ids)
		{
			try
			{
				foreach (var id in ids)
					MailMessage.Move(id, NetSteps.Data.Entities.Mail.Constants.MailFolderType.Inbox.ToShort());

				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		static Dictionary<string, bool> _internalDomains;
		protected virtual Dictionary<string, bool> InternalDomains
		{
			get
			{
				if (_internalDomains == null)
				{
					//TODO: cache internal domains elsewhere
					_internalDomains = new Dictionary<string, bool>(StringComparer.InvariantCultureIgnoreCase);
					foreach (var domain in MailDomain.LoadInternalDomains())
					{
						if (!_internalDomains.ContainsKey(domain))
						{
							_internalDomains.Add(domain, true);
						}
					}
				}

				return _internalDomains;
			}
		}

		protected virtual bool IsInternal(int? accountID, string emailAddress)
		{
			emailAddress = (emailAddress ?? "").Trim();

			if (accountID > 0 && emailAddress.IsNullOrEmpty())
			{
				//we only have the accountID so it can only be internal
				return true;
			}

			// Check domains to see if it is an internal address
			int domainIndex = emailAddress.LastIndexOf('@') + 1;

			if (domainIndex > 0 && domainIndex < emailAddress.Length)
			{
				// internal if it has an @ symbol and exists in InternalDomains dictionary
				string domain = emailAddress.Substring(domainIndex);
				return InternalDomains.ContainsKey(domain);
			}
			return false;
		}

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Communications-Compose Email", "~/Email", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult Compose(EmailAction emailAction = EmailAction.TestEmail, int mailMessageID = 0, string toAddress = null, string subject = null, string htmlBody = null)
		{
			try
			{
				MailAccount mailAccount = CurrentMailAccount;
				MailMessage mailMessage;

				switch (emailAction)
				{
					case EmailAction.Reply:
						mailMessage = MailMessage.Load(mailMessageID).Reply(mailAccount);
						break;
					case EmailAction.ReplyToAll:
						mailMessage = MailMessage.Load(mailMessageID).ReplyToAll(mailAccount);
						break;
					case EmailAction.Forward:
						mailMessage = MailMessage.Load(mailMessageID).Forward(mailAccount);
						break;
					case EmailAction.TestEmail:
						mailMessage = FakeObjects.GetTestMailMessage();
						break;
					case EmailAction.EditExisting:
						mailMessage = MailMessage.Load(mailMessageID);
						break;
					default:
						mailMessage = new MailMessage(mailAccount);
						break;
				}

				if (!string.IsNullOrWhiteSpace(toAddress))
				{
					mailMessage.To.Add(new MailMessageRecipient(toAddress));
				}
				if (!subject.IsNullOrEmpty())
					mailMessage.Subject = subject;
				if (!htmlBody.IsNullOrEmpty())
					mailMessage.HTMLBody = htmlBody;

				//set whether email is internal or external
				foreach (var v in mailMessage.To)
				{
					v.Internal = IsInternal(v.AccountID, v.Email);
				}

				if (mailMessage.MailMessageTypeID == (short)MailConstants.EmailMessageType.Downline)
				{
					if (mailMessage.To.Count > 0)
					{
						ViewData["DownlineGroup"] = new List<RecipientGroup>() { new RecipientGroup() { Name = Translation.GetTerm("Downline", "Downline"), Recipients = mailMessage.To } };
						mailMessage.To = new ObservableCollection<MailMessageRecipient>();
					}
				}

                //@01 20150724 BR-COM-002 G&S LIB: Se cambia a obtener los datos de los Newsletters
				ViewBag.EmailTemplates = EmailTemplate.Search(new NetSteps.Data.Entities.Business.EmailTemplateSearchParameters()
				{
					PageIndex = 0,
					PageSize = null,
					EmailTemplateTypeIDs = new List<short>() { (short)NetSteps.Data.Entities.Constants.EmailTemplateType.Newsletter },
					Active = true
				});

				return View(new ComposeMailMessageModel(mailMessage, new List<Archive>()));
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				throw exception;
			}
		}

		[ValidateInput(false)]
		[AcceptVerbs(HttpVerbs.Post)]
		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Communications-Compose Email", "~/Email", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult Send(string toAddress, string subject, string htmlBody, bool saveDraft, int id = 0, bool isDownline = false, bool isDownlineEmailAll = false)
		{
			try
			{
				MailAccount mailAccount = CurrentMailAccount;

				MailMessage mailMessage;
				if (id != 0)
					mailMessage = MailMessage.Load(id);
				else
					mailMessage = new MailMessage();

				var sendTo = JsonConvert.DeserializeObject<ObservableCollection<MailMessageRecipient>>(toAddress);

				var mailRecipient = sendTo.FirstOrDefault();

				if (isDownlineEmailAll && mailRecipient != null && mailRecipient.AccountID == 0)
				{
					var dwsEmailHelper = Create.New<IDwsEmailHelper>();

					List<int> accountIDs = (List<int>)TempData["EmailDownline"];
					int currentAccountID = CurrentAccount.AccountID;

					mailMessage.To = dwsEmailHelper.AddMailMessageRecipients(currentAccountID, accountIDs);
				}
				else
				{
					mailMessage.To = sendTo;
				}

				mailMessage.Subject = subject;
				mailMessage.HTMLBody = htmlBody;
				mailMessage.MailMessageTypeID = (short)(isDownline ? MailConstants.EmailMessageType.Downline : MailConstants.EmailMessageType.AdHoc);

				// Set the "Reply-To" to the user's external email.              
				if (MailMessage.SetReplyToEmailAddress())
				{
					if (mailAccount.Account != null && !mailAccount.Account.EmailAddress.IsNullOrEmpty())
					{
						mailMessage.ReplyToAddress = mailAccount.Account.EmailAddress;
					}
				}

				// Handle Attachments
				foreach (string file in Request.Files)
				{
					HttpPostedFileBase httpPostedFileBase = Request.Files[file] as HttpPostedFileBase;
					if (httpPostedFileBase.ContentLength == 0)
						continue;

					// TODO: Save these to the Temp directory (a network share) that will be configured in the web.config later - JHE
					string savedFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "\\Uploads\\Temp", Path.GetFileName(httpPostedFileBase.FileName));
					httpPostedFileBase.SaveAs(savedFileName);

					mailMessage.Attachments.Add(new MailAttachment()
					{
						FileName = savedFileName,
						Size = httpPostedFileBase.ContentLength
					});
				}

				if (saveDraft)
					mailMessage.SaveAsDraft(mailAccount);
				else
					mailMessage.Send(mailAccount, CurrentSite != null ? CurrentSite.SiteID : 1);

				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				throw exception;
			}
		}

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Communications-Email", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult GetMail(int page, int pageSize, DateTime? startDate, DateTime? endDate, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection, NetSteps.Data.Entities.Mail.Constants.MailFolderType? folder = null, bool includeCheckbox = true, int? mailAccountID = null, int? length = 100)
		{
			try
			{
				if (startDate.HasValue && startDate.Value.Year < 1900)
					startDate = null;
				if (endDate.HasValue && endDate.Value.Year < 1900)
					endDate = null;

				//MailAccountID should not be requestable, this is a security hole.
				//if (mailAccountID == null)
				if (CurrentMailAccount == null)
					return Json(new { result = true, totalPages = 0, page = "<tr><td colspan=\"9\">" + Translation.GetTerm("ThereWereNoRecordsFoundThatMeetThatCriteriaPleaseTryAgain", "There were no records found that meet that criteria. Please try again.") + "</td></tr>" });

				var messages = MailMessage.Search(new NetSteps.Data.Entities.Business.MailMessageSearchParameters()
				{
					MailAccountID = CurrentMailAccount.MailAccountID,
					PageIndex = page,
					PageSize = pageSize,
					MailFolderTypeID = folder.ToShort(),
					StartDate = startDate.StartOfDay(),
					EndDate = endDate.EndOfDay(),
					OrderBy = orderBy,
					OrderByDirection = orderByDirection
				});

				if (messages.Count > 0)
				{
					StringBuilder builder = new StringBuilder();
					foreach (var message in messages)
					{
						string cssClass = message.BeenRead ? string.Empty : "emailRowUnread";
						string url = "~/Communication/Email/View/" + message.MailMessageID.ToString();
						if (folder == NetSteps.Data.Entities.Mail.Constants.MailFolderType.Drafts)
							url = "~/Communication/Email/Compose?emailAction=EditExisting&mailMessageID=" + message.MailMessageID.ToString();

						string abbreviatedBody = (!message.HTMLBody.IsNullOrEmpty()) ? message.HTMLBody : message.Body;
						abbreviatedBody = abbreviatedBody.StripTags().Truncate(length.Value, true);
						if (!abbreviatedBody.IsNullOrEmpty())
							abbreviatedBody = " / " + abbreviatedBody;

						string subjectLineText = string.Format("{0}<span class=\"Lawyer\">{1}</span>", (!message.Subject.IsNullOrEmpty()) ? message.Subject : "<i>(no subject)</i>", abbreviatedBody);
						builder.Append("<tr>");
						if (includeCheckbox)
							builder.AppendCheckBoxCell(message.MailMessageID.ToString(), cssClass, message.MailMessageID.ToString(), "mailMessages");
						builder.AppendLinkCell(url, subjectLineText, cssClass)
						.AppendCell(message.DateAdded.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo), cssClass)
						.Append("</tr>");
					}
					return Json(new { result = true, totalPages = Math.Ceiling(messages.TotalCount / (double)pageSize), page = builder.ToString() });
				}

				return Json(new { result = true, totalPages = 0, page = "<tr><td colspan=\"9\">" + Translation.GetTerm("ThereWereNoRecordsFoundThatMeetThatCriteriaPleaseTryAgain", "There were no records found that meet that criteria. Please try again.") + "</td></tr>" });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}



		//[OutputCache(CacheProfile = "PagedGridData")]
		//public virtual ActionResult GetForGrid(int page, int pageSize, DateTime? startDate, DateTime? endDate, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection, NetSteps.Data.Entities.Mail.Constants.MailFolderType? folder = null, bool includeCheckbox = true, int? mailAccountID = null, int? length = 100)
		//{
		//    try
		//    {
		//        if (startDate.HasValue && startDate.Value.Year < 1900)
		//            startDate = null;
		//        if (endDate.HasValue && endDate.Value.Year < 1900)
		//            endDate = null;

		//        if (mailAccountID == null)
		//            return Json(new { result = true, totalPages = 0, page = "<tr><td colspan=\"9\">" + Translation.GetTerm("ThereWereNoRecordsFoundThatMeetThatCriteriaPleaseTryAgain", "There were no records found that meet that criteria. Please try again.") + "</td></tr>" });

		//        var messages = MailMessage.Search(new NetSteps.Data.Entities.Business.MailMessageSearchParameters()
		//        {
		//            MailAccountID = mailAccountID,
		//            PageIndex = page,
		//            PageSize = pageSize,
		//            MailFolderTypeID = folder.ToShort(),
		//            StartDate = startDate.StartOfDay(),
		//            EndDate = endDate.EndOfDay(),
		//            OrderBy = orderBy,
		//            OrderByDirection = orderByDirection
		//        });

		//        if (messages.Count > 0)
		//        {
		//            StringBuilder builder = new StringBuilder();
		//            foreach (var message in messages)
		//            {
		//                builder.Append("<tr>")
		//                         .AppendCell(message.Subject + " / " + message.HTMLBody.Truncate(35, true))
		//                         .Append("</tr>");
		//            }
		//            return Json(new { totalPages = messages.TotalPages, page = builder.ToString() });
		//        }

		//        return Json(new { totalPages = 0, page = "<tr><td colspan=\"9\">" + Translation.GetTerm("ThereWereNoRecordsFoundThatMeetThatCriteriaPleaseTryAgain", "There were no records found that meet that criteria. Please try again.") + "</td></tr>" });
		//    }
		//    catch (Exception ex)
		//    {
		//        var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
		//        return Json(new { result = false, message = exception.PublicMessage });
		//    }
		//}



		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Communications-Email", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult GetOverview(int page, int pageSize, DateTime? startDate, DateTime? endDate, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection, NetSteps.Data.Entities.Mail.Constants.MailFolderType? folder = null, int? length = 100)
		{
			try
			{

				int mailAccountID = CurrentMailAccount.MailAccountID;

				if (startDate.HasValue && startDate.Value.Year < 1900)
					startDate = null;
				if (endDate.HasValue && endDate.Value.Year < 1900)
					endDate = null;

				var messages = MailMessage.Search(new NetSteps.Data.Entities.Business.MailMessageSearchParameters()
				{
					MailAccountID = mailAccountID,
					PageIndex = page,
					PageSize = pageSize,
					MailFolderTypeID = folder.ToShort(),
					StartDate = startDate.StartOfDay(),
					EndDate = endDate.EndOfDay(),
					OrderBy = orderBy,
					OrderByDirection = orderByDirection,
				});

				if (messages.Count > 0)
				{
					StringBuilder builder = new StringBuilder();

					builder.Append("<ul class=\"listNav\">");

					foreach (var message in messages)
					{
						string cssClass = message.BeenRead ? string.Empty : "class=\"emailRowUnread\"";

						builder.Append(string.Format("<li {0}>", cssClass));

						builder.Append(string.Format("<a href=\"/Communication/Email/View/{0}\" title=\"{1} {2}\">",
											 message.MailMessageID,
											 Translation.GetTerm("Click to read entire message from", "Click to read entire message from"),
											 message.From)
											 );

						builder.Append(string.Format("<span class=\"FR lawyer\">{0}</span>", message.DateAdded.ToString("MM/dd/yyyy h:mm tt")));
						builder.Append(string.Format("<span class=\"FL from\">{0}</span>", message.From));
						builder.Append(string.Format("<span class=\"clr message\">{0}</span>"
							, (string.IsNullOrEmpty(message.Subject) ? string.Format("<i>({0})</i>", Translation.GetTerm("NoSubject", "No Subject")) : message.Subject)));

						builder.Append("</a></li>");
					}

					builder.Append("</ul>");

					return Json(new { result = true, totalPages = Math.Ceiling(messages.TotalCount / (double)pageSize), data = builder.ToString() });
				}

				return Json(new { result = true, totalPages = 0, data = string.Format("<div class=\"NoData\">{0}</div>", Translation.GetTerm("There are no messages.")) });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Communications-Delete Email", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult Delete(int id)
		{
			try
			{
				MailMessage.Delete(id);

				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Communications-Delete Email", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult DeleteSelected(List<int> ids)
		{
			try
			{
				if (ids != null)
				{
					MailMessage.DeleteBatch(ids);
				}

				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Communications-Delete Email", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult EmptyDeletedFolder()
		{
			try
			{
				MailAccount mailAccount = CurrentMailAccount;
				MailMessage.PurgeFolder(NetSteps.Data.Entities.Mail.Constants.MailFolderType.Trash, mailAccount);

				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Communications-Email", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult Undelete(int id)
		{
			try
			{
				MailMessage.Move(id, NetSteps.Data.Entities.Mail.Constants.MailFolderType.Inbox.ToShort());

				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Communications-Email", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult MarkSelectedAsUnRead(List<int> ids)
		{
			try
			{
				MailMessage.MarkAsUnReadBatch(ids);

				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Communications-Email", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult MarkSelectedAsRead(List<int> ids)
		{
			try
			{
				MailMessage.MarkAsReadBatch(ids);

				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Communications-Email", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult Search(string query)
		{
			try
			{
				query = query.ToLower();

				var builder = new StringBuilder();
				string fileUploadWebPath = ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.FileUploadWebPath);
				var archives = Archives.Where(a => a.IsDownloadable && a.Translations.Any(t => (t.Name != null && t.Name.ToLower().Contains(query)) || (t.LongDescription != null && t.LongDescription.ToLower().Contains(query)) || (t.ShortDescription != null && t.ShortDescription.ToLower().Contains(query))));

				foreach (Archive archive in archives)
				{
					string icon, fileName = archive.ArchivePath.Substring(archive.ArchivePath.LastIndexOf('/') + 1);
					var archiveImage = archive.ArchiveImage.ReplaceFileUploadPathToken();
					var archivePath = archive.ArchivePath.ReplaceFileUploadPathToken();
					var type = fileName.GetFileType();
					switch (type)
					{
						case NetSteps.Common.Constants.FileType.PDF:
							icon = "pdf.png";
							break;
						case NetSteps.Common.Constants.FileType.Audio:
							icon = "audio.png";
							break;
						case NetSteps.Common.Constants.FileType.Flash:
							icon = "flash.png";
							break;
						case NetSteps.Common.Constants.FileType.Image:
							icon = "image.png";
							break;
						case NetSteps.Common.Constants.FileType.Video:
							icon = "wmv.png";
							break;
						case NetSteps.Common.Constants.FileType.Powerpoint:
							icon = "powerpoint.png";
							break;
						case NetSteps.Common.Constants.FileType.Excel:
							icon = "excel.png";
							break;
						case NetSteps.Common.Constants.FileType.Word:
							if (archive.ArchivePath.EndsWith(".txt"))
								icon = "text.png";
							else
								icon = "word.png";
							break;
						default:
							icon = "file.png";
							break;
					}

					builder.Append("<div class=\"Resource\"><div class=\"FL Rthumb\"><span class=\"Rwrapper\"><img src=\"");
					if (string.IsNullOrEmpty(archive.ArchiveImage))
					{
						builder.Append("~/Content/Images/DocumentTypes/file.png".ResolveUrl());
					}
					else
					{
						builder.Append(archiveImage);
					}
					builder.Append("\" /></span></div><div class=\"FL Rdesc\"><span class=\"FileType\"><img src=\"");


					builder.Append("~/Content/Images/DocumentTypes/Icons/".ResolveUrl()).Append(icon).Append("\" /></span><a href=\"")
						.Append(archivePath)
						.Append("\" target=\"_blank\" rel=\"external\" class=\"Rtitle\">").Append(archive.Translations.Name()).Append("</a></div><span class=\"ClearAll\"></span></div>");
				}

				return Json(archives);
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Communications-Email", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult SearchArchives(string query)
		{
			try
			{
				query = query.ToLower();

				Dictionary<int, string> groups = new Dictionary<int, string>();

				return Json(Archives.Where(a => a.IsEmailable && a.Translations.Any(t => (t.Name != null && t.Name.ToLower().Contains(query)) || (t.LongDescription != null && t.LongDescription.ToLower().Contains(query)) || (t.ShortDescription != null && t.ShortDescription.ToLower().Contains(query)))).Select(a => new
				{
					id = a.ArchiveID,
					text = Server.HtmlEncode(a.Translations.Name())
				}));
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Communications-Email", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult SearchRecipients(string query)
		{
			try
			{
				query = query.ToLower();
				int currentAccountID = CurrentAccount.AccountID;
				IEnumerable<dynamic> matchingItems;

				bool allowExternal = ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.AllowExternalEmail, false);
				if (allowExternal)
				{
					Dictionary<int, string> groups = new Dictionary<int, string>();
					foreach (DistributionList group in DistributionListCacheHelper.GetDistributionListByAccountID(currentAccountID).Where(g => g.Active && g.Name.ToLower().Contains(query)))
					{
						groups.Add(
							-group.DistributionListID,
							string.Format("{0} ({1})", group.Name, Translation.GetTerm("Group"))
						);
					}

					Dictionary<int, string> contacts = new Dictionary<int, string>();
					contacts = NetSteps.Data.Entities.Account.SlimSearchEmail(query, sponsorID: currentAccountID);

					matchingItems = groups
						.Select(p => new
						{
							id = p.Key,
							text = p.Value
						})
						.Union(contacts.Select(p => new
						{
							id = p.Key,
							text = Server.HtmlEncode(p.Value)
						}))
						.Distinct();
				}
				else
				{
					matchingItems = DownlineCache.SearchDownline(currentAccountID, query).Select(a => new
					{
						id = (int)a.AccountID,
						text = string.Format("{0} {1}", a.FirstName, a.LastName)
					});
				}

				return Json(matchingItems);
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		#region New Mail Message Recipients
		protected virtual MailMessageRecipient NewMailMessageRecipient(string firstName, string lastName, string email)
		{
			return new MailMessageRecipient()
			{
				Name = string.Format("{0} {1}", firstName, lastName).Trim(),
				MailMessageRecipientType = MailConstants.MailMessageRecipientType.Individual,
				Internal = IsInternal(null, email),
				Email = email,
			};
		}

		protected virtual MailMessageRecipient NewMailMessageRecipient(string firstName, string lastName, int accountID)
		{
			return new MailMessageRecipient()
			{
				Name = string.Format("{0} {1}", firstName, lastName).Trim(),
				MailMessageRecipientType = MailConstants.MailMessageRecipientType.Individual,
				Internal = true,
				AccountID = accountID,
			};
		}

		protected virtual MailMessageRecipient NewMailMessageRecipient(string firstName, string lastName, int accountID, string email)
		{
			return new MailMessageRecipient()
			{
				Name = string.Format("{0} {1}", firstName, lastName).Trim(),
				MailMessageRecipientType = MailConstants.MailMessageRecipientType.Individual,
				Internal = IsInternal(accountID, email),
				AccountID = accountID,
				Email = email,
			};
		}
		#endregion

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Communications-Email", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult VerifyContact(string text)
		{
			try
			{
				bool allowExternal = ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.AllowExternalEmail, false);

				List<MailMessageRecipient> recipients = null;
				RecipientGroup recipientGroup = null;

				int id;
				if (int.TryParse(text, out id))
				{
					if (id > 0)
					{
						//AccountID
						var result = NetSteps.Data.Entities.Account.LoadSlim(id);
						if (!string.IsNullOrEmpty(result.EmailAddress))
						{
							recipients = new List<MailMessageRecipient>();

							if (allowExternal)
							{
								recipients.Add(NewMailMessageRecipient(result.FirstName, result.LastName, id, result.EmailAddress));
							}
							else
							{
								recipients.Add(NewMailMessageRecipient(result.FirstName, result.LastName, id));
							}
						}
					}
					else if (id < 0)
					{
						//DistributionList
						if (allowExternal)
						{
							id = Math.Abs(id);

							var group = DistributionListCacheHelper.GetDistributionListByAccountID(CurrentAccount.AccountID).FirstOrDefault(g => g.DistributionListID == id);
							recipientGroup = new RecipientGroup()
							{
								Name = group.Name,
							};

							List<MailMessageRecipient> groupRecipients = new List<MailMessageRecipient>();
							recipientGroup.Recipients = groupRecipients;

							var accounts = NetSteps.Data.Entities.Account.LoadBatchSlim(group.DistributionSubscribers.Where(c => c.Active && c.AccountID.HasValue).Select(c => c.AccountID.ToInt()));
							foreach (var account in accounts)
							{
								groupRecipients.Add(NewMailMessageRecipient(account.FirstName, account.LastName, account.AccountID, account.EmailAddress));
								//groupRecipients.Add(new MailMessageRecipient()
								//{
								//    Name = string.Format("{0} {1}", account.FirstName, account.LastName),
								//    Email = account.EmailAddress,
								//    MailMessageRecipientType = NetSteps.Data.Entities.Mail.Constants.MailMessageRecipientType.Individual,
								//    Internal = IsInternal(account.EmailAddress),
								//});
							}
						}
					}
					else
					{
						//invalid
					}
				}
				else
				{
					//Email address
					if (allowExternal)
					{
						//verify it's a valid email
						if (text.IsValidEmail())
						{
							recipients = new List<MailMessageRecipient>()
							{
								NewMailMessageRecipient(null, null, text)
								//new MailMessageRecipient()
								//{
								//    //Name=//Need a way of looking up
								//    Email = text,
								//    MailMessageRecipientType = NetSteps.Data.Entities.Mail.Constants.MailMessageRecipientType.Individual,
								//    Internal = false,//??
								//}
							};
						}
					}
				}

				return Json(new { result = recipients != null || recipientGroup != null, recipients = recipients, group = recipientGroup, });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		public virtual ActionResult GetRecipients(int mailMessageId)
		{
			try
			{
				var mailMessage = MailMessage.Load(mailMessageId);

				return Json(new { result = true, recipients = mailMessage.To, groups = mailMessage.MailMessageGroups });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);

				return Json(new { result = false, message = ex.Message });
			}
		}

		protected virtual NetSteps.Data.Entities.Business.MailMessageSearchParameters GetDefaultReportParameters()
		{
			return new NetSteps.Data.Entities.Business.MailMessageSearchParameters()
			{
				OrderBy = "DateAddedUTC",
				OrderByDirection = NetSteps.Common.Constants.SortDirection.Descending
			};
		}

		public static readonly string MailAttachmentFolder = ConfigurationManager.GetAbsoluteFolder("Attachments");

		#region Add Attachments
		protected virtual void AddAttachmentToMessage(MailMessage message, Stream stream, string fileName)
		{
			if (fileName.Contains("\\"))
			{
				fileName = fileName.Substring(fileName.LastIndexOf("\\") + 1);
			}

			string finalFileName = Path.Combine(MailAttachmentFolder, message.AttachmentUniqueID + "_" + fileName);

			if (System.IO.File.Exists(finalFileName))
			{
				System.IO.File.Delete(finalFileName);
			}

			using (BinaryReader reader = new BinaryReader(stream))
			{
				byte[] attachmentData = reader.ReadBytes((int)reader.BaseStream.Length);

				using (BinaryWriter writer = new BinaryWriter(System.IO.File.Create(finalFileName)))
				{
					writer.Write(attachmentData);
				}

				//System.IO.File.Move(finalFileName, finalFileName);
			}

			message.Attachments.RemoveAll(x => x.FileName.Trim().ToLower() == fileName.Trim().ToLower());

			MailAttachment attachment = new MailAttachment
			{
				FileName = fileName,
				Size = (int)stream.Length
			};

			message.Attachments.Add(attachment);
		}

		protected virtual void AddAttachmentToMessage(MailMessage message, FileInfo file)
		{
			string tempFileName = Path.Combine(MailAttachment.UploadTempFolder, message.AttachmentUniqueID + "_" + file.Name);
			string finalFileName = Path.Combine(MailAttachment.UploadFinalFolder, message.AttachmentUniqueID + "_" + file.Name);
			if (System.IO.File.Exists(tempFileName))
				System.IO.File.Delete(tempFileName);
			if (System.IO.File.Exists(finalFileName))
				System.IO.File.Delete(finalFileName);

			file.CopyTo(tempFileName);
			System.IO.File.Move(tempFileName, finalFileName);

			message.Attachments.RemoveAll(x => x.FileName.Trim().ToLower() == file.Name.Trim().ToLower());

			MailAttachment attachment = new MailAttachment
			{
				FileName = file.Name,
				Size = (int)file.Length
			};

			message.Attachments.Add(attachment);
		}
		#endregion

		[HttpPost, ValidateInput(false)]
		[FunctionFilter("Communications-Email", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult GetEmailTemplate(int emailTemplateTranslationID)
		{
			try
			{
				var emailTemplate = EmailTemplateTranslation.LoadFull(emailTemplateTranslationID);
				return Json(new { result = true, subject = emailTemplate.Subject, template = emailTemplate.Body, thumbnail = emailTemplate.AttachmentPath });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[HttpPost, ValidateInput(false)]
		[FunctionFilter("Communications-Email Attachments", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult AddAttachmentToMessage(int mailMessageId = 0, string to = null, string subject = "", string message = "", int archiveId = -1)
		{
			try
			{
				MailMessage mailMessage = mailMessageId == 0 ? new MailMessage() : MailMessage.Load(mailMessageId);
				if ((mailMessage.AttachmentUniqueID ?? "").Replace("0", "").Replace("-", "").Trim() == "") // EMPTY GUID
					mailMessage.AttachmentUniqueID = Guid.NewGuid().ToString();

				//We are no longer saving the subject or the message.  We were getting this error from the UI.  'The length of the query string for this request exceeds the configured maxQueryStringLength value'
				//mailMessage.HTMLBody = message;
				//mailMessage.Subject = subject;

				mailMessage.FromAddress = CurrentAccount.EmailAddress;
				if (mailMessage.MailMessageID == 0)
					mailMessage.SaveAsDraft(CurrentMailAccount);

				string cleanedFileName = string.Empty;

				bool nonHtml5Browser = Request.Files.Count > 0;
				var cleanFileNameRegex = new System.Text.RegularExpressions.Regex(@"[^a-zA-Z0-9\s_\.]");

				if (nonHtml5Browser)
				{
					foreach (string fileTag in Request.Files)
					{
						HttpPostedFileBase file = Request.Files[fileTag];
						if (file != null && file.ContentLength > 0)
						{
							cleanedFileName = file.FileName;
							if (file.FileName.Contains(':'))
							{
								cleanedFileName = Path.GetFileName(cleanedFileName);
							}
							cleanedFileName = cleanFileNameRegex.Replace(cleanedFileName, "");
							string fullPath = ConfigurationManager.GetAbsoluteFolder("Attachments") + Path.GetFileName(cleanedFileName);
							AddAttachmentToMessage(mailMessage, file.InputStream, fullPath);
						}
					}
				}
				else if (Request.ContentType.Equals("application/octet-stream", StringComparison.InvariantCultureIgnoreCase))
				{
					string fileName = Request.Params["qqfile"];

					cleanedFileName = cleanFileNameRegex.Replace(fileName, "");
					string fullPath = ConfigurationManager.GetAbsoluteFolder("Attachments") + Path.GetFileName(cleanedFileName);

					AddAttachmentToMessage(mailMessage, Request.InputStream, fullPath);
				}
				else if (archiveId > 0)
				{
					Archive archive = CurrentSite.Archives.FirstOrDefault(a => a.ArchiveID == archiveId);
					if (archive != null)
					{
						string filePath = archive.ArchivePath.ReplaceFileUploadPathToken().WebUploadPathToAbsoluteUploadPath();

						FileInfo attachment = new System.IO.FileInfo(filePath);
						if (!attachment.Exists)
						{
							var attachmentDoesntExist = new { success = false, error = Translation.GetTerm("CouldntFindFile", "Couldn't find the file specified.") };
							if (nonHtml5Browser)
								return Content(attachmentDoesntExist.ToJSON(), "text/html");
							else
								return Json(attachmentDoesntExist);
						}

						AddAttachmentToMessage(mailMessage, attachment);
					}
				}
				else
				{
					return Json(new { success = false, error = "No files uploaded." });
				}

				mailMessage.SaveAsDraft(CurrentMailAccount);

				var json = new { success = true, attachments = mailMessage.Attachments.ToList().ConvertAll(x => x.FileName), mailMessageId = mailMessage.MailMessageID };
				if (nonHtml5Browser)
					return Content(json.ToJSON(), "text/html");
				else
					return Json(json);
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { success = false, error = exception.PublicMessage });
			}
		}

		[HttpPost]
		[FunctionFilter("Communications-Email Attachments", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult RemoveAttachmentFromMessage(string fileName, int mailMessageId = -1)
		{
			if (mailMessageId == -1)
				return Json(false);

			MailMessage message = MailMessage.Load(mailMessageId);
			message.Attachments.RemoveAll(x => x.FileName.Trim().ToLower() == fileName.Trim().ToLower());

			message.SaveAsDraft(CurrentMailAccount);
			return Json(new { attachments = message.Attachments.ToList().ConvertAll(x => x.FileName), mailMessageId = message.MailMessageID });
		}

		[HttpPost]
		[ValidateInput(false)]
		public virtual ActionResult GeneratePreview(EmailTemplateContentModel model)
		{
			try
			{
				Session["EmailTemplateContentModel"] = model;
				return Json(new { success = true, result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return JsonError(exception.PublicMessage);
			}
		}

		[HttpGet]
		public virtual ActionResult Preview()
		{
			var model = Session["EmailTemplateContentModel"] as EmailTemplateContentModel;
			if (model == null)
			{
				return Content("<html><body>No preview data</body></html>", "text/html");
			}

			EmailTemplateTranslation ett = new EmailTemplateTranslation
			{
				Subject = model.Subject,
				Body = model.Body
			};
			var mailMessage = EmailTemplateContentModel.GetPreviewMailMessage(model, ett);

			mailMessage.AppendOptOutFooter("optouttest@email.com", NetSteps.Data.Entities.Constants.Language.English.ToShort());

			return Content(mailMessage.HTMLBody ?? mailMessage.Body, "text/html");
		}
		#region 6-29-2011: OptOut Email - Moved to nsDistributor/StaticController - Tenzin

		//[HttpGet]
		//public virtual ActionResult Optout(string emailAddress)
		//{
		//    try
		//    {
		//        return View();
		//    }
		//    catch (Exception ex)
		//    {
		//        var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
		//        return Json(new { success = false, error = exception.PublicMessage });
		//    }
		//}

		//[HttpPost]
		//public virtual ActionResult Optout(string emailAddress, FormCollection values)
		//{
		//    try
		//    {
		//        OptOut optOut = OptOut.Search(emailAddress);

		//        // Only add new optout emails
		//        if (optOut == null)
		//        {
		//            OptOut optOutEmailAddress = new OptOut();
		//            optOutEmailAddress.EmailAddress = emailAddress;
		//            optOutEmailAddress.OptOutTypeID = NetSteps.Data.Entities.Constants.OptOutType.EndUser.ToShort();
		//            optOutEmailAddress.Save();
		//        }

		//        return Json(new { success = true, message = Translation.GetTerm("EmailAddressSuccessfullyOptedOUt", "Email address successfully opted out!") });
		//    }
		//    catch (Exception ex)
		//    {
		//        var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
		//        return Json(new { success = false, error = exception.PublicMessage });
		//    }
		//}

		#endregion
	}
}
