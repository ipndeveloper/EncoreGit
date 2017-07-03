using System.Collections.Generic;
using System.Web.Mvc;

namespace NetSteps.Web.Mvc
{

    public class ExtendedSelectListItem<TKey, TValue> : SelectListItem
    {
        #region Properties
        private KeyValuePair<TKey, TValue> selectItem = new KeyValuePair<TKey, TValue>();

        public int? SelectedID { get; set; }
        public string Title { get; set; }

        public KeyValuePair<TKey, TValue> SelectItem
        {
            get { return selectItem; }
            set
            {
                selectItem = value;
                string[] stringArr = selectItem.Value.ToString().Split(',');
                Text = stringArr[0];
                Value = selectItem.Key.ToString();
                Selected = (selectItem.Key.ToString() == (SelectedID != null ? SelectedID.ToString() : ""));
                if (stringArr.Length > 1)
                    Title = stringArr[1];
            }
        }
        #endregion
    }
}
