using NetSteps.Common.Base;

namespace NetSteps.Data.Entities.Business
{
    public class NoteSearchParameters : FilterDateRangePaginatedListParameters<Note>
    {
        public int? OrderID { get; set; }

        public int? AccountID { get; set; }

        public int? UserID { get; set; }

        public int? NoteTypeID { get; set; }

        public string SearchText { get; set; }
    }
}
