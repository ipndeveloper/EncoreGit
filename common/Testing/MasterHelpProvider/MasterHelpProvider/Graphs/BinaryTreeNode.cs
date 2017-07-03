using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestMasterHelpProvider.Graphs
{
	public class BinaryTreeNode<T> : Node<T>
	{
		#region Fields

		private const int LeftIndex = 0;
		private const int RightIndex = 1;
		private const int MaxChildNodeCount = 2;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the Left node of the binary tree node.
		/// </summary>
		public BinaryTreeNode<T> Left
		{
			get 
			{
				BinaryTreeNode<T> left = null;

				if (base.Neighbors != null)
				{
					left = base.Neighbors[LeftIndex] as BinaryTreeNode<T>;
				}

				return left;
			}
			set 
			{
				if (base.Neighbors == null)
				{
					base.Neighbors = new NodeList<T>(MaxChildNodeCount);
				}

				base.Neighbors[LeftIndex] = value;
			}
		}

		/// <summary>
		/// Gets or sets the Right node of the binary tree node.
		/// </summary>
		public BinaryTreeNode<T> Right
		{
			get
			{
				BinaryTreeNode<T> right = null;

				if (base.Neighbors != null)
				{
					right = base.Neighbors[RightIndex] as BinaryTreeNode<T>;
				}

				return right;
			}
			set
			{
				if (base.Neighbors == null)
				{
					base.Neighbors = new NodeList<T>(MaxChildNodeCount);
				}

				base.Neighbors[RightIndex] = value;
			}
		}

		#endregion
	}
}
