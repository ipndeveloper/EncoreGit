using System;
using System.Diagnostics.Contracts;

namespace NetSteps.Encore.Core.IoC
{
    /// <summary>
    /// Intermediate class used by the framework to capture 
    /// a newly created object and initialize it from data provided
    /// by another object.
    /// </summary>
    /// <typeparam name="T">object type T</typeparam>
    public sealed class Initialize<T>
    {
        IContainer _container;
        T _instance;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="it"></param>
        internal Initialize(IContainer c, T it)
        {
            Contract.Requires<ArgumentNullException>(c != null);
            Contract.Requires<ArgumentNullException>(it != null);

            _container = c;
            _instance = it;
        }

        /// <summary>
        /// Initializes the newly created instance from values given.
        /// </summary>
        /// <typeparam name="TInit"></typeparam>
        /// <param name="init"></param>
        /// <returns></returns>
        public T Init<TInit>(TInit init)
        {
            Contract.Requires<ArgumentNullException>(init != null);

            ICopier<TInit, T> copier = _container.New<ICopier<TInit, T>>();
            copier.CopyTo(_instance, init, CopyKind.Loose, _container);
            _container.NotifyObserversOfCreationEvent<T>(typeof(T), _instance, null, CreationEventKind.Initialized);
            return _instance;
        }

        /// <summary>
        /// The newly created instance.
        /// </summary>
        public T Instance { get { return _instance; } }
    }
}
