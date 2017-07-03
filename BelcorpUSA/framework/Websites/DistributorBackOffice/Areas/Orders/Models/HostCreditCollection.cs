using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using NetSteps.Data.Entities;

namespace DistributorBackOffice.Areas.Orders.Models
{
	public class HostCreditItem
	{
		public List<HostessRewardRule> Rules { get; set; }
		public Dictionary<string, int> RemainingDictionary { get; set; }
		public int? RemainingNumber { get; set; }
		public PrefixHelper CartPrefix { get; set; }
		public SuffixHelper CartSuffix { get; set; }
		public PrefixHelper HeaderPrefix { get; set; }
		public string Title { get; set; }
		public string HiddenClass { get; set; }
		public int HostessRuleId { get; set; }
	}

	public class HostCreditCollection
	{
		public HostCreditItem ProductDiscounts { get; set; }
		public HostCreditItem ItemDiscounts { get; set; }
		public HostCreditItem HostCredit { get; set; }
		public HostCreditItem ExclusiveProducts { get; set; }
		public HostCreditItem BookingCredit { get; set; }
	}

	public class PrefixHelper
	{
		private readonly string _htmlPrefix;

		public PrefixHelper(string htmlPrefix)
		{
			_htmlPrefix = htmlPrefix;
		}

		public IHtmlString GetId(string label)
		{
			return new MvcHtmlString(_htmlPrefix + label);
		}
	}

	public class SuffixHelper
	{
		private readonly string _htmlSuffix;

		public SuffixHelper(string htmlSuffix)
		{
			_htmlSuffix = htmlSuffix;
		}

		public IHtmlString GetId(string label)
		{
			return new MvcHtmlString(label + _htmlSuffix);
		}
	}
}