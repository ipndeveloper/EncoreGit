using NetSteps.Data.Entities.Business;
namespace nsCore.Areas.Support.Models.SupportLevel
{
    public class EditLevelTreeModel
    {
        public string Levels { get; set; }
        public string SaveURL { get; set; }
        public string DeleteURL { get; set; }
        public string GetURL { get; set; }
        public string MoveURL { get; set; }
        public SupportLevelSearchData LevelTree { get; set; }
        public string levels { get; set; }
        public int? levelId { get; set; }
    }
}