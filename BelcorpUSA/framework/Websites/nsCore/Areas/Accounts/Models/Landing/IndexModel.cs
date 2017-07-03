using System.Collections.Generic;
using System.Web.Mvc;
using NetSteps.Encore.Core.IoC;
using NetSteps.Commissions.Common;
using System.Linq;
using NetSteps.Commissions.Common.Models;
using System; 
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Business;

namespace nsCore.Areas.Accounts.Models.Landing
{
	public class IndexModel
	{
		public int? StateProvinceID { get; set; }
		public IEnumerable<SelectListItem> States { get; set; }

		public Dictionary<string, string> Titles
		{
			get { return Title.ListTitlesCombo().ToDictionary(sp => sp.TitleId.ToString(), sp => this.GetTerm(sp.TermName, sp.Name)); }
		}

        string GetTerm(string TermName, string TitleName)
		{
            return CachedData.Translation.GetTerm(TermName, TitleName);
		}
	}
}