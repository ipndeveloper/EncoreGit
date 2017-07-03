using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetSteps.Commissions.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Data.Common.Entities;
using NetSteps.Web.Mvc.Helpers;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.EntityModels;

namespace nsCore.Areas.Accounts.Models.Shared
{
    public class YellowWidgetModel
    {
        ICommissionsService _commissionsService = Create.New<ICommissionsService>();
        NetSteps.Data.Entities.Services.TitleService _titleService = new NetSteps.Data.Entities.Services.TitleService();
		Downline downLine = null; // CGI(AHAA) - 2597
        string _careerAsTitle = string.Empty;
        string _paidAsTitle = string.Empty;											 

        public YellowWidgetModel()
        {
            this.CurrentAccount = CoreContext.CurrentAccount;
            this.AccountStatusesExclude = new String[] { "" };
			
			int accountID = 0;
			if (this.CurrentAccount != null)
			{
				TranslationCache term = new TranslationCache();
				accountID = this.CurrentAccount.AccountID;
				IEnumerable<TitleInformation> lista;
				lista = Title.ListTitlesByAccount(accountID);
				if (lista == null) return;
				if (lista.Count() == 0) return;
				_careerAsTitle = term.GetTerm(lista.ElementAt(0).CareerAsTitle);
				_paidAsTitle = term.GetTerm(lista.ElementAt(0).PaidAsTitle);
			}

			this.downLine = new Downline();
			this.downLine.LastAccessed = DateTime.Now;
		}
			
		public YellowWidgetModel(String[] AccountStatusesExclude)
		{
			this.CurrentAccount = CoreContext.CurrentAccount;
			this.AccountStatusesExclude = AccountStatusesExclude;
		}
		public String[] AccountStatusesExclude { get; set; }

		int? PeriodId
		{
			get
			{
				var currentPeriod = this._commissionsService.GetCurrentPeriod();
				int? periodId = currentPeriod != null ? (int?)currentPeriod.PeriodId : null;
				return periodId;
			}
		}

		public Account CurrentAccount { get; private set; }
		public bool UsesCommissions
		{
			get
			{
				bool usesComissions = ApplicationContext.Instance.UsesEncoreCommissions;
				return usesComissions;
			}
		}

		public IAccountTitle AccountPaidAsTitle
		{
			get
			{
				var accountPaidAsTitle = _titleService.GetAccountTitle(this.CurrentAccount.AccountID, (int)NetSteps.Data.Entities.Constants.TitleType.PaidAS, this.PeriodId);
				return accountPaidAsTitle;
			}
		}

		public IAccountTitle AccountRecognitionTitle
		{
			get
			{
				var accountRecognitionTitle = _titleService.GetAccountTitle(this.CurrentAccount.AccountID, (int)NetSteps.Data.Entities.Constants.TitleType.Recognition, this.PeriodId);
				return accountRecognitionTitle;
			}
		}
		public string PaidAsTitle
		{
			get { return _paidAsTitle; }
		}

		public string CareerTitle
		{
			get { return _careerAsTitle; }
		}						 

		
	}
}