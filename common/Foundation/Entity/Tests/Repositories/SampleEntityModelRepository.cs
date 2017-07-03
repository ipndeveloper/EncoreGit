using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using NetSteps.Foundation.Entity.Mocks;

namespace NetSteps.Foundation.Entity.Tests
{
    public class SampleEntityModelRepository : EntityModelRepository<SampleEntity, SampleModel, SampleContext>
    {
        public override Expression<Func<SampleEntity, bool>> GetPredicateForModel(SampleModel model)
        {
            return x => x.Id == model.Id;
        }

        public override void UpdateEntity(SampleEntity entity, SampleModel model)
        {
            entity.EntityName = model.ModelName;
        }

        public override void UpdateModel(SampleModel model, SampleEntity entity)
        {
            model.Id = entity.Id;
            model.ModelName = entity.EntityName;
        }
    }

    public class SampleEntity
    {
        public int Id { get; set; }
        public string EntityName { get; set; }
    }

    public class SampleModel
    {
        public int Id { get; set; }
        public string ModelName { get; set; }
    }

    public class SampleContext : MockDbContext
    {
        public SampleContext() : this(new SampleDatabase()) { }

        public SampleContext(SampleDatabase database)
        {
            Contract.Requires<ArgumentNullException>(database != null);

            _database = database;
            _sampleEntities = new MockDbSet<SampleEntity>(_database.SampleEntities, x => x.Id);
        }

        private readonly SampleDatabase _database;

        private readonly IDbSet<SampleEntity> _sampleEntities;
        public IDbSet<SampleEntity> SampleEntities { get { return _sampleEntities; } }
    }

    public class SampleDatabase
    {
        public HashSet<SampleEntity> SampleEntities = new HashSet<SampleEntity>();
    }
}
