using System.ComponentModel;

namespace NetSteps.Common.Interfaces
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Interface for an Event to fire when a new property value has been set
    /// but before the value has been set to the property. Gives an opportunity to do logic injection.
    /// Created: 03-05-2010
    /// </summary>
    public interface INotifyPropertyChanging
    {
        /// <summary>
        /// Occurs before a property value changes.
        /// </summary>
        event PropertyChangingEventHandler PropertyChanging;
    }

    public delegate void PropertyChangingEventHandler(object sender, PropertyChangingEventArgs e);

    public class PropertyChangingEventArgs : PropertyChangedEventArgs
    {
        public virtual string PropertyValue { get; set; }

        public PropertyChangingEventArgs(string propertyName, string propertyValue)
            : base(propertyName)
        {
            PropertyValue = propertyValue;
        }
    }
}
