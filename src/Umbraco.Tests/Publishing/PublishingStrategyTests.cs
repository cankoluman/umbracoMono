﻿using System;
using System.Web;
using System.Xml;
using NUnit.Framework;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.ObjectResolution;
using Umbraco.Tests.TestHelpers;
using Umbraco.Tests.TestHelpers.Entities;
using Umbraco.Web.Strategies;
using umbraco.editorControls.tinyMCE3;
using umbraco.interfaces;

namespace Umbraco.Tests.Publishing
{
    [TestFixture]
    public class PublishingStrategyTests : BaseDatabaseFactoryTest
    {
        [SetUp]
        public override void Initialize()
        {
            //this ensures its reset
            PluginManager.Current = new PluginManager();

            //for testing, we'll specify which assemblies are scanned for the PluginTypeResolver
            PluginManager.Current.AssembliesToScan = new[]
				{
                    typeof(IDataType).Assembly,
                    typeof(tinyMCE3dataType).Assembly
				};

            DataTypesResolver.Current = new DataTypesResolver(
                PluginManager.Current.ResolveDataTypes());

            base.Initialize();

            CreateTestData();
        }

        [TearDown]
        public override void TearDown()
        {
            DatabaseContext.Database.Dispose();

            //TestHelper.ClearDatabase();

            //reset the app context
            DataTypesResolver.Reset();
            ApplicationContext.Current = null;
            Resolution.IsFrozen = false;

            string path = TestHelper.CurrentAssemblyDirectory;
            AppDomain.CurrentDomain.SetData("DataDirectory", null);
        }

        [Test]
        public void Can_Publish_And_Update_Xml_Cache()
        {
            // Arrange
            var httpContext = base.GetUmbracoContext("/test", 1234).HttpContext;
            var updateContentCache = new UpdateContentCache(httpContext);
            var contentService = ServiceContext.ContentService;
            var content = contentService.GetById(1046);

            // Act
            bool published = contentService.Publish(content, 0);

            // Assert
            Assert.That(published, Is.True);
            Assert.That(content.Published, Is.True);
            Assert.IsTrue(httpContext.Items.Contains("UmbracoXmlContextContent"));

            var document = httpContext.Items["UmbracoXmlContextContent"] as XmlDocument;
            Console.Write(document.OuterXml);
            document.Save("umbraco.config");
        }

        public void CreateTestData()
        {
            //NOTE Maybe not the best way to create/save test data as we are using the services, which are being tested.

            //Create and Save ContentType "umbTextpage" -> 1045
            ContentType contentType = MockedContentTypes.CreateSimpleContentType("umbTextpage", "Textpage");
            ServiceContext.ContentTypeService.Save(contentType);

            //Create and Save Content "Homepage" based on "umbTextpage" -> 1046
            Content textpage = MockedContent.CreateSimpleContent(contentType);
            ServiceContext.ContentService.Save(textpage, 0);

            //Create and Save Content "Text Page 1" based on "umbTextpage" -> 1047
            Content subpage = MockedContent.CreateSimpleContent(contentType, "Text Page 1", textpage.Id);
            ServiceContext.ContentService.Save(subpage, 0);

            //Create and Save Content "Text Page 1" based on "umbTextpage" -> 1048
            Content subpage2 = MockedContent.CreateSimpleContent(contentType, "Text Page 2", textpage.Id);
            ServiceContext.ContentService.Save(subpage2, 0);
        }
    }
}