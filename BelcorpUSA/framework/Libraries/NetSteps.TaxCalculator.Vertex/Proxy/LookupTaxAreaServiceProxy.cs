using System;
using System.Linq;
using NetSteps.Encore.Core;
using NetSteps.Encore.Core.IoC;
using NetSteps.TaxCalculator.Vertex.Config;
using NetSteps.TaxCalculator.Vertex.TaxAreaService60;
using NetSteps.Taxes.Common.Models;

namespace NetSteps.TaxCalculator.Vertex
{
    public static class LookupTaxAreaServiceProxy
    {
        static LoginType __login;
        public static LoginType Login
        {
            get
            {
                return Util.NonBlockingLazyInitializeVolatile(ref __login, () =>
                {
                    var config = VertexTaxCalculatorConfigSection.Current;
                    var login = config.FillLogin(new LoginType());
                    return login;
                });
            }
        }

        public static ITaxArea Lookup(ITaxAddress addr)
        {
            var request = new TaxAreaRequestType
            {
                TaxAreaLookup = new TaxAreaLookupType
                {
                    Item = new PostalAddressType
                        {
                            StreetAddress1 = addr.StreetAddress1,
                            StreetAddress2 = addr.StreetAddress2,
                            City = addr.City.NameOrCodeOrDefault(),
                            SubDivision = addr.SubDivision.NameOrCodeOrDefault(),
                            MainDivision = addr.MainDivision.NameOrCodeOrDefault(),
                            PostalCode = addr.PostalCode.CodeOrNameOrDefault(),
                            Country = addr.Country.CodeOrNameOrDefault(),
                        }
                }
            };

            var response = Invoke<TaxAreaRequestType, TaxAreaResponseType>(request);
            if (response.TaxAreaResult.Length > 0)
            {
                var area = response.TaxAreaResult[0];
                if (area.Status.Any(it => it.lookupResult == LookupResultType.NORMAL))
                {
                    var result = Create.New<ITaxArea>();
                    result.TaxAreaID = area.taxAreaId;
                    int confidence;
                    if (Int32.TryParse(area.confidenceIndicator, out confidence))
                    {
                        result.HasConfidenceLevel = true;
                        result.ConfidenceLevel = confidence;
                    }
                    result.Jurisdictions = (from j in area.Jurisdiction
                                            select Create.Mutation(Create.New<IJurisdiction>(),
                                             it =>
                                             {
                                                 it.Level = (JurisdictionLevel)j.jurisdictionLevel;
                                                 it.Name = j.Value;
                                             })).ToArray();
                    return result;
                }
            }
            return default(ITaxArea);
        }

        internal static TResponse Invoke<TRequest, TResponse>(TRequest request)
        {
            var envelope = new VertexEnvelope();            
            envelope.Login = Login;
            envelope.Item = request;
            using (var svc = new LookupTaxAreasWS60Client())
            {
                svc.LookupTaxAreas60(ref envelope);
            }
            return (TResponse)envelope.Item;
        }
    }
}
