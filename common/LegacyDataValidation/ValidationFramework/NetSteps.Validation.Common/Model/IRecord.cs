using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Validation.Common.Model
{
    /// <summary>
    /// Represents a database record (i.e. one row in a database table)
    /// </summary>
    [ContractClass(typeof(RecordContract))]
    public interface IRecord
    {
        /// <summary>
        /// Gets the kind of the record, or the name of the database table of which the record is a member.
        /// </summary>
        /// <value>
        /// The kind of the record.
        /// </value>
        string RecordKind { get;}

        /// <summary>
        /// Gets the record identity.
        /// </summary>
        /// <value>
        /// The record identity.
        /// </value>
        object RecordIdentity { get; }

        /// <summary>
        /// Gets the child records.
        /// </summary>
        /// <value>
        /// The child records.
        /// </value>
        ICollection<IRecord> ChildRecords { get; }

        /// <summary>
        /// Gets the record identity field.
        /// </summary>
        /// <value>
        /// The record identity field.
        /// </value>
        string RecordIdentityField { get; }

        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        ValidationResultKind Result { get; }

        /// <summary>
        /// Gets the property validations.
        /// </summary>
        /// <value>
        /// The property validations.
        /// </value>
        IDictionary<string, IRecordProperty> Properties { get; }

  
        /// <summary>
        /// Gets the parent record validation.
        /// </summary>
        /// <value>
        /// The parent record validation.
        /// </value>
        IRecord Parent { get; }

        /// <summary>
        /// Sets the result.
        /// </summary>
        /// <param name="result">The result.</param>
        void SetResult(ValidationResultKind result);

        /// <summary>
        /// Gets the validation comments.
        /// </summary>
        /// <value>
        /// The validation comments.
        /// </value>
        IEnumerable<IValidationComment> ValidationComments { get; }

        /// <summary>
        /// Adds a validation comment.
        /// </summary>
        /// <param name="commentKind">Kind of the comment.</param>
        /// <param name="comment">The comment.</param>
        /// <returns>The added validation comment.</returns>
        IValidationComment AddValidationComment(ValidationCommentKind commentKind, string comment);

        /// <summary>
        /// Gets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        IRecordSource Source { get; }
    }

    [ContractClassFor(typeof(IRecord))]
    internal abstract class RecordContract : IRecord
    {
        public string RecordKind
        {
            get 
            {
                Contract.Ensures(!String.IsNullOrEmpty(Contract.Result<string>()));
                throw new NotImplementedException();
            }
        }


        public object RecordIdentity
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


        public IDictionary<string, object> Properties
        {
            get
            {
                Contract.Ensures(Contract.Result<IDictionary<string, object>>() != null);
                throw new NotImplementedException();
            }
        }

        public ICollection<IRecord> ChildRecords
        {
            get
            {
                Contract.Ensures(Contract.Result<ICollection<IRecord>>() != null);
                throw new NotImplementedException();
            }
        }


        public string RecordIdentityField
        {
            get
            {
                throw new NotImplementedException();
            }
        }


        public ValidationResultKind Result
        {
            get { throw new NotImplementedException(); }
        }

        public IRecord Record
        {
            get { throw new NotImplementedException(); }
        }

        IDictionary<string, IRecordProperty> IRecord.Properties
        {
            get { throw new NotImplementedException(); }
        }

        public IRecord Parent
        {
            get { throw new NotImplementedException(); }
        }


        public void SetResult(ValidationResultKind result)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IValidationComment> ValidationComments
        {
            get { throw new NotImplementedException(); }
        }


        public IRecordSource Source
        {
            get { throw new NotImplementedException(); }
        }


        public IValidationComment AddValidationComment(ValidationCommentKind commentKind, string comment)
        {
            throw new NotImplementedException();
        }
    }
}
