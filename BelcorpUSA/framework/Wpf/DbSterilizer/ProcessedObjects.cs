using System;
using System.Collections.Generic;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities;

namespace DbSterilizer
{
	public class ProcessedObjects : Dictionary<Type, List<int>>
	{
		public bool HasBeenProcessed(Type type, int id)
		{
			if (this.ContainsKey(type) && this[type].Contains(id) && id > 0)
				return true;
			else
				return false;
		}

		public bool HasBeenProcessed(IObjectWithChangeTracker obj)
		{
			Type type = obj.GetType();
			int id = (obj as IListValue).ID;
			if (this.ContainsKey(type) && this[type].Contains(id) && id > 0)
				return true;
			else
				return false;
		}

		public void Processed(IObjectWithChangeTracker obj)
		{
			Type type = obj.GetType();

			if (!this.ContainsKey(type))
				this.Add(type, new List<int>());

			this[type].Add((obj as IListValue).ID);
		}
	}
}
