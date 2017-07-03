using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Data.Entities.EntityModels;

namespace NetSteps.Data.Entities.Business.Logic
{
    public class AccountSuppliedIDsBusinessLogic
    {
        public dynamic Insert(AccountSuppliedIDsParameters model)
        {
            var table = new AccountSuppliedIDs();

            try
            {
                if (model.IDTypeID == 4)
                {
                    return table.Insert(new
                    {
                        IDTypeID = model.IDTypeID,
                        AccountID = model.AccountID,
                        AccountSuppliedIDValue = model.AccountSuppliedIDValue,
                        IsPrimaryID = model.IsPrimaryID,
                        IDExpeditionIDate = model.IDExpeditionIDate,
                        ExpeditionEntity = model.ExpeditionEntity
                    });
                }
                else
                {
                    return table.Insert(new
                    {
                        IDTypeID = model.IDTypeID,
                        AccountID = model.AccountID,
                        AccountSuppliedIDValue = model.AccountSuppliedIDValue,
                        IsPrimaryID = model.IsPrimaryID
                    });
                }
            }
            catch
            {
                return null;
            }
        }

        public bool Update(dynamic model)
        {
            var table = new AccountSuppliedIDs();

            try
            {
                if (model.IDTypeID == 4)
                {
                    table.Update(new
                    {
                        IDTypeID = model.IDTypeID,
                        AccountID = model.AccountID,
                        AccountSuppliedIDValue = model.AccountSuppliedIDValue,
                        IsPrimaryID = model.IsPrimaryID,
                        IDExpeditionIDate = model.IDExpeditionIDate,
                        ExpeditionEntity = model.ExpeditionEntity
                    },
                    model.AccountSuppliedID
                    );
                }
                else
                {
                    table.Update(new
                    {
                        IDTypeID = model.IDTypeID,
                        AccountID = model.AccountID,
                        AccountSuppliedIDValue = model.AccountSuppliedIDValue,
                        IsPrimaryID = model.IsPrimaryID,
                    },
                       model.AccountSuppliedID
                       );
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public void InsertAccountSuppliedIDs(AccountSuppliedIDsParameters AccountSuppliedIDsParameters)
        {
            try
            {
                new AccountSuppliedIDsRepository().InsertAccountSuppliedIDs(AccountSuppliedIDsParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<AccountSuppliedIDsTable> GetAccountSuppliedIDByAccountID(AccountSuppliedIDsParameters AccountSuppliedIDsParameters)
        {
            try
            {
                AccountSuppliedIDsRepository repository = new AccountSuppliedIDsRepository();
                List<AccountSuppliedIDsTable> lista = repository.GetAccountSuppliedIDByAccountID(AccountSuppliedIDsParameters);
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DeleteAccountSuppliedIDsByAccountID(int AccountID)
        {
            try
            {
                AccountSuppliedIDsRepository repository = new AccountSuppliedIDsRepository();
                string resultado = repository.DeleteAccountSuppliedIDsByAccountID(AccountID);
                return resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
