

namespace NetSteps.Testing.Integration
{
    public static class OrderType
    {
        public enum ID
        {
            Autoship,
            Comp,
            Employee,
            Enrollment,
            Online,
            Override,
            Party,
            Portal,
            Replacement,
            Return,
            Workstation
        }

        public static string ToPattern(this OrderType.ID orderTypeId)
        {
           return string.Format("{0} Order", orderTypeId);
        }
    }
}
