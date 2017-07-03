using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Belcorp.Policies.Service.DTO
{
    public class AccountPolicyDetailsDTO
    {
        public int AccountID { get; set; }
        public int LanguageID { get; set; }
        public string IPAddress { get; set; }
        public string FilePath { get; set; } /*R2908 - HUNDRED(JAUF)*/
        public DateTime? DateAccepted { get; set; }
        public bool IsApplicableAccount { get; set; }

        public string DateFormatedAccepted { 
            get 
            {
                return DateAccepted != null ? ((DateTime)DateAccepted).ToShortDateString(): "";
            } 
        }
        public bool IsAcceptedPolicy
        {
            get
            {
                return string.IsNullOrEmpty(DateFormatedAccepted) ? false : true;
            }
        }
    }
}
