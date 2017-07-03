using NetSteps.Data.Entities;

namespace nsCore.Models
{
	public class EditCategoryTreeModel
	{
		public Constants.CategoryType CategoryType { get; set; }
		public Category CategoryTree { get; set; }
		public int? EditingCategoryID { get; set; }
		public string SaveImageURL { get; set; }
        public string DeleteImageURL { get; set; }
		public string SaveURL { get; set; }
		public string DeleteURL { get; set; }
		public string GetCategoryURL { get; set; }
		public string MoveURL { get; set; }
		public string Links { get; set; }
		public string Categories { get; set; }
	}
}