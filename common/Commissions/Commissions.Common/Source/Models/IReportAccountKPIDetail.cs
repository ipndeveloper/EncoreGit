namespace NetSteps.Commissions.Common.Models
{
    /// <summary>
    /// Represent a Report AccountKPI Detail Data Returned
    /// </summary>
    public interface IReportAccountKPIDetail
    {
        /// <summary>
        /// Obtiene o establece AccountKPIDetailID
        /// </summary>
        int? AccountKPIDetailID { get; }

        /// <summary>
        /// Obtiene o establece DownlineID
        /// </summary>
        int? DownlineID { get; }

        /// <summary>
        /// Obtiene o establece DownlineName
        /// </summary>
        string DownlineName { get; }

        /// <summary>
        /// Obtiene o establece Level
        /// </summary>
        int? Level { get; }

        /// <summary>
        /// Obtiene o establece Generation
        /// </summary>
        int? Generation { get; }

        /// <summary>
        /// Obtiene o establece CareerTitle
        /// </summary>
        //int? CareerTitle { get; } Antonio Campos Santos()11/fe/2016
        string CareerTitle { get; }

        /// <summary>
        /// Obtiene o establece DownlinePaidAsTitle
        /// </summary>
        string DownlinePaidAsTitle { get; }

        /// <summary>
        /// Obtiene o establece PQV
        /// </summary>
        decimal? PQV { get; }

        /// <summary>
        /// Obtiene o establece DQV
        /// </summary>
        decimal? DQV { get; }

        /// <summary>
        /// Obtiene o establece QV
        /// </summary>
        decimal? QV { get; }
    }
}
