using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Web;

namespace Umbraco.Core.Configuration
{
	public class ConfigurationManagerFromExeConfig : IConfigurationManager
	{
		private System.Configuration.Configuration _configuration;
		private NameValueCollection _appSettings;

		public ConfigurationManagerFromExeConfig
			(ConfigurationUserLevel userLevel = ConfigurationUserLevel.None)
		{
			_configuration = 
				ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

			_appSettings = new NameValueCollection();
			foreach (KeyValueConfigurationElement keyValueElement in _configuration.AppSettings.Settings)
			{
				_appSettings.Add(keyValueElement.Key, keyValueElement.Value);
			}

		}

		public NameValueCollection AppSettings
		{
			get { return _appSettings; }
		}

		public ConnectionStringSettingsCollection ConnectionStrings
		{
			get { return _configuration.ConnectionStrings.ConnectionStrings; }
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

		public T GetSection<T>(string SectionName)
			where T : class
		{
			return _configuration.GetSection(SectionName) as T;
		}

		public void RefreshSection(string SectionName)
		{
			ConfigurationManager.RefreshSection(SectionName);
		}
	}
}

