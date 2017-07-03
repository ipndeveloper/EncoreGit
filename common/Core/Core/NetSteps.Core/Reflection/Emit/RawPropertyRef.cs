﻿using System;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Reflection.Emit;


namespace NetSteps.Encore.Core.Reflection.Emit
{
	internal class RawPropertyRef : IPropertyRef
	{
		PropertyInfo _prop;

		public RawPropertyRef(PropertyInfo prop)
		{
			Contract.Requires<ArgumentNullException>(prop != null, "prop cannot be null");

			_prop = prop;
		}

		public string Name
		{
			get { return _prop.Name; }
		}

		public PropertyInfo GetPropertyInfo()
		{
			return _prop;
		}

		public void LoadAddress(ILGenerator il)
		{
			throw new NotImplementedException();
		}

		public void LoadValue(ILGenerator il)
		{
			Contract.Assert(il != null);

			il.LoadProperty(_prop, false);
		}

		public void StoreValue(ILGenerator il)
		{
			Contract.Assert(il != null);

			il.StoreProperty(_prop, false);
		}

		public Type TargetType
		{
			get { return _prop.PropertyType; }
		}
	}
}
