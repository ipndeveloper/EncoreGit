using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Commissions.Common.Models;
namespace NetSteps.Data.Entities.Dto
{
    public class AccountEnrollDisbursementProfile
    {
        public int id;
        public DisbursementMethodKind preference;
        public Address address;
        public bool? useAddressOfRecord;
        public bool? isActive;
        public bool? agreementOnFile;
        public IEnumerable<IEFTAccount> accounts;
    }
}
