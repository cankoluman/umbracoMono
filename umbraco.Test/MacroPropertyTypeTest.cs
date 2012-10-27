using umbraco.cms.businesslogic.macro;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace umbraco.Test
{
    
    
    /// <summary>
    ///This is a test class for MacroPropertyTypeTest and is intended
    ///to contain all MacroPropertyTypeTest Unit Tests
    ///</summary>
    [TestFixture]
    public class MacroPropertyTypeTest
    {

        /// <summary>
        /// Test the constructor to throw an exception when the object is not found by id
        ///</summary>
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void MacroProperty_Not_Found_Constructor()
        {
            MacroProperty u = new MacroProperty(-1111);
        }


        #region Tests to write
        

        ///// <summary>
        /////A test for MacroPropertyType Constructor
        /////</summary>
        //[Test]
        //public void MacroPropertyTypeConstructorTest1()
        //{
        //    MacroPropertyType target = new MacroPropertyType();
        //    Assert.Inconclusive("TODO: Implement code to verify target");
        //}

        ///// <summary>
        /////A test for MacroPropertyType Constructor
        /////</summary>
        //[Test]
        //public void MacroPropertyTypeConstructorTest2()
        //{
        //    int Id = 0; // TODO: Initialize to an appropriate value
        //    MacroPropertyType target = new MacroPropertyType(Id);
        //    Assert.Inconclusive("TODO: Implement code to verify target");
        //}

        ///// <summary>
        /////A test for Alias
        /////</summary>
        //[Test]
        //public void AliasTest()
        //{
        //    MacroPropertyType target = new MacroPropertyType(); // TODO: Initialize to an appropriate value
        //    string actual;
        //    actual = target.Alias;
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for Assembly
        /////</summary>
        //[Test]
        //public void AssemblyTest()
        //{
        //    MacroPropertyType target = new MacroPropertyType(); // TODO: Initialize to an appropriate value
        //    string actual;
        //    actual = target.Assembly;
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for BaseType
        /////</summary>
        //[Test]
        //public void BaseTypeTest()
        //{
        //    MacroPropertyType target = new MacroPropertyType(); // TODO: Initialize to an appropriate value
        //    string actual;
        //    actual = target.BaseType;
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for GetAll
        /////</summary>
        //[Test]
        //public void GetAllTest()
        //{
        //    List<MacroPropertyType> actual;
        //    actual = MacroPropertyType.GetAll;
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for Id
        /////</summary>
        //[Test]
        //public void IdTest()
        //{
        //    MacroPropertyType target = new MacroPropertyType(); // TODO: Initialize to an appropriate value
        //    int actual;
        //    actual = target.Id;
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for Type
        /////</summary>
        //[Test]
        //public void TypeTest()
        //{
        //    MacroPropertyType target = new MacroPropertyType(); // TODO: Initialize to an appropriate value
        //    string actual;
        //    actual = target.Type;
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //} 
        #endregion

        #region Additional test attributes
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
