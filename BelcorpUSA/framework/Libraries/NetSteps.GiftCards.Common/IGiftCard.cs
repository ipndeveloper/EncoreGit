namespace NetSteps.GiftCards.Common
{
    using System;
using NetSteps.Encore.Core.Dto;

    [DTO]
    public interface IGiftCard
    {
        int GiftCardID { get; set; }
        string Code { get; set; }
        decimal? InitialAmount { get; set; }
        decimal? Balance { get; set; }
        DateTime? ExpirationDate { get; set; }
        int? CurrencyID { get; set; }
        int? OriginOrderItemID { get; set; }
    }
}
