using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business
{
    #region Comment

    /// <summary>
    /// @01: BR-CB-002 Auxiliar class to generate SPC Report (Body Detail)
    /// </summary>

    #endregion

    public class OverduePaymentDetail
    {

        public int AccountID { get; set; }

        public string AccountName { get; set; }     // Length: 60

        public string Birthday { get; set; }      // Length: 8

        public string CPF { get; set; }             // Length: 11 (Fill Zeros "0000CPF")

        public string Address1 { get; set; }        // Length: 40

        public string County { get; set; }          // Length: 45

        public string PostalCode { get; set; }      // Length: 8

        public string City { get; set; }            // Length: 20

        public string State { get; set; }           // Length: 2

        public string Gender { get; set; }          // Length: 1

        #region DetailTxt
        public string TypeB { get; set; }           // Length: 1
        public string SequentialB { get; set; }     // Length: 5 (00001) (contador)
        public string OperationTypeB { get; set; }  // Length: 1
        public string Nationality { get; set; }     // Length: 1

        #region EmptyFields
        public string Identity { get; set; }        // Length: 13
        public string StateEmiting { get; set; }    // Length: 2
        public string Naturality { get; set; }      // Length: 20
        public string NaturalityState { get; set; } // Length: 2
        public string MotherName { get; set; }      // Length: 40
        public string FatherName { get; set; }      // Length: 40
        public string MaritalStatus { get; set; }   // Length: 1
        public string Spouse { get; set; }          // Length: 40
        public string SpouseBirthday { get; set; }  // Length: 8
        #endregion

        #region ToStringMethod

        public string BodyDetailToString()
        {
            return String.Concat(TypeB, SequentialB, OperationTypeB, AccountName, Birthday, Identity, StateEmiting, CPF, Nationality,
                                    Naturality, NaturalityState, MotherName, FatherName, Address1, County, PostalCode, City, State,
                                    MaritalStatus, Spouse, SpouseBirthday, Gender);
        }

        #endregion
        #endregion

        public List<OverduePaymentOccurrence> OverduePaymentOcurrences { get; set; }

        public int CountOcurrences
        {
            get
            {
                if (OverduePaymentOcurrences != null)
                    return OverduePaymentOcurrences.Count();

                return 0;
            }
        }

    }
}
