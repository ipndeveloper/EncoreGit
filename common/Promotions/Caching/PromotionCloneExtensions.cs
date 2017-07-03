using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Common.Model;
using NetSteps.Encore.Core;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace NetSteps.Promotions.Caching
{
	internal static class PromotionCloneExtension
	{
		internal static IPromotion Clone(this IPromotion promo)
		{
			using (MemoryStream stream = new MemoryStream())
			{
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(stream, promo);
				stream.Position = 0;
				return (IPromotion)formatter.Deserialize(stream);
			}
		}
	}
}
