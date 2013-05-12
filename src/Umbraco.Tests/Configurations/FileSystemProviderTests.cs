using System.Configuration;
using System.Linq;
using NUnit.Framework;
using Umbraco.Core.Configuration;

namespace Umbraco.Tests.Configurations
{
    [TestFixture]
    public class FileSystemProviderTests
    {
        [Test]
        public void Can_Get_Media_Provider()
        {
			var config = ConfigurationManagerProvider.Instance.GetConfigManager().GetSection<FileSystemProvidersSection>("FileSystemProviders");
            var providerConfig = config.Providers["media"];

            Assert.That(providerConfig, Is.Not.Null);
            Assert.That(providerConfig.Parameters.AllKeys.Any(), Is.True);
        }
    }
}