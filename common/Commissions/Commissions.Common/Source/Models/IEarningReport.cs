using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
    /// <summary>
    /// Represent a Earning Model
    /// </summary>
    public interface IEarningReport
    {
        //TODO: modificar documentacion
        /// <summary>
        /// Obtiene o establece CompanyTerm
        /// </summary>
        string CompanyTerm { get; }

        /// <summary>
        /// Obtiene o establece Company
        /// </summary>
        string Company { get;}
        
        /// <summary>
        /// Obtiene o establece AccountNumberTerm
        /// </summary>
        string AccountNumberTerm { get; }

        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        string AccountNumber { get;}

        /// <summary>
        /// Obtiene o establece CareerTitleTerm
        /// </summary>
        string CareerTitleTerm { get; }

        /// <summary>
        /// The AccountNumber is for TODO: STRING
        /// </summary>
        int? CareerTitle { get; }

        /// <summary>
        /// Obtiene o establece PaidAsTitleTerm
        /// </summary>
        string PaidAsTitleTerm { get; }

        /// <summary>
        /// Obtiene o establece PaidAsTitle
        /// </summary>
        string PaidAsTitle { get;}

        /// <summary>
        /// Obtiene o establece EnrollmentDateTerm
        /// </summary>
        string EnrollmentDateTerm { get; }

        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        DateTime? EnrollmentDate { get; }

        /// <summary>
        /// Obtiene o establece AccountNameTerm
        /// </summary>
        string AccountNameTerm { get; }

        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        string AccountName { get; }

        /// <summary>
        /// Obtiene o establece AddressTerm
        /// </summary>
        string AddressTerm { get; }

        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        string Address { get; }

        /// <summary>
        /// Obtiene o establece StateTerm
        /// </summary>
        string StateTerm { get; }

        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        string State { get; }

        /// <summary>
        /// Obtiene o establece PostalCodeTerm
        /// </summary>
        string PostalCodeTerm { get;}

        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        string PostalCode { get; }

        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? Level1CV { get; }
        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? Level1CBPer { get; }
        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? Level1CB { get; }
        /// <summary>
        /// Term field - not provided
        /// </summary>
        string Level1Term { get; }
        /// <summary>
        /// Code
        /// </summary>
        string Level1Code { get; }

        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? Level2CV { get; }
        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? Level2CBPer { get; }
        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? Level2CB { get; }
        /// <summary>
        /// Term field - not provided
        /// </summary>
        string Level2Term { get; }
        /// <summary>
        /// Code
        /// </summary>
        string Level2Code { get; }

        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? Level3CV { get; }
        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? Level3CBPer { get; }
        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? Level3CB { get; }
        /// <summary>
        /// Term field - not provided
        /// </summary>
        string Level3Term { get; }
        /// <summary>
        /// Code
        /// </summary>
        string Level3Code { get; }

        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? Level4CV { get; }
        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? Level4CBPer { get; }
        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? Level4CB { get; }
        /// <summary>
        /// Term field - not provided
        /// </summary>
        string Level4Term { get; }
        /// <summary>
        /// Code
        /// </summary>
        string Level4Code { get; }

        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? Generation1Title7CV { get; }
        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? Generation1Title7CBPer { get; }
        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? Generation1Title7CB { get; }
        /// <summary>
        /// Term field - not provided
        /// </summary>
        string Generation1Title7Term { get; }
        /// <summary>
        /// Code
        /// </summary>
        string Generation1Title7Code { get; }

        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? Generation2Title7CV { get; }
        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? Generation2Title7CBPer { get; }
        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? Generation2Title7CB { get; }
        /// <summary>
        /// Term field - not provided
        /// </summary>
        string Generation2Title7Term { get; }
        /// <summary>
        /// Code
        /// </summary>
        string Generation2Title7Code { get; }

        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? Generation3Title7CV { get; }
        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? Generation3Title7CBPer { get; }
        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? Generation3Title7CB { get; }
        /// <summary>
        /// Term field - not provided
        /// </summary>
        string Generation3Title7Term { get; }
        /// <summary>
        /// Code
        /// </summary>
        string Generation3Title7Code { get; }

        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? Generation4Title7CV { get; }
        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? Generation4Title7CBPer { get; }
        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? Generation4Title7CB { get; }
        /// <summary>
        /// Term field - not provided
        /// </summary>
        string Generation4Title7Term { get; }
        /// <summary>
        /// Code
        /// </summary>
        string Generation4Title7Code { get; }

        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? Generation5Title7CV { get; }
        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? Generation5Title7CBPer { get; }
        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? Generation5Title7CB { get; }
        /// <summary>
        /// Term field - not provided
        /// </summary>
        string Generation5Title7Term { get; }
        /// <summary>
        /// Code
        /// </summary>
        string Generation5Title7Code { get; }

        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? Generation1Title10CV { get; }
        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? Generation1Title10CBPer { get; }
        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? Generation1Title10CB { get; }
        /// <summary>
        /// Term field - not provided
        /// </summary>
        string Generation1Title10Term { get; }
        /// <summary>
        /// Code
        /// </summary>
        string Generation1Title10Code { get; }

        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? Generation2Title10CV { get; }
        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? Generation2Title10CBPer { get; }
        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? Generation2Title10CB { get; }
        /// <summary>
        /// Term field - not provided
        /// </summary>
        string Generation2Title10Term { get; }
        /// <summary>
        /// Code
        /// </summary>
        string Generation2Title10Code { get; }

        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? TurboInfinityBonusCV { get; }
        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? TurboInfinityBonusCBPer { get; }
        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? TurboInfinityBonusCB { get; }
        /// <summary>
        /// Term field - not provided
        /// </summary>
        string TurboInfinityBonusTerm { get; }
        /// <summary>
        /// Code
        /// </summary>
        string TurboInfinityBonusCode { get; }

        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? FastStartBonusCV { get; }
        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? FastStartBonusCBPer { get; }
        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? FastStartBonusCB { get; }
        /// <summary>
        /// Term field - not provided
        /// </summary>
        string FastStartBonusTerm { get; }
        /// <summary>
        /// Code
        /// </summary>
        string FastStartBonusCode { get; }

        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? CoachingBonusCV { get; }
        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? CoachingBonusCBPer { get; }
        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? CoachingBonusCB { get; }
        /// <summary>
        /// Term field - not provided
        /// </summary>
        string CoachingBonusTerm { get; }
        /// <summary>
        /// Code
        /// </summary>
        string CoachingBonusCode { get; }

        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? TeamBuildingBonusCV { get; }
        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? TeamBuildingBonusCBPer { get; }
        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? TeamBuildingBonusCB { get; }
        /// <summary>
        /// Term field - not provided
        /// </summary>
        string TeamBuildingBonusTerm { get; }
        /// <summary>
        /// Code
        /// </summary>
        string TeamBuildingBonusCode { get; }

        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? AdvancementBonusCV { get; }
        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? AdvancementBonusCBPer { get; }
        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? AdvancementBonusCB { get; }
        /// <summary>
        /// Term field - not provided
        /// </summary>
        string AdvancementBonusTerm { get; }
        /// <summary>
        /// Code
        /// </summary>
        string AdvancementBonusCode { get; }

        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? MatchingAdvacementBonusCV { get; }
        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? MatchingAdvacementBonusCBPer { get; }
        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? MatchingAdvacementBonusCB { get; }
        /// <summary>
        /// Term field - not provided
        /// </summary>
        string MatchingAdvacementBonusTerm { get; }
        /// <summary>
        /// Code
        /// </summary>
        string MatchingAdvacementBonusCode { get; }

        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? ConsistencyBonusCV { get; }
        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? ConsistencyBonusCBPer { get; }
        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? ConsistencyBonusCB { get; }
        /// <summary>
        /// Term field - not provided
        /// </summary>
        string ConsistencyBonusTerm { get; }
        /// <summary>
        /// Code
        /// </summary>
        string ConsistencyBonusCode { get; }

        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? RetailProfitBonusCV { get; }
        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? RetailProfitBonusCBPer { get; }
        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? RetailProfitBonusCB { get; }
        /// <summary>
        /// Term field - not provided
        /// </summary>
        string RetailProfitBonusTerm { get; }
        /// <summary>
        /// Code
        /// </summary>
        string RetailProfitBonusCode { get; }

        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? TotalCV { get; }
        /// <summary>
        /// The AccountNumber is for
        /// </summary>
        decimal? TotalCB { get; }
    }
}
