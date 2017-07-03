using System.ComponentModel.DataAnnotations;

namespace NetSteps.Common.Validation
{
    public class StringRangeAttribute : ValidationAttribute
    {
        public int MinLength { get; set; }
        public int MaxLength { get; set; }

        public StringRangeAttribute(int minLength, int maxLength)
        {
            this.MinLength = (minLength < 0) ? 0 : minLength;
            this.MaxLength = (maxLength < 0) ? 0 : maxLength;
        }

        public override bool IsValid(object value)
        {
            //null or empty is <EM>not</EM> invalid
            var str = (string)value;
            if (string.IsNullOrEmpty(str))
                return true;

            return (str.Length >= this.MinLength && str.Length <= this.MaxLength);
        }
    }
}
