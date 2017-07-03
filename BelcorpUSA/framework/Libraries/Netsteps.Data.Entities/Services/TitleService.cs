using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Commissions.Common;
using NetSteps.Data.Common.Entities;
using NetSteps.Data.Common.Services;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.Services
{
	public class TitleService : ITitleService
	{
		private ICommissionsService _commissionsService;
		private ICommissionsService CommissionsService
		{
			get
			{
				if (_commissionsService == null)
				{
					_commissionsService = Create.New<ICommissionsService>();
				}
				return _commissionsService;
			}
		}

		private ITitle ConvertCommissionsTitle(NetSteps.Commissions.Common.Models.ITitle commissionsTitle)
		{
			if (commissionsTitle == null)
			{
				return null;
			}

			var title = Create.New<ITitle>();
			title.Active = commissionsTitle.Active;
			title.SortOrder = commissionsTitle.SortOrder;
			title.TermName = commissionsTitle.TermName;
			title.TitleCode = commissionsTitle.TitleCode;
			title.TitleID = commissionsTitle.TitleId;
			return title;
		}

		private IAccountTitle ConvertCommissionsAccountTitle(NetSteps.Commissions.Common.Models.IAccountTitle commissionsAccountTitle)
		{
			if (commissionsAccountTitle == null)
			{
				return null;
			}

			var accountTitle = Create.New<IAccountTitle>();
			accountTitle.AccountID = commissionsAccountTitle.AccountId;
			accountTitle.PeriodID = commissionsAccountTitle.PeriodId;
			accountTitle.TitleTypeID = commissionsAccountTitle.TitleKindId;
			accountTitle.Title = ConvertCommissionsTitle(CommissionsService.GetTitle(commissionsAccountTitle.TitleId));
			return accountTitle;
		}

		public IEnumerable<ITitle> GetTitles()
		{
			return CommissionsService.GetTitles().Select(x =>
			{
				var title = Create.New<ITitle>();
				title.TitleID = x.TitleId;
				title.SortOrder = x.SortOrder;
				title.TermName = x.TermName ?? x.TitleCode;
				title.TitleCode = x.TitleCode;
				title.Active = x.Active;
				return title;
			});
		}

		public IAccountTitle GetAccountTitle(int accountID, int titleTypeID, int? periodID)
		{
			var commissionsAccountTitle = CommissionsService.GetAccountTitle(accountID, titleTypeID, periodID);
			var accountTitle = ConvertCommissionsAccountTitle(commissionsAccountTitle);
			if (accountTitle == null)
			{
				throw new TitleNotFoundException(accountID, titleTypeID, periodID ?? CommissionsService.GetCurrentPeriod().PeriodId);
			}

			return accountTitle;
		}

		public class TitleNotFoundException : Exception
		{
			public int AccountId { get; set; }
			public int? TitleTypeId { get; set; }
			public int PeriodId { get; set; }

			public TitleNotFoundException(int accountId, int titleTypeId, int periodId)
			{
				AccountId = accountId;
				TitleTypeId = titleTypeId;
				PeriodId = periodId;
			}

			public TitleNotFoundException(int accountId, int periodId)
			{
				AccountId = accountId;
				PeriodId = periodId;
			}
		}

		public IEnumerable<IAccountTitle> GetAccountTitles(int accountID, int? periodID)
		{
			var commissionsTitles = CommissionsService.GetAccountTitles(accountID, periodID);
			if (commissionsTitles == null || !commissionsTitles.Any())
			{
				return Enumerable.Empty<IAccountTitle>();
			}
			return commissionsTitles.Select(x => ConvertCommissionsAccountTitle(x));
		}
	}

	[ContainerRegister(typeof(IAccountTitle), RegistrationBehaviors.Default)]
	public class AccountTitle : IAccountTitle
	{
		public int AccountID { get; set; }
		public int PeriodID { get; set; }
		public ITitle Title { get; set; }
		public int TitleTypeID { get; set; }
	}

	[ContainerRegister(typeof(ITitle), RegistrationBehaviors.Default)]
	public class Title : ITitle
	{
		public int TitleID { get; set; }
		public int SortOrder { get; set; }
		public string TermName { get; set; }
		public string TitleCode { get; set; }
		public bool Active { get; set; }
	}
}
