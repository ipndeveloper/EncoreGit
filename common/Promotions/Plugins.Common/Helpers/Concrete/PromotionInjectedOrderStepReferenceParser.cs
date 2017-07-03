using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace NetSteps.Promotions.Plugins.Common.Helpers.Concrete
{
	public class PromotionInjectedOrderStepReferenceParser : IPromotionInjectedOrderStepReferenceParser
	{
		private const char delimeter = '-';

		public void SetStepReferenceID(string injectedOrderStepReferenceID)
		{
			var parsed = injectedOrderStepReferenceID.Split(delimeter);
			Contract.Assert(parsed.Count() == 3);
			try
			{
				ComponentHandlerProviderKey = parsed[0];
				ComponentExtensionID = int.Parse(parsed[1]);
				CustomerAccountID = int.Parse(parsed[2]);
			}
			catch (Exception ex)
			{
				throw new Exception("InjectedOrderStepReferenceID is in an invalid format.", ex);
			}
		}

		public string GetStepReferenceID()
		{
			return String.Format("{1}{0}{2}{0}{3}", delimeter, ComponentHandlerProviderKey, ComponentExtensionID, CustomerAccountID);
		}

		public int CustomerAccountID { get; set; }

		public string ComponentHandlerProviderKey { get; set; }

		public int ComponentExtensionID { get; set; }
	}
}
