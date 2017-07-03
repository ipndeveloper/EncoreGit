using NetSteps.Data.Entities.Business.Interfaces;

namespace NetSteps.Data.Entities
{
    public partial class LocalizedKind
    {
        int ILanguageID.LanguageID { get { return LanguageId; } set { LanguageId = value; } }
    }
}
