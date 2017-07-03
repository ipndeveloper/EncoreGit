using NetSteps.Repository.Common.Interfaces;

namespace NetSteps.Repository.Common.Examples
{
    public class Animal : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOfLegs { get; set; }
    }
}