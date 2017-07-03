using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business
{
    #region Comment

    /// <summary>
    /// @01: BR-CB-002 Auxiliar class to generate SPC Report (Header & Footer)
    /// </summary>

    #endregion

    public class OverduePaymentReport
    {

        #region OverduePayment
        public List<OverduePaymentDetail> OverduePaymentDetails { get; set; }
        #endregion

        #region HeaderTxt
        public string TypeA { get; set; }           // Length: 1
        public string Sequential { get; set; }      // Length: 5 (00001) (fileCode)
        public string Date { get; set; }            // Length: 8
        public string CompanyCode { get; set; }     // Length: 5
        public string Destination { get; set; }     // Length: 6
        #endregion

        #region FooterTxt

        public string TypeZ { get; set; }           // Length: 1

        public string SumB
        {
            get
            {
                if (OverduePaymentDetails != null)
                    return OverduePaymentDetails.Count().ToString("0000000");

                return new String('0', 7);
            }
        }   // Length: 7 (Count TypeB)
        public string SumC
        {
            get
            {
                if (OverduePaymentDetails != null)
                    return OverduePaymentDetails.Sum(opd => opd.CountOcurrences).ToString("0000000");

                return new String('0', 7);
            }
        }   // Length: 7 (Count TypeC)
        public string Zeros { get; set; }           // Length: 35 (Full Zeros)
        #endregion

        #region CamposEnBlanco
        public string RemainingA { get; set; }      // Length: 314

        public string SUMI { get; set; }            // Length: 7
        public string SUMJ { get; set; }            // Length: 7
        public string SUMK { get; set; }            // Length: 7
        public string RemainingZ { get; set; }      // Length: 268
        #endregion


        #region ToStringMethods

        public string HeaderToString()
        {
            return String.Concat(TypeA, Sequential, Date, CompanyCode, Destination, RemainingA);
        }

        public string FooterToString()
        {
            return String.Concat(TypeZ, SumB, SumC, Zeros, SUMI, SUMJ, SUMK, RemainingZ);
        }

        #endregion

    }
}
