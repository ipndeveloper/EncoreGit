using System;

namespace NetSteps.Data.Entities
{
    public partial class Order 
    {
        public Nullable<decimal> QualificationTotal
        {
            get { return _qualificationTotal; }
            set
            {
                if (_qualificationTotal != value)
                {
                    _qualificationTotal = value;
                }
            }
        }
        private Nullable<decimal> _qualificationTotal;

    }
}
