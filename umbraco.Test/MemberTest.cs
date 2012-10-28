﻿using umbraco.cms.businesslogic.member;
using NUnit.Framework;
using System;
using System.Collections;
using umbraco.BusinessLogic;
using System.Xml;
using System.Linq;

using System.Web;
using System.Web.Caching;

namespace umbraco.Test
{
    
    
    /// <summary>
    ///This is a test class for MemberTest and is intended
    ///to contain all MemberTest Unit Tests
    ///</summary>
    [TestFixture]
    public class MemberTest
    {

		[TestFixtureSetUp]
		public void InitTestFixture()
		{
			SetUpUtilities.InitConfigurationManager();
			m_User = new User(0);

		}        


		/// <summary>
        /// Creates a new member type and member, then deletes it
        ///</summary>
        [Test]
        public void Member_Make_New()
        {
           
            var mt = MemberType.MakeNew(m_User, "TEST" + Guid.NewGuid().ToString("N"));
            var m = Member.MakeNew("TEST" + Guid.NewGuid().ToString("N"),
                "TEST" + Guid.NewGuid().ToString("N") + "@test.com", mt, m_User);

            Assert.IsInstanceOf<Member>(m);
            Assert.IsTrue(m.Id > 0);

            m.delete();
            Assert.IsFalse(Member.IsNode(m.Id));

            mt.delete();
            Assert.IsFalse(MemberType.IsNode(mt.Id));
        }

        /// <summary>
        ///Creates a new member type, member group and a member, then adds the member to the group. 
        ///then deletes the data in order for cleanup
        ///</summary>
        [Test]
        public void Member_Add_To_Group()
        {
            var mt = MemberType.MakeNew(m_User, "TEST" + Guid.NewGuid().ToString("N"));
            var m = Member.MakeNew("TEST" + Guid.NewGuid().ToString("N"),
                "TEST" + Guid.NewGuid().ToString("N") + "@test.com", mt, m_User);

            var mg = MemberGroup.MakeNew("TEST" + Guid.NewGuid().ToString("N"), m_User);
            Assert.IsInstanceOf<MemberGroup>(mg);
            Assert.IsTrue(mg.Id > 0);

            //add the member to the group
            m.AddGroup(mg.Id);

            //ensure they are added
            Assert.AreEqual(1, m.Groups.Count);
            Assert.AreEqual(mg.Id, ((MemberGroup)m.Groups.Cast<DictionaryEntry>().First().Value).Id);

            //remove the grup association
            m.RemoveGroup(mg.Id);

            //ensure they are removed
            Assert.AreEqual(0, m.Groups.Count);

            mg.delete();
            Assert.IsFalse(Member.IsNode(mg.Id));

            m.delete();
            Assert.IsFalse(Member.IsNode(m.Id));

            mt.delete();
            Assert.IsFalse(MemberType.IsNode(mt.Id));
        }

        #region Private members
        private User m_User; 
        #endregion

        #region Test to write
        ///// <summary>
        /////A test for Member Constructor
        /////</summary>
        //[Test]
        //public void MemberConstructorTest()
        //{
        //    int id = 0; // TODO: Initialize to an appropriate value
        //    bool noSetup = false; // TODO: Initialize to an appropriate value
        //    Member target = new Member(id, noSetup);
        //    Assert.Inconclusive("TODO: Implement code to verify target");
        //}

        ///// <summary>
        /////A test for Member Constructor
        /////</summary>
        //[Test]
        //public void MemberConstructorTest1()
        //{
        //    Guid id = new Guid(); // TODO: Initialize to an appropriate value
        //    bool noSetup = false; // TODO: Initialize to an appropriate value
        //    Member target = new Member(id, noSetup);
        //    Assert.Inconclusive("TODO: Implement code to verify target");
        //}

        ///// <summary>
        /////A test for Member Constructor
        /////</summary>
        //[Test]
        //public void MemberConstructorTest2()
        //{
        //    int id = 0; // TODO: Initialize to an appropriate value
        //    Member target = new Member(id);
        //    Assert.Inconclusive("TODO: Implement code to verify target");
        //}

        ///// <summary>
        /////A test for Member Constructor
        /////</summary>
        //[Test]
        //public void MemberConstructorTest3()
        //{
        //    Guid id = new Guid(); // TODO: Initialize to an appropriate value
        //    Member target = new Member(id);
        //    Assert.Inconclusive("TODO: Implement code to verify target");
        //}

        

        ///// <summary>
        /////A test for AddMemberToCache
        /////</summary>
        //[Test]
        //public void AddMemberToCacheTest()
        //{
        //    Member m = null; // TODO: Initialize to an appropriate value
        //    Member.AddMemberToCache(m);
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}

        ///// <summary>
        /////A test for AddMemberToCache
        /////</summary>
        //[Test]
        //public void AddMemberToCacheTest1()
        //{
        //    Member m = null; // TODO: Initialize to an appropriate value
        //    bool UseSession = false; // TODO: Initialize to an appropriate value
        //    TimeSpan TimespanForCookie = new TimeSpan(); // TODO: Initialize to an appropriate value
        //    Member.AddMemberToCache(m, UseSession, TimespanForCookie);
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}

        ///// <summary>
        /////A test for CachedMembers
        /////</summary>
        //[Test]
        //public void CachedMembersTest()
        //{
        //    Hashtable expected = null; // TODO: Initialize to an appropriate value
        //    Hashtable actual;
        //    actual = Member.CachedMembers();
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for ChangePassword
        /////</summary>
        //[Test]
        //public void ChangePasswordTest()
        //{
        //    Guid id = new Guid(); // TODO: Initialize to an appropriate value
        //    Member target = new Member(id); // TODO: Initialize to an appropriate value
        //    string newPassword = string.Empty; // TODO: Initialize to an appropriate value
        //    target.ChangePassword(newPassword);
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}

        ///// <summary>
        /////A test for ClearMemberFromClient
        /////</summary>
        //[Test]
        //public void ClearMemberFromClientTest()
        //{
        //    int NodeId = 0; // TODO: Initialize to an appropriate value
        //    Member.ClearMemberFromClient(NodeId);
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}

        ///// <summary>
        /////A test for ClearMemberFromClient
        /////</summary>
        //[Test]
        //public void ClearMemberFromClientTest1()
        //{
        //    Member m = null; // TODO: Initialize to an appropriate value
        //    Member.ClearMemberFromClient(m);
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}

        ///// <summary>
        /////A test for CurrentMemberId
        /////</summary>
        //[Test]
        //public void CurrentMemberIdTest()
        //{
        //    int expected = 0; // TODO: Initialize to an appropriate value
        //    int actual;
        //    actual = Member.CurrentMemberId();
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for DeleteFromType
        /////</summary>
        //[Test]
        //public void DeleteFromTypeTest()
        //{
        //    MemberType dt = null; // TODO: Initialize to an appropriate value
        //    Member.DeleteFromType(dt);
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}

        ///// <summary>
        /////A test for GetCurrentMember
        /////</summary>
        //[Test]
        //public void GetCurrentMemberTest()
        //{
        //    Member expected = null; // TODO: Initialize to an appropriate value
        //    Member actual;
        //    actual = Member.GetCurrentMember();
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for GetMemberByName
        /////</summary>
        //[Test]
        //public void GetMemberByNameTest()
        //{
        //    string usernameToMatch = string.Empty; // TODO: Initialize to an appropriate value
        //    bool matchByNameInsteadOfLogin = false; // TODO: Initialize to an appropriate value
        //    Member[] expected = null; // TODO: Initialize to an appropriate value
        //    Member[] actual;
        //    actual = Member.GetMemberByName(usernameToMatch, matchByNameInsteadOfLogin);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for GetMemberFromCache
        /////</summary>
        //[Test]
        //public void GetMemberFromCacheTest()
        //{
        //    int id = 0; // TODO: Initialize to an appropriate value
        //    Member expected = null; // TODO: Initialize to an appropriate value
        //    Member actual;
        //    actual = Member.GetMemberFromCache(id);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for GetMemberFromEmail
        /////</summary>
        //[Test]
        //public void GetMemberFromEmailTest()
        //{
        //    string email = string.Empty; // TODO: Initialize to an appropriate value
        //    Member expected = null; // TODO: Initialize to an appropriate value
        //    Member actual;
        //    actual = Member.GetMemberFromEmail(email);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for GetMemberFromLoginAndEncodedPassword
        /////</summary>
        //[Test]
        //public void GetMemberFromLoginAndEncodedPasswordTest()
        //{
        //    string loginName = string.Empty; // TODO: Initialize to an appropriate value
        //    string password = string.Empty; // TODO: Initialize to an appropriate value
        //    Member expected = null; // TODO: Initialize to an appropriate value
        //    Member actual;
        //    actual = Member.GetMemberFromLoginAndEncodedPassword(loginName, password);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for GetMemberFromLoginName
        /////</summary>
        //[Test]
        //public void GetMemberFromLoginNameTest()
        //{
        //    string loginName = string.Empty; // TODO: Initialize to an appropriate value
        //    Member expected = null; // TODO: Initialize to an appropriate value
        //    Member actual;
        //    actual = Member.GetMemberFromLoginName(loginName);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for GetMemberFromLoginNameAndPassword
        /////</summary>
        //[Test]
        //public void GetMemberFromLoginNameAndPasswordTest()
        //{
        //    string loginName = string.Empty; // TODO: Initialize to an appropriate value
        //    string password = string.Empty; // TODO: Initialize to an appropriate value
        //    Member expected = null; // TODO: Initialize to an appropriate value
        //    Member actual;
        //    actual = Member.GetMemberFromLoginNameAndPassword(loginName, password);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for InUmbracoMemberMode
        /////</summary>
        //[Test]
        //public void InUmbracoMemberModeTest()
        //{
        //    bool expected = false; // TODO: Initialize to an appropriate value
        //    bool actual;
        //    actual = Member.InUmbracoMemberMode();
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for IsLoggedOn
        /////</summary>
        //[Test]
        //public void IsLoggedOnTest()
        //{
        //    bool expected = false; // TODO: Initialize to an appropriate value
        //    bool actual;
        //    actual = Member.IsLoggedOn();
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for IsMember
        /////</summary>
        //[Test]
        //public void IsMemberTest()
        //{
        //    string loginName = string.Empty; // TODO: Initialize to an appropriate value
        //    bool expected = false; // TODO: Initialize to an appropriate value
        //    bool actual;
        //    actual = Member.IsMember(loginName);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for IsUsingUmbracoRoles
        /////</summary>
        //[Test]
        //public void IsUsingUmbracoRolesTest()
        //{
        //    bool expected = false; // TODO: Initialize to an appropriate value
        //    bool actual;
        //    actual = Member.IsUsingUmbracoRoles();
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        

        ///// <summary>
        /////A test for MakeNew
        /////</summary>
        //[Test]
        //public void MakeNewTest1()
        //{
        //    string Name = string.Empty; // TODO: Initialize to an appropriate value
        //    MemberType mbt = null; // TODO: Initialize to an appropriate value
        //    User u = null; // TODO: Initialize to an appropriate value
        //    Member expected = null; // TODO: Initialize to an appropriate value
        //    Member actual;
        //    actual = Member.MakeNew(Name, mbt, u);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for RemoveGroup
        /////</summary>
        //[Test]
        //public void RemoveGroupTest()
        //{
        //    Guid id = new Guid(); // TODO: Initialize to an appropriate value
        //    Member target = new Member(id); // TODO: Initialize to an appropriate value
        //    int GroupId = 0; // TODO: Initialize to an appropriate value
        //    target.RemoveGroup(GroupId);
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}

        ///// <summary>
        /////A test for RemoveMemberFromCache
        /////</summary>
        //[Test]
        //public void RemoveMemberFromCacheTest()
        //{
        //    Member m = null; // TODO: Initialize to an appropriate value
        //    Member.RemoveMemberFromCache(m);
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}

        ///// <summary>
        /////A test for RemoveMemberFromCache
        /////</summary>
        //[Test]
        //public void RemoveMemberFromCacheTest1()
        //{
        //    int NodeId = 0; // TODO: Initialize to an appropriate value
        //    Member.RemoveMemberFromCache(NodeId);
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}

        ///// <summary>
        /////A test for Save
        /////</summary>
        //[Test]
        //public void SaveTest()
        //{
        //    Guid id = new Guid(); // TODO: Initialize to an appropriate value
        //    Member target = new Member(id); // TODO: Initialize to an appropriate value
        //    target.Save();
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}

        ///// <summary>
        /////A test for ToXml
        /////</summary>
        //[Test]
        //public void ToXmlTest()
        //{
        //    Guid id = new Guid(); // TODO: Initialize to an appropriate value
        //    Member target = new Member(id); // TODO: Initialize to an appropriate value
        //    XmlDocument xd = null; // TODO: Initialize to an appropriate value
        //    bool Deep = false; // TODO: Initialize to an appropriate value
        //    XmlNode expected = null; // TODO: Initialize to an appropriate value
        //    XmlNode actual;
        //    actual = target.ToXml(xd, Deep);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for XmlGenerate
        /////</summary>
        //[Test]
        //public void XmlGenerateTest()
        //{
        //    Guid id = new Guid(); // TODO: Initialize to an appropriate value
        //    Member target = new Member(id); // TODO: Initialize to an appropriate value
        //    XmlDocument xd = null; // TODO: Initialize to an appropriate value
        //    target.XmlGenerate(xd);
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}

     

        ///// <summary>
        /////A test for getAllOtherMembers
        /////</summary>
        //[Test]
        //public void getAllOtherMembersTest()
        //{
        //    Member[] expected = null; // TODO: Initialize to an appropriate value
        //    Member[] actual;
        //    actual = Member.getAllOtherMembers();
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for getMemberFromFirstLetter
        /////</summary>
        //[Test]
        //public void getMemberFromFirstLetterTest()
        //{
        //    char letter = '\0'; // TODO: Initialize to an appropriate value
        //    Member[] expected = null; // TODO: Initialize to an appropriate value
        //    Member[] actual;
        //    actual = Member.getMemberFromFirstLetter(letter);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for Email
        /////</summary>
        //[Test]
        //public void EmailTest()
        //{
        //    Guid id = new Guid(); // TODO: Initialize to an appropriate value
        //    Member target = new Member(id); // TODO: Initialize to an appropriate value
        //    string expected = string.Empty; // TODO: Initialize to an appropriate value
        //    string actual;
        //    target.Email = expected;
        //    actual = target.Email;
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for GetAll
        /////</summary>
        //[Test]
        //public void GetAllTest()
        //{
        //    Member[] actual;
        //    actual = Member.GetAll;
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for Groups
        /////</summary>
        //[Test]
        //public void GroupsTest()
        //{
        //    Guid id = new Guid(); // TODO: Initialize to an appropriate value
        //    Member target = new Member(id); // TODO: Initialize to an appropriate value
        //    Hashtable actual;
        //    actual = target.Groups;
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for LoginName
        /////</summary>
        //[Test]
        //public void LoginNameTest()
        //{
        //    Guid id = new Guid(); // TODO: Initialize to an appropriate value
        //    Member target = new Member(id); // TODO: Initialize to an appropriate value
        //    string expected = string.Empty; // TODO: Initialize to an appropriate value
        //    string actual;
        //    target.LoginName = expected;
        //    actual = target.LoginName;
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for Password
        /////</summary>
        //[Test]
        //public void PasswordTest()
        //{
        //    Guid id = new Guid(); // TODO: Initialize to an appropriate value
        //    Member target = new Member(id); // TODO: Initialize to an appropriate value
        //    string expected = string.Empty; // TODO: Initialize to an appropriate value
        //    string actual;
        //    target.Password = expected;
        //    actual = target.Password;
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for Text
        /////</summary>
        //[Test]
        //public void TextTest()
        //{
        //    Guid id = new Guid(); // TODO: Initialize to an appropriate value
        //    Member target = new Member(id); // TODO: Initialize to an appropriate value
        //    string expected = string.Empty; // TODO: Initialize to an appropriate value
        //    string actual;
        //    target.Text = expected;
        //    actual = target.Text;
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

		private string m_umbracoConfigFile = "/home/kol3/Development/umbraco/290912/u4.7.2/umbraco/presentation/config/umbracoSettings.config";
        [SetUp]
        public void MyTestInitialize()
        {
			SetUpUtilities.AddUmbracoConfigFileToHttpCache();

        }
        
        /// <summary>
        /// Remove the created document type
        /// </summary>

        
		[TearDown]
        public void MyTestCleanup()
        {
			SetUpUtilities.RemoveUmbracoConfigFileFromHttpCache();
        }

        #endregion

    }
}
