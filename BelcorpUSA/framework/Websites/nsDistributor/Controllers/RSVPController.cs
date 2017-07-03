using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NetSteps.Common.TokenReplacement;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Mail;
using NetSteps.Data.Entities.TokenValueProviders;
using NetSteps.Encore.Core.IoC;
using Constants = NetSteps.Data.Entities.Constants;

namespace nsDistributor.Controllers
{
    public class RSVPController : BaseController
    {
        public virtual ActionResult Yes(int partyGuestId)
        {
            PartyGuest guest = PartyGuest.Load(partyGuestId);
            PartyRsvp rsvp = new PartyRsvp();
            rsvp.PartyGuestID = partyGuestId;
            rsvp.ResponseDate = DateTime.Now;
            rsvp.IsComing = true;
            rsvp.PartyID = guest.PartyID;
            rsvp.Save();

            var party = Party.LoadWithGuests(guest.PartyID);
            ViewBag.Party = party;

            SendConfirmation(guest, party, Constants.EmailTemplateType.EvitesYesConfirmation);

            return View(CurrentSite.GetPageByUrl("/RSVP/Yes"));
        }

        public virtual ActionResult No(int partyGuestId)
        {
            PartyGuest guest = PartyGuest.Load(partyGuestId);
            PartyRsvp rsvp = new PartyRsvp();
            rsvp.PartyGuestID = partyGuestId;
            rsvp.ResponseDate = DateTime.Now;
            rsvp.IsComing = false;
            rsvp.PartyID = guest.PartyID;
            rsvp.Save();

            var party = Party.LoadWithGuests(guest.PartyID);
            ViewBag.Party = party;

            SendConfirmation(guest, party, Constants.EmailTemplateType.EvitesNoConfirmation);

            return View(CurrentSite.GetPageByUrl("/RSVP/No"));
        }

        protected static MailAccount _corporateMailAccount = null;
        private static object _lock = new object();

        protected virtual void SendConfirmation(PartyGuest guest, Party party, Constants.EmailTemplateType emailTemplateType)
        {
            var emailTemplate = EmailTemplate.Search(new NetSteps.Data.Entities.Business.EmailTemplateSearchParameters()
            {
                Active = true,
                PageIndex = 0,
                PageSize = 1,
                EmailTemplateTypeIDs = new List<short>() { (short)emailTemplateType }
            }).FirstOrDefault();
            var translation = emailTemplate.EmailTemplateTranslations.GetByLanguageID(ApplicationContext.Instance.CurrentLanguageID);
        	if (translation == null)
        	{
        		return;
        	}

			
        	var partyTokenValueProvider = Create.NewWithParams<PartyTokenValueProvider>(LifespanTracking.External, Param.Value(party));
        	var message = translation.GetTokenReplacedMailMessage(new CompositeTokenValueProvider(partyTokenValueProvider, new PartyGuestTokenValueProvider(guest)));
        	if (_corporateMailAccount == null)
        	{
        		lock (_lock)
        		{
        			if (_corporateMailAccount == null)
        				_corporateMailAccount = MailAccount.GetCorporateMailAccount();
        		}
        	}
        	message.To.Add(new NetSteps.Data.Entities.Mail.MailMessageRecipient(guest.EmailAddress));
        	var host = party.Order.GetHostess();
        	if (MailMessage.SetReplyToEmailAddress())
        	{
        		message.ReplyToAddress = host.AccountInfo.EmailAddress;
        	}
        	message.Send(_corporateMailAccount, CurrentSite.SiteID);
        }
    }
}
