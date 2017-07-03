using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Commissions.Common.Models;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Commissions.Service.Models
{
    [ContainerRegister(typeof(IEarningReport), RegistrationBehaviors.Default), Serializable]
    public class EarningReport : IEarningReport
    {
        #region Term
        public string AdvancementBonusTerm
        {
            get;
            set;
        }

        public string CoachingBonusTerm
        {
            get;
            set;
        }

        public string ConsistencyBonusTerm
        {
            get;
            set;
        }

        public string FastStartBonusTerm
        {
            get;
            set;
        }

        public string Generation1Title10Term
        {
            get;
            set;
        }

        public string Generation1Title7Term
        {
            get;
            set;
        }

        public string Generation2Title10Term
        {
            get;
            set;
        }

        public string Generation2Title7Term
        {
            get;
            set;
        }

        public string Generation3Title7Term
        {
            get;
            set;
        }

        public string Generation4Title7Term
        {
            get;
            set;
        }

        public string Generation5Title7Term
        {
            get;
            set;
        }

        public string Level1Term
        {
            get;
            set;
        }

        public string Level2Term
        {
            get;
            set;
        }

        public string Level3Term
        {
            get;
            set;
        }

        public string Level4Term
        {
            get;
            set;
        }

        public string MatchingAdvacementBonusTerm
        {
            get;
            set;
        }

        public string RetailProfitBonusTerm
        {
            get;
            set;
        }

        public string TeamBuildingBonusTerm
        {
            get;
            set;
        }

        public string TurboInfinityBonusTerm
        {
            get;
            set;
        }
        #endregion


        public string AccountName
        {
            get;
            set;
        }

        public string AccountNumber
        {
            get;
            set;
        }

        public string Address
        {
            get;
            set;
        }

        public decimal? AdvancementBonusCB
        {
            get;
            set;
        }

        public decimal? AdvancementBonusCBPer
        {
            get;
            set;
        }

        public decimal? AdvancementBonusCV
        {
            get;
            set;
        }

        public string AdvancementBonusCode
        {
            get;
            set;
        }

        public int? CareerTitle
        {
            get;
            set;
        }

        public decimal? CoachingBonusCB
        {
            get;
            set;
        }

        public decimal? CoachingBonusCBPer
        {
            get;
            set;
        }

        public decimal? CoachingBonusCV
        {
            get;
            set;
        }

        public string CoachingBonusCode
        {
            get;
            set;
        }

        public decimal? ConsistencyBonusCB
        {
            get;
            set;
        }

        public decimal? ConsistencyBonusCBPer
        {
            get;
            set;
        }

        public decimal? ConsistencyBonusCV
        {
            get;
            set;
        }

        public string ConsistencyBonusCode
        {
            get;
            set;
        }

        public DateTime? EnrollmentDate
        {
            get;
            set;
        }

        public decimal? FastStartBonusCB
        {
            get;
            set;
        }

        public decimal? FastStartBonusCBPer
        {
            get;
            set;
        }

        public decimal? FastStartBonusCV
        {
            get;
            set;
        }

        public string FastStartBonusCode
        {
            get;
            set;
        }

        public decimal? Generation1Title10CB
        {
            get;
            set;
        }

        public decimal? Generation1Title10CBPer
        {
            get;
            set;
        }

        public decimal? Generation1Title10CV
        {
            get;
            set;
        }

        public string Generation1Title10Code
        {
            get;
            set;
        }

        public decimal? Generation1Title7CB
        {
            get;
            set;
        }

        public decimal? Generation1Title7CBPer
        {
            get;
            set;
        }

        public decimal? Generation1Title7CV
        {
            get;
            set;
        }

        public string Generation1Title7Code
        {
            get;
            set;
        }

        public decimal? Generation2Title10CB
        {
            get;
            set;
        }

        public decimal? Generation2Title10CBPer
        {
            get;
            set;
        }

        public decimal? Generation2Title10CV
        {
            get;
            set;
        }

        public string Generation2Title10Code
        {
            get;
            set;
        }

        public decimal? Generation2Title7CB
        {
            get;
            set;
        }

        public decimal? Generation2Title7CBPer
        {
            get;
            set;
        }

        public decimal? Generation2Title7CV
        {
            get;
            set;
        }

        public string Generation2Title7Code
        {
            get;
            set;
        }

        public decimal? Generation3Title7CB
        {
            get;
            set;
        }

        public decimal? Generation3Title7CBPer
        {
            get;
            set;
        }

        public decimal? Generation3Title7CV
        {
            get;
            set;
        }

        public string Generation3Title7Code
        {
            get;
            set;
        }

        public decimal? Generation4Title7CB
        {
            get;
            set;
        }

        public decimal? Generation4Title7CBPer
        {
            get;
            set;
        }

        public decimal? Generation4Title7CV
        {
            get;
            set;
        }

        public string Generation4Title7Code
        {
            get;
            set;
        }

        public decimal? Generation5Title7CB
        {
            get;
            set;
        }

        public decimal? Generation5Title7CBPer
        {
            get;
            set;
        }

        public decimal? Generation5Title7CV
        {
            get;
            set;
        }

        public string Generation5Title7Code
        {
            get;
            set;
        }

        public decimal? Level1CB
        {
            get;
            set;
        }

        public decimal? Level1CBPer
        {
            get;
            set;
        }

        public decimal? Level1CV
        {
            get;
            set;
        }

        public string Level1Code
        {
            get;
            set;
        }

        public decimal? Level2CB
        {
            get;
            set;
        }

        public decimal? Level2CBPer
        {
            get;
            set;
        }

        public decimal? Level2CV
        {
            get;
            set;
        }

        public string Level2Code
        {
            get;
            set;
        }

        public decimal? Level3CB
        {
            get;
            set;
        }

        public decimal? Level3CBPer
        {
            get;
            set;
        }

        public decimal? Level3CV
        {
            get;
            set;
        }

        public string Level3Code
        {
            get;
            set;
        }

        public decimal? Level4CB
        {
            get;
            set;
        }

        public decimal? Level4CBPer
        {
            get;
            set;
        }

        public decimal? Level4CV
        {
            get;
            set;
        }

        public string Level4Code
        {
            get;
            set;
        }

        public decimal? MatchingAdvacementBonusCB
        {
            get;
            set;
        }

        public decimal? MatchingAdvacementBonusCBPer
        {
            get;
            set;
        }

        public decimal? MatchingAdvacementBonusCV
        {
            get;
            set;
        }

        public string MatchingAdvacementBonusCode
        {
            get;
            set;
        }

        public string PostalCode
        {
            get;
            set;
        }

        public decimal? RetailProfitBonusCB
        {
            get;
            set;
        }

        public decimal? RetailProfitBonusCBPer
        {
            get;
            set;
        }

        public decimal? RetailProfitBonusCV
        {
            get;
            set;
        }

        public string RetailProfitBonusCode
        {
            get;
            set;
        }

        public string State
        {
            get;
            set;
        }

        public decimal? TeamBuildingBonusCB
        {
            get;
            set;
        }

        public decimal? TeamBuildingBonusCBPer
        {
            get;
            set;
        }

        public decimal? TeamBuildingBonusCV
        {
            get;
            set;
        }

        public string TeamBuildingBonusCode
        {
            get;
            set;
        }

        public decimal? TotalCB
        {
            get;
            set;
        }

        public decimal? TotalCV
        {
            get;
            set;
        }

        public decimal? TurboInfinityBonusCB
        {
            get;
            set;
        }

        public decimal? TurboInfinityBonusCBPer
        {
            get;
            set;
        }

        public decimal? TurboInfinityBonusCV
        {
            get;
            set;
        }

        public string TurboInfinityBonusCode
        {
            get;
            set;
        }


        public string AccountNameTerm
        {
            get;
            set;
        }

        public string AccountNumberTerm
        {
            get;
            set;
        }

        public string AddressTerm
        {
            get;
            set;
        }

        public string CareerTitleTerm
        {
            get;
            set;
        }

        public string EnrollmentDateTerm
        {
            get;
            set;
        }

        public string PostalCodeTerm
        {
            get;
            set;
        }

        public string StateTerm
        {
            get;
            set;
        }


        public string Company
        {
            get;
            set;
        }

        public string CompanyTerm
        {
            get;
            set;
        }


        public string PaidAsTitle
        {
            get;
            set;
        }

        public string PaidAsTitleTerm
        {
            get;
            set;
        }
    }
}
