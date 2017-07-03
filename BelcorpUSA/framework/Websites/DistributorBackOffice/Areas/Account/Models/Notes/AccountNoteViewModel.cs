using NetSteps.Data.Entities;

namespace DistributorBackOffice.Areas.Account.Models.Notes
{
    public class AccountNoteViewModel
    {
        public Note Note { get; set; }
        public bool isDisabled { get; set; }
        public int AccountID { get; set; }
    }
}