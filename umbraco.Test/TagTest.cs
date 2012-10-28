using umbraco.cms.businesslogic.Tags;
using NUnit.Framework;
using System;
using umbraco.cms.businesslogic.web;
using System.Collections.Generic;
using umbraco.cms.businesslogic;
using System.Linq;
using umbraco.BusinessLogic;

using System.Xml;
using System.Web;
using System.Web.Caching;

namespace umbraco.Test
{
    
    
    /// <summary>
    ///This is a test class for TagTest and is intended
    ///to contain all TagTest Unit Tests
    ///</summary>
    [TestFixture]
    public class TagTest
    {

        
		[TestFixtureSetUp]
		public void InitTestFixture()
		{
			SetUpUtilities.InitConfigurationManager();
			m_User = new User(0);
			SetUpUtilities.InitAppDomainDynamicBase();
		}

		/// <summary>
        /// Create a new tag and delete it
        ///</summary>
        [Test]
        public void Tag_Make_New()
        {
            var t = Tag.AddTag(Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"));
            Assert.IsTrue(t > 0); //id should be greater than zero
            Assert.AreEqual(1, Tag.GetTags().Where(x => x.Id == t).Count());

            Tag.RemoveTag(t);
            //make sure it's gone
            Assert.AreEqual(0, Tag.GetTags().Where(x => x.Id == t).Count());
         
        }


        /// <summary>
        /// Creates a new tag and a new document, assigns the tag to the document and deletes the document
        /// </summary>
        [Test]
        public void Tag_Make_New_Assign_Node_Delete_Node()
        {

            var t = Tag.AddTag(Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"));
            Assert.IsTrue(t > 0); //id should be greater than zero
            Assert.AreEqual(1, Tag.GetTags().Where(x => x.Id == t).Count());

            var dt = DocumentType.GetAllAsList().First();
            var doc = Document.MakeNew(Guid.NewGuid().ToString("N"), dt, m_User, -1);

            Tag.AssociateTagToNode(doc.Id, t);
            //make sure it's associated
            Assert.AreEqual(1, Tag.GetTags(doc.Id).Count());

            //delete the doc
            doc.delete(true);

            //make sure it's not related any more
            Assert.AreEqual(0, Tag.GetTags(doc.Id).Count());

            Tag.RemoveTag(t);
            //make sure it's gone
            Assert.AreEqual(0, Tag.GetTags().Where(x => x.Id == t).Count());

        }

        /// <summary>
        /// Test the AddTagsToNode method and deletes it
        /// </summary>
        [Test]
        public void Tag_Add_Tags_To_Node()
        {
            var dt = DocumentType.GetAllAsList().First();
            var doc = Document.MakeNew(Guid.NewGuid().ToString("N"), dt, m_User, -1);

            var grp = Guid.NewGuid().ToString("N");
            Tag.AddTagsToNode(doc.Id, string.Format("{0},{1}", Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N")), grp);

            var tags = Tag.GetTags(doc.Id);
            //make sure they are there by document
            Assert.AreEqual(2, tags.Count());

            //make sure they are there by group
            Assert.AreEqual(2, Tag.GetTags(grp).Count());

            //make sure they are there by both group and node
            Assert.AreEqual(2, Tag.GetTags(doc.Id, grp).Count());

            doc.delete(true);

            //make sure associations are gone
            Assert.AreEqual(0, Tag.GetTags(doc.Id).Count());

            //delete the tags
            foreach (var t in tags)
            {
                Tag.RemoveTag(t.Id);
                Assert.AreEqual(0, Tag.GetTags().Where(x => x.Id == t.Id).Count());
            }
        }

        private User m_User;

        #region Tests to write

        ///// <summary>
        /////A test for Tag Constructor
        /////</summary>
        //[Test]
        //public void TagConstructorTest()
        //{
        //    int id = 0; // TODO: Initialize to an appropriate value
        //    string tag = string.Empty; // TODO: Initialize to an appropriate value
        //    string group = string.Empty; // TODO: Initialize to an appropriate value
        //    int nodeCount = 0; // TODO: Initialize to an appropriate value
        //    Tag target = new Tag(id, tag, group, nodeCount);
        //    Assert.Inconclusive("TODO: Implement code to verify target");
        //}

        ///// <summary>
        /////A test for Tag Constructor
        /////</summary>
        //[Test]
        //public void TagConstructorTest1()
        //{
        //    Tag target = new Tag();
        //    Assert.Inconclusive("TODO: Implement code to verify target");
        //}

        

        ///// <summary>
        /////A test for AddTagsToNode
        /////</summary>
        //[Test]
        //public void AddTagsToNodeTest()
        //{
        //    int nodeId = 0; // TODO: Initialize to an appropriate value
        //    string tags = string.Empty; // TODO: Initialize to an appropriate value
        //    string group = string.Empty; // TODO: Initialize to an appropriate value
        //    Tag.AddTagsToNode(nodeId, tags, group);
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}

        ///// <summary>
        /////A test for AssociateTagToNode
        /////</summary>
        //[Test]
        //public void AssociateTagToNodeTest()
        //{
        //    int nodeId = 0; // TODO: Initialize to an appropriate value
        //    int tagId = 0; // TODO: Initialize to an appropriate value
        //    Tag.AssociateTagToNode(nodeId, tagId);
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}

        ///// <summary>
        /////A test for GetDocumentsWithTags
        /////</summary>
        //[Test]
        //public void GetDocumentsWithTagsTest()
        //{
        //    string tags = string.Empty; // TODO: Initialize to an appropriate value
        //    IEnumerable<Document> expected = null; // TODO: Initialize to an appropriate value
        //    IEnumerable<Document> actual;
        //    actual = Tag.GetDocumentsWithTags(tags);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for GetNodesWithTags
        /////</summary>
        //[Test]
        //public void GetNodesWithTagsTest()
        //{
        //    string tags = string.Empty; // TODO: Initialize to an appropriate value
        //    IEnumerable<CMSNode> expected = null; // TODO: Initialize to an appropriate value
        //    IEnumerable<CMSNode> actual;
        //    actual = Tag.GetNodesWithTags(tags);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for GetTagId
        /////</summary>
        //[Test]
        //public void GetTagIdTest()
        //{
        //    string tag = string.Empty; // TODO: Initialize to an appropriate value
        //    string group = string.Empty; // TODO: Initialize to an appropriate value
        //    int expected = 0; // TODO: Initialize to an appropriate value
        //    int actual;
        //    actual = Tag.GetTagId(tag, group);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for GetTags
        /////</summary>
        //[Test]
        //public void GetTagsTest()
        //{
        //    string group = string.Empty; // TODO: Initialize to an appropriate value
        //    IEnumerable<Tag> expected = null; // TODO: Initialize to an appropriate value
        //    IEnumerable<Tag> actual;
        //    actual = Tag.GetTags(group);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for GetTags
        /////</summary>
        //[Test]
        //public void GetTagsTest1()
        //{
        //    IEnumerable<Tag> expected = null; // TODO: Initialize to an appropriate value
        //    IEnumerable<Tag> actual;
        //    actual = Tag.GetTags();
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for GetTags
        /////</summary>
        //[Test]
        //public void GetTagsTest2()
        //{
        //    int nodeId = 0; // TODO: Initialize to an appropriate value
        //    IEnumerable<Tag> expected = null; // TODO: Initialize to an appropriate value
        //    IEnumerable<Tag> actual;
        //    actual = Tag.GetTags(nodeId);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for GetTags
        /////</summary>
        //[Test]
        //public void GetTagsTest3()
        //{
        //    int nodeId = 0; // TODO: Initialize to an appropriate value
        //    string group = string.Empty; // TODO: Initialize to an appropriate value
        //    IEnumerable<Tag> expected = null; // TODO: Initialize to an appropriate value
        //    IEnumerable<Tag> actual;
        //    actual = Tag.GetTags(nodeId, group);
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for RemoveTagFromNode
        /////</summary>
        //[Test]
        //public void RemoveTagFromNodeTest()
        //{
        //    int nodeId = 0; // TODO: Initialize to an appropriate value
        //    string tag = string.Empty; // TODO: Initialize to an appropriate value
        //    string group = string.Empty; // TODO: Initialize to an appropriate value
        //    Tag.RemoveTagFromNode(nodeId, tag, group);
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}

        ///// <summary>
        /////A test for RemoveTagsFromNode
        /////</summary>
        //[Test]
        //public void RemoveTagsFromNodeTest()
        //{
        //    int nodeId = 0; // TODO: Initialize to an appropriate value
        //    string group = string.Empty; // TODO: Initialize to an appropriate value
        //    Tag.RemoveTagsFromNode(nodeId, group);
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}

        ///// <summary>
        /////A test for RemoveTagsFromNode
        /////</summary>
        //[Test]
        //public void RemoveTagsFromNodeTest1()
        //{
        //    int nodeId = 0; // TODO: Initialize to an appropriate value
        //    Tag.RemoveTagsFromNode(nodeId);
        //    Assert.Inconclusive("A method that does not return a value cannot be verified.");
        //}

        ///// <summary>
        /////A test for Group
        /////</summary>
        //[Test]
        //public void GroupTest()
        //{
        //    Tag target = new Tag(); // TODO: Initialize to an appropriate value
        //    string expected = string.Empty; // TODO: Initialize to an appropriate value
        //    string actual;
        //    target.Group = expected;
        //    actual = target.Group;
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for Id
        /////</summary>
        //[Test]
        //public void IdTest()
        //{
        //    Tag target = new Tag(); // TODO: Initialize to an appropriate value
        //    int expected = 0; // TODO: Initialize to an appropriate value
        //    int actual;
        //    target.Id = expected;
        //    actual = target.Id;
        //    Assert.AreEqual(expected, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        ///// <summary>
        /////A test for TagCaption
        /////</summary>
        //[Test]
        //public void TagCaptionTest()
        //{
        //    Tag target = new Tag(); // TODO: Initialize to an appropriate value
        //    string expected = string.Empty; // TODO: Initialize to an appropriate value
        //    string actual;
        //    target.TagCaption = expected;
        //    actual = target.TagCaption;
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
