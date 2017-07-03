using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetSteps.Commissions.Common.Models;

namespace nsDistributor.Areas.Enroll.Models.Shared
{
    public class DirectDepositPaymentMethodsAccountModel
    {
        public DirectDepositPaymentMethodsAccountModel(IEFTDisbursementProfile eftProfile, int profileNumber, bool enabled, string lookupUrl, int marketID)
        {
            EFTProfile = eftProfile;
            ProfileNumber = profileNumber;
            Enabled = enabled;
            LookupUrl = lookupUrl;
            MarketID = marketID;
        }

        public IEFTDisbursementProfile EFTProfile { get; set; }
        public int ProfileNumber { get; set; }
        public bool Enabled { get; set; }
        public string LookupUrl { get; set; }
        public int MarketID { get; set; }
    }
}