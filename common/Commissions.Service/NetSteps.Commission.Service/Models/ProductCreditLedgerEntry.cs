﻿using NetSteps.Commissions.Common.Models;
using NetSteps.Encore.Core.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Commissions.Service.Models
{
    [ContainerRegister(typeof(IProductCreditLedgerEntry), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest), Serializable]
	public class ProductCreditLedgerEntry : IProductCreditLedgerEntry
	{

		public int AccountId { get; set; }

		public int? BonusTypeId { get; set; }

		public int? BonusValueId { get; set; }

		public int CurrencyTypeId { get; set; }

		public DateTime EffectiveDate { get; set; }

		public decimal? EndingBalance { get; set; }

		public decimal EntryAmount { get; set; }

		public DateTime EntryDate { get; set; }

		public string EntryDescription { get; set; }

		public int EntryId { get; set; }

		public ILedgerEntryKind EntryKind { get; set; }

		public string EntryNotes { get; set; }

		public ILedgerEntryOrigin EntryOrigin { get; set; }

		public ILedgerEntryReason EntryReason { get; set; }

		public int? OrderId { get; set; }

		public int? OrderPaymentId { get; set; }

		public int UserId { get; set; }
	}
}