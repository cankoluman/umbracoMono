using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Web;

namespace Umbraco.Core.Configuration
{
	public class ConfigurationManagerTest : IConfigurationManager
	{
		private NameValueCollection _appSettings;

		public ConfigurationManagerTest(NameValueCollection appSettings)
		{
			_appSettings = new NameValueCollection();
			_appSettings.Add(appSettings);
		}

		public NameValueCollection AppSettings
		{
			get
			{
				return MergeAppSettings(ConfigurationManager.AppSettings);
			}
		}

		public void SetAppSetting(string key, string val)
		{
			if (_appSettings.ContainsKey(key))
				_appSettings.Set(key, val);
			else
				_appSettings.Add(key,val);
		}

		public void ClearAppSetting(string key)
		{
			_appSettings.Set(key, String.Empty);
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

