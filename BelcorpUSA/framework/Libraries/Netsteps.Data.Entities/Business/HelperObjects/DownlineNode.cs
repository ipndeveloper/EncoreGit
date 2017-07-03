using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Extensions;

namespace NetSteps.Data.Entities.Business
{
	[Serializable]
	public class DownlineNode
	{
		public int AccountID { get; set; }

		public int? ParentID { get; set; }

		public int Level { get; set; }

		public int TotalDownlineCount { get { return FlatChildren.Count; } }

		public DownlineNode Parent { get; set; }

		public int? AccountTypeID { get; set; }

		public int? AccountStatusID { get; set; }

		private bool? _hasChildren = null;
		public bool? HasChildren
		{
			get
			{
				if (_hasChildren == null)
				{
					if(_children != null)
					{
						_hasChildren = Children.Any();
					}
					else
					{
						_hasChildren = false;
					}
				}
				return _hasChildren;
			}
		}

		private IEnumerable<DownlineNode> _children = null;
		public IEnumerable<DownlineNode> Children
		{
			get
			{
				return _children;
			}
			set
			{
				_children = value;
				_hasChildren = null;
			}
		}

		private List<DownlineNode> _flatChildren = new List<DownlineNode>();
		public List<DownlineNode> FlatChildren
		{
			get
			{
				return _flatChildren;
			}
		}

		private ConcurrentDictionary<int, int> _treeVisits;
		public void VisitTree(Action<IEnumerable<DownlineNode>> operation, DownlineNode parent = null)
		{
			if (parent == null)
			{
				_treeVisits = new ConcurrentDictionary<int, int>();
				parent = this;
			}
			if (parent.Children != null && parent.Children.Any())
			{
				var visitableChildren = parent.Children.Where(c => !_treeVisits.ContainsKey(c.AccountID)).ToList();
				operation(visitableChildren);

				foreach(var item in visitableChildren.Select(c => c.AccountID))
				{
					_treeVisits.Add(item, item);
				}

				foreach(var child in visitableChildren)
				{
					VisitTree(operation, child);
				}
			}
		}

		/// <summary>
		/// stopAtAccountID is optional for use in 'virtual' trees (subsets of the full tree) - JHE
		/// </summary>
		/// <param name="parentIds"></param>
		/// <param name="node"></param>
		/// <param name="stopAtAccountID"></param>
		/// <returns></returns>
		public static Dictionary<int, int> GetParentIDsRecursive(DownlineNode node, int? stopAtAccountID = null, Dictionary<int, int> parentIds = null)
		{
			if (parentIds == null)
				parentIds = new Dictionary<int, int>();

			if (node.Parent != null)
			{
				if (stopAtAccountID != null && stopAtAccountID.ToInt() == node.Parent.AccountID)
				{
					parentIds.Add(node.ParentID.ToInt(), node.ParentID.ToInt());
				}
				else
				{
					parentIds.Add(node.ParentID.ToInt(), node.ParentID.ToInt());
					GetParentIDsRecursive(node.Parent, stopAtAccountID, parentIds);
				}
			}
			return parentIds;
		}

		public static Dictionary<int, int> GetChildIDsRecursive(DownlineNode node, Dictionary<int, int> childIds = null)
		{
			if (childIds == null)
				childIds = new Dictionary<int, int>();

			if (node.Children != null)
			{
				foreach (var child in node.Children)
				{
					childIds.Add(child.AccountID, child.AccountID);
					GetChildIDsRecursive(child, childIds);
				}
			}
			return childIds;
		}
	}
}