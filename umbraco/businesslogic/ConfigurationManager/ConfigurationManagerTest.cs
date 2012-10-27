using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Web;

using umbraco.interfaces;

namespace umbraco.BusinessLogic
{
	public class ConfigurationManagerTest : IConfigurationManager
	{
		public ConfigurationManagerTest(NameValueCollection appSettings)
		{
			_appSettings = new NameValueCollection();
			_appSettings.Add(appSettings);
		}

		private NameValueCollection _appSettings;

		public NameValueCollection AppSettings
		{
			get
			{
				return MergeAppSettings(ConfigurationManager.AppSettings);
			}
		}

		public Object GetSection(string SectionName)
		{
			return ConfigurationManager.GetSection(SectionName);
		}

		public void RefreshSection(string SectionName)
		{
			ConfigurationManager.RefreshSection(SectionName);
		}

		private NameValueCollection MergeAppSettings(NameValueCollection appSettings)
		{
			NameValueCollection mergedAppSettings;
			mergedAppSettings = new NameValueCollection();

			if (appSettings.HasKeys())
				foreach (string key in appSettings)
				{
					if (_appSettings[key] != null)
						mergedAppSettings.Add(key.ToString(), _appSettings[key].ToString());
					else
						mergedAppSettings.Add(key.ToString(), appSettings[key].ToString());
				}
			else 
				mergedAppSettings.Add(_appSettings);


			return mergedAppSettings;
		}
	}
}

