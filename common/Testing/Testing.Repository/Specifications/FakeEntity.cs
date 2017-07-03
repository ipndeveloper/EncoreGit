using NetSteps.Repository.Common.Interfaces;

namespace Testing.Repository.Specifications
{
    public class FakeEntity : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public bool IsSpecial { get; set; }
    }
}