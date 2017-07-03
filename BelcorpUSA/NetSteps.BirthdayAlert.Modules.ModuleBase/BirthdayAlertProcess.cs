using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NetSteps.Data.Entities;


namespace NetSteps.BirthdayAlert.Modules.ModuleBase
{
    public class BirthdayAlertProcess
    {
        public static void ProcessAccountBirthday()
        {
            List<Account> Accounts = BirthdayAlertDAC.GetAccounts();

            foreach (Account account in Accounts)
            {
                if (account.BirthdayUTC == DateTime.Now)
                { 
                    //Enviar Email.
                }
            }
        }
    }
}
