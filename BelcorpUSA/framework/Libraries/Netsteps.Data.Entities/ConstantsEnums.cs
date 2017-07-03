using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NetSteps.Data.Entities.Generated
{
    public partial class ConstantsGenerated : NetSteps.Common.Constants
    {
        #region "StatusProcessMonthlyClosureLog"

        public class StringValue : System.Attribute
        {
            private string _value;

            public StringValue(string value)
            {
                _value = value;
            }

            public string Value
            {
                get { return _value; }
            }

        }

        public static class StringEnum
        {
            public static string GetStringValue(Enum value)
            {
                string output = null;
                Type type = value.GetType();

                //Check first in our cached results...

                //Look for our 'StringValueAttribute' 

                //in the field's custom attributes

                FieldInfo fi = type.GetField(value.ToString());
                StringValue[] attrs =
                   fi.GetCustomAttributes(typeof(StringValue),
                                           false) as StringValue[];
                if (attrs.Length > 0)
                {
                    output = attrs[0].Value;
                }

                return output;
            }
        }

        public enum StatusProcessMonthlyClosureLog : int
        {
            [StringValue("CodeStatusProcessing")]
            CodeStatusProcessing = 0,

            [StringValue("CodeStatusFinished")]
            CodeStatusFinished = 1,

            [StringValue("CodeStatusFinishedError")]
            CodeStatusFinishedError = 2,

            [StringValue("CodeStatusCanceled")]
            CodeStatusCanceled = 3
        }

        #endregion
        #region "Sub Process"

        public enum SubProcess : int
        {
            [StringValue("CodeSubProcessPaymentTitle")]
            CodeSubProcessPaymentTitle = 1,

            [StringValue("CodeSubProcessCommissions")]
            CodeSubProcessCommissions = 2,

            [StringValue("CodeSubProcessBonus")]
            CodeSubProcessBonus = 3,

            [StringValue("CodeSubProcessCareerTitle")]
            CodeSubProcessCareerTitle = 4,

            [StringValue("CodeSubProcessNetworkCompression")]
            CodeSubProcessNetworkCompression = 5
        }

        #endregion

        #region "Main Processes"

        public enum MainProcesses : int
        {
            [StringValue("MainProcessMonthlyClosure")]
            MainProcessMonthlyClosure = 1,

            [StringValue("MainProcessPersonalIndicator")]
            MainProcessPersonalIndicator = 2
        }

        #endregion

        #region LedgerEntry
        public enum LedgerEntryTypes : int
        {
            /// <summary>
            /// 1 - Return Adjustment
            /// </summary>
            ReturnAdjustment = 1,
            EnrrollmentCredit = 11
        }
        public enum LedgerEntryReasons : int
        {
            /// <summary>
            /// 1 - Product Return
            /// </summary>
            ProductReturn = 1,
            ProductCredit = 8
        }
        public enum LedgerEntryOrigins : int
        {
            OrderEntry = 4
        }
        #endregion

        #region ExpirationStatuses
        public enum ExpirationStatuses : int
        {
            Expired = 1,
            Unexpired = 2,
        }
        #endregion

        #region NegotiationLevel
        public enum NegotiationLevel
        {
            Original = 1,
            FirstNegotiation = 2,
            SecondNegotiation = 3,
            ThirdNegotiation = 4
        }
        #endregion

        #region Promotion Types
        public enum PromotionEngineType : short
        {
            DiscountType1 = 1,
            DiscountType2 = 2,
            DiscountType3 = 3,
            DiscountType4 = 4
        }
        #endregion

    }
}
