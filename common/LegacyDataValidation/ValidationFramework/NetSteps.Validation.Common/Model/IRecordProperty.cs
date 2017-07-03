using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Validation.Common.Model
{
    /// <summary>
    /// Validation for a record property
    /// </summary>
    [ContractClass(typeof(RecordPropertyValidationContract))]
    public interface IRecordProperty
    {
        /// <summary>
        /// Specifies behavior to adjust the validation handler handling of this property.
        /// </summary>
        /// <value>
        /// The type of the validation to perform.
        /// </value>
        PropertyValidationType ValidationInstruction { get; set; }
        
        /// <summary>
        /// Gets the original value.
        /// </summary>
        /// <value>
        /// The original value.
        /// </value>
        object OriginalValue { get; }

        /// <summary>
        /// Gets the parent record validation.
        /// </summary>
        /// <value>
        /// The parent record validation.
        /// </value>
        IRecord ParentRecord { get; }

        ValidationResultKind ResultKind { get; }

        /// <summary>
        /// Gets or sets the expected value.
        /// </summary>
        /// <value>
        /// The expected value.
        /// </value>
        object ExpectedValue { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; set; }

        /// <summary>
        /// Gets the property role.
        /// </summary>
        /// <value>
        /// The property role.
        /// </value>
        RecordPropertyRole PropertyRole { get; }

        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        /// <value>
        /// The type of the property.
        /// </value>
        Type PropertyType { get; }

        /// <summary>
        /// Sets the result.
        /// </summary>
        /// <param name="kind">The kind.</param>
        void SetResult(ValidationResultKind kind);
    }

    [ContractClassFor(typeof(IRecordProperty))]
    internal abstract class RecordPropertyValidationContract : IRecordProperty
    {


        public PropertyValidationType ValidationInstruction
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public object OriginalValue
        {
            get { throw new NotImplementedException(); }
        }

        public IRecord ParentRecord
        {
            get { throw new NotImplementedException(); }
        }

        public ValidationResultKind ResultKind
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public object ExpectedValue
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public RecordPropertyRole PropertyRole
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public Type PropertyType
        {
            get { throw new NotImplementedException(); }
        }


        public void SetResult(ValidationResultKind kind)
        {
            throw new NotImplementedException();
        }
    }
}
