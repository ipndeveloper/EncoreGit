using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Security;

namespace NetSteps.Data.Entities.Repositories
{
    public partial class AccountSuppliedIDRepository : IAccountSuppliedIDRepository
    {
        public AccountSuppliedID GetByAccountID(int AccountID, string CFP)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var account = context.AccountSuppliedIDs.Where(a => a.AccountID == AccountID && a.AccountSuppliedIDValue == CFP).FirstOrDefault();
                    return account;
                }
            });
        }
    }
}
