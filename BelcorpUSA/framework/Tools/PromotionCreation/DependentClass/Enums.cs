using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependentClass
{
    public enum OrderType : short
    {
        NotSet = 0,
        OnlineOrder = 1,
        WorkstationOrder = 2,
        PartyOrder = 3,
        PortalOrder = 4,
        AutoshipTemplate = 5,
        AutoshipOrder = 6,
        OverrideOrder = 7,
        ReturnOrder = 8,
        CompOrder = 9,
        ReplacementOrder = 10,
        EnrollmentOrder = 11,
        EmployeeOrder = 12,
        BusinessMaterialsOrder = 13,
        HostessRewardsOrder = 14,
        FundraiserOrder = 15,
        OnlinePartyOrder = 16,
    }

    public enum ProductPriceType : int
    {
        NotSet = 0,
        Retail = 1,
        PreferredCustomer = 2,
        ShippingFee = 10,
        HandlingFee = 11,
        CV = 18,
        HostBase = 20,
        QV = 21,
        Wholesale = 22,
    }
}
