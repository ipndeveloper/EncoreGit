using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestMasterHelpProvider.Graphs
{
	public class BinaryTree<T>
	{
		#region Fields

		private BinaryTreeNode<T> _root;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the root node of this BinaryTree.
		/// </summary>
		public BinaryTreeNode<T> Root
		{
			get { return _root; }
			set { _root = value; }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Creates an instance of BinaryTree.
		/// </summary>
		public BinaryTree()
		{
			// Left intentionally blank.
		}

		/// <summary>
		/// Creates an instance of BinaryTree.
		/// </summary>
		/// <param name="other"></param>
		public BinaryTree(BinaryTree<T> other)
		{
			_root = other._root;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Clears this BinaryTree.
		/// </summary>
		public virtual void Clear()
		{
			_root = null;
		}

		#endregion
	}
}
