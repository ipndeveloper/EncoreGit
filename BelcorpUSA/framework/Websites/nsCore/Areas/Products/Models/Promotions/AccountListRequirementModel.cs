using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetSteps.Extensibility.Core;
using NetSteps.Promotions.Plugins.Requirements;

namespace nsCore.Areas.Products.Models.Promotions
{
    public class AccountListRequirementModel : BaseRequirementOptionModel, IAccountTypeRequirementExtension
    {
        public AccountListRequirementModel(string extensionProviderKey, IDataObjectExtension extension) : base(extensionProviderKey, extension)
        {
        }

        public int PromotionRequirementID { get; set; }
        public IList<short> AccountTypes { get; private set; }
    }
}