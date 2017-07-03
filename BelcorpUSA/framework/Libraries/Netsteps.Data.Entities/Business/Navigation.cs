using System;
using System.Collections.Generic;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities
{
	public partial class Navigation
	{
		#region Properties
		#endregion

		#region Methods
		public static List<Navigation> LoadSingleLevelNav(int siteId, int navigationTypeId, int? parentId, bool showAll = true)
		{
			try
			{
				var list = Repository.LoadSingleLevelNav(siteId, navigationTypeId, parentId, showAll);
				foreach (var item in list)
				{
					item.StartTracking();
					item.IsLazyLoadingEnabled = true;
				}
				return list;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public string GetLinkText(int languageID)
		{
			if (this.Translations != null)
			{
				var pageTranslation = this.Translations.GetByLanguageID(languageID);
				if (pageTranslation != null)
					return pageTranslation.LinkText;
				else
				{
                    pageTranslation = this.Translations.GetByLanguageIdOrDefaultForDisplay();
					if (pageTranslation != null)
						return pageTranslation.LinkText;
					else
						return string.Empty;
				}
			}
			else
				return string.Empty;
		}
		#endregion
	}
}
