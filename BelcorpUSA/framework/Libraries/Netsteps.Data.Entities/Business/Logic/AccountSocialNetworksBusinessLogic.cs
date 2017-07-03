using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business.Logic
{
    public class AccountSocialNetworksBusinessLogic
    {
        public dynamic Insert(AccountSocialNetworksParameters model)
        {
            var table = new AccountSocialNetworks();
            try
            {               
                return table.Insert(new
                {
                    AccountID = model.AccountID ,
                    SocialNetworkID = model.SocialNetworkID,
                    Value = model.Value           
                });                
            }
            catch
            {
                return null;
            }
        }

        public bool Update(dynamic model)
        {
            var table = new AccountSocialNetworks();

            try
            {              
                table.Update(new
                {
                    AccountID = model.AccountID,
                    SocialNetworkID = model.SocialNetworkID,
                    Value = model.Value     
                },
                model.AccountSocialNetworkID
                );
                return true;
            }
            catch
            {
                return false;
            }
        }


    }
}
