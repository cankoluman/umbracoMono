using NUnit.Framework;
using Umbraco.Core.Configuration;
using Umbraco.Core.Persistence.Migrations.Initial;
using Umbraco.Tests.TestHelpers;

namespace Umbraco.Tests.Persistence
{
    [TestFixture]
    public class SchemaValidationTest : BaseDatabaseFactoryTest
    {
        [SetUp]
        public override void Initialize()
        {
			ConfigurationManagerProvider
				.Instance
					.SetManager(new ConfigurationManagerFromExeConfig());  

			base.Initialize();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
		[Ignore("Validation code test is not working with mysql atm")]
        public void DatabaseSchemaCreation_Produces_DatabaseSchemaResult_With_Zero_Errors()
        {
            // Arrange
            var db = DatabaseContext.Database;
            var schema = new DatabaseSchemaCreation(db);

            // Act
            var result = schema.ValidateSchema();

            // Assert
            Assert.That(result.Errors.Count, Is.EqualTo(0));
            Assert.AreEqual(result.DetermineInstalledVersion(), UmbracoVersion.Current);
        }
    }
}