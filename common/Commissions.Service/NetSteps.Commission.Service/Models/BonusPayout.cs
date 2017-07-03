using NetSteps.Commissions.Common.Models;
using NetSteps.Encore.Core.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.Models
{
    [ContainerRegister(typeof(IBonusPayout), RegistrationBehaviors.Default), Serializable]
	public class BonusPayout : IBonusPayout
	{
		public int AccountID { get; set; }

		public int SortOrder { get; set; }

		public decimal BonusAmount { get; set; }

		public string BonusCategory { get; set; }

		public string BonusName { get; set; }

		public string BonusSubcategory { get; set; }

		public int PeriodID { get; set; }
	}
}
