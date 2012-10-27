using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Web;

using umbraco.interfaces;

namespace umbraco.BusinessLogic
{
	public class ConfigurationManagerFactory
	{ 

		private ConfigurationManagerFactory() {}


		public static IConfigurationManager GetConfigManager(IConfigurationManager configManager)
		{
			if (configManager == null)
				return new ConfigurationManagerDefault();
			else
				return configManager;
		}

	}
}

