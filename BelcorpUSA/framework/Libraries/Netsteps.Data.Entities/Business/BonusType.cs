namespace NetSteps.Data.Entities.Business
{
    using System.Collections.Generic;
    using System.Linq;
    using NetSteps.Data.Entities.Business.Interfaces;
    using NetSteps.Data.Entities.Repositories;
    using NetSteps.Data.Entities.Repositories.Interfaces;

    /// <summary>
    /// Business Object for Bonus Type
    /// </summary>
    public class BonusType : ITermName
    {
        #region properties

        /// <summary>
        /// Gets or sets Bonus Type Id
        /// </summary>
        public int BonusTypeId { get; set; }

        /// <summary>
        /// Gets or sets Bonus Code
        /// </summary>
        public string BonusCode { get; set; }

        /// <summary>
        /// Gets or sets Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Term Name
        /// </summary>
        public string TermName { get; set; }

        /// <summary>
        /// Gets or sets whether Bonus Type is Enabled
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets whether Bonus Type is Editable
        /// </summary>
        public bool Editable { get; set; }

        /// <summary>
        /// Gets or sets Plan Id
        /// </summary>
        public int? PlanId { get; set; }

        /// <summary>
        /// Gets or sets Earnings Type Id
        /// </summary>
        public int? EarningsTypeId { get; set; }

        /// <summary>
        /// Gets or sets client Name
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// Gets or sets whether Bonus Type is commission
        /// </summary>
        public bool? IsCommission { get; set; }

        /// <summary>
        /// Gets or sets Client Code
        /// </summary>
        public string ClientCode { get; set; }
                
        #region external properties
        /// <summary>
        /// Gets or sets Name from Plans Table
        /// </summary>
        public string PlanName { get; set; }
        #endregion
       
        #endregion
    }
}
