using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestMasterHelpProvider.Graphs
{
	public class GraphNode<T> : Node<T>
	{
		#region Fields

		private IList<double> _edgeWeights;

		#endregion

		#region Properties

		/// <summary>
		/// Gets the GraphNode's neighbors.
		/// </summary>
		public new NodeList<T> Neighbors
		{
			get
			{
				if (base.Neighbors == null)
				{
					base.Neighbors = new NodeList<T>();
				}

				return base.Neighbors;
			}
		}

		/// <summary>
		/// Gets the GraphNode's edge weights.
		/// </summary>
		public IList<double> EdgeWeights
		{
			get { return _edgeWeights; }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Creates an instance of GraphNode.
		/// </summary>
		public GraphNode()
			: base()
		{
			_edgeWeights = new List<double>();
		}

		/// <summary>
		/// Creates an instance of GraphNode.
		/// </summary>
		/// <param name="value"></param>
		public GraphNode(T value)
			: base(value)
		{
			_edgeWeights = new List<double>();
		}

		/// <summary>
		/// Creates an instance of GraphNode.
		/// </summary>
		/// <param name="neighbors"></param>
		/// <param name="value"></param>
		public GraphNode(T value, NodeList<T> neighbors)
			: base(value, neighbors)
		{
			_edgeWeights = new List<double>();
		}

		/// <summary>
		/// Determines whether this GraphNode equals another object.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			bool result = false;

			if (obj.GetType() == this.GetType())
			{
				if (((GraphNode<T>)obj).Value.Equals(this.Value))
				{
					result = true;
				}
			}

			return result;
		}

		/// <summary>
		/// Gets this GraphNode's hash code.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		#endregion
	}
}
