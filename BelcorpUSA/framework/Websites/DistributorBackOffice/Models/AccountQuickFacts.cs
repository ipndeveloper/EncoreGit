using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities;
using NetSteps.Commissions.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Commissions.Common.Models;
using NetSteps.Common.Globalization;
using NetSteps.Diagnostics.Utilities;

namespace DistributorBackOffice.Models
{
	public class AccountQuickFacts
	{
		#region Properties

		public string AccountNumber { get; set; }

		public string CareerTitle { get; set; }

		public string PaidAsTitle { get; set; }

		public DateTime EnrollmentDate { get; set; }

		public IEnumerable<int> Periods { get; set; }

		#endregion

		#region Constructors

		public AccountQuickFacts()
		{ }

		#endregion
	}

	public static class AccountQuickFactsExtensions
	{
		public static IEnumerable<ITitleKind> TitleKinds { get; private set; }
		public static IEnumerable<ITitle> Titles { get; private set; }

		static AccountQuickFactsExtensions()
		{
			var comSvc = Create.New<ICommissionsService>();
			var titleKinds = comSvc.GetTitleKinds();
			if (titleKinds != null && titleKinds.Any())
			{
				TitleKinds = titleKinds.ToArray();
			}
			var titles = comSvc.GetTitles();
			if (titles != null && titles.Any())
			{
				Titles = titles;
			}
		}

		public static AccountQuickFacts ForAccount(this AccountQuickFacts aqf, Account forAccount, int? periodId = null)
		{
			if (forAccount != null)
			{
				try
				{
					aqf.AccountNumber = forAccount.AccountNumber;
					aqf.EnrollmentDate = forAccount.EnrollmentDate.GetValueOrDefault();

					var comSvc = Create.New<ICommissionsService>();
					var acctPeriods = comSvc.GetPeriodsForAccount(forAccount.AccountID);
                  	if (acctPeriods.Any())
					{
						aqf.Periods = acctPeriods.OrderByDescending(p => p.PeriodId).Select(p => p.PeriodId).ToArray();
					}

					ITitleKind ct, pt;
                    if (Titles != null && TitleKinds != null
                        && (TitleKinds.FirstOrDefault(tk => tk.TitleKindCode == "CT")) != null
                        && (TitleKinds.FirstOrDefault(tk => tk.TitleKindCode == "PAT")) != null)
                    {
                        ct = TitleKinds.FirstOrDefault(tk => tk.TitleKindCode == "CT");
                        pt = TitleKinds.FirstOrDefault(tk => tk.TitleKindCode == "PAT");

						var period = periodId ?? aqf.Periods.First();
						var aTitles = comSvc.GetAccountTitles(forAccount.AccountID, period);
						if (aTitles.Any())
						{
							if (aTitles.Any(at => at.TitleKindId == ct.TitleKindId))
							{
								ITitle cTitle = Titles.First(t => t.TitleId == aTitles.First(at => at.TitleKindId == ct.TitleKindId).TitleId);
								aqf.CareerTitle = Translation.GetTerm(cTitle.TermName);
							}
							if (aTitles.Any(at => at.TitleKindId == pt.TitleKindId))
							{
								ITitle paTitle = Titles.First(t => t.TitleId == aTitles.First(at => at.TitleKindId == pt.TitleKindId).TitleId);
								aqf.PaidAsTitle = Translation.GetTerm(paTitle.TermName);
							}
						}
					}
				}
				catch (Exception ex)
				{
					typeof(AccountQuickFactsExtensions).TraceException(ex);
				}
			}

			return aqf;
		}
	}
}
