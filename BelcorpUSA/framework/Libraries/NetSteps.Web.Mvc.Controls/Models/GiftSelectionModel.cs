using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace NetSteps.Web.Mvc.Controls.Models
{
	public class GiftSelectionModel
	{
		public string GetStepInfoUrl { get; set; }
		public string SaveGiftSelectionUrl { get; set; }
		public string JavaScriptSaveCallbackFunctionName { get; set; }
		public string StepID { get; set; }
		public int MaxQuantity { get; set; }
		public List<GiftModel> AvailableOptions { get; set; }
		public List<GiftModel> SelectedOptions { get; set; }
        public bool? IsEspecialPromo { get; set; }
       
        public GiftSelectionModel(string getStepInfoUrl, string saveGiftSelectionUrl, string stepID = null, List<GiftModel> availableOptions = null, List<GiftModel> selectedOptions = null, string callbackFunctionName = null, bool? isEspecialPromo = null)
        {
            GetStepInfoUrl = getStepInfoUrl;
            SaveGiftSelectionUrl = saveGiftSelectionUrl;
            StepID = stepID;
            AvailableOptions = availableOptions ?? new List<GiftModel>();
            SelectedOptions = selectedOptions ?? new List<GiftModel>();
            JavaScriptSaveCallbackFunctionName = callbackFunctionName;
            IsEspecialPromo = isEspecialPromo;
         }

		[ContractInvariantMethod]
		void ObjectInvariant()
		{
			Contract.Invariant(!string.IsNullOrWhiteSpace(GetStepInfoUrl));
			Contract.Invariant(!string.IsNullOrWhiteSpace(SaveGiftSelectionUrl));
			Contract.Invariant(AvailableOptions != null);
			Contract.Invariant(SelectedOptions != null);
		}
	}

	public class GiftModel
	{
		public string Name { get; set; }
		public string Image { get; set; }
		public string Description { get; set; }
		public int ProductID { get; set; }
		public string Value { get; set; }
		public bool IsOutOfStock { get; set; }
	}
}
