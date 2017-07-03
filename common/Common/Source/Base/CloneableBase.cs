using System;
using System.ComponentModel;
using NetSteps.Common.Serialization;
using System.Runtime.Serialization;

namespace NetSteps.Common.Base
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Base class for business object to provide clonability via binary serialization.
    /// Created: 01-20-2009
    /// </summary>
    [Serializable]
    public class CloneableBase<T> : ICloneable
    {
        #region ICloneable
        /// <summary>
        ///  To clone and return an object of same type.
        /// </summary>
        public T Clone()
        {
            return (T)GetClone();
        }

        Object ICloneable.Clone()
        {
            return this.GetClone();
        }

        /// <summary>
        /// Creates a clone of the object.
        /// </summary>
        /// <returns>
        /// A new object containing the exact data of the original object. - JHE
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual Object GetClone()
        {
            return ObjectCloner.Clone(this);
        }
        #endregion
    }
}
