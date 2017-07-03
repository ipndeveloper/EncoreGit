using System;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
	[Serializable]
	public struct ArchiveSearchData
	{
		[TermName("ID")]
		[Display(AutoGenerateField = false)]
		public int ArchiveID { get; set; }

		[TermName("Name")]
		public string Name { get; set; }

		//		//[Display(AutoGenerateField = false)]
		//public int ArchiveCategoryID { get; set; }

		//		//[Display(Name = "Category")]
		//public string ArchiveCategory { get; set; }

		[TermName("StartDate", "Start Date")]
		public DateTime? StartDate { get; set; }

		[TermName("EndDate", "End Date")]
		public DateTime? EndDate { get; set; }

		[TermName("Status")]
		[Display(Name = "Status")]
		public bool Active { get; set; }

		[TermName("Path")]
		public string ArchivePath { get; set; }

		[TermName("Thumbnail")]
		public string ArchiveImage { get; set; }

        [TermName("IsDownloadable")]
        public bool IsDownloadable { get; set; }

        [TermName("IsEmailable")]
        public bool IsEmailable { get; set; }
	}
}
