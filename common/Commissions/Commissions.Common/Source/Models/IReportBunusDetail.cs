namespace NetSteps.Commissions.Common.Models
{
    /// <summary>
    /// Represent a Report Bonus Detail Data Returned
    /// </summary>
    public interface IReportBonusDetail
    {
        /// <summary>
        /// Obtiene o establece DownlineID
        /// </summary>
        int? DownlineID { get; }

        /// <summary>
        /// Obtiene o establece DownlineName
        /// </summary>
        string DownlineName { get; }

        /// <summary>
        /// Obtiene o establece PQV
        /// </summary>
        decimal? PQV { get; }

        /// <summary>
        /// Obtiene o establece PCV
        /// </summary>
        decimal? PCV { get; }

        /// <summary>
        /// Obtiene o establece CB
        /// </summary>
        decimal? CB { get; }

        /// <summary>
        /// Obtiene o establece AmountPaid
        /// </summary>
        decimal? AmountPaid { get; }

        /// <summary>
        /// Obtiene o establece BonusTypeID
        /// </summary>
        int? BonusTypeID { get; }

        /// <summary>
        /// Obtiene o establece BonusTypeName
        /// </summary>
        string BonusTypeName { get; }

        /// <summary>
        /// Obtiene o establece BonusValue
        /// </summary>
        decimal? BonusValue { get; }
    }
}