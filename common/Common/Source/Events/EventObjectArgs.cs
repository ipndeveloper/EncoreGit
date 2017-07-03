using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.WebControls.Events
{
    public delegate void EventObjectHandler<T, P>(object sender, EventObjectArgs<T, P> e);

    public class EventObjectArgs<T, P> : EventArgs
    {
        public T Entity { get; set; }
        public P ParentEntity { get; set; }

        public EventObjectArgs(T entity)
        {
            Entity = entity;
        }

        public EventObjectArgs(P parentEntity)
        {
            ParentEntity = parentEntity;
        }

        public EventObjectArgs(T entity, P parentEntity)
        {
            Entity = entity;
            ParentEntity = parentEntity;
        }
    }
}
