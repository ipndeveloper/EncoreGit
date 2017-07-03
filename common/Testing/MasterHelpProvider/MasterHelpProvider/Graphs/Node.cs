using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestMasterHelpProvider.Graphs
{
	public class Node<T>
	{
		#region Fields

		private T _value;
		private NodeList<T> _neighbors;

		#endregion

		#region Properties

		/// <summary>
		/// Gets whether this Node has neighbors.
		/// </summary>
		public bool HasNeighbors
		{
			get { return (_neighbors.Count > 0); }
		}

		/// <summary>
		/// Gets or sets the value of this Node.
		/// </summary>
		public T Value
		{
			get 
			{
				if (_value == null)
				{
					_value = default(T);
				}

				return _value; 
			}
			set { _value = value; }
		}

		/// <summary>
		/// Gets or sets the neighbors of this Node.
		/// </summary>
		public NodeList<T> Neighbors
		{
			get 
			{
				if (_neighbors == null)
				{
					_neighbors = new NodeList<T>();
				}

				return _neighbors; 
			}
			set { _neighbors = value; }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Creates an instance of Node.
		/// </summary>
		public Node()
		{
			_neighbors = new NodeList<T>();
		}

		/// <summary>
		/// Creates an instance of Node.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="neighbors"></param>
		public Node(T value, NodeList<T> neighbors = null)
		{
			_value = value;

			if (neighbors == null)
			{
				_neighbors = new NodeList<T>();
			}
			else
			{
				_neighbors = neighbors;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Determines whether this Node equals another object.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			bool result = false;

			if (obj.GetType() == this.GetType())
			{
				if (((Node<T>)obj).Value.Equals(this.Value))
				{
					result = true;
				}
			}

			return result;
		}

		/// <summary>
		/// Gets this Node's hash code.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		/// <summary>
		/// Gets a string-representation of this Node.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format("{{{0}}}", _value);
		}

		#endregion
	}
}
