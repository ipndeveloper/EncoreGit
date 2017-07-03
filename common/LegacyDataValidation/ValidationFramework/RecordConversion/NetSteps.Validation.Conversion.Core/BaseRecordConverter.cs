using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Reflection;
using NetSteps.Validation.Common;
using NetSteps.Validation.Common.Model;
using NetSteps.Validation.Conversion.Core.Model;
using System.Text;

namespace NetSteps.Validation.Conversion.Core
{
    public abstract class BaseRecordConverter<TObject> : IRecordConverter<TObject>
         where TObject : class
    {
        private readonly Func<string, IRecordConverter> _converterFactory;

        public BaseRecordConverter(Func<string, IRecordConverter> converterFactory)
        {
            _converterFactory = converterFactory;
        }

        public virtual IRecord Convert(object originalObject, IRecord parent)
        {
            Contract.Assert(originalObject is TObject);
            var record = Convert((TObject) originalObject, parent);
            AddRecordIdentifierComment(record);
            return record;
        }

        protected abstract string GetKeyFieldName();

        public IRecord Convert(TObject originalObject, IRecord parent)
        {
            var record = new Record(parent, SchemaName, TableName);
            var classData = ClassDataManager.GetClassData(originalObject.GetType());
            record.RecordIdentityField = GetKeyFieldName();
            var type = originalObject.GetType();
            record.RecordKind = type.Name;
            foreach (var propertyAccessor in classData.PropertyAccessors)
            {
                var propertyValue = propertyAccessor.Value.PropertyValue(originalObject);
                record.Properties.Add(propertyAccessor.Key, new RecordProperty(propertyAccessor.Key, propertyValue, record, GetRecordPropertyRole(propertyAccessor.Key), propertyAccessor.Value.PropertyType));
            }
            foreach (var collectionAccessor in classData.CollectionAccessors)
            {
                var collectionValue = collectionAccessor.Value.PropertyValue(originalObject);
                var collection = (IEnumerable) collectionValue;
                foreach (var child in collection)
                {
                    var converter =
                        _converterFactory(collectionAccessor.Value.PropertyType.GetGenericArguments()[0].FullName);
                    record.ChildRecords.Add(converter.Convert(child, record));
                }
            }
            return record;
        }


        protected virtual IDictionary<string, string> GetRecordCommentProperties(IRecord record)
        {
            return new Dictionary<string, string>();
        }

        public void AddRecordIdentifierComment(IRecord record)
        {
            var newComment = record.AddValidationComment(record.Parent == null ? ValidationCommentKind.PrimaryRecordIdentifier : ValidationCommentKind.ChildRecordIdentifier , record.RecordKind);
            newComment.AdditionalMessageComponents.Add(record.RecordIdentityField, record.RecordIdentity.ToString());
            var properties = GetRecordCommentProperties(record);
            foreach (var property in properties)
            {
                newComment.AdditionalMessageComponents.Add(property.Key, property.Value);
            }
        }

        protected abstract RecordPropertyRole GetRecordPropertyRole(string propertyName);

        protected abstract string SchemaName { get;}

        protected abstract string TableName { get; }
    }
}