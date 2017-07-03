using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Data.Entities.Cache;

namespace nsCore.Areas.Admin.Models.ReEntryRules
{
    public class ReEntryRulesModel
    {
        public IEnumerable<dynamic> listReEntryRules = null;

        public int cantReEntryRules = 0;

    }
}