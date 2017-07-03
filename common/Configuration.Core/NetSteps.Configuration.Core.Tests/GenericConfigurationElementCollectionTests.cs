using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetSteps.Configuration.Core.Tests
{
	public class GenericConfigurationElementCollectionTestsTestConfigurationSection : DeclaredMemberConfigurationSection
	{
		public static readonly ConfigurationProperty CollectionProperty = new ConfigurationProperty("collection", typeof(GenericConfigurationElementCollectionTestsTestConfigurationElementCollection));
		public GenericConfigurationElementCollectionTestsTestConfigurationElementCollection Collection
		{
			get { return (GenericConfigurationElementCollectionTestsTestConfigurationElementCollection)this[CollectionProperty]; }
			set { this[CollectionProperty] = value; }
		}
	}

	public class GenericConfigurationElementCollectionTestsTestConfigurationElement : DeclaredMemberConfigurationElement
	{
		public static readonly ConfigurationProperty KeyProperty = new ConfigurationProperty("key", typeof(string));
		public string Key
		{
			get { return (string)this[KeyProperty]; }
			set { this[KeyProperty] = value; }
		}

		public static readonly ConfigurationProperty ValueProperty = new ConfigurationProperty("value", typeof(string));
		public string Value
		{
			get { return (string)this[ValueProperty]; }
			set { this[ValueProperty] = value; }
		}
	}

	public class GenericConfigurationElementCollectionTestsTestConfigurationElementCollection : GenericConfigurationElementCollection<GenericConfigurationElementCollectionTestsTestConfigurationElement>
	{
		public GenericConfigurationElementCollectionTestsTestConfigurationElementCollection()
			: base(t => t.Key)
		{ }
	}

	[TestClass]
	public class GenericConfigurationElementCollectionTests
	{
		[TestMethod]
		public void CanRetriveItemFromCollectionByNaturalKey()
		{
			var col = new GenericConfigurationElementCollectionTestsTestConfigurationElementCollection();
			col.Add(new GenericConfigurationElementCollectionTestsTestConfigurationElement() { Key = "Key1", Value = "Value1" });
			col.Add(new GenericConfigurationElementCollectionTestsTestConfigurationElement() { Key = "Key2", Value = "Value2" });

			Assert.IsNotNull(col["Key1"]);
			Assert.AreEqual("Value1", col["Key1"].Value);
		}

		[TestMethod]
		public void CanRetriveItemFromCollectionByNaturalKeyWhenLoadedFromFile()
		{
			GenericConfigurationElementCollectionTestsTestConfigurationSection config = Configuration.ConfigurationUtility.GetSection<GenericConfigurationElementCollectionTestsTestConfigurationSection>();

			Assert.IsNotNull(config);
			Assert.IsNotNull(config.Collection["fileKey1"]);
			Assert.AreEqual("fileValue1", config.Collection["fileKey1"].Value);
		}
	}
}
