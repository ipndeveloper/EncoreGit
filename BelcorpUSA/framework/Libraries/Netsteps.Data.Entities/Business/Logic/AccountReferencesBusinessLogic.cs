using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business.Logic
{
    public class AccountReferencesBusinessLogic
    { 
        public dynamic Insert(AccountPropertiesParameters model)
        {
            var table = new AccountReferences();
            try
            {               
                return table.Insert(new
                {
                    AccountID=model.AccountID,
                    Name = model.ReferenceName,
                    PersonalRelationshipID = model.RelationShip,
                    Phone = model.PhoneNumberMain,
                    referenceTypeID = 3                 
                });  
            }
            catch
            {
                return null;
            }
        }

        public bool Update(dynamic model)
        {
            var table = new AccountReferences();
            try
            {               
                table.Update(new
                {
                    AccountID = model.AccountID,
                    Name = model.ReferenceName,
                    PersonalRelationshipID = model.RelationShip,
                    Phone = model.PhoneNumberMain,
                    referenceTypeID = 3    
                },
                model.AccountReferencID
                );
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static IEnumerable<dynamic> GetDinamicFormMotive(int ID)
        {
            var table = new AccountProperties();
            IEnumerable<dynamic> List = null;
            List = table.Query("EXEC uspGetDinamicFormMotiveByID @0", new object[] { ID });
            return List;
        }
        
    }
}
