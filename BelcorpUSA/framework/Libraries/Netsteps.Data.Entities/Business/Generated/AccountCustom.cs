namespace NetSteps.Data.Entities
{
    public partial class Account
    {
        public string TaxNumberCustom
        {
            set
            {
                if (_taxNumber != value)
                {
                    ChangeTracker.RecordOriginalValue("TaxNumber", _taxNumber);
                    _taxNumber = value;
                    TaxNumberChanged();
                    OnPropertyChanged("TaxNumber");
                }
            }
        }
    }
}
