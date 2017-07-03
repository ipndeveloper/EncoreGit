using System.Linq;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;

namespace NetSteps.Web.Mvc.Attributes
{
	public class AnyFunctionFilterAttribute : FunctionFilterAttribute
	{
		protected string[] Functions
		{
			get;
			set;
		}

		public AnyFunctionFilterAttribute(string redirectUrl, Constants.SiteType siteTypeID = Data.Entities.Generated.ConstantsGenerated.SiteType.NotSet, params string[] functions)
			: base(redirectUrl, siteTypeID)
		{
			Functions = functions;
		}

		protected override bool IsMatch()
		{
			bool result = false;
			bool checkWorkstationUserRole = false;
			SiteType backOfficeSiteType = SmallCollectionCache.Instance.SiteTypes.FirstOrDefault(t => t.Name == "BackOffice");
			if (SiteTypeID > 0 && backOfficeSiteType != null && SiteTypeID == (Constants.SiteType)backOfficeSiteType.SiteTypeID) checkWorkstationUserRole = true;
			foreach (string function in Functions)
			{
				result = result || (User == null && Anonymous.HasFunction(function)) || (User != null && User.HasFunction(function, true, checkWorkstationUserRole));
				if (result) break;
			}
			return result;
		}
	}
}
