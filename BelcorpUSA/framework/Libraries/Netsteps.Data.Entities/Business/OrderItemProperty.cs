using NetSteps.Data.Common.Entities;

namespace NetSteps.Data.Entities
{
	public partial class OrderItemProperty : IOrderItemProperty
	{
		public string Name
		{
			get
			{
				string n = null;
				if (this.OrderItemPropertyType != null)
				{
					n = this.OrderItemPropertyType.Name;
				}
				return n;
			}
		}

		public string Value
		{
			get
			{
				string v = null;
				if (this.OrderItemPropertyValue != null)
				{
					v = this.OrderItemPropertyValue.Value;
				}
				else
				{
					v = this.PropertyValue;
				}
				return v;
			}
		}
	}
}
