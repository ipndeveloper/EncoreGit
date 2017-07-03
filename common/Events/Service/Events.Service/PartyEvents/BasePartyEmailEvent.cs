using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using NetSteps.Common.Interfaces;
using NetSteps.Encore.Core.IoC;
using NetSteps.Events.Common.Repositories;
using NetSteps.Orders.Common.Models;

namespace NetSteps.Events.Service.PartyEvents
{
	public abstract class BasePartyEmailEvent : BasePartyEvent
	{
		private IPartyRepository _partyRepository;
		protected IPartyRepository PartyRepository
		{
			get
			{
				if (_partyRepository == null)
				{
					_partyRepository = Create.New<IPartyRepository>();
				}
				return _partyRepository;
			}
		}

		private IEmailRepository _emailRepository;
		protected IEmailRepository EmailRepository
		{
			get
			{
				if (_emailRepository == null)
				{
					_emailRepository = Create.New<IEmailRepository>();
				}
				return _emailRepository;
			}
		}

		public override bool Execute(int eventID)
		{
			Contract.Requires<ArgumentException>(eventID > 0);

			int partyID = ArgumentRepository.GetPartyIDByEventID(eventID);
			if (partyID < 1)
			{
				throw new Exception(string.Format("There is no party associated with eventID: {0}", eventID));
			}

			int eventTypeID = EventRepository.GetEventTypeID(eventID);
			if (eventTypeID < 1)
			{
				throw new Exception(string.Format("The associated event has no eventTypeID"));
			}

			try
			{
				SendEmail(partyID, eventTypeID);
				return true;
			}
			catch (Exception ex)
			{
				EmailRepository.LogException(ex);
			}
			return false;
		}

		protected virtual IEnumerable<string> GetAdditionalRecipientEmailAddresses()
		{
			return new string[0];
		}

		protected virtual IEnumerable<string> GetCcEmailAddresses()
		{
			return new string[0];
		}

		protected abstract ITokenValueProvider GetTokenValueProvider(IParty party);
		protected virtual void SendEmail(int partyID, int eventTypeId)
		{
			IParty party = PartyRepository.GetPartyByPartyID(partyID);
			if (party == null)
			{
				throw new Exception(string.Format("No party for partyID: {0}", partyID));
			}

			ITokenValueProvider tokenValueProvider = GetTokenValueProvider(party);
			if (tokenValueProvider == null)
			{
				throw new Exception(string.Format("No token value provider"));
			}

			int distributorID = PartyRepository.GetDistributorIDFromParty(party);

			EmailRepository.SendEmail(distributorID, eventTypeId, tokenValueProvider, GetAdditionalRecipientEmailAddresses().Concat(GetCcEmailAddresses()));
		}
	}
}
