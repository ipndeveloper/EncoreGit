using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.BirthdayAlert.Modules.ModuleBase
{
    public class Account
    {
        public int Id { get; set; }
        public DateTime?  BirthdayUTC { get; set; }
        public string Email { get; set; }
    }
}
