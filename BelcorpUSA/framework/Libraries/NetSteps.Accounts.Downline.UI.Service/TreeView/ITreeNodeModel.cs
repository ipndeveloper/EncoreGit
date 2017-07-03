using System.Collections.Generic;
using NetSteps.Accounts.Downline.UI.Common.TreeView;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Accounts.Downline.UI.Service.TreeView
{
	[ContainerRegister(typeof(ITreeNodeModel), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Default)]
	public class TreeNodeModel : ITreeNodeModel
	{
		public virtual int Id { get; set; }
		public virtual int? ParentId { get; set; }
		public virtual string Text { get; set; }
		public virtual string HoverText { get; set; }
		public virtual string NodeClass { get; set; }
		public virtual IDictionary<string, object> LinkHtmlAttributes { get; set; }

		protected ICollection<ITreeNodeModel> _childNodes;
		public virtual ICollection<ITreeNodeModel> ChildNodes
		{
			get
			{
				return _childNodes ?? (_childNodes = new List<ITreeNodeModel>());
			}
			set
			{
				_childNodes = value;
			}
		}
	}
}
