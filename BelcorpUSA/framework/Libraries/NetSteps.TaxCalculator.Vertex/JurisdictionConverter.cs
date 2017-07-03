using NetSteps.TaxCalculator.Vertex.CalculateTaxService60;
using NetSteps.Taxes.Common.Models;


namespace NetSteps.TaxCalculator.Vertex
{
    public class JurisdictionConverter : IEnumConverter<JurisdictionLevelCodeType, JurisdictionLevel>
    {
        public JurisdictionLevel Convert(JurisdictionLevelCodeType value)
        {
            JurisdictionLevel convertedValue = 0;
            
            switch (value)
            {
                case JurisdictionLevelCodeType.APO:
                    convertedValue = JurisdictionLevel.Apo;
                    break;
                case JurisdictionLevelCodeType.BOROUGH:
                    convertedValue = JurisdictionLevel.Borough;
                    break;
                case JurisdictionLevelCodeType.CITY:
                    convertedValue = JurisdictionLevel.City;
                    break;
                case JurisdictionLevelCodeType.COUNTRY:
                    convertedValue = JurisdictionLevel.Country;
                    break;
                case JurisdictionLevelCodeType.COUNTY:
                    convertedValue = JurisdictionLevel.County;
                    break;
                case JurisdictionLevelCodeType.DISTRICT:
                    convertedValue = JurisdictionLevel.District;
                    break;
                case JurisdictionLevelCodeType.FPO:
                    convertedValue = JurisdictionLevel.Fpo;
                    break;
                case JurisdictionLevelCodeType.LOCAL_IMPROVEMENT_DISTRICT:
                    convertedValue = JurisdictionLevel.LocalImprovementDistrict;
                    break;
                case JurisdictionLevelCodeType.PARISH:
                    convertedValue = JurisdictionLevel.Parish;
                    break;
                case JurisdictionLevelCodeType.PROVINCE:
                    convertedValue = JurisdictionLevel.Province;
                    break;
                case JurisdictionLevelCodeType.SPECIAL_PURPOSE_DISTRICT:
                    convertedValue = JurisdictionLevel.SpecialPurposeDistrict;
                    break;
                case JurisdictionLevelCodeType.STATE:
                    convertedValue = JurisdictionLevel.State;
                    break;
                case JurisdictionLevelCodeType.TERRITORY:
                    convertedValue = JurisdictionLevel.Territory;
                    break;
                case JurisdictionLevelCodeType.TOWNSHIP:
                    convertedValue = JurisdictionLevel.Township;
                    break;
                case JurisdictionLevelCodeType.TRADE_BLOCK:
                    convertedValue = JurisdictionLevel.TradeBlock;
                    break;
                case JurisdictionLevelCodeType.TRANSIT_DISTRICT:
                    convertedValue = JurisdictionLevel.TransitDistrict;
                    break;
            }
            return convertedValue;
        }
    }
}
