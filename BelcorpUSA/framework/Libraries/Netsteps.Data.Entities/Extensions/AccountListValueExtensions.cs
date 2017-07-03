using System.Collections.Generic;
using System.Linq;

namespace NetSteps.Data.Entities.Extensions
{
    /// <summary>
    /// Author: John Egbert
    /// Description: AccountListValue Extensions
    /// Created: 07-15-2010
    /// </summary>
    public static class AccountListValueExtensions
    {
        public static string GetTranslatedValue(this IEnumerable<AccountListValue> accountListValues, int accountListValueID)
        {
            var description = accountListValues.FirstOrDefault(l => l.AccountListValueID == accountListValueID);

            if (description == null)
                return string.Empty;
            else
                return description.GetTerm();
        }
    }
}
