using System;
using System.ComponentModel.DataAnnotations;

namespace nsDistributor.Areas.Enroll.Models.Shared
{
    public class DateModel
    {
        [JSRequireAny(new[] { "Year", "Month", "Day" })]
        //[JSDateModel]
        public virtual int? Year { get; set; }

        [JSRequireAny(new[] { "Year", "Month", "Day" })]
        //[JSDateModel]
        public virtual int? Month { get; set; }

        [JSRequireAny(new[] { "Year", "Month", "Day" })]
        //[JSDateModel]
        public virtual int? Day { get; set; }

        public virtual DateTime? Date
        {
            get
            {
                return IsValid ? (DateTime?)new DateTime(Year.Value, Month.Value, Day.Value) : null;
            }
            set
            {
                if (value == null)
                {
                    Year = Month = Day = null;
                }
                else
                {
                    Year = value.Value.Year;
                    Month = value.Value.Month;
                    Day = value.Value.Day;
                }
            }
        }

        public virtual bool IsBlank
        {
            get
            {
                return Year == null
                    && Month == null
                    && Day == null;
            }
        }

        public virtual bool IsValid
        {
            get
            {
                return IsValidYear && IsValidMonth && IsValidDay;
            }
        }

        public virtual bool IsValidYear
        {
            get
            {
                return Year != null && Year >= 1000 && Year <= DateTime.MaxValue.Year;
            }
        }

        public virtual bool IsValidMonth
        {
            get
            {
                return Month != null && Month >= 1 && Month <= 12;
            }
        }

        public virtual bool IsValidDay
        {
            get
            {
                if (IsValidYear && IsValidMonth)
                {
                    return Day != null && Day >= 1 && Day <= DateTime.DaysInMonth(Year.Value, Month.Value);
                }

                return Day != null && Day >= 1 && Day <= 31;
            }
        }
    }
}