using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Cache.AppFabric.Tests
{
	[DTO]
	public interface IMyTestClass
	{
		int IntProp { get; set; }
		string StringProp { get; set; }
	}

	[Serializable]
	public class MyTestClass : IMyTestClass
	{
		public int IntProp { get; set; }
		public string StringProp { get; set; }
	}

	[DTO]
	public interface IMyEquatableTestClass
	{
		int IntProp { get; set; }
		string StringProp { get; set; }
	}

	[Serializable]
	public class MyEquatableTestClass : IMyEquatableTestClass, IEquatable<MyEquatableTestClass>, IEquatable<IMyEquatableTestClass>
	{
		public int IntProp { get; set; }
		public string StringProp { get; set; }

		public bool Equals(MyEquatableTestClass other)
		{
			if (other != null)
			{
				if (this.IntProp == other.IntProp && this.StringProp == other.StringProp)
					return true;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj is MyEquatableTestClass)
				return this.Equals(obj as MyEquatableTestClass);
			return false;
		}

		public override int GetHashCode()
		{
			int prime = 5387;
			int result = 0;
			result ^= IntProp.GetHashCode() * prime;
			if (StringProp != null)
				result ^= StringProp.GetHashCode() * prime;

			return result;
		}

		public bool Equals(IMyEquatableTestClass other)
		{
			return this.Equals(other as MyEquatableTestClass);
		}
	}

	public class ClassTypeKey
	{
		public string Param1 { get; set; }
		public int Param2 { get; set; }
		public bool Param3 { get; set; }

		public override int GetHashCode()
		{
			int prime = 5387;
			int result = 0;
			result ^= Param1.GetHashCode() * prime;
			result ^= Param2.GetHashCode() * prime;
			result ^= Param3.GetHashCode() * prime;
			return result;
		}
	}
}
