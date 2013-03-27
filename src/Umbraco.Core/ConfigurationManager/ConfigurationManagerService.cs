using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Web;

namespace Umbraco.Core.Configuration
{
	public sealed class ConfigurationManagerService
	{

		private static volatile IConfigurationManager _instance;
		private static object _syncRoot = new Object(); 

		private static IConfigurationManager _configManager = null;
		public static IConfigurationManager ConfigManager
		{
			get {return _configManager;}
			set
			{
				_configManager = value;
			}
		}

		private ConfigurationManagerService (){}

		public static IConfigurationManager Instance
		{
			get
			{
				if (_instance == null)
				{
					lock(_syncRoot)
					{
						if (_instance == null)
							_instance = ConfigurationManagerFactory.GetConfigManager(_configManager);
					}
				}

				return _instance;
			}
		}

	}
}

