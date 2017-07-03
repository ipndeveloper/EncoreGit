
namespace NetSteps.Data.Entities.Interfaces
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Interface to interact with tax info on applicable entities. - JHE
    /// Created: 04-07-2010
    /// </summary>
    public interface ITaxInfo
    {
        decimal? TaxPercent { get; set; }
        decimal? TaxAmount { get; set; }

        decimal? TaxPercentCity { get; set; }
        decimal? TaxAmountCity { get; set; }

        decimal? TaxAmountCityLocal { get; set; }
        decimal? TaxAmountCountyLocal { get; set; }
        
        decimal? TaxPercentState { get; set; }
        decimal? TaxAmountState { get; set; }

        decimal? TaxPercentCounty { get; set; }
        decimal? TaxAmountCounty { get; set; }

        decimal? TaxPercentDistrict { get; set; }
        decimal? TaxAmountDistrict { get; set; }

        decimal? TaxAmountCountry { get; set; }

        decimal? TaxableTotal { get; set; }
    }
}
