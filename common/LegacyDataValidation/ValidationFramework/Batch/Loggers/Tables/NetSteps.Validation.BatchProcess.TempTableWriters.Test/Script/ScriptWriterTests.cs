using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Validation.BatchProcess.TempTableWriters.Implementation;
using NetSteps.Validation.Common.Model;
using Moq;
using System.Collections.Generic;
using System.Data.SqlTypes;
using NetSteps.Validation.BatchProcess.TempTableWriters.Implementation.Script;

namespace NetSteps.Validation.BatchProcess.TempTableWriters.Test
{
    [TestClass]
    public class ScriptWriterTests
    {
        private IRecordProperty GetPropertyMock(Type targetType, RecordPropertyRole role)
        {
            if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return GetPropertyMock(targetType, role, "testNullable" + targetType.GetGenericArguments()[0].Name);
            }
            else
            {
                return GetPropertyMock(targetType, role, "test" + targetType.Name);
            }
        }

        private IRecordProperty GetPropertyMock(Type targetType, RecordPropertyRole role, string name)
        {
            var propertyMock = new Mock<IRecordProperty>();
            propertyMock.SetupAllProperties();
            propertyMock.SetupGet<Type>(x => x.PropertyType).Returns(targetType);
            propertyMock.SetupGet<RecordPropertyRole>(x => x.PropertyRole).Returns(role);
            propertyMock.Object.Name = name;
            return propertyMock.Object;
        }

        [TestMethod]
        public void TableScriptWriter_GetTableCreationScript_should_have_correctly_converted_types()
        {
            #region Setup

            var recordMock = new Mock<IRecord>();
            recordMock.SetupGet<string>(x => x.RecordKind).Returns("TestObject");
            var propertyDictionary = new Dictionary<string, IRecordProperty>();
            recordMock.SetupGet<IDictionary<string, IRecordProperty>>(x => x.Properties).Returns(propertyDictionary);
            var mockSource = new Mock<IRecordSource>();
            mockSource.SetupGet<string>(x => x.SchemaName).Returns("originalSchema");
            mockSource.SetupGet<string>(x => x.TableName).Returns("originalTable");
            recordMock.SetupGet<IRecordSource>(x => x.Source).Returns(mockSource.Object);

            var typeArray = new Type[] 
            {
                typeof(SqlInt64),
                typeof(Int64),
                typeof(SqlInt32),
                typeof(Int32),
                typeof(SqlBytes),
                typeof(SqlBinary),
                typeof(Byte[]),
                typeof(SqlBoolean),
                typeof(Boolean),
                typeof(SqlDateTime),
                typeof(DateTime),
                typeof(SqlDecimal),
                typeof(SqlDouble),
                typeof(Double),
                typeof(SqlMoney),
                typeof(Decimal),
                typeof(SqlChars),
                typeof(SqlString),
                typeof(String),
                typeof(Char[]),
                typeof(SqlSingle),
                typeof(Single),
                typeof(SqlInt16),
                typeof(Int16),
                typeof(SqlByte),
                typeof(Byte),
                typeof(SqlGuid),
                typeof(Guid),
                typeof(SqlInt64?),
                typeof(Int64?),
                typeof(SqlInt32?),
                typeof(Int32?),
                typeof(SqlBinary?),
                typeof(SqlBoolean?),
                typeof(Boolean?),
                typeof(SqlDateTime?),
                typeof(DateTime?),
                typeof(SqlDecimal?),
                typeof(SqlDouble?),
                typeof(Double?),
                typeof(SqlMoney?),
                typeof(Decimal?),
                typeof(SqlString?),
                typeof(SqlSingle?),
                typeof(Single?),
                typeof(SqlInt16?),
                typeof(Int16?),
                typeof(SqlByte?),
                typeof(Byte?),
                typeof(SqlGuid?),
                typeof(Guid?)
            };

            foreach (var type in typeArray)
            {
                propertyDictionary.Add(GetPropertyMock(type, RecordPropertyRole.ValidatedField));
            }
            #endregion

            var schema = "validationschema";
            var scriptContainer = TableScriptWriter.GetScriptContainer(TempTableType.Update, recordMock.Object, schema);

            Assert.IsNotNull(scriptContainer);
            Assert.IsFalse(scriptContainer.CreateScript.Contains("UNHANDLED"));
            foreach (var type in typeArray)
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    Assert.IsTrue(scriptContainer.CreateScript.Contains(String.Format("Nullable{0}", type.GetGenericArguments()[0].Name)));
                }
                else
                {
                    Assert.IsTrue(scriptContainer.CreateScript.Contains(type.Name));
                }
            }
        }

        [TestMethod]
        public void TableScriptWriter_GetTableCreationScript_should_have_correctly_defined_tablename()
        {
            #region Setup

            var recordMock = new Mock<IRecord>();
            recordMock.SetupGet<string>(x => x.RecordKind).Returns("TestObject");
            var propertyDictionary = new Dictionary<string, IRecordProperty>();
            recordMock.SetupGet<IDictionary<string, IRecordProperty>>(x => x.Properties).Returns(propertyDictionary);
            var mockSource = new Mock<IRecordSource>();
            mockSource.SetupGet<string>(x => x.SchemaName).Returns("originalSchema");
            mockSource.SetupGet<string>(x => x.TableName).Returns("originalTable");
            recordMock.SetupGet<IRecordSource>(x => x.Source).Returns(mockSource.Object);

            #endregion

            var schema = "tempValidation";
            var scriptContainer = TableScriptWriter.GetScriptContainer(TempTableType.Update, recordMock.Object, schema);

            Assert.IsNotNull(scriptContainer);
            Assert.IsTrue(scriptContainer.CreateScript.Contains(String.Format("CREATE TABLE [{0}].[tmp_{1}]", schema, recordMock.Object.RecordKind)));
        }

        [TestMethod]
        public void TableScriptWriter_GetTableCreationScript_should_only_create_table_fields_for_necessary_properties()
        {
            #region Setup

            var recordMock = new Mock<IRecord>();
            recordMock.SetupGet<string>(x => x.RecordKind).Returns("TestObject");
            var propertyDictionary = new Dictionary<string, IRecordProperty>();
            recordMock.SetupGet<IDictionary<string, IRecordProperty>>(x => x.Properties).Returns(propertyDictionary);
            var mockSource = new Mock<IRecordSource>();
            mockSource.SetupGet<string>(x => x.SchemaName).Returns("originalSchema");
            mockSource.SetupGet<string>(x => x.TableName).Returns("originalTable");
            recordMock.SetupGet<IRecordSource>(x => x.Source).Returns(mockSource.Object);

            #endregion

            propertyDictionary.Add(GetPropertyMock(typeof(int), RecordPropertyRole.ForeignKey, "ForeignKey"));
            propertyDictionary.Add(GetPropertyMock(typeof(int), RecordPropertyRole.PrimaryKey, "PrimaryKey"));
            propertyDictionary.Add(GetPropertyMock(typeof(int), RecordPropertyRole.Fact, "Fact"));
            propertyDictionary.Add(GetPropertyMock(typeof(int), RecordPropertyRole.ValidatedField, "ValidatedField"));

            var schema = "validation";
            var scriptContainer = TableScriptWriter.GetScriptContainer(TempTableType.Update, recordMock.Object, schema);

            Assert.IsNotNull(scriptContainer);
            Assert.IsFalse(scriptContainer.CreateScript.Contains("Fact"));
            Assert.IsTrue(scriptContainer.CreateScript.Contains("ForeignKey int"));
            Assert.IsTrue(scriptContainer.CreateScript.Contains("PrimaryKey int"));
            Assert.IsTrue(scriptContainer.CreateScript.Contains("ValidatedField_original int"));
            Assert.IsTrue(scriptContainer.CreateScript.Contains("ValidatedField_expected int"));
        }
    }

    internal static class DictionaryExtensions
    {
        internal static void Add(this IDictionary<string, IRecordProperty> dictionary, IRecordProperty property)
        {
            dictionary.Add(property.Name, property);
        }
    }
}
