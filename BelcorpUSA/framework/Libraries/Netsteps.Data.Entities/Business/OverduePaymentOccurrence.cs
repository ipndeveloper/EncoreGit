using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business
{
    #region Comment

    /// <summary>
    /// @01: BR-CB-002 Auxiliar class to generate SPC Report (Body Occurrence)
    /// </summary>

    #endregion

    public class OverduePaymentOccurrence
    {

        public int OverduePaymentID { get; set; }

        public int AccountID { get; set; }

        public string ExpirationDate { get; set; }

        public string OrderDate { get; set; }

        public string Title { get; set; }

        public decimal TotalAmount { get; set; }

        #region OccurrenceTxt
        public string TypeC { get; set; }           // Length: 1
        public string SequentialC1 { get; set; }    // EQUALS TO SequentialB
        public string SequentialC2 { get; set; }    // Length: 3 (001) (contador)
        public string DebtType { get; set; }        // Length: 1
        public string OcurrenceCode { get; set; }   // Length: 3
        public string SPCCode { get; set; }         // Length: 5
        public string OperationTypeC { get; set; }  // Length: 1

        #region EmptyFields
        public string Blanks { get; set; }          // Length: 30
        public string RemainingC { get; set; }      // Length: 248
        #endregion

        #region ToStringMethod

        public string BodyOccurrenceToString()
        {
            return String.Concat(TypeC, SequentialC1, SequentialC2, ExpirationDate, OrderDate, Title, DebtType, Blanks, OcurrenceCode,
                                    TotalAmount, SPCCode, OperationTypeC, RemainingC);
        }

        #endregion
        #endregion


    }
}
