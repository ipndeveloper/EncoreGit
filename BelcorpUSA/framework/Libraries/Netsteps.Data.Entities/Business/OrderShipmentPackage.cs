using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Data.Entities.Cache;

namespace NetSteps.Data.Entities
{
	public partial class OrderShipmentPackage : IDateLastModified
	{
		public string ShippingMethodName
		{
			get
			{
				if (this.ShippingMethodID.HasValue)
				{
					var shippingMethod = SmallCollectionCache.Instance.ShippingMethods.GetById(ShippingMethodID.Value);
					return SmallCollectionCache.Instance.ShippingMethodTranslations.GetTranslatedName(this.ShippingMethodID.Value, (shippingMethod != null) ? shippingMethod.Name : null);
				}
				else
					return string.Empty;
			}
		}
	}
}
