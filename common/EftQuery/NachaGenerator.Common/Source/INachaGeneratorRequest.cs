using System;
using NetSteps.Encore.Core.Dto;

namespace EFTQuery.NachaGenerator.Common
{
    [DTO]
    public interface INachaGeneratorRequest
    {
        string CompanyName { get; set; }
        string CompanyRoutingNumber { get; set; }
        
        string EftClassType { get; set; }
        string EftComapnyIdentificationNumber { get; set; }

        string DescriptionOfTransction { get; set; }
        
        DateTime CompanyDescriptiveDate { get; set; }
        DateTime EffectiveDate { get; set; }
        
        string BatchNumber { get; set; }
        string CountryCode { get; set; }
        string ImmediateDestinationName { get; set; }
        string SecurityString { get; set; }
        string DetailTrace { get; set; }
        string ServiceClassCode { get; set; }
    }
}