using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NetSteps.Accounts.Downline.UI.Common.Search;
using NetSteps.Accounts.Downline.UI.Common.TreeView;

namespace NetSteps.Accounts.Downline.UI.Common
{
	[ContractClass(typeof(Contracts.DownlineUIServiceContracts))]
	public interface IDownlineUIService
	{
		ITreeNodeModel GetTreeNodes(int rootAccountId, int? maxLevels = null);
		IList<ISearchDownlineResultModel> SearchDownline(int rootAccountId, string query);
		IList<string> GetTreePath(int rootAccountId, int targetAccountId);
		void LoadDownlineInfoCardModelOptions(dynamic optionsBag, string getDataUrl, string baseEmailUrl);
		void LoadDownlineInfoCardModelData(dynamic dataBag, int rootAccountId, int accountId);

        int AccountId { get; set; } //Developed by WCS - CSTI
        int paidAsTitleID { get; set; } //Developed by WCS - CSTI
        int currentTitleID { get; set; } //Developed by WCS - CSTI
        string pv { get; set; } //Developed by WCS - CSTI
        string gv { get; set; } //Developed by WCS - CSTI
        string dv { get; set; } //Developed by WCS - CSTI
	
    }

	namespace Contracts
	{
		[ContractClassFor(typeof(IDownlineUIService))]
		internal abstract class DownlineUIServiceContracts : IDownlineUIService
		{
			ITreeNodeModel IDownlineUIService.GetTreeNodes(int rootAccountId, int? maxLevels)
			{
				Contract.Requires<ArgumentOutOfRangeException>(rootAccountId > 0);
				Contract.Requires<ArgumentOutOfRangeException>(maxLevels == null || maxLevels >= 0);
				throw new NotImplementedException();
			}

			IList<ISearchDownlineResultModel> IDownlineUIService.SearchDownline(int rootAccountId, string query)
			{
				Contract.Requires<ArgumentOutOfRangeException>(rootAccountId > 0);
				Contract.Requires<ArgumentNullException>(query != null);
				Contract.Requires<ArgumentException>(query.Length > 0);
				Contract.Ensures(Contract.Result<IList<ISearchDownlineResultModel>>() != null);
				throw new NotImplementedException();
			}

			IList<string> IDownlineUIService.GetTreePath(int rootAccountId, int targetAccountId)
			{
				Contract.Requires<ArgumentOutOfRangeException>(rootAccountId > 0);
				Contract.Requires<ArgumentOutOfRangeException>(targetAccountId > 0);
				Contract.Ensures(Contract.Result<IList<string>>() != null);
				throw new NotImplementedException();
			}

			void IDownlineUIService.LoadDownlineInfoCardModelOptions(dynamic optionsBag, string getDataUrl, string baseEmailUrl)
			{
				Contract.Requires<ArgumentNullException>(getDataUrl != null);
				Contract.Requires<ArgumentException>(getDataUrl.Length > 0);
				Contract.Requires<ArgumentNullException>(baseEmailUrl != null);
				Contract.Requires<ArgumentException>(baseEmailUrl.Length > 0);
				throw new NotImplementedException();
			}

			void IDownlineUIService.LoadDownlineInfoCardModelData(dynamic dataBag, int rootAccountId, int accountId)
			{
				Contract.Requires<ArgumentOutOfRangeException>(rootAccountId > 0);
				Contract.Requires<ArgumentOutOfRangeException>(accountId > 0);
				throw new NotImplementedException();
			}

            int IDownlineUIService.AccountId { get; set; } //Developed by WCS - CSTI
            int IDownlineUIService.paidAsTitleID { get; set; } //Developed by WCS - CSTI
            int IDownlineUIService.currentTitleID { get; set; } //Developed by WCS - CSTI
            string IDownlineUIService.pv { get; set; } //Developed by WCS - CSTI
            string IDownlineUIService.gv { get; set; } //Developed by WCS - CSTI
            string IDownlineUIService.dv { get; set; } //Developed by WCS - CSTI
		}
	}
}
