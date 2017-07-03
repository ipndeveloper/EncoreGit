using System;
namespace Belcorp.Policies.Core
{
    public interface IWorkPolicy
    {
        void AddAccountPolicyDetail(int pAccountID, int pPolicyID, string pIPAddress, DateTime? pDateAccepted);
        void Commit();
        DateTime? DateAcceptedPolicy(int pAccountId);
        System.Collections.Generic.IEnumerable<Belcorp.Policies.Entities.AccountPolicies> GetAccountPolicies(int pAccountId);
        System.Collections.Generic.IEnumerable<Belcorp.Policies.Entities.Policies> GetAllActivePolicies();
        System.Collections.Generic.IEnumerable<Belcorp.Policies.Entities.Policies> GetAllActivePoliciesByLanguage(int pLanguageID);
        System.Collections.Generic.IEnumerable<Belcorp.Policies.Entities.Policies> GetAllPolicies();
        bool IsApplicableAccount(int pAccountId, int pTypeBA);
    }
}
