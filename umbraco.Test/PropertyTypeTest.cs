using umbraco.cms.businesslogic.propertytype;
using NUnit.Framework;
using System;
using umbraco.interfaces;
using umbraco.cms.businesslogic.datatype;
using umbraco.cms.businesslogic;
using System.Linq;
using umbraco.cms.businesslogic.web;

namespace umbraco.Test
{


    /// <summary>
    ///This is a test class for PropertyTypeTest and is intended
    ///to contain all PropertyTypeTest Unit Tests
    ///</summary>
    [TestFixture]
    public class PropertyTypeTest
    {

		[TestFixtureSetUp]
		public void InitTestFixture()
		{
			SetUpUtilities.InitConfigurationManager();
		}        

		/// <summary>
        ///A test for MakeNew
        ///</summary>
        [Test]
        public void PropertyType_Make_New()
        {

            var allDataTypes = DataTypeDefinition.GetAll().ToList(); //get all definitions
            var contentTypes = DocumentType.GetAllAsList();            
            
            var name = "TEST" + Guid.NewGuid().ToString("N");
            var pt = PropertyType.MakeNew(allDataTypes.First(), contentTypes.First(), name, name);

            Assert.IsTrue(pt.Id > 0);
            Assert.IsInstanceOf<PropertyType>(pt);

            pt.delete();

            //make sure it's gone
            Assert.IsFalse(PropertyType.GetAll().Select(x => x.Id).Contains(pt.Id));
            
        }


        #region Tests to write
        ///// <summary>
        /////A test for PropertyType Constructor
        /////</summary>
        //[Test]
        //public void PropertyTypeConstructorTest()
        //{
        //    int id = 0; // TODO: Initialize to an appropriate value
        //    PropertyType target = new PropertyType(id);
        //    Assert.Inconclusive("TODO: Implement code to verify target");
        //}

        ///// <summary>
        /////A test for GetAll
        /////</summary>
        //[Test]
        //public void GetAllTest()
        //{
        //    PropertyType[] expected = null; // TODO: Initialize to an appropriate value
        //    PropertyType[] actual;
        //    actual = PropertyType.GetAll();
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for GetEditControl
        /////</summary>
        //[Test]
        //public void GetEditControlTest()
        //{
        //    int id = 0; // TODO: Initialize to an appropriate value
        //    PropertyType target = new PropertyType(id); // TODO: Initialize to an appropriate value
        //    object Value = null; // TODO: Initialize to an appropriate value
        //    bool IsPostBack = false; // TODO: Initialize to an appropriate value
        //    IDataType expected = null; // TODO: Initialize to an appropriate value
        //    IDataType actual;
        //    actual = target.GetEditControl(Value, IsPostBack);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for GetPropertyType
        /////</summary>
        //[Test]
        //public void GetPropertyTypeTest()
        //{
        //    int id = 0; // TODO: Initialize to an appropriate value
        //    PropertyType expected = null; // TODO: Initialize to an appropriate value
        //    PropertyType actual;
        //    actual = PropertyType.GetPropertyType(id);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for GetRawDescription
        /////</summary>
        //[Test]
        //public void GetRawDescriptionTest()
        //{
        //    int id = 0; // TODO: Initialize to an appropriate value
        //    PropertyType target = new PropertyType(id); // TODO: Initialize to an appropriate value
        //    string expected = string.Empty; // TODO: Initialize to an appropriate value
        //    string actual;
        //    actual = target.GetRawDescription();
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for GetRawName
        /////</summary>
        //[Test]
        //public void GetRawNameTest()
        //{
        //    int id = 0; // TODO: Initialize to an appropriate value
        //    PropertyType target = new PropertyType(id); // TODO: Initialize to an appropriate value
        //    string expected = string.Empty; // TODO: Initialize to an appropriate value
        //    string actual;
        //    actual = target.GetRawName();
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

       

        ///// <summary>
        /////A test for Save
        /////</summary>
        //[Test]
        //public void SaveTest()
        //{
        //    int id = 0; // TODO: Initialize to an appropriate value
        //    PropertyType target = new PropertyType(id); // TODO: Initialize to an appropriate value
        //    target.Save();
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}

        ///// <summary>
        /////A test for delete
        /////</summary>
        //[Test]
        //public void deleteTest()
        //{
        //    int id = 0; // TODO: Initialize to an appropriate value
        //    PropertyType target = new PropertyType(id); // TODO: Initialize to an appropriate value
        //    target.delete();
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}

        ///// <summary>
        /////A test for Alias
        /////</summary>
        //[Test]
        //public void AliasTest()
        //{
        //    int id = 0; // TODO: Initialize to an appropriate value
        //    PropertyType target = new PropertyType(id); // TODO: Initialize to an appropriate value
        //    string expected = string.Empty; // TODO: Initialize to an appropriate value
        //    string actual;
        //    target.Alias = expected;
        //    actual = target.Alias;
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for ContentTypeId
        /////</summary>
        //[Test]
        //public void ContentTypeIdTest()
        //{
        //    int id = 0; // TODO: Initialize to an appropriate value
        //    PropertyType target = new PropertyType(id); // TODO: Initialize to an appropriate value
        //    int actual;
        //    actual = target.ContentTypeId;
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for DataTypeDefinition
        /////</summary>
        //[Test]
        //public void DataTypeDefinitionTest()
        //{
        //    int id = 0; // TODO: Initialize to an appropriate value
        //    PropertyType target = new PropertyType(id); // TODO: Initialize to an appropriate value
        //    DataTypeDefinition expected = null; // TODO: Initialize to an appropriate value
        //    DataTypeDefinition actual;
        //    target.DataTypeDefinition = expected;
        //    actual = target.DataTypeDefinition;
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for Description
        /////</summary>
        //[Test]
        //public void DescriptionTest()
        //{
        //    int id = 0; // TODO: Initialize to an appropriate value
        //    PropertyType target = new PropertyType(id); // TODO: Initialize to an appropriate value
        //    string expected = string.Empty; // TODO: Initialize to an appropriate value
        //    string actual;
        //    target.Description = expected;
        //    actual = target.Description;
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for Id
        /////</summary>
        //[Test]
        //public void IdTest()
        //{
        //    int id = 0; // TODO: Initialize to an appropriate value
        //    PropertyType target = new PropertyType(id); // TODO: Initialize to an appropriate value
        //    int actual;
        //    actual = target.Id;
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for Mandatory
        /////</summary>
        //[Test]
        //public void MandatoryTest()
        //{
        //    int id = 0; // TODO: Initialize to an appropriate value
        //    PropertyType target = new PropertyType(id); // TODO: Initialize to an appropriate value
        //    bool expected = false; // TODO: Initialize to an appropriate value
        //    bool actual;
        //    target.Mandatory = expected;
        //    actual = target.Mandatory;
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for Name
        /////</summary>
        //[Test]
        //public void NameTest()
        //{
        //    int id = 0; // TODO: Initialize to an appropriate value
        //    PropertyType target = new PropertyType(id); // TODO: Initialize to an appropriate value
        //    string expected = string.Empty; // TODO: Initialize to an appropriate value
        //    string actual;
        //    target.Name = expected;
        //    actual = target.Name;
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for SortOrder
        /////</summary>
        //[Test]
        //public void SortOrderTest()
        //{
        //    int id = 0; // TODO: Initialize to an appropriate value
        //    PropertyType target = new PropertyType(id); // TODO: Initialize to an appropriate value
        //    int expected = 0; // TODO: Initialize to an appropriate value
        //    int actual;
        //    target.SortOrder = expected;
        //    actual = target.SortOrder;
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for TabId
        /////</summary>
        //[Test]
        //public void TabIdTest()
        //{
        //    int id = 0; // TODO: Initialize to an appropriate value
        //    PropertyType target = new PropertyType(id); // TODO: Initialize to an appropriate value
        //    int expected = 0; // TODO: Initialize to an appropriate value
        //    int actual;
        //    target.TabId = expected;
        //    actual = target.TabId;
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for ValidationRegExp
        /////</summary>
        //[Test]
        //public void ValidationRegExpTest()
        //{
        //    int id = 0; // TODO: Initialize to an appropriate value
        //    PropertyType target = new PropertyType(id); // TODO: Initialize to an appropriate value
        //    string expected = string.Empty; // TODO: Initialize to an appropriate value
        //    string actual;
        //    target.ValidationRegExp = expected;
        //    actual = target.ValidationRegExp;
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}
        #endregion


        #region Initialize and cleanup
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use TestFixtureSetUp to run code before running the first test in the class
        //[TestFixtureSetUp]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use TestFixtureTearDown to run code after all tests in a class have run
        //[TestFixtureTearDown]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use SetUp to run code before running each test
        //[SetUp]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TearDown]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion
    }
}
