using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Encore.Core.IoC;
using NetSteps.Extensibility.Core;
using NetSteps.Promotions.Common.Model;

namespace NetSteps.Promotions.Common.ModelConcrete
{
	[Serializable]
	public abstract class BasePromotion : IPromotion
	{
		public BasePromotion()
        {
            PromotionQualifications = new Dictionary<string, IPromotionQualificationExtension>();
            PromotionRewards = new Dictionary<string, IPromotionReward>();
        }
		public int PromotionID { get; set; }

		public IDictionary<string, IPromotionQualificationExtension> PromotionQualifications { get; set; }

		public IDictionary<string, IPromotionReward> PromotionRewards { get; set; }

		public DateTime? StartDate { get; set; }

		public DateTime? EndDate { get; set; }

		public int PromotionStatusTypeID { get; set; }

		public string Description { get; set; }

		public IEnumerable<string> AssociatedPropertyNames
		{
			get
			{
				List<string> properties = new List<string>();
				foreach (var qual in PromotionQualifications.Values)
				{
					var extension = qual as IPromotionQualificationExtension;
					properties.AddRange(extension.AssociatedPropertyNames);
				}
				return properties.Distinct();
			}
		}

		public bool ValidFor<TPropertyType>(string propertyName, TPropertyType value)
		{
			var registry = Create.New<IDataObjectExtensionProviderRegistry>();
			foreach (var qual in PromotionQualifications.Values)
			{
				var handler = registry.RetrieveExtensionProvider(qual.ExtensionProviderKey) as IPromotionQualificationHandler;
				var extension = qual as IPromotionQualificationExtension;
				if (!handler.ValidFor<TPropertyType>(extension, propertyName, value))
				{
					return false;
				}
			}
			return true;
		}

		public abstract string PromotionKind { get; }
	}
}
