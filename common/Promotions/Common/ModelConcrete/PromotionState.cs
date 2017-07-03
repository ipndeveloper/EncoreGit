using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Common.Model;

namespace NetSteps.Promotions.Common.ModelConcrete
{
	public class PromotionState : IPromotionState
	{
		public PromotionState()
		{
			_constructionErrors = new List<string>();
		}

		public bool IsValid
		{
			get
			{
				return ConstructionErrors.Count() == 0;
			}
		}

		private List<string> _constructionErrors;

		public IEnumerable<string> ConstructionErrors
		{
			get { return _constructionErrors.ToArray(); }
		}

		public void AddConstructionError(string component, string message)
		{
			_constructionErrors.Add(String.Format("{0}:{1};{2}", component, message, Environment.NewLine));
		}
	}
}
