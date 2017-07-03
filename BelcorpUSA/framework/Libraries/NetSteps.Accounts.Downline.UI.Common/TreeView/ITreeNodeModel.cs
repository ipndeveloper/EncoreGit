using System.Collections.Generic;

namespace NetSteps.Accounts.Downline.UI.Common.TreeView
{
	public interface ITreeNodeModel
	{
		int Id { get; set; }
		int? ParentId { get; set; }
		string Text { get; set; }
		string HoverText { get; set; }
		string NodeClass { get; set; }
		IDictionary<string, object> LinkHtmlAttributes { get; set; }
		ICollection<ITreeNodeModel> ChildNodes { get; set; }
	}
}
