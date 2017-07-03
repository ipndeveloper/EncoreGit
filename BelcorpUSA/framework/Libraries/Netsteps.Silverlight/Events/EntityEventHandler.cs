using System;
using System.Windows.Controls;

namespace NetSteps.Silverlight
{
    /// <summary>
    /// Class to pass back a specific object when an event occurred with that object. - JHE
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void EntityEventHandler<T>(object sender, EntityEventHandlerArgs<T> e);

    public class EntityEventHandlerArgs<T> : EventArgs
    {
        /// <summary>
        /// Set sender (optionally to interact with the originating control) - JHE
        /// </summary>
        public UserControl Sender { get; set; }
        public T Entity { get; set; }

        public EntityEventHandlerArgs()
        {
        }

        public EntityEventHandlerArgs(T entity)
        {
            Entity = entity;
        }

        public EntityEventHandlerArgs(UserControl sender, T entity)
        {
            Sender = sender;
            Entity = entity;
        }
    }
}
