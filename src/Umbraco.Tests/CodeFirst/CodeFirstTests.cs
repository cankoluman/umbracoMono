using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Umbraco.Core;
using Umbraco.Core.Configuration;
using Umbraco.Core.IO;
using Umbraco.Core.Models;
using Umbraco.Core.ObjectResolution;
using Umbraco.Core.Serialization;
using Umbraco.Tests.CodeFirst.Definitions;
using Umbraco.Tests.CodeFirst.TestModels;
using Umbraco.Tests.CodeFirst.TestModels.Composition;
using Umbraco.Tests.TestHelpers;
using Umbraco.Tests.TestHelpers.Entities;
using umbraco.editorControls.tinyMCE3;
using umbraco.interfaces;

namespace Umbraco.Tests.CodeFirst
{
    [TestFixture]
    public class CodeFirstTests : BaseDatabaseFactoryTest
    {
        [SetUp]
        public override void Initialize()
        {
			ConfigurationManagerProvider
				.Instance
					.SetManager(new ConfigurationManagerFromExeConfig());            

			UmbracoSettings.SettingsFilePath = IOHelper.MapPath(SystemDirectories.Config + Path.DirectorySeparatorChar, false);

            //this ensures its reset
            PluginManager.Current = new PluginManager();

            //for testing, we'll specify which assemblies are scanned for the PluginTypeResolver
            PluginManager.Current.AssembliesToScan = new[]
				{
                    typeof(IDataType).Assembly,
                    typeof(tinyMCE3dataType).Assembly,
                    typeof (ContentTypeBase).Assembly
				};

            DataTypesResolver.Current = new DataTypesResolver(
                () => PluginManager.Current.ResolveDataTypes());

            base.Initialize();

			CreateTestData();

            var serviceStackSerializer = new ServiceStackJsonSerializer();
            SerializationService = new SerializationService(serviceStackSerializer);
        }

        [Test]
        public void Can_Create_Model_With_NonExisting_DataTypeDefinition()
        {
            ContentTypeDefinitionFactory.ClearContentTypeCache();

            var modelType = typeof(ModelWithNewDataType);
            var contentType = ContentTypeDefinitionFactory.GetContentTypeDefinition(modelType);

            var mappedContentTypes = ContentTypeDefinitionFactory.RetrieveMappedContentTypes();
            ServiceContext.ContentTypeService.Save(mappedContentTypes);

            var model = ServiceContext.ContentTypeService.GetContentType(1045);
            Assert.That(model, Is.Not.Null);
        }

        [Test]
        public void Can_Resolve_ContentType_From_Decorated_Home_Model()
        {
            var modelType = typeof(Home);
            var contentType = ContentTypeDefinitionFactory.GetContentTypeDefinition(modelType);

            Assert.That(contentType, Is.Not.Null);
            Assert.That(contentType.Value.PropertyGroups, Is.Not.Null);
            Assert.That(contentType.Value.PropertyTypes.Any(), Is.True);
            Assert.That(contentType.Value.PropertyTypes.Count(), Is.EqualTo(2));

            var result = SerializationService.ToStream(contentType.Value);
            var xml = result.ResultStream.ToJsonString();
            Console.WriteLine(xml);
        }

        [Test]
        public void Can_Resolve_ContentType_From_Decorated_ContentPage_Model()
        {
            var modelType = typeof(ContentPage);
            var contentType = ContentTypeDefinitionFactory.GetContentTypeDefinition(modelType);

            Assert.That(contentType, Is.Not.Null);
            Assert.That(contentType.Value.PropertyGroups, Is.Not.Null);
            Assert.That(contentType.Value.PropertyGroups.Any(), Is.True);
            Assert.That(contentType.Value.PropertyGroups.Count(), Is.EqualTo(1));
            Assert.That(contentType.Value.PropertyTypes.Any(), Is.True);
            Assert.That(contentType.Value.PropertyTypes.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Can_Resolve_ContentType_From_PlainPocoType_Model()
        {
            var modelType = typeof(PlainPocoType);
            var contentType = ContentTypeDefinitionFactory.GetContentTypeDefinition(modelType);

            Assert.That(contentType, Is.Not.Null);
            Assert.That(contentType.Value.PropertyGroups, Is.Not.Null);
            Assert.That(contentType.Value.PropertyTypes.Any(), Is.True);
            Assert.That(contentType.Value.PropertyGroups.Count(), Is.EqualTo(1));
            Assert.That(contentType.Value.PropertyTypes.Count(), Is.EqualTo(5));

            var result = SerializationService.ToStream(contentType.Value);
            var xml = result.ResultStream.ToJsonString();
            Console.WriteLine(xml);
        }

        [Test]
        public void Can_Retrieve_ContentTypes_After_Resolving()
        {
            ContentTypeDefinitionFactory.ClearContentTypeCache();

            var modelType = typeof(Home);
            var contentType = ContentTypeDefinitionFactory.GetContentTypeDefinition(modelType);
            var mappedContentTypes = ContentTypeDefinitionFactory.RetrieveMappedContentTypes();

            Assert.That(mappedContentTypes, Is.Not.Null);
            Assert.That(mappedContentTypes.Any(), Is.True);
            Assert.That(mappedContentTypes.Count(), Is.EqualTo(2));
        }

        [Test]
        public void Can_Resolve_Existing_ContentType_With_Decorated_Model()
        {
            var textPage = MockedContentTypes.CreateTextpageContentType();
            ServiceContext.ContentTypeService.Save(textPage);

            var modelType = typeof(TextPage);
            var contentType = ContentTypeDefinitionFactory.GetContentTypeDefinition(modelType);

            Assert.That(contentType.Value.Id, Is.EqualTo(textPage.Id));

            ServiceContext.ContentTypeService.Save(contentType.Value);
        }

        [Test]
        public void Can_Save_Models_To_Database()
        {
            ContentTypeDefinitionFactory.ClearContentTypeCache();

            var homeModel = typeof(Home);
            var textPageModel = typeof(TextPage);
            var homeContentType = ContentTypeDefinitionFactory.GetContentTypeDefinition(homeModel);
            var textPageContentType = ContentTypeDefinitionFactory.GetContentTypeDefinition(textPageModel);

            var mappedContentTypes = ContentTypeDefinitionFactory.RetrieveMappedContentTypes().ToList();
            ServiceContext.ContentTypeService.Save(mappedContentTypes);
        }

        [Test]
        public void Can_Resolve_Parent_Child_ContentTypes_And_Save_To_Database()
        {
            ContentTypeDefinitionFactory.ClearContentTypeCache();

            var simplemodel = typeof(SimpleContentPage);
            var model = typeof(AdvancedContentPage);
            var sContentType = ContentTypeDefinitionFactory.GetContentTypeDefinition(simplemodel);
            var aContentType = ContentTypeDefinitionFactory.GetContentTypeDefinition(model);

            var mappedContentTypes = ContentTypeDefinitionFactory.RetrieveMappedContentTypes().ToList();
            ServiceContext.ContentTypeService.Save(mappedContentTypes);

            var type1 = ServiceContext.ContentTypeService.GetContentType(1044);
            var type2 = ServiceContext.ContentTypeService.GetContentType(1045);

            Assert.That(type1, Is.Not.Null);
            Assert.That(type2, Is.Not.Null);
        }

        [Test]
        public void Can_Resolve_And_Save_Decorated_Model_To_Database()
        {
            ContentTypeDefinitionFactory.ClearContentTypeCache();

            var model = typeof(DecoratedModelPage);
            var modelContentType = ContentTypeDefinitionFactory.GetContentTypeDefinition(model);

            var mappedContentTypes = ContentTypeDefinitionFactory.RetrieveMappedContentTypes().ToList();
            ServiceContext.ContentTypeService.Save(mappedContentTypes);

            var type1 = ServiceContext.ContentTypeService.GetContentType(1045);

            Assert.That(type1, Is.Not.Null);
            Assert.That(type1.PropertyGroups.Count(), Is.EqualTo(1));
            Assert.That(type1.PropertyTypes.Count(), Is.EqualTo(3));

        }

        [Test]
        public void Can_Resolve_ContentType_Composition_And_Save_To_Database()
        {
            ContentTypeDefinitionFactory.ClearContentTypeCache();

            var metaSeoModel = typeof(MetaSeo);
            var seoContentType = ContentTypeDefinitionFactory.GetContentTypeDefinition(metaSeoModel);
            var metaModel = typeof(Meta);
            var metaContentType = ContentTypeDefinitionFactory.GetContentTypeDefinition(metaModel);
            var baseModel = typeof(Base);
            var baseContentType = ContentTypeDefinitionFactory.GetContentTypeDefinition(baseModel);
            var newsModel = typeof(News);
            var newsContentType = ContentTypeDefinitionFactory.GetContentTypeDefinition(newsModel);

            var mappedContentTypes = ContentTypeDefinitionFactory.RetrieveMappedContentTypes().ToList();
            ServiceContext.ContentTypeService.Save(mappedContentTypes);

            Assert.That(mappedContentTypes.Count(), Is.EqualTo(4));
        }

        [Test]
		[Ignore("Fails in original 6.0.3")]
        public void Can_Resolve_Full_List_Of_Models_Implementing_ContentTypeBase()
        {
            ContentTypeDefinitionFactory.ClearContentTypeCache();

            var foundTypes = PluginManager.Current.ResolveContentTypeBaseTypes();
            var contentTypeList = foundTypes.Select(ContentTypeDefinitionFactory.GetContentTypeDefinition).ToList();

            var mappedContentTypes = ContentTypeDefinitionFactory.RetrieveMappedContentTypes();

            Assert.That(contentTypeList.Count(), Is.EqualTo(mappedContentTypes.Count()));

            ServiceContext.ContentTypeService.Save(mappedContentTypes);//Save to db
        }

        private SerializationService SerializationService { get; set; }

        [TearDown]
        public override void TearDown()
        {
			base.TearDown();

            //reset the app context
            DataTypesResolver.Reset();
            ApplicationContext.Current = null;
            Resolution.IsFrozen = false;
            PluginManager.Current = null;

            string path = TestHelper.CurrentAssemblyDirectory;
            AppDomain.CurrentDomain.SetData("DataDirectory", null);
            
            SerializationService = null;

            UmbracoSettings.ResetSetters();
        }

		public void CreateTestData()
		{
			//Create and Save ContentType "umbTextpage" -> 1044
			//Bring MySql table id value up so that we do not need to 
			//update all other values
			ContentType ignoreType = MockedContentTypes.CreateSimpleContentType("mysqlBuffer", "IgnorePage");
			ignoreType.Key = new Guid("1D3A8E6E-0000-4CC1-0000-1AEE19821522");
			ServiceContext.ContentTypeService.Save(ignoreType);            

			//Create and Save ContentType "umbTextpage" -> 1045
			ContentType contentType = MockedContentTypes.CreateSimpleContentType("umbTextpage", "Textpage");
			contentType.Key = new Guid("1D3A8E6E-2EA9-4CC1-B229-1AEE19821522");
			ServiceContext.ContentTypeService.Save(contentType);

			//Create and Save Content "Homepage" based on "umbTextpage" -> 1046
			Content textpage = MockedContent.CreateSimpleContent(contentType);
			textpage.Key = new Guid("B58B3AD4-62C2-4E27-B1BE-837BD7C533E0");
			ServiceContext.ContentService.Save(textpage, 0);

			//Create and Save Content "Text Page 1" based on "umbTextpage" -> 1047
			Content subpage = MockedContent.CreateSimpleContent(contentType, "Text Page 1", textpage.Id);
			subpage.Key = new Guid("FF11402B-7E53-4654-81A7-462AC2108059");
			ServiceContext.ContentService.Save(subpage, 0);

			//Create and Save Content "Text Page 2" based on "umbTextpage" -> 1048
			Content subpage2 = MockedContent.CreateSimpleContent(contentType, "Text Page 2", textpage.Id);
			subpage2.Key = new Guid("3291fa37-2a6d-449c-953a-559ce34ae889");
			ServiceContext.ContentService.Save(subpage2, 0);

			//Create and Save Content "Text Page Deleted" based on "umbTextpage" -> 1049
			Content trashed = MockedContent.CreateSimpleContent(contentType, "Text Page Deleted", -20);
			trashed.Trashed = true;
			ServiceContext.ContentService.Save(trashed, 0);
		}
    }
}