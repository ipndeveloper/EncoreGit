using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestMasterHelpProvider.Graphs
{
	public class Graph<T>
	{
		#region Fields

		private NodeList<T> _nodes;

		#endregion

		#region Properties

		/// <summary>
		/// Gets the node list for this Graph.
		/// </summary>
		public NodeList<T> Nodes
		{
			get { return _nodes; }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Creates an instance of Graph.
		/// </summary>
		public Graph()
		{
			_nodes = new NodeList<T>();
		}

		/// <summary>
		/// Creates an instance of Graph.
		/// </summary>
		/// <param name="nodes"></param>
		public Graph(NodeList<T> nodes)
		{
			if (nodes == null)
			{
				_nodes = new NodeList<T>();
			}
			else
			{
				_nodes = nodes;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Adds a node to this Graph.
		/// </summary>
		/// <param name="node"></param>
		public void AddNode(GraphNode<T> node)
		{
			_nodes.Add(node);
		}

		/// <summary>
		/// Adds a node to this Graph.
		/// </summary>
		/// <param name="value"></param>
		public void AddNode(T value)
		{
			_nodes.Add(new GraphNode<T>(value));
		}

		/// <summary>
		/// Adds a directed edge between two nodes.
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <param name="weight"></param>
		public void AddDirectedEdge(GraphNode<T> from, GraphNode<T> to, double weight = 0.0)
		{
			from.Neighbors.Add(to);
			from.EdgeWeights.Add(weight);
		}

		/// <summary>
		/// Adds a directed edge between two nodes.
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <param name="weight"></param>
		public void AddDirectedEdge(T from, T to, double weight = 0.0)
		{
			((GraphNode<T>)_nodes.FindByValue(from)).Neighbors.Add(_nodes.FindByValue(to));
			((GraphNode<T>)_nodes.FindByValue(from)).EdgeWeights.Add(weight);
		}

		/// <summary>
		/// Adds an undirected edge between two nodes.
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <param name="weight"></param>
		public void AddUndirectedEdge(GraphNode<T> from, GraphNode<T> to, double weight = 0.0)
		{
			from.Neighbors.Add(to);
			from.EdgeWeights.Add(weight);

			to.Neighbors.Add(from);
			to.EdgeWeights.Add(weight);
		}

		/// <summary>
		/// Adds an undirected edge between two nodes.
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <param name="weight"></param>
		public void AddUndirectedEdge(T from, T to, double weight = 0.0)
		{
			((GraphNode<T>)_nodes.FindByValue(from)).Neighbors.Add(_nodes.FindByValue(to));
			((GraphNode<T>)_nodes.FindByValue(from)).EdgeWeights.Add(weight);

			((GraphNode<T>)_nodes.FindByValue(to)).Neighbors.Add(_nodes.FindByValue(from));
			((GraphNode<T>)_nodes.FindByValue(to)).EdgeWeights.Add(weight);
		}

		/// <summary>
		/// Clears the Graph.
		/// </summary>
		public void Clear()
		{
			_nodes.Clear();
		}

		/// <summary>
		/// Determines whether this Graph contains a certain value.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool ContainsValue(T value)
		{
			bool result = false;

			if (_nodes.FindByValue(value) != null)
			{
				result = true;
			}

			return result;
		}

		/// <summary>
		/// Removes a node by its value.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool Remove(T value)
		{
			bool result = false;
			GraphNode<T> nodeToRemove = _nodes.FindByValue(value) as GraphNode<T>;

			if (nodeToRemove != null)
			{
				_nodes.Remove(nodeToRemove);

				foreach (GraphNode<T> nextNode in _nodes)
				{
					int indexOfNode = nextNode.Neighbors.IndexOf(nodeToRemove);

					if (indexOfNode != -1)
					{
						nextNode.Neighbors.RemoveAt(indexOfNode);
						nextNode.EdgeWeights.RemoveAt(indexOfNode);
					}
				}

				result = true;
			}

			return result;
		}

		/// <summary>
		/// Returns a string-representation of this Graph.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format("{{0}}", _nodes.Count);
		}

		#endregion
	}
}
