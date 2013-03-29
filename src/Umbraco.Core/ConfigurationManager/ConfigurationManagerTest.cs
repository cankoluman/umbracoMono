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
				return _appSettings;
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

	}
}

