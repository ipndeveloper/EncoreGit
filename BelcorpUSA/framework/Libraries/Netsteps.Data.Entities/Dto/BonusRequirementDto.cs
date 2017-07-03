namespace NetSteps.Data.Entities.Dto
{
    using System;

    public class BonusRequirementDto
    {
        /// <summary>
        /// Gets or sets Bonus Requirement Id
        /// </summary>
        public int BonusRequirementId { get; set; }

        /// <summary>
        /// Gets or sets Bonus Type Id
        /// </summary>
        public int BonusTypeId { get; set; }

        /// <summary>
        /// Gets or sets Requirement Tree Id
        /// </summary>
        public string RequirementsTreeId { get; set; }

        /// <summary>
        /// Gets or sets Bonus Amount
        /// </summary>
        public decimal? BonusAmount { get; set; }

        /// <summary>
        /// Gets or sets Bonus Percent
        /// </summary>
        public decimal? BonusPercent { get; set; }

        /// <summary>
        /// Gets or sets Bonus Max Amount
        /// </summary>
        public decimal? BonusMaxAmount { get; set; }

        /// <summary>
        /// Gets or sets 
        /// </summary>
        public decimal? BonusMaxPercent { get; set; }

        /// <summary>
        /// Gets or sets Min Title Id
        /// </summary>
        public int? MinTitleId { get; set; }

        /// <summary>
        /// Gets or sets Max Title Id
        /// </summary>
        public int? MaxTitleId { get; set; }

        /// <summary>
        /// Obtiene o establece BonusMinAmount
        /// </summary>
        public int? BonusMinAmount { get; set; }

        /// <summary>
        /// Obtiene o establece PayMonth
        /// </summary>
        public int? PayMonth { get; set; }

        /// <summary>
        /// Gets or sets Country Id
        /// </summary>
        public int CountryId { get; set; }

        /// <summary>
        /// Gets or sets Currency Type Id
        /// </summary>
        public int CurrencyTypeId { get; set; }

        /// <summary>
        /// Gets or sets Effective Date
        /// </summary>
        public DateTime EffectiveDate { get; set; }

        #region external Properties
        /// <summary>
        /// Gets or sets Type Name from BonusTypes Table
        /// </summary>
        public string BonusTypeName { get; set; }

        /// <summary>
        /// Gets or sets Plan Name from Plans Table
        /// </summary>
        public string PlanName { get; set; }

        /// <summary>
        /// Gets or sets Rows Count
        /// </summary>
        public int RowsCount { get; set; }
        #endregion
    }
}
