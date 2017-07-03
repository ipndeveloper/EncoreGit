using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Promotions.Plugins.Common.Qualifications.Concrete
{
	[Serializable]
	public class AccountHasTitleQualification : IAccountHasTitleQualificationExtension
	{
		public static class PropertyNames
		{
			public const string Title = "Title";
			public const string TitleID = "TitleID";
			public const string AccountID = "AccountID";
		}

		public AccountHasTitleQualification()
		{
			AllowedTitles = new List<IAccountTitleOption>();
		}

		public IList<IAccountTitleOption> AllowedTitles { get; private set; }

		public IEnumerable<string> AssociatedPropertyNames
		{
			get { return new string[] { PropertyNames.TitleID, PropertyNames.AccountID }; }
		}

		public string ExtensionProviderKey
		{
			get { return QualificationExtensionProviderKeys.AccountHasTitleProviderKey; }
		}

		public bool ValidFor<TProperty>(string propertyName, TProperty value)
		{
			var handler = Create.New<IAccountHasTitleQualificationHandler>();
			return handler.ValidFor<TProperty>(this, propertyName, value);
		}

		public int PromotionQualificationID { get; set; }
	}
}
