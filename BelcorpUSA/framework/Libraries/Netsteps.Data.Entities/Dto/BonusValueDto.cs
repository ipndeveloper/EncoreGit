namespace NetSteps.Data.Entities.Dto
{
    using System;

    /// <summary>
    /// Descripcion de la clase
    /// </summary>
    public partial class BonusValueDto
    {
        /// <summary>
        /// Obtiene o establece BonusValueID
        /// </summary>
        public int BonusValueID { get; set; }

        public int BonusTypeID { get; set; }

        public int AccountID { get; set; }

        public decimal? BonusAmount { get; set; }

        public int CurrencyTypeID { get; set; }

        public decimal? CorpBonusAmount { get; set; }

        public int? CorpCurrencyTypeID { get; set; }

        public int PeriodID { get; set; }

        public int? CountryID { get; set; }

        public DateTime DateModified { get; set; }
    }
}