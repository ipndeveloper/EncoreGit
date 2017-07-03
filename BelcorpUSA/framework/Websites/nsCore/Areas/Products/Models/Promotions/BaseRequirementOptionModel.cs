using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetSteps.Extensibility.Core;

namespace nsCore.Areas.Products.Models.Promotions
{
    public class BaseRequirementOptionModel : BaseRewardOptionModel,IPromotionRequirementModel
    {
        public BaseRequirementOptionModel(string extensionProviderKey, IDataObjectExtension extension) : base(extensionProviderKey, extension)
        {
        }
    }
}