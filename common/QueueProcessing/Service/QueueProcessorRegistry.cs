using NetSteps.Encore.Core;
using NetSteps.Encore.Core.IoC;
using NetSteps.QueueProcessing.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace NetSteps.QueueProcessing.Service
{
    [ContainerRegister(typeof(IQueueProcessorRegistry), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public class QueueProcessorRegistry : IQueueProcessorRegistry
    {
        private ConcurrentDictionary<string, Type> _registry = new ConcurrentDictionary<string, Type>();

        public IEnumerable<Type> AllProcessorsTypes
        {
            get
            {
                return Extensions.ToReadOnly<Type>(this._registry.Values);
            }
        }

        public QueueProcessorRegistry()
        {
        }

        public Type GetRegisteredProcessorType(string name)
        {
            Type type;
            Contract.Requires<ArgumentNullException>(name != null, "name != null");
            Contract.Requires<ArgumentException>(name.Length > 0, "name.Length > 0");
            this._registry.TryGetValue(name, out type);
            return type;
        }

        public void Register<P>(string name)
        where P : IQueueProcessor
        {
            Contract.Requires<ArgumentNullException>(name != null, "name != null");
            Contract.Requires<ArgumentException>(name.Length > 0, "name.Length > 0");
            if (!this._registry.TryAdd(name, typeof(P)))
            {
                throw new InvalidOperationException("already registered");
            }
        }

        public bool Unregister(string name)
        {
            Type type;
            Contract.Requires<ArgumentNullException>(name != null, "name != null");
            Contract.Requires<ArgumentException>(name.Length > 0, "name.Length > 0");
            return this._registry.TryRemove(name, out type);
        }
    }
}