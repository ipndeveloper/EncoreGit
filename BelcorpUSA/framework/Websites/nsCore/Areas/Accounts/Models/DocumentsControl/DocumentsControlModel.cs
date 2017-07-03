using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetSteps.Commissions.Common.Models;
using NetSteps.Commissions.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Data.Entities.Cache;

namespace nsCore.Areas.Accounts.Models.DocumentsControl
{
    public class DocumentsControlModel
    {
        public IEnumerable<dynamic> listCreditRequirement = null;

        public IEnumerable<dynamic> lisRequirementStatuses = null;

        public int cantRequirement = 0;
    }
}