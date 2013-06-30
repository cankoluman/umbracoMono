using System;

namespace Umbraco.Core.Configuration
{	
	public sealed class ConfigurationManagerProvider
	{
		private IConfigurationManager _configManager = null;

		private static readonly Lazy<ConfigurationManagerProvider> _configurationProvider =
			new Lazy<ConfigurationManagerProvider>(() => new ConfigurationManagerProvider());

		public static ConfigurationManagerProvider Instance { get { return _configurationProvider.Value; } }
		
		public IConfigurationManager GetConfigManager()
		{
			if (_configManager == null)
				SetManager();

			return _configManager;
		}

		public void SetManager(IConfigurationManager configManager = null)
		{
			if (configManager == null)
				_configManager  = new ConfigurationManagerDefault();
			else
				_configManager = configManager;
		}

		private ConfigurationManagerProvider()
		{
		}
	}
}