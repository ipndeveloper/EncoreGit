using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using NetSteps.Objects.Business;
using NetSteps.Common.Cache;
using NetSteps.Common;
using NetSteps.Common.Extensions;

namespace NetSteps.Web.Extensions
{
    public static class DropDownListExtensions
    {
        #region Fill Types Methods
        public static void FillOrderStatuses(this DropDownList dl)
        {
            dl.FillOrderStatuses(string.Empty, string.Empty);
        }
        public static void FillOrderStatuses(this DropDownList dl, string blankItemText, string blankItemValue)
        {
            dl.Items.Clear();
            ListItem li;

            if (!string.IsNullOrEmpty(blankItemText))
            {
                li = new ListItem(blankItemText, blankItemValue);
                dl.Items.Add(li);
            }

            foreach (var status in SmallCollectionCache.OrderStatuses)
            {
                li = new ListItem(status.Name, status.OrderStatusId.ToString());
                dl.Items.Add(li);
            }
        }

        public static void FillOrderTypes(this DropDownList dl)
        {
            dl.FillOrderTypes(string.Empty, string.Empty);
        }
        public static void FillOrderTypes(this DropDownList dl, string blankItemText, string blankItemValue)
        {
            dl.Items.Clear();
            ListItem li;

            if (!string.IsNullOrEmpty(blankItemText))
            {
                li = new ListItem(blankItemText, blankItemValue);
                dl.Items.Add(li);
            }

            foreach (var status in SmallCollectionCache.OrderTypes)
            {
                li = new ListItem(status.Name, status.OrderTypeId.ToString());
                dl.Items.Add(li);
            }
        }

        public static void FillAccountTypes(this DropDownList dl)
        {
            dl.FillAccountTypes(string.Empty, string.Empty);
        }
        public static void FillAccountTypes(this DropDownList dl, string blankItemText, string blankItemValue)
        {
            dl.Items.Clear();
            ListItem li;

            if (!string.IsNullOrEmpty(blankItemText))
            {
                li = new ListItem(blankItemText, blankItemValue);
                dl.Items.Add(li);
            }

            foreach (var status in SmallCollectionCache.AccountTypes)
            {
                li = new ListItem(status.Name, status.AccountTypeId.ToString());
                dl.Items.Add(li);
            }
        }

        public static void FillAccountStatuses(this DropDownList dl)
        {
            dl.FillAccountStatuses(string.Empty, string.Empty);
        }
        public static void FillAccountStatuses(this DropDownList dl, string blankItemText, string blankItemValue)
        {
            dl.Items.Clear();
            ListItem li;

            if (!string.IsNullOrEmpty(blankItemText))
            {
                li = new ListItem(blankItemText, blankItemValue);
                dl.Items.Add(li);
            }

            foreach (var status in SmallCollectionCache.AccountStatuses)
            {
                li = new ListItem(status.Name, status.AccountStatusId.ToString());
                dl.Items.Add(li);
            }
        }

        public static void FillAccountStatusChangeReasons(this DropDownList dl)
        {
            dl.FillAccountStatusChangeReasons(string.Empty, string.Empty);
        }
        public static void FillAccountStatusChangeReasons(this DropDownList dl, string blankItemText, string blankItemValue)
        {
            dl.Items.Clear();
            ListItem li;

            if (!string.IsNullOrEmpty(blankItemText))
            {
                li = new ListItem(blankItemText, blankItemValue);
                dl.Items.Add(li);
            }

            foreach (var status in SmallCollectionCache.AccountStatusChangeReasons)
            {
                li = new ListItem(status.Name, status.AccountStatusChangeId.ToString());
                dl.Items.Add(li);
            }
        }

        public static void FillCompOrderReasons(this DropDownList dl)
        {
            dl.FillCompOrderReasons(string.Empty, string.Empty);
        }
        public static void FillCompOrderReasons(this DropDownList dl, string blankItemText, string blankItemValue)
        {
            dl.Items.Clear();
            ListItem li;

            if (!string.IsNullOrEmpty(blankItemText))
            {
                li = new ListItem(blankItemText, blankItemValue);
                dl.Items.Add(li);
            }

            foreach (var compOrderReason in SmallCollectionCache.CompOrderReasons)
            {
                li = new ListItem(compOrderReason.Description, compOrderReason.CompReasonID.ToString());
                dl.Items.Add(li);
            }
        }

        public static void FillOverrideReasons(this DropDownList dl)
        {
            dl.FillOverrideReasons(string.Empty, string.Empty);
        }
        public static void FillOverrideReasons(this DropDownList dl, string blankItemText, string blankItemValue)
        {
            dl.Items.Clear();
            ListItem li;

            if (!string.IsNullOrEmpty(blankItemText))
            {
                li = new ListItem(blankItemText, blankItemValue);
                dl.Items.Add(li);
            }

            foreach (var overrideReason in SmallCollectionCache.OverrideReasons)
            {
                li = new ListItem(overrideReason.Description, overrideReason.Id.ToString());
                dl.Items.Add(li);
            }
        }

        public static void FillCommissionPeriods(this DropDownList dl)
        {
            dl.FillCommissionPeriods(string.Empty, string.Empty);
        }
        public static void FillCommissionPeriods(this DropDownList dl, string blankItemText, string blankItemValue)
        {
            dl.Items.Clear();
            ListItem li;

            if (!string.IsNullOrEmpty(blankItemText))
            {
                li = new ListItem(blankItemText, blankItemValue);
                dl.Items.Add(li);
            }

            foreach (var periodCollection in SmallCollectionCache.CommisionPeriods)
            {
                li = new ListItem(periodCollection.PeriodID.ToString(), periodCollection.PeriodID.ToString());
                dl.Items.Add(li);
            }
        }

        public static void FillCommissionTitles(this DropDownList dl)
        {
            dl.FillCommissionTitles(string.Empty, string.Empty);
        }
        public static void FillCommissionTitles(this DropDownList dl, string blankItemText, string blankItemValue)
        {
            dl.Items.Clear();
            ListItem li;

            if (!string.IsNullOrEmpty(blankItemText))
            {
                li = new ListItem(blankItemText, blankItemValue);
                dl.Items.Add(li);
            }

            foreach (var title in SmallCollectionCache.CommisionTitles)
            {
                li = new ListItem(title.TitleCode, title.TitleID.ToString());
                dl.Items.Add(li);
            }
        }

        // TODO: We need to add support for more countries than US - JHE
        public static void FillStates(this DropDownList dl, int countryId)
        {
            dl.FillStates(countryId, string.Empty, string.Empty);
        }
        // TODO: We need to add support for more countries than US - JHE
        public static void FillStates(this DropDownList dl, int countryId, string blankItemText, string blankItemValue)
        {
            dl.Items.Clear();
            ListItem li;

            if (!string.IsNullOrEmpty(blankItemText))
            {
                li = new ListItem(blankItemText, blankItemValue);
                dl.Items.Add(li);
            }

            // This will eliminate the duplicated from the DB - JHE
            NetSteps.Objects.Business.StateProvinceCollection states = SmallCollectionCache.StateProvinces[countryId];
            List<StateProvince> distinctStates = states.Distinct((x, y) => x.State == y.State).ToList();

            foreach (var title in distinctStates)
            {
                li = new ListItem(title.State, title.State.ToString());
                dl.Items.Add(li);
            }
        }



        public static void FillLanguages(this DropDownList dl)
        {
            dl.FillLanguages(string.Empty, string.Empty);
        }
        public static void FillLanguages(this DropDownList dl, string blankItemText, string blankItemValue)
        {
            dl.Items.Clear();
            ListItem li;

            if (!string.IsNullOrEmpty(blankItemText))
            {
                li = new ListItem(blankItemText, blankItemValue);
                dl.Items.Add(li);
            }

            foreach (var title in SmallCollectionCache.Languages)
            {
                li = new ListItem(title.Name, title.LanguageId.ToString());
                dl.Items.Add(li);
            }
        }


        public static void FillSiteStatuses(this DropDownList dl)
        {
            dl.FillSiteStatuses(string.Empty, string.Empty);
        }
        public static void FillSiteStatuses(this DropDownList dl, string blankItemText, string blankItemValue)
        {
            dl.Items.Clear();
            ListItem li;

            if (!string.IsNullOrEmpty(blankItemText))
            {
                li = new ListItem(blankItemText, blankItemValue);
                dl.Items.Add(li);
            }

            foreach (Constants.SiteStatus status in Enum.GetValues(typeof(Constants.SiteStatus)))
            {
                string statusName = status.ToString().Replace("_", " ").ToTitleCase();

                li = new ListItem(statusName, ((int)status).ToString());
                dl.Items.Add(li);
            }
        }

        public static void FillAutoshipSchedules(this DropDownList dl)
        {
            dl.FillAutoshipSchedules(string.Empty, string.Empty);
        }
        public static void FillAutoshipSchedules(this DropDownList dl, string blankItemText, string blankItemValue)
        {
            dl.Items.Clear();
            ListItem li;

            if (!string.IsNullOrEmpty(blankItemText))
            {
                li = new ListItem(blankItemText, blankItemValue);
                dl.Items.Add(li);
            }

            foreach (var periodCollection in SmallCollectionCache.AutoshipSchedules)
            {
                li = new ListItem(periodCollection.Name.ToString(), periodCollection.AutoshipScheduleId.ToString());
                dl.Items.Add(li);
            }
        }


        public static void FillProductTypes(this DropDownList dl)
        {
            dl.FillAccountStatuses(string.Empty, string.Empty);
        }
        public static void FillProductTypes(this DropDownList dl, string blankItemText, string blankItemValue)
        {
            dl.Items.Clear();
            ListItem li;

            if (!string.IsNullOrEmpty(blankItemText))
            {
                li = new ListItem(blankItemText, blankItemValue);
                dl.Items.Add(li);
            }

            foreach (var status in SmallCollectionCache.ProductTypes)
            {
                li = new ListItem(status.Name, status.Id.ToString());
                dl.Items.Add(li);
            }
        }

        #endregion
    }
}
