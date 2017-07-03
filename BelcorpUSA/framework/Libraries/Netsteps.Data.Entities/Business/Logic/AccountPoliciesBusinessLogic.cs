using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic
{
    public class AccountPoliciesBusinessLogic
    {
        public string DeleteAccountPoliciesByAccountID(int AccountID)
        {
            try
            {
                AccountPolicyRepository repository = new AccountPolicyRepository();
                string resultado = repository.DeleteAccountPoliciesByAccountID(AccountID);
                return resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
