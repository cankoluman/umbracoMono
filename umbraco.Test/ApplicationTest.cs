﻿using umbraco.BusinessLogic;
using NUnit.Framework;
using NMock2;
using System;
using umbraco.interfaces;
using System.Collections.Generic;
using umbraco.DataLayer;
using System.Linq;

namespace umbraco.Test
{
    
    
    /// <summary>
    ///This is a test class for ApplicationTest and is intended
    ///to contain all ApplicationTest Unit Tests
    ///</summary>
    [TestFixture]
    public class ApplicationTest
    {

		[TestFixtureSetUp]
		public void InitTestFixture()
		{
			SetUpUtilities.InitConfigurationManager();
		}


		/// <summary>
        /// Create a new application and delete it
        ///</summary>
        [Test]
        public void Application_Make_New()
        {
            var name = Guid.NewGuid().ToString("N");
            Application.MakeNew(name, name, "icon.jpg");
            
            //check if it exists
            var app = Application.getByAlias(name);
            Assert.IsNotNull(app);

            //now remove it
            app.Delete();
            Assert.IsNull(Application.getByAlias(name));
        }

        /// <summary>
        /// Creates a new user, assigns the user to existing application, 
        /// then deletes the user
        /// </summary>
        [Test]
        public void Application_Create_New_User_Assign_Application_And_Delete_User()
        {
            var name = Guid.NewGuid().ToString("N");
         
            //new user
            var ut = UserType.GetAllUserTypes().First();
            var user = User.MakeNew(name, name, name, ut);

            //get application
            var app = Application.getAll().First();

            //assign the app
            user.addApplication(app.alias);
            //ensure it's added
            Assert.AreEqual(1, user.Applications.Where(x => x.alias == app.alias).Count());

            //delete the user
            user.delete();

            //make sure the assigned applications are gone
            Assert.AreEqual(0, user.Applications.Where(x => x.alias == name).Count());
        }

        /// <summary>
        /// create a new application and assigne an new user and deletes the application making sure the assignments are removed
        /// </summary>
        [Test]
        public void Application_Make_New_Assign_User_And_Delete()
        {
            var name = Guid.NewGuid().ToString("N");

            //new user
            var ut = UserType.GetAllUserTypes().First();
            var user = User.MakeNew(name, name, name, ut);

            Application.MakeNew(name, name, "icon.jpg");

            //check if it exists
            var app = Application.getByAlias(name);
            Assert.IsNotNull(app);

            //assign the app
            user.addApplication(app.alias);
            //ensure it's added
            Assert.AreEqual(1, user.Applications.Where(x => x.alias == app.alias).Count());

            //delete the app
            app.Delete();

            //make sure the assigned applications are gone
            Assert.AreEqual(0, user.Applications.Where(x => x.alias == name).Count());
        }

        #region Tests to write


        ///// <summary>
        /////A test for Application Constructor
        /////</summary>
        //[Test]
        //public void ApplicationConstructorTest()
        //{
        //    string name = string.Empty; // TODO: Initialize to an appropriate value
        //    string alias = string.Empty; // TODO: Initialize to an appropriate value
        //    string icon = string.Empty; // TODO: Initialize to an appropriate value
        //    Application target = new Application(name, alias, icon);
        //    Assert.Inconclusive("TODO: Implement code to verify target");
        //}

        ///// <summary>
        /////A test for Application Constructor
        /////</summary>
        //[Test]
        //public void ApplicationConstructorTest1()
        //{
        //    Application target = new Application();
        //    Assert.Inconclusive("TODO: Implement code to verify target");
        //}

        ///// <summary>
        /////A test for Delete
        /////</summary>
        //[Test]
        //public void DeleteTest()
        //{
        //    Application target = new Application(); // TODO: Initialize to an appropriate value
        //    target.Delete();
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}

       

        ///// <summary>
        /////A test for MakeNew
        /////</summary>
        //[Test]
        //public void MakeNewTest1()
        //{
        //    string name = string.Empty; // TODO: Initialize to an appropriate value
        //    string alias = string.Empty; // TODO: Initialize to an appropriate value
        //    string icon = string.Empty; // TODO: Initialize to an appropriate value
        //    Application.MakeNew(name, alias, icon);
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}

        ///// <summary>
        /////A test for RegisterIApplications
        /////</summary>
        //[Test]
        //public void RegisterIApplicationsTest()
        //{
        //    Application.RegisterIApplications();
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}

        ///// <summary>
        /////A test for getAll
        /////</summary>
        //[Test]
        //public void getAllTest()
        //{
        //    List<Application> expected = null; // TODO: Initialize to an appropriate value
        //    List<Application> actual;
        //    actual = Application.getAll();
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for getByAlias
        /////</summary>
        //[Test]
        //public void getByAliasTest()
        //{
        //    string appAlias = string.Empty; // TODO: Initialize to an appropriate value
        //    Application expected = null; // TODO: Initialize to an appropriate value
        //    Application actual;
        //    actual = Application.getByAlias(appAlias);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for SqlHelper
        /////</summary>
        //[Test]
        //public void SqlHelperTest()
        //{
        //    ISqlHelper actual;
        //    actual = Application.SqlHelper;
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for alias
        /////</summary>
        //[Test]
        //public void aliasTest()
        //{
        //    Application target = new Application(); // TODO: Initialize to an appropriate value
        //    string expected = string.Empty; // TODO: Initialize to an appropriate value
        //    string actual;
        //    target.alias = expected;
        //    actual = target.alias;
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for icon
        /////</summary>
        //[Test]
        //public void iconTest()
        //{
        //    Application target = new Application(); // TODO: Initialize to an appropriate value
        //    string expected = string.Empty; // TODO: Initialize to an appropriate value
        //    string actual;
        //    target.icon = expected;
        //    actual = target.icon;
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for name
        /////</summary>
        //[Test]
        //public void nameTest()
        //{
        //    Application target = new Application(); // TODO: Initialize to an appropriate value
        //    string expected = string.Empty; // TODO: Initialize to an appropriate value
        //    string actual;
        //    target.name = expected;
        //    actual = target.name;
        //    Assert.AreEqual(expected, actual);
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

		[TestFixtureTearDown]
		public void Dispose()
		{
			ConfigurationManagerService.ConfigManager = new ConfigurationManagerDefault();
		}

    }
}
