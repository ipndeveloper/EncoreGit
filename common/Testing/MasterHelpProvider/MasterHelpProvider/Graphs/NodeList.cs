using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TestMasterHelpProvider.Graphs
{
	public class NodeList<T> : Collection<Node<T>>
	{
		#region Constructors

		/// <summary>
		/// Creates an instance of NodeList.
		/// </summary>
		public NodeList()
			: base()
		{
			// Left intentionally empty.
		}

		/// <summary>
		/// Creates an instance of NodeList.
		/// </summary>
		/// <param name="initialSize"></param>
		public NodeList(int initialSize)
			: base()
		{
			Fill(default(Node<T>), initialSize);
		}

		#endregion

		#region Methods

		/// <summary>
		/// Fills the NodeList with a particular node a certain number of times.
		/// </summary>
		/// <param name="node"></param>
		/// <param name="numEntries"></param>
		public void Fill(Node<T> node, int numEntries)
		{
			for (int entryCounter = 0; entryCounter < numEntries; entryCounter++)
			{
				Items.Add(node);
			}
		}

		/// <summary>
		/// Finds a node in the NodeList by its value.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public Node<T> FindByValue(T value)
		{
			Node<T> result = default(Node<T>);

			foreach (Node<T> node in Items)
			{
				if (node.Value.Equals(value))
				{
					result = node;
					break;
				}
			}

			return result;
		}

		#endregion
	}
}
