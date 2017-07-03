namespace NetSteps.Data.Entities.Dto
{
    using System;

    public partial class AccountPerformanceDataDto
    {
        /// <summary>
        /// Gets or sets Period Id
        /// </summary>
        public int PeriodId { get; set; }

        /// <summary>
        /// Gets or sets Calculated Date UTC
        /// </summary>
        public DateTime CalculatedDateUTC { get; set; }

        /// <summary>
        /// Gets or sets Account Id
        /// </summary>
        public int AccountID { get; set; }
        
        /// <summary>
        /// Gets or sets Account Number
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// Gets or sets Sponsor Id
        /// </summary>
        public int? SponsorId { get; set; }

        /// <summary>
        /// Gets or sets HLevel
        /// </summary>
        public int? HLevel { get; set; }

        /// <summary>
        /// Gets or sets HGen
        /// </summary>
        public int? HGen { get; set; }

        /// <summary>
        /// Gets or sets Flat Downline
        /// </summary>
        public int? FlatDownline { get; set; }

        /// <summary>
        /// Gets or sets Left Bower
        /// </summary>
        public int? LeftBower { get; set; }

        /// <summary>
        /// Gets or sets Right Bower
        /// </summary>
        public int? RightBower { get; set; }

        /// <summary>
        /// Gets or sets Sorth Path
        /// </summary>
        public byte?[] SorthPath { get; set; }

        /// <summary>
        /// Gets or sets First Name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets Last name
        /// </summary>
        public string Lastname { get; set; }

        /// <summary>
        /// Gets or sets 
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets Birthday UTC
        /// </summary>
        public DateTime? BirthdayUTC { get; set; }

        /// <summary>
        /// Gets or sets Account Type Id
        /// </summary>
        public int? AccountTypeId { get; set; }

        /// <summary>
        /// Gets or sets Account Status Id
        /// </summary>
        public int? AccountStatusId { get; set; }

        /// <summary>
        /// Gets or sets PAT
        /// </summary>
        public int? PAT { get; set; }

        /// <summary>
        /// Gets or sets CT
        /// </summary>
        public int? CT { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether CQ
        /// </summary>
        public bool? CQ { get; set; }

        /// <summary>
        /// Gets or sets PQV
        /// </summary>
        public decimal? PQV { get; set; }

        /// <summary>
        /// Gets or sets PCV
        /// </summary>
        public decimal? PCV { get; set; }

        /// <summary>
        /// Gets or sets GQV
        /// </summary>
        public decimal? GQV { get; set; }

        /// <summary>
        /// Gets or sets 
        /// </summary>
        public decimal? DQV { get; set; }

        /// <summary>
        /// Gets or sets BTL
        /// </summary>
        public int? BTL { get; set; }

        /// <summary>
        /// Gets or sets Autoship Process Date
        /// </summary>
        public DateTime? AutoshipProcessDate { get; set; }

        /// <summary>
        /// Gets or sets Enroller Id
        /// </summary>
        public int? EnrollerId { get; set; }

        /// <summary>
        /// Gets or sets Enrollment Date UTC
        /// </summary>
        public DateTime? EnrollmentDateUTC { get; set; }

        /// <summary>
        /// Gets or sets Location
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets Last Order Commission Date UTC
        /// </summary>
        public DateTime? LastOrderCommissionDateUTC { get; set; }
        
        #region external properties

        /// <summary>
        /// Gets or sets Bonus Type Id
        /// </summary>
        public int BonusTypeID { get; set; }

        /// <summary>
        /// Gets or sets Bonus Requirement Bonus Percent
        /// </summary>
        public decimal BonusPercent { get; set; }

        /// <summary>
        /// Gets or sets Currency Type Id
        /// </summary>
        public int CurrencyTypeID { get; set; }
        #endregion
    }
}