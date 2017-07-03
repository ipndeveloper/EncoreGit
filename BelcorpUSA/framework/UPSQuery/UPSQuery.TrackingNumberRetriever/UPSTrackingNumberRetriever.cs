using System;
using System.Diagnostics.Contracts;
using NetSteps.Encore.Core.IoC;
using UPSQuery.TrackingNumberRetriever.UPS;

namespace UPSQuery.TrackingNumberRetriever
{
	#region Interface and Contracts

	[ContractClass(typeof(Contracts.UPSTrackingNumberRetrieverContracts))]
    public interface IUPSTrackingNumberRetriever
    {
        string GetTrackingNumber(string referenceNumber);
    }

	namespace Contracts
	{
		[ContractClassFor(typeof(IUPSTrackingNumberRetriever))]
		abstract class UPSTrackingNumberRetrieverContracts : IUPSTrackingNumberRetriever
		{
			public string GetTrackingNumber(string referenceNumber)
			{
				Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(referenceNumber));

				throw new NotImplementedException();
			}
		}
	}

	#endregion

	[ContainerRegister(typeof(IUPSTrackingNumberRetriever), RegistrationBehaviors.Default)]
    public class UPSTrackingNumberRetriever : IUPSTrackingNumberRetriever
    {
        TrackPortTypeClient _client;
        UPSSecurity _security;

        public UPSTrackingNumberRetriever()
        {
            _client = new TrackPortTypeClient();
            _security = GetSecurityToken();
        }

        UPSSecurity GetSecurityToken()
        {
            //move this to a config at some point
            var security = new UPSSecurity
            {
	            ServiceAccessToken = new UPSSecurityServiceAccessToken {AccessLicenseNumber = "9CB91273AB15A9A2"},
	            UsernameToken = new UPSSecurityUsernameToken {Username = "dev-netsteps", Password = "N3tst3ps"}
            };
	        return security;
        }

        public string GetTrackingNumber(string referenceNumber)
        {
            var tr = new TrackRequest {InquiryNumber = referenceNumber};

	        var response = _client.ProcessTrack(_security, tr);
            var trackingNum = response.Shipment[0].Package[0].TrackingNumber;
            return trackingNum;
        }
    }
}