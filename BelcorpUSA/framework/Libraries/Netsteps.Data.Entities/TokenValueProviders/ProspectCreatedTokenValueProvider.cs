using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Generated;

namespace NetSteps.Data.Entities.TokenValueProviders
{
    public class ProspectCreatedValueProvider : ITokenValueProvider
    {
        private const string CUSTOMER_FIRST_NAME = "CustomerFirstName";
        private const string CUSTOMER_LAST_NAME = "CustomerLastName";
        private const string CONSULTANT_FIRST_NAME = "ConsultantFirstName";
        private const string CUSTOMER_ADDRESS = "CustomerAddress";

        private readonly string[] _knownTokens = new string[] { CUSTOMER_FIRST_NAME, CUSTOMER_LAST_NAME, CUSTOMER_ADDRESS, CONSULTANT_FIRST_NAME };

        private readonly Account _prospect;
        private readonly IList<Address> _prospectAddresses;
        private readonly Account _consultant;

        public ProspectCreatedValueProvider(Account prospect, Account consultant)
        {
            this._prospect = Account.Load(prospect.AccountID);
            this._prospectAddresses = Address.LoadByAccountId(this._prospect.AccountID);
            this._consultant = Account.Load(consultant.AccountID);
        }

        public IEnumerable<string> GetKnownTokens()
        {
            return _knownTokens.ToList();
        }

        public string GetTokenValue(string token)
        {
            switch (token)
            {
                case CUSTOMER_FIRST_NAME:
                    return _prospect != null ? _prospect.FirstName : string.Empty;
                case CUSTOMER_LAST_NAME:
                    return _prospect != null ? _prospect.LastName : string.Empty;
                case CONSULTANT_FIRST_NAME:
                    return _prospect != null ? _consultant.FirstName : string.Empty;
                case CUSTOMER_ADDRESS:
                    var address = this._prospectAddresses.GetAllByTypeID(ConstantsGenerated.AddressType.Main).FirstOrDefault();
                    return address != null ? address.ToDisplay(IAddressExtensions.AddressDisplayTypes.Web) : string.Empty;

                default:
                    return null;
            }
        }
    }
}
