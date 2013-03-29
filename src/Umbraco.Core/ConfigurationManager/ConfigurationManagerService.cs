using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Web;

namespace Umbraco.Core.Configuration
{
	public sealed class ConfigurationManagerService
	{

		private static ConfigurationManagerService _instance = null;
		private static readonly object _syncRoot = new Object(); 

		private IConfigurationManager _configManager = null;

		public IConfigurationManager GetConfigManager()
		{
			return ConfigurationManagerFactory.GetConfigManager(_configManager);
		}

		public void SetManager(IConfigurationManager configManager = null)
		{
			_configManager = configManager;
		}

		private ConfigurationManagerService (){}

		public static ConfigurationManagerService Instance
		{
			get
			{
				lock(_syncRoot)
				{
					if (_instance == null)
						_instance = new ConfigurationManagerService();

					return _instance;
				}
			}
		}

	}
}

