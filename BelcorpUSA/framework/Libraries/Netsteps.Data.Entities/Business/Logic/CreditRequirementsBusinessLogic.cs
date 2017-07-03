using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic
{
    public class CreditRequirementsBusinessLogic
    {
        public IEnumerable<dynamic> GetAllCreditRequirements()
        {
            var table = new CreditRequirements();
            var lista = table.All();
            return lista;
        }

        public IEnumerable<dynamic> GetCreditRequirementsByAccount(int AccountID)
        {
            var table = new CreditRequirements();
            IEnumerable<dynamic> List = null;
            List = table.Query("EXEC uspGetCreditRequirementsByAccount @0", new object[] { AccountID });
            return List;
        }

        public dynamic GetCreditRequirementsByCreditRequirementId(decimal creditRequirementId)
        {
            dynamic table = new CreditRequirements();
            var result = table.Single("CreditRequirementID=" + creditRequirementId.ToString(), args: new object[] { creditRequirementId });
            return result;
        }

        public dynamic Insert(CreditRequirementSearchData model)
        {
            var table = new CreditRequirements();

            try
            {
                return table.Insert(new
                {
                    AccountID = model.AccountID,
                    RequirementTypeID = model.RequirementTypeID,
                    RequirementStatusID = model.RequirementStatusID,
                    CreationDate = model.CreationDate,
                    LastModifiedDate = model.LastModifiedDate,
                    UserCreatedID = model.UserCreatedID,
                    LastUserModifiedID = model.UserCreatedID,
                    Observations = model.Observations
                });


            }
            catch
            {
                return null;
            }
        }

        public bool Update(dynamic model)
        {
            var table = new CreditRequirements();

            try
            {
                table.Update(new
                {
                    RequirementStatusID = model.RequirementStatusID,
                    LastModifiedDate = model.LastModifiedDate,
                    LastUserModifiedID = model.UserCreatedID,
                    Observations = model.Observations
                },
                model.CreditRequirementID
                );

                return true;
            }
            catch
            {
                return false;
            }
        }

        public string DeleteCreditRequirementsByAccountID(int AccountID)
        {
            try
            {
                CreditRequirementsRepository repository = new CreditRequirementsRepository();
                string resultado = repository.DeleteCreditRequirementsByAccountID(AccountID);
                return resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
