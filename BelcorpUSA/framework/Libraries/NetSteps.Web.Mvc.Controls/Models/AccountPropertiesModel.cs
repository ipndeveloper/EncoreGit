using System.Collections.Generic;
using NetSteps.Data.Entities;

namespace NetSteps.Web.Mvc.Controls.Models
{
    public class AccountPropertiesModel
    {
        public List<AccountPropertyType> AccountPropertyTypes { get; set; }
        public List<AccountProperty> AccountProperties { get; set; }
		public bool IsAvataxEnabled { get; set; }
		public string AvataxPropertyTypeName { get; set; }
    }
}