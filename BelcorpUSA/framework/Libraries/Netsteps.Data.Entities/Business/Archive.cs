using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
namespace NetSteps.Data.Entities
{
	public partial class Archive : IValidatedModel
	{
		#region Methods
		public static List<Archive> GetRecent100()
		{
			return Repository.GetRecent100();
		}

		public static PaginatedList<ArchiveSearchData> Search(ArchiveSearchParameters searchParams)
		{
			try
			{
				return Repository.Search(searchParams);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static List<Archive> LoadAllFull()
		{
			try
			{
				return Repository.LoadAllFull();
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static List<Archive> LoadAllFullBySiteID(int siteID)
		{
			try
			{
				return Repository.LoadAllFullBySiteID(siteID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

        public string GetArchiveName()
        {
            return !Translations.Any(t => t.LanguageID == ApplicationContext.Instance.CurrentLanguageID) ? Translations.Count() > 0 ? Translations.FirstOrDefault().Name : "" : Translations.FirstOrDefault(t => t.LanguageID == ApplicationContext.Instance.CurrentLanguageID).Name;
        }

		//public static List<Archive> LoadBatch(List<int> ids)
		//{
		//    try
		//    {
		//        return Repository.LoadBatch(ids);
		//    }
		//    catch (Exception ex)
		//    {
		//        throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
		//    }
		//}
		#endregion
	}

	/// <summary>
	/// TODO: Research putting this in another assembly - JHE
	/// http://www.paraesthesia.com/archive/2010/01/28/separating-metadata-classes-from-model-classes-in-dataannotations-using-custom.aspx
	/// </summary>
	public partial class ArchiveMetaData
	{
		[Required]
		[Display(Name = "Name:")]
		[StringLength(255)]
		public string Name { get; set; }

		[Display(Name = "Description:", Description = "Please enter a short description.")]
		[StringLength(255)]
		public string Description { get; set; }

		[Required]
		[Display(Name = "Archive Date:")]
		public Nullable<System.DateTime> ArchiveDate { get; set; }

		[Required]
		[Display(Name = "Active:")]
		public Nullable<bool> Active { get; set; }
	}

}
