using System;
using System.Collections.Generic;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Mvc.Helpers;
using NetSteps.Commissions.Common;
using NetSteps.Encore.Core.IoC;

namespace DistributorBackOffice.Helpers.Performance
{

    public class GenealogyHelpers
    {
        private ICommissionsService _commissionsService = Create.New<ICommissionsService>();
        #region Exposed Methods

        public virtual string IntegerColumnValues(Downline downline, int accountID, string columnName, object data)
        {
            int? value = data as int?;

            string result = value == null || value<0
                                ? DisplayNA()
                                : value.ToString(); //GetIntegerColumnValues(downline, accountID, columnName, data, value); Quitado trae problemas de funcionalidad

            return result;
        }

        public virtual string DoubleColumnValues(object data)
        {
            double? value = data as double?;

            string result = value == null
                                ? DisplayNA()
                                : value.Value.TruncateDoubleInsertCommas();

            return result;
        }

        public virtual string StringColumnValues(string columnName, object data)
        {
            string value = data as string;

            string result = value == null
                                ? DisplayNA()
                                : GetStringColumnValues(columnName, value);

            return result;
        }

        public virtual string DateTimeColumnValues(string columnName, object data)
        {
            DateTime? value = data as DateTime?;

            return DisplayColumnForNullableValues(value, columnName, GetDateTimeColumnValues);
        }

        public virtual string DecimalColumnValues(object data, string columnName)
        {
            decimal? value = data as decimal?;

            return DisplayColumnForNullableValues(value, columnName, GetDecimalColumnValues);
        }

        public virtual string ShortColumnValues(object data, string columnName)
        {
            short? value = data as short?;

            return DisplayColumnForNullableValues(value, columnName, GetShortColumnValues);
        }

        #endregion


        #region Helpers

        public virtual string ColumnNamePaidAsTitleOrCurrentTitle(int? value)
        {
            string result = string.Empty;
            if (value.HasValue)
            {
                var title = _commissionsService.GetTitle(value.Value);
                if (title != null)
                {
                    result = Translation.GetTerm(title.TermName);
                }
            }
            return result;
        }

        public virtual string ColumnNameLevel(Downline downline, int accountID, int? value)
        {
            string result;
            // Set Dynamically calculated values - JHE
            if (IsKeyPresentInDictionary(downline.LookupNode, CoreContext.CurrentAccount.AccountID)
                && IsKeyPresentInDictionary(downline.LookupNode, accountID))
            {
                int currentAccountNodeLevel = downline.LookupNode[CoreContext.CurrentAccount.AccountID].Level;
                DownlineNode downlineNode = downline.LookupNode[accountID];

                value = downlineNode.Level - currentAccountNodeLevel;

                result = value.ToInt().ToString().PadLeft(value.ToInt() + 1, '.');
            }
            else
            {
                result = DisplayNA();
            }
            return result;
        }

        public virtual string ColumnNameSponsorID(Downline downline, object data)
        {
            return AccountName(downline, data);
        }

        public virtual string ColumnNameEnrollerID(Downline downline, object data)
        {
            return AccountName(downline, data);
        }

        private string AccountName(Downline downline, object data)
        {
            int accountID = (int)data;
            if (IsKeyPresentInDictionary(downline.Lookup, accountID))
            {
                var downlineNodeData = downline.Lookup[accountID];
                return Account.ToFullName(downlineNodeData.FirstName, "",
                                                downlineNodeData.LastName,
                                                CoreContext.CurrentCultureInfo.ToString());
            }

            return DisplayNA();
        }



        public string DisplayColumnForNullableValues<T>(T value, string columnName, Func<string, T, string> methodToCall)
        {
            //string result = "$0.00";

            //if (value.IsNullableType())
            //{


            //    result = value == null
            //                      ? "$0.00"
            //                      : string.Concat("$", decimal.Parse(value.ToString()).ToString("N", new System.Globalization.CultureInfo("en-US"))); //string.Format("{0:0.00}", value));//methodToCall(columnName, value);
            //}

            
             return   methodToCall(columnName, value);
            


            
            //return result;
        }


        public virtual string GetIntegerColumnValues(Downline downline, int accountID, string columnName, object data, int? value)
        {
            string result;

            switch (columnName)
            {
                case "PaidAsTitle":
                case "CurrentTitle":
                    result = ColumnNamePaidAsTitleOrCurrentTitle(value);
                    break;
                case "Level":
                    result = ColumnNameLevel(downline, accountID, value);
                    break;
                case "SponsorID":
                    result = ColumnNameSponsorID(downline, data);
                    break;
                case "EnrollerID":
                    result = ColumnNameEnrollerID(downline, data);
                    break;
                default:
                    result = value.ToInt().ToString();
                    break;
            }

            return result;
        }

        protected virtual string GetStringColumnValues(string columnName, string value)
        {
            switch (columnName)
            {
                case "PhoneNumber":
                    return value.FormatPhone(CoreContext.CurrentCultureInfo);

                default:
                    return value;
            }
        }


        public virtual string GetDecimalColumnValues(string columnName, decimal? value)
        {
            string result;

            switch (columnName)
            {
                case "PV":
                case "GV":
                case "DV":
                case "TotalCommissionsPaid":
                    // inicio 30052017=> agregado por hundred para generalizar  el formato moneda;
                       result = value.ToMoneyString(CoreContext.CurrentCultureInfo);
                    // fin 30052017=> agregado por hundred para generalizar  el formato moneda;
                    //result = value.ToMoneyString();
                    break;
                default:
                    // inicio 30052017=> agregado por hundred para generalizar  el formato moneda;
                    //var X = TruncateDoubleInsertCommas(value);;
                    result = value.Value.ToString("N",CoreContext.CurrentCultureInfo);
                    // fin 30052017=> agregado por hundred para generalizar  el formato moneda;
                    //result = TruncateDoubleInsertCommas(value);
                    break;
            }

            return result;
        }

        public virtual string TruncateDoubleInsertCommas(decimal? value)
        {
            return Convert.ToDouble(value.Value).TruncateDoubleInsertCommas();
        }

        public virtual string GetShortColumnValues(string columnName, short? value)
        {
            string result;

            switch (columnName)
            {
                case "AccountTypeID":
                    result = GetTermAccountType(value);
                    break;
                default:
                    result = value.ToShort().ToString();
                    break;
            }

            return result;
        }

        public virtual string GetDateTimeColumnValues(string columnName, DateTime? value)
        {
            string result;

            switch (columnName)
            {
                case "EnrollmentDate":
                case "LastOrderCommissionDateUTC":
                case "AutoshipProcessDate":
                case "EnrollmentDateUTC":
                case "RenewalDate":
                case "JoinDate":
                    result = value.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo);
                    break;
                default:
                    result = value.ToStringDisplay(CoreContext.CurrentCultureInfo);
                    break;
            }

            return result;
        }

        public virtual string DisplayNA()
        {
            return Translation.GetTerm("N/A");
        }

        public virtual string GetTermAccountType(short? value)
        {
            return SmallCollectionCache.Instance.AccountTypes.GetById(value.ToShort()).GetTerm();
        }

        /// <summary>
        /// Ensures that the specified dictionary contains the given key
        /// </summary>
        private bool IsKeyPresentInDictionary<T, V>(Dictionary<T, V> dict, T key)
        {
            return dict.ContainsKey(key);
        }

        #endregion
    }
}